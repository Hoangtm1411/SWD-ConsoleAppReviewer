namespace EntertainmentReviews.Models;

public class Review
{
	public string Title { get; set; }
	public Category Category { get; set; }
	public int Rating { get; set; }
	public string Content { get; set; }
	public string AuthorName { get; set; }

	public Review(string title, Category category, int rating, string content, string authorName)
	{
		Title = title;
		Category = category;
		Rating = rating;
		Content = content;
		AuthorName = authorName;
	}

	public override string ToString()
	{
		return $"[{Category}] {Title} - {Rating}/10 by {AuthorName}: {Content}";
	}
}
