using EntertainmentReviews.Models;

namespace EntertainmentReviews.Interfaces;

public interface IReviewSubject
{
	void Attach(IReviewObserver observer);
	void Detach(IReviewObserver observer);
	void Notify(Review review);
}
