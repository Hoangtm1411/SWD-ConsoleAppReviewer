# System Class Diagram

This class diagram explains the data structures and relationships within the `SWD-ConsoleAppReviewer` system. It visualizes the core models, the [ReviewSystem](file:///d:/University%20stuffs/Coding/SWD/SWD-ConsoleAppReviewer/Entertainment%20Reviews/Entertainment%20Reviews/System/ReviewSystem.cs#6-77) that manages them, and the [DataManager](file:///d:/University%20stuffs/Coding/SWD/SWD-ConsoleAppReviewer/Entertainment%20Reviews/Entertainment%20Reviews/System/DataManager.cs#7-54) responsible for persistence.

```mermaid
classDiagram
    class Category {
        <<enumeration>>
        Game
        Movie
        Music
    }

    class EntertainmentItem {
        +string Title
        +Category Category
        +EntertainmentItem(title: string, category: Category)
        +ToString() string
    }

    class Review {
        +string Title
        +Category Category
        +int Rating
        +string Content
        +string AuthorName
        +Review(title: string, category: Category, rating: int, content: string, authorName: string)
        +ToString() string
    }

    class UserSubscriber {
        +string UserName
        +bool IsAdmin
        +bool IsSubscribed
        +List~Category~ InterestedCategories
        +UserSubscriber()
        +UserSubscriber(userName: string, isAdmin: bool, interestedCategories: List~Category~)
        +Update(review: Review) void
    }

    class DataManager {
        <<static>>
        -string UsersFile
        -string ItemsFile
        -string ReviewsFile
        -JsonSerializerOptions Options
        +SaveUsers(users: IEnumerable~UserSubscriber~) void
        +LoadUsers() List~UserSubscriber~
        +SaveItems(items: IEnumerable~EntertainmentItem~) void
        +LoadItems() List~EntertainmentItem~
        +SaveReviews(reviews: IEnumerable~Review~) void
        +LoadReviews() List~Review~
    }

    class ReviewSystem {
        -List~IReviewObserver~ _observers
        -List~Review~ _reviews
        -List~EntertainmentItem~ _items
        +IReadOnlyList~Review~ Reviews
        +IReadOnlyList~EntertainmentItem~ Items
        +LoadState(items: List~EntertainmentItem~, reviews: List~Review~) void
        +AddItem(item: EntertainmentItem) void
        +Attach(observer: IReviewObserver) void
        +Detach(observer: IReviewObserver) void
        +Notify(review: Review) void
        +AddReview(review: Review) void
    }

    class IReviewObserver {
        <<interface>>
        +Update(review: Review) void
    }

    class IReviewSubject {
        <<interface>>
        +Attach(observer: IReviewObserver) void
        +Detach(observer: IReviewObserver) void
        +Notify(review: Review) void
    }

    %% Relationships
    UserSubscriber ..|> IReviewObserver : implements
    ReviewSystem ..|> IReviewSubject : implements

    IReviewSubject --> IReviewObserver : notifies
    
    ReviewSystem "1" *-- "*" Review : manages
    ReviewSystem "1" *-- "*" EntertainmentItem : manages
    ReviewSystem "1" o-- "*" IReviewObserver : observes
    
    EntertainmentItem --> Category : has
    Review --> Category : has
    UserSubscriber --> Category : interested in

    DataManager ..> UserSubscriber : loads/saves
    DataManager ..> EntertainmentItem : loads/saves
    DataManager ..> Review : loads/saves
```

## Key Components
- **Models ([EntertainmentItem](file:///d:/University%20stuffs/Coding/SWD/SWD-ConsoleAppReviewer/Entertainment%20Reviews/Entertainment%20Reviews/Models/EntertainmentItem.cs#3-17), [Review](file:///d:/University%20stuffs/Coding/SWD/SWD-ConsoleAppReviewer/Entertainment%20Reviews/Entertainment%20Reviews/Models/Review.cs#3-25), `Category`)**: Represent the core data objects. Reviews and Items are categorized by the `Category` enum.
- **[UserSubscriber](file:///d:/University%20stuffs/Coding/SWD/SWD-ConsoleAppReviewer/Entertainment%20Reviews/Entertainment%20Reviews/Subscribers/UserSubscriber.cs#14-16)**: Represents a user. Implements `IReviewObserver` to receive notifications when new reviews are posted on categories they're interested in.
- **[ReviewSystem](file:///d:/University%20stuffs/Coding/SWD/SWD-ConsoleAppReviewer/Entertainment%20Reviews/Entertainment%20Reviews/System/ReviewSystem.cs#6-77)**: The central state manager. It acts as the `IReviewSubject` (Publisher in the Observer pattern). It holds the lists of reviews, items, and subscribed observers, handling the core business logic during runtime.
- **[DataManager](file:///d:/University%20stuffs/Coding/SWD/SWD-ConsoleAppReviewer/Entertainment%20Reviews/Entertainment%20Reviews/System/DataManager.cs#7-54)**: A static utility class used for serializing and deserializing the system's data ([Users](file:///d:/University%20stuffs/Coding/SWD/SWD-ConsoleAppReviewer/Entertainment%20Reviews/Entertainment%20Reviews/System/DataManager.cs#21-27), [Items](file:///d:/University%20stuffs/Coding/SWD/SWD-ConsoleAppReviewer/Entertainment%20Reviews/Entertainment%20Reviews/System/DataManager.cs#34-40), [Reviews](file:///d:/University%20stuffs/Coding/SWD/SWD-ConsoleAppReviewer/Entertainment%20Reviews/Entertainment%20Reviews/System/DataManager.cs#47-53)) to and from JSON files.
