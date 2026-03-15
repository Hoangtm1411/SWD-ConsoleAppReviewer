using EntertainmentReviews.Interfaces;
using EntertainmentReviews.Models;

namespace EntertainmentReviews.Subscribers;

public class UserSubscriber : IReviewObserver
{
    public string UserName { get; private set; }
    private readonly List<Category> _interestedCategories;

    public UserSubscriber(string userName, List<Category>? interestedCategories = null)
    {
        UserName = userName;
        _interestedCategories = interestedCategories ?? Enum.GetValues<Category>().ToList();
    }

    public void Update(Review review)
    {
        if (_interestedCategories.Contains(review.Category))
        {
            Console.WriteLine($"--> [{UserName}] received a new {review.Category} review: {review}");
        }
    }
}
