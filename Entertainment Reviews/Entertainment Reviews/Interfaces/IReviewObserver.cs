using EntertainmentReviews.Models;

namespace EntertainmentReviews.Interfaces;

public interface IReviewObserver
{
	void Update(Review review);
}
