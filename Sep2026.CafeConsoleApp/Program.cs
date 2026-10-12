using System;
using System.Collections.Generic;
using Sep2026.CafeConsoleApp;

CafeService cafeService = new CafeService();

bool running = true;

while (running)
{
    Console.WriteLine("==================================");
    Console.WriteLine("       CAFE MANAGEMENT SYSTEM");
    Console.WriteLine("==================================");
    Console.WriteLine("1. Show Menu");
    Console.WriteLine("2. Order Menu");
    Console.WriteLine("3. Exit");
    Console.WriteLine("==================================");

    Console.Write("Choose an option: ");
    string? option = Console.ReadLine();

    switch (option)
    {
        case "1":
            // Display all menu items.
            cafeService.ShowMenu();
            break;

        case "2":
            // Display the menu before taking the order.
            cafeService.ShowMenu();

            Dictionary<int, int> orderItems = new Dictionary<int, int>();

            Console.WriteLine();
            Console.WriteLine("You can order up to 10 different menu items.");
            Console.WriteLine();

            while (orderItems.Count < 10)
            {
                int menuId = ReadNumber("Enter Menu ID: ");

                // Check whether the menu ID exists.
                if (!cafeService.IsValidMenuId(menuId))
                {
                    Console.WriteLine("Invalid Menu ID. Please select an item from the menu.");
                    continue;
                }

                // Prevent the same menu item from being selected twice.
                if (orderItems.ContainsKey(menuId))
                {
                    Console.WriteLine("This item is already in your order. Please choose another item.");
                    continue;
                }

                int quantity = ReadPositiveNumber("Enter Quantity: ");

                // Add the selected item and its quantity to the order.
                orderItems.Add(menuId, quantity);

                Console.WriteLine("Item added successfully.");

                // Stop automatically when 10 different items have been selected.
                if (orderItems.Count == 10)
                {
                    Console.WriteLine("You have reached the 10-item limit.");
                    break;
                }

                // Ask whether the customer wants another item.
                string answer = ReadYesNo(
                    "Do you want to add another item? (Y/N): "
                );

                if (answer == "N")
                {
                    break;
                }

                Console.WriteLine();
            }

            // Do not create an order if no items were selected.
            if (orderItems.Count == 0)
            {
                Console.WriteLine("No items were ordered.");
                break;
            }

            // Create the order and calculate the bill.
            int orderId = cafeService.PlaceOrder(orderItems);

            // Print the receipt automatically.
            if (orderId > 0)
            {
                cafeService.PrintReceipt(orderId);
            }

            break;

        case "3":
            running = false;
            break;

        default:
            Console.WriteLine("Invalid option. Please choose 1, 2, or 3.");
            break;
    }

    Console.WriteLine();
}

Console.WriteLine("Thank you for visiting our cafe!");
Console.WriteLine("Exiting Cafe Management App.");


// Helper method: read a whole number.
static int ReadNumber(string message)
{
    while (true)
    {
        Console.Write(message);

        string? input = Console.ReadLine();

        if (int.TryParse(input, out int number))
        {
            return number;
        }

        Console.WriteLine("Please enter a valid whole number.");
    }
}


// Helper method: read a number greater than zero.
static int ReadPositiveNumber(string message)
{
    while (true)
    {
        int number = ReadNumber(message);

        if (number > 0)
        {
            return number;
        }

        Console.WriteLine("Quantity must be greater than zero.");
    }
}


// Helper method: accept only Y or N.
static string ReadYesNo(string message)
{
    while (true)
    {
        Console.Write(message);

        string? input = Console.ReadLine();

        if (input == null)
        {
            return "N";
        }

        string answer = input.Trim().ToUpperInvariant();

        if (answer == "Y" || answer == "N")
        {
            return answer;
        }

        Console.WriteLine("Please enter Y or N.");
    }
}