using EntertainmentReviews.Models;
using EntertainmentReviews.System;
using EntertainmentReviews.Subscribers;

Console.WriteLine("=== Entertainment Reviews - Observer Pattern Demo ===\n");

// 1. Create the Subject (Publisher)
var reviewSystem = new ReviewSystem();

// 2. Create Observers (Subscribers)
var gamerAlice = new UserSubscriber("Alice (Gamer)", new List<Category> { Category.Game });
var cinephileBob = new UserSubscriber("Bob (Movie Fan)", new List<Category> { Category.Movie });
var allRounderCharlie = new UserSubscriber("Charlie (All)", new List<Category> { Category.Game, Category.Movie, Category.Music });

// 3. Attach Observers
reviewSystem.Attach(gamerAlice);
reviewSystem.Attach(cinephileBob);
reviewSystem.Attach(allRounderCharlie);

// 4. Add Reviews and trigger notifications
var review1 = new Review("The Witcher 3", Category.Game, 10, "A masterpiece of storytelling and RPG elements.");
reviewSystem.AddReview(review1);

var review2 = new Review("Inception", Category.Movie, 9, "Mind-bending plot and stunning visuals.");
reviewSystem.AddReview(review2);

var review3 = new Review("Daft Punk - Discovery", Category.Music, 10, "A legendary electronic music album.");
reviewSystem.AddReview(review3);

Console.WriteLine("\n[System] Bob is no longer interested in updates.");
reviewSystem.Detach(cinephileBob);

var review4 = new Review("The Matrix", Category.Movie, 8, "Classic sci-fi action.");
reviewSystem.AddReview(review4);

Console.WriteLine("\n=== Demo Finished ===");
