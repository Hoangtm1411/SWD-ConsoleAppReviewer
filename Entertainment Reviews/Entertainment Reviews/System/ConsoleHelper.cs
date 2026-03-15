using EntertainmentReviews.Models;

namespace EntertainmentReviews.System;

public static class ConsoleHelper
{
	public static void PrintSystem(string message)
	{
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(message);
		Console.ResetColor();
	}

	public static void PrintNotification(string message)
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine(message);
		Console.ResetColor();
	}

	public static void PrintError(string message)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(message);
		Console.ResetColor();
	}

	public static void WriteCategoryColoredMessage(string message, Category category)
	{
		ConsoleColor tempColor = Console.ForegroundColor;
		Console.ForegroundColor = category switch
		{
			Category.Game => ConsoleColor.Cyan,
			Category.Movie => ConsoleColor.Magenta,
			Category.Music => ConsoleColor.Green,
			_ => ConsoleColor.White
		};
		Console.WriteLine(message);
		Console.ForegroundColor = tempColor;
	}

	public static void WriteCategoryColoredText(string message, Category category)
	{
		ConsoleColor tempColor = Console.ForegroundColor;
		Console.ForegroundColor = category switch
		{
			Category.Game => ConsoleColor.Cyan,
			Category.Movie => ConsoleColor.Magenta,
			Category.Music => ConsoleColor.Green,
			_ => ConsoleColor.White
		};
		Console.Write(message);
		Console.ForegroundColor = tempColor;
	}
}
