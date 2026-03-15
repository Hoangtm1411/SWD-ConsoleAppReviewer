using EntertainmentReviews.Interfaces;
using EntertainmentReviews.Models;

namespace EntertainmentReviews.System;

public class ReviewSystem : IReviewSubject
{
	private readonly List<IReviewObserver> _observers = new();
	private List<Review> _reviews = new();
	private List<EntertainmentItem> _items = new();

	public IReadOnlyList<Review> Reviews => _reviews;
	public IReadOnlyList<EntertainmentItem> Items => _items;

	public void LoadState(List<EntertainmentItem> items, List<Review> reviews)
	{
		_items = items;
		_reviews = reviews;
	}

	public void AddItem(EntertainmentItem item)
	{
		_items.Add(item);
		ConsoleHelper.PrintSystem($"\n[System] New entertainment item added: {item.Title} ({item.Category})");
	}

	public void Attach(IReviewObserver observer)
	{
		if (!_observers.Contains(observer))
		{
			_observers.Add(observer);
			ConsoleHelper.PrintSystem($"[System] An observer has been attached.");
		}
	}

	public void Detach(IReviewObserver observer)
	{
		if (_observers.Contains(observer))
		{
			_observers.Remove(observer);
			ConsoleHelper.PrintSystem($"[System] An observer has been detached.");
		}
	}

	public void Notify(Review review)
	{
		ConsoleHelper.PrintSystem($"\n[System] Notifying observers about a {review.Category} review: '{review.Title}'...");
		foreach (var observer in _observers)
		{
			observer.Update(review);
		}
	}

	public void AddReview(Review review)
	{
		var existingReview = _reviews.FirstOrDefault(r => 
			r.AuthorName.Equals(review.AuthorName, StringComparison.OrdinalIgnoreCase) && 
			r.Title.Equals(review.Title, StringComparison.OrdinalIgnoreCase) &&
			r.Category == review.Category);

		if (existingReview != null)
		{
			existingReview.Category = review.Category;
			existingReview.Rating = review.Rating;
			existingReview.Content = review.Content;
			ConsoleHelper.PrintSystem($"\n[System] Existing review updated: {review.Title}");
			Notify(existingReview);
		}
		else
		{
			_reviews.Add(review);
			ConsoleHelper.PrintSystem($"\n[System] New review added: {review.Title}");
			Notify(review);
		}
	}
}