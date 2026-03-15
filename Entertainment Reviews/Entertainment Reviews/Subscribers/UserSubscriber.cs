using EntertainmentReviews.Interfaces;
using EntertainmentReviews.Models;
using EntertainmentReviews.System;

namespace EntertainmentReviews.Subscribers;

public class UserSubscriber : IReviewObserver
{
	public string UserName { get; set; }
	public bool IsAdmin { get; set; }
	public bool IsSubscribed { get; set; } = true;
	public List<Category> InterestedCategories { get; set; }

	// Parameterless constructor needed for JSON deserialization
	public UserSubscriber() { }

	public UserSubscriber(string userName, bool isAdmin = false, List<Category>? interestedCategories = null)
	{
		UserName = userName;
		IsAdmin = isAdmin;
		InterestedCategories = interestedCategories ?? Enum.GetValues<Category>().ToList();
	}

	public void Update(Review review)
	{
		if (!IsSubscribed) return;

		if (InterestedCategories.Contains(review.Category))
		{
			Console.Write($"--> [{UserName}] received a new ");
			ConsoleHelper.WriteCategoryColoredText($"{review.Category}", review.Category);
			Console.WriteLine($" review: {review}");
		}
	}
}
