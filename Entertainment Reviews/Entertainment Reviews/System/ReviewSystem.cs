using EntertainmentReviews.Interfaces;
using EntertainmentReviews.Models;

namespace EntertainmentReviews.System;

public class ReviewSystem : IReviewSubject
{
    private readonly List<IReviewObserver> _observers = new();
    private readonly List<Review> _reviews = new();

    public void Attach(IReviewObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            Console.WriteLine($"[System] An observer has been attached.");
        }
    }

    public void Detach(IReviewObserver observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
            Console.WriteLine($"[System] An observer has been detached.");
        }
    }

    public void Notify(Review review)
    {
        Console.WriteLine($"\n[System] Notifying observers about a new {review.Category} review: '{review.Title}'...");
        foreach (var observer in _observers)
        {
            observer.Update(review);
        }
    }

    public void AddReview(Review review)
    {
        _reviews.Add(review);
        Console.WriteLine($"\n[System] New review added: {review.Title}");
        Notify(review);
    }
}
