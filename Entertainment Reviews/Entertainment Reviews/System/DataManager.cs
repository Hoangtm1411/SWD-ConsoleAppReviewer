using System.Text.Json;
using EntertainmentReviews.Models;
using EntertainmentReviews.Subscribers;

namespace EntertainmentReviews.System;

public static class DataManager
{
	private static readonly string UsersFile = "users.dat";
	private static readonly string ItemsFile = "items.dat";
	private static readonly string ReviewsFile = "reviews.dat";

	private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

	public static void SaveUsers(IEnumerable<UserSubscriber> users)
	{
		var json = JsonSerializer.Serialize(users, Options);
		File.WriteAllText(UsersFile, json);
	}

	public static List<UserSubscriber> LoadUsers()
	{
		if (!File.Exists(UsersFile)) return new List<UserSubscriber>();
		var json = File.ReadAllText(UsersFile);
		return JsonSerializer.Deserialize<List<UserSubscriber>>(json, Options) ?? new List<UserSubscriber>();
	}

	public static void SaveItems(IEnumerable<EntertainmentItem> items)
	{
		var json = JsonSerializer.Serialize(items, Options);
		File.WriteAllText(ItemsFile, json);
	}

	public static List<EntertainmentItem> LoadItems()
	{
		if (!File.Exists(ItemsFile)) return new List<EntertainmentItem>();
		var json = File.ReadAllText(ItemsFile);
		return JsonSerializer.Deserialize<List<EntertainmentItem>>(json, Options) ?? new List<EntertainmentItem>();
	}

	public static void SaveReviews(IEnumerable<Review> reviews)
	{
		var json = JsonSerializer.Serialize(reviews, Options);
		File.WriteAllText(ReviewsFile, json);
	}

	public static List<Review> LoadReviews()
	{
		if (!File.Exists(ReviewsFile)) return new List<Review>();
		var json = File.ReadAllText(ReviewsFile);
		return JsonSerializer.Deserialize<List<Review>>(json, Options) ?? new List<Review>();
	}
}
