using EntertainmentReviews.Models;
using EntertainmentReviews.System;
using EntertainmentReviews.Subscribers;

namespace EntertainmentReviews.Menus;

public static class MenuHelper
{
	public static UserSubscriber? LoginMenu(List<UserSubscriber> users)
	{
		Console.WriteLine("\n--- Login ---");
		for (int i = 0; i < users.Count; i++)
		{
			Console.WriteLine($"{i + 1}. {users[i].UserName}");
		}
		Console.WriteLine("0. Cancel");
		Console.Write("Select your account: ");
		
		if (int.TryParse(Console.ReadLine(), out int userChoice) && userChoice > 0 && userChoice <= users.Count)
		{
			return users[userChoice - 1];
		}
		
		ConsoleHelper.PrintError("Invalid choice or cancelled.");
		return null;
	}

	public static UserSubscriber? RegisterMenu(List<UserSubscriber> users)
	{
		Console.WriteLine("\n--- Register ---");
		Console.Write("Enter a new username: ");
		string username = Console.ReadLine() ?? "Unknown User";

		if (users.Any(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)))
		{
			ConsoleHelper.PrintError("Username already exists.");
			return null;
		}

		var newUser = new UserSubscriber(username);
		users.Add(newUser);
		DataManager.SaveUsers(users);
		Console.WriteLine($"Account '{username}' created successfully.");
		return newUser;
	}

	public static void AddReviewMenu(ReviewSystem reviewSystem, UserSubscriber currentAccount)
	{
		Console.WriteLine("\n--- Add a New Review ---");
		
		if (!reviewSystem.Items.Any())
		{
			Console.WriteLine("There are no entertainment items available to review. An admin needs to add some first.");
			return;
		}

		Console.WriteLine("Select an Item to Review:");
		for (int i = 0; i < reviewSystem.Items.Count; i++)
		{
			Console.Write($"{i + 1}. ");
			ConsoleHelper.WriteCategoryColoredText($"[{reviewSystem.Items[i].Category}]", reviewSystem.Items[i].Category);
			Console.WriteLine($" {reviewSystem.Items[i].Title}");
		}
		Console.WriteLine("0. Cancel");
		Console.Write($"Choice (0-{reviewSystem.Items.Count}): ");
		
		if (!int.TryParse(Console.ReadLine(), out int itemChoice) || itemChoice <= 0 || itemChoice > reviewSystem.Items.Count)
		{
			ConsoleHelper.PrintError("Invalid choice or cancelled.");
			return;
		}
		
		var selectedItem = reviewSystem.Items[itemChoice - 1];

		Console.Write("Enter Rating (1-10) : ");
		if (!int.TryParse(Console.ReadLine(), out int rating))
		{
			rating = 5; // Default if invalid
		}

		Console.Write("Enter Content: ");
		string content = Console.ReadLine() ?? "";

		var review = new Review(selectedItem.Title, selectedItem.Category, rating, content, currentAccount.UserName);
		reviewSystem.AddReview(review);
		DataManager.SaveReviews(reviewSystem.Reviews);
	}

	public static void AddEntertainmentItemMenu(ReviewSystem reviewSystem)
	{
		Console.WriteLine("\n--- Add a New Entertainment Item (Admin) ---");
		Console.Write("Enter Title: ");
		string title = Console.ReadLine() ?? "Unknown";

		Console.WriteLine("Select Category:");
		Console.WriteLine("1. Game");
		Console.WriteLine("2. Movie");
		Console.WriteLine("3. Music");
		Console.Write("Choice (1-3): ");
		var catChoice = Console.ReadLine();
		Category category = catChoice switch
		{
			"1" => Category.Game,
			"2" => Category.Movie,
			"3" => Category.Music,
			_ => Category.Game // Default
		};

		var existingItem = reviewSystem.Items.FirstOrDefault(i => 
			i.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && 
			i.Category == category);

		if (existingItem != null)
		{
			ConsoleHelper.PrintError("This item already exists in the system.");
			return;
		}

		var newItem = new EntertainmentItem(title, category);
		reviewSystem.AddItem(newItem);
		DataManager.SaveItems(reviewSystem.Items);
	}

	public static void ViewOverallReviewsMenu(ReviewSystem reviewSystem)
	{
		Console.WriteLine("\n--- Overall Reviews grouped by Item ---");
		
		var groupedReviews = reviewSystem.Reviews
			.GroupBy(r => new { r.Title, r.Category })
			.OrderBy(g => g.Key.Category).ThenBy(g => g.Key.Title)
			.ToList();

		if (!groupedReviews.Any())
		{
			Console.WriteLine("No reviews exist yet.");
			return;
		}

		foreach (var group in groupedReviews)
		{
			double avgRating = group.Average(r => r.Rating);
			Console.WriteLine();
			ConsoleHelper.WriteCategoryColoredText($"[{group.Key.Category}]", group.Key.Category);
			Console.WriteLine($" {group.Key.Title} (Average Rating: {avgRating:F1}/10)");
			foreach (var review in group)
			{
				Console.WriteLine($"  - {review.Rating}/10 by {review.AuthorName}: {review.Content}");
			}
		}
		
		Console.WriteLine("\nPress Enter to continue...");
		Console.ReadLine();
	}

	public static void ShowUnreviewedNotifications(ReviewSystem reviewSystem, UserSubscriber currentAccount)
	{
		// Find items reviewed by current user
		var reviewedByMe = reviewSystem.Reviews
			.Where(r => r.AuthorName.Equals(currentAccount.UserName, StringComparison.OrdinalIgnoreCase))
			.Select(r => new { r.Title, r.Category })
			.Distinct()
			.ToList();

		var unreviewedItems = reviewSystem.Items
			.Where(i => !reviewedByMe.Any(r => r.Title.Equals(i.Title, StringComparison.OrdinalIgnoreCase) && r.Category == i.Category))
			.DistinctBy(i => new { i.Title, i.Category })
			.ToList();

		if (unreviewedItems.Any())
		{
			ConsoleHelper.PrintNotification("\n[Notification] You haven't reviewed the following items in the system:");
			foreach (var item in unreviewedItems)
			{
				Console.Write(" - ");
				ConsoleHelper.WriteCategoryColoredText($"[{item.Category}]", item.Category);
				Console.WriteLine($" {item.Title}");
			}
		}
	}

	public static void ToggleSubscriptionMenu(UserSubscriber currentAccount, List<UserSubscriber> users)
	{
		currentAccount.IsSubscribed = !currentAccount.IsSubscribed;
		DataManager.SaveUsers(users);
		
		string status = currentAccount.IsSubscribed ? "ON" : "OFF";
		ConsoleHelper.PrintNotification($"\n[Notification] Review notifications are now turned {status}.");
	}
}
