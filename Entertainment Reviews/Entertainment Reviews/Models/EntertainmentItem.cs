namespace EntertainmentReviews.Models;

public class EntertainmentItem
{
	public string Title { get; set; }
	public Category Category { get; set; }

	public EntertainmentItem(string title, Category category)
	{
		Title = title;
		Category = category;
	}

	public override string ToString()
	{
		return $"[{Category}] {Title}";
	}
}
