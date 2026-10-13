using System;
using System.Collections.Generic;

namespace CafeConsoleApp
{
    public class CafeService
    {
        // Default cafe menu.
        private readonly List<MenuItem> menuItems = new List<MenuItem>()
        {
            new MenuItem(1, "Americano", 2500m),
            new MenuItem(2, "Cappuccino", 3500m),
            new MenuItem(3, "Cafe Latte", 4000m),
            new MenuItem(4, "Mocha", 4500m),
            new MenuItem(5, "Milk Tea", 3000m),
            new MenuItem(6, "Chocolate Cake", 5000m),
            new MenuItem(7, "Croissant", 2800m)
        };

        // Store orders in memory while the application is running.
        private readonly List<CafeOrder> orders = new List<CafeOrder>();

        private int nextOrderId = 1;


        // Display all cafe menu items.
        public void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("==============================================");
            Console.WriteLine("                  CAFE MENU");
            Console.WriteLine("==============================================");

            Console.WriteLine(
                "{0,-10} {1,-22} {2,12}",
                "Menu ID",
                "Item Name",
                "Price"
            );

            Console.WriteLine("----------------------------------------------");

            foreach (MenuItem item in menuItems)
            {
                Console.WriteLine(
                    "{0,-10} {1,-22} {2,12:N2}",
                    item.MenuId,
                    item.ItemName,
                    item.Price
                );
            }

            Console.WriteLine("==============================================");
        }


        // Check whether the menu ID exists.
        public bool IsValidMenuId(int menuId)
        {
            return menuItems.Exists(item => item.MenuId == menuId);
        }


        // Create an order and calculate its total amount.
        public int PlaceOrder(Dictionary<int, int> orderItems)
        {
            if (orderItems == null ||
                orderItems.Count == 0 ||
                orderItems.Count > 10)
            {
                Console.WriteLine("Invalid order.");
                return 0;
            }

            CafeOrder order = new CafeOrder
            {
                OrderId = nextOrderId,
                OrderDate = DateTime.Now
            };

            foreach (KeyValuePair<int, int> selectedItem in orderItems)
            {
                MenuItem? menuItem = menuItems.Find(
                    item => item.MenuId == selectedItem.Key
                );

                if (menuItem == null)
                {
                    Console.WriteLine("Invalid menu item.");
                    return 0;
                }

                if (selectedItem.Value <= 0)
                {
                    Console.WriteLine("Quantity must be greater than zero.");
                    return 0;
                }

                decimal lineTotal = menuItem.Price * selectedItem.Value;

                OrderLine orderLine = new OrderLine
                {
                    MenuId = menuItem.MenuId,
                    ItemName = menuItem.ItemName,
                    Quantity = selectedItem.Value,
                    UnitPrice = menuItem.Price,
                    LineTotal = lineTotal
                };

                order.OrderLines.Add(orderLine);

                order.TotalAmount += lineTotal;
            }

            // Save the order in memory.
            orders.Add(order);

            nextOrderId++;

            Console.WriteLine();
            Console.WriteLine("Order created successfully!");

            return order.OrderId;
        }


        // Print the receipt for the newly created order.
        public void PrintReceipt(int orderId)
        {
            CafeOrder? order = orders.Find(
                item => item.OrderId == orderId
            );

            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("==============================================================");
            Console.WriteLine("                       CAFE RECEIPT");
            Console.WriteLine("==============================================================");

            Console.WriteLine($"Order ID   : {order.OrderId}");
            Console.WriteLine(
                $"Order Date : {order.OrderDate:dd-MM-yyyy HH:mm:ss}"
            );

            Console.WriteLine("--------------------------------------------------------------");

            Console.WriteLine(
                "{0,-20} {1,8} {2,12} {3,14}",
                "Item",
                "Quantity",
                "Unit Price",
                "Subtotal"
            );

            Console.WriteLine("--------------------------------------------------------------");

            foreach (OrderLine line in order.OrderLines)
            {
                Console.WriteLine(
                    "{0,-20} {1,8} {2,12:N2} {3,14:N2}",
                    line.ItemName,
                    line.Quantity,
                    line.UnitPrice,
                    line.LineTotal
                );
            }

            Console.WriteLine("--------------------------------------------------------------");

            Console.WriteLine(
                "{0,-42} {1,14:N2}",
                "TOTAL BILL:",
                order.TotalAmount
            );

            Console.WriteLine("==============================================================");
            Console.WriteLine("              Thank you for visiting our cafe!");
            Console.WriteLine("==============================================================");
        }
    }


    // Represents one menu item.
    public class MenuItem
    {
        public int MenuId { get; set; }

        public string ItemName { get; set; }

        public decimal Price { get; set; }


        public MenuItem(int menuId, string itemName, decimal price)
        {
            MenuId = menuId;
            ItemName = itemName;
            Price = price;
        }
    }


    // Represents one complete customer order.
    public class CafeOrder
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public List<OrderLine> OrderLines { get; set; }
            = new List<OrderLine>();
    }


    // Represents one menu item within an order.
    public class OrderLine
    {
        public int MenuId { get; set; }

        public string ItemName { get; set; } = "";

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal LineTotal { get; set; }
    }
}