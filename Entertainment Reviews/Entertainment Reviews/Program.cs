using EntertainmentReviews.Models;
using EntertainmentReviews.System;
using EntertainmentReviews.Subscribers;
using EntertainmentReviews.Menus;

var users = DataManager.LoadUsers();
if (!users.Any())
{
	users = new List<UserSubscriber>
	{
		new UserSubscriber("Admin", true), // Admin account
		new UserSubscriber("Alice (Gamer)", false, new List<Category> { Category.Game }),
		new UserSubscriber("Bob (Movie Fan)", false, new List<Category> { Category.Movie }),
		new UserSubscriber("Charlie (All)", false, new List<Category> { Category.Game, Category.Movie, Category.Music })
	};
	DataManager.SaveUsers(users);
}

var items = DataManager.LoadItems();
var reviews = DataManager.LoadReviews();

if (!items.Any())
{
	items = new List<EntertainmentItem>
	{
		new EntertainmentItem("The Witcher 3", Category.Game),
		new EntertainmentItem("Inception", Category.Movie),
		new EntertainmentItem("Old Town Road", Category.Music)
	};
	DataManager.SaveItems(items);
}

var reviewSystem = new ReviewSystem();
reviewSystem.LoadState(items, reviews);

foreach (var user in users)
{
	reviewSystem.Attach(user);
}

ConsoleHelper.PrintSystem($"[System] Loaded {users.Count} users, {items.Count} items, and {reviews.Count} reviews.");

UserSubscriber? currentAccount = null;

while (true)
{
	if (currentAccount == null)
	{
		Console.WriteLine("\n=== Entertainment Reviews - Welcome ===");
		Console.WriteLine("1. Login");
		Console.WriteLine("2. Register");
		Console.WriteLine("3. Exit");
		Console.Write("Select an option (1-3): ");

		var choice = Console.ReadLine();
		switch (choice)
		{
			case "1":
				currentAccount = MenuHelper.LoginMenu(users);
				break;
			case "2":
				currentAccount = MenuHelper.RegisterMenu(users);
				break;
			case "3":
				Console.WriteLine("Exiting... Goodbye!");
				return;
			default:
				ConsoleHelper.PrintError("Invalid option. Please try again.");
				break;
		}
	}
	else
	{
		MenuHelper.ShowUnreviewedNotifications(reviewSystem, currentAccount);

		Console.WriteLine($"\n=== Entertainment Reviews - Main Menu (Logged in as: {currentAccount.UserName}) ===");
		Console.WriteLine("1. Add/Edit a Review");
		Console.WriteLine("2. View Overall Reviews");
		Console.WriteLine("3. Logout");
		
		string subStatus = currentAccount.IsSubscribed ? "ON" : "OFF";
		Console.WriteLine($"4. Toggle Notifications (Currently: {subStatus})");

		int exitOptionNumber = 5;
		if (currentAccount.IsAdmin)
		{
			Console.WriteLine("5. Manage Items (Admin)");
			Console.WriteLine("6. Exit");
			exitOptionNumber = 6;
		}
		else
		{
			Console.WriteLine("5. Exit");
		}
		
		Console.Write($"Select an option (1-{exitOptionNumber}): ");

		var choice = Console.ReadLine();

		if (currentAccount.IsAdmin)
		{
			switch (choice)
			{
				case "1":
					MenuHelper.AddReviewMenu(reviewSystem, currentAccount);
					break;
				case "2":
					MenuHelper.ViewOverallReviewsMenu(reviewSystem);
					break;
				case "3":
					Console.WriteLine($"Logging out {currentAccount.UserName}...");
					currentAccount = null;
					break;
				case "4":
					MenuHelper.ToggleSubscriptionMenu(currentAccount, users);
					break;
				case "5":
					MenuHelper.AddEntertainmentItemMenu(reviewSystem);
					break;
				case "6":
					Console.WriteLine("Exiting... Goodbye!");
					return;
				default:
					ConsoleHelper.PrintError("Invalid option. Please try again.");
					break;
			}
		}
		else
		{
			switch (choice)
			{
				case "1":
					MenuHelper.AddReviewMenu(reviewSystem, currentAccount);
					break;
				case "2":
					MenuHelper.ViewOverallReviewsMenu(reviewSystem);
					break;
				case "3":
					Console.WriteLine($"Logging out {currentAccount.UserName}...");
					currentAccount = null;
					break;
				case "4":
					MenuHelper.ToggleSubscriptionMenu(currentAccount, users);
					break;
				case "5":
					Console.WriteLine("Exiting... Goodbye!");
					return;
				default:
					ConsoleHelper.PrintError("Invalid option. Please try again.");
					break;
			}
		}
	}
}
