Laboratory assignment 1 objectives:
- [x] Create a repository on GitHub
- [x] Creating and using your own class, struct, record and enum ||| struct and record in ChatModel.cs Line: 8; enum in DataFilterController.cs line: 19

Struct and record usage in ChatModel.cs
```csharp
public record struct MessageData : IComparable<MessageData> 
{

    public string User { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
    ...
}
```

Enumerator usage in DataFilterController.cs
```csharp
public enum SortType
{
    Ascending,
    Descending
}
```

###

- [x] Property usage in struct and class
- [x] Named and optional argument usage ||| named is in ; *optional is in DataFilterController.cs line: 34*

Named argument usage in ChatHub.cs
```csharp
public async Task SendMessage(string user, string message)
{
    ...
    AddJSONMessage(path: path, messageData: chatMessage, messages: chatMessages);
    await ...
}
```

Optional argument usage in DataFilterController.cs
```csharp
public IActionResult GetFilteredData(
    string searchMessage = null,
    string searchUsername = null,
    DateTime? searchDate = null,
    SortType sortType = SortType.Ascending)
{
    ...
}
```

###

- [ ] Extension method usage
- [x] Iterating through collection the right way

Proper iteration through a collection used in ChatHub.cs
```csharp
public async Task LoadMessage()
{
    ...
    foreach (MessageData msg in chatMessages)
    {
        ...
    }
    ...
}
```

###

- [x] Reading from a file using a stream

JSONParser.cs
```csharp
public static List<T> ReadFromJSON<T>(string filePath)
{
    ...
    using (FileStream file = File.OpenRead(filePath))
    {
        items = System.Text.Json.JsonSerializer.Deserialize<List<T>>(file)!;
    }
    return items;
}
```
###

- [x] Create and use at least 1 generic type
  Method with generic type is used in JSONParser.cs
```csharp
public static List<T> ReadFromJSON<T>(string filePath)
{
    ...
}
```

###

- [ ] Boxing and unboxing (most likely going to be unused)
- [x] LINQ to Objects usage (methods or queries)
```csharp
public IActionResult GetFilteredData(
    string searchMessage = null,
    string searchUsername = null,
    DateTime? searchDate = null,
    SortType sortType = SortType.Ascending)
{
    var filteredMessages = chatMessages;

    // Filter by message content
    if (!string.IsNullOrEmpty(searchMessage))
    {
        filteredMessages = filteredMessages
            .Where(m => m.Message.Contains(searchMessage))
            .ToList();
    }

    // Filter by username
    if (!string.IsNullOrEmpty(searchUsername))
    {
        ...
    }

    // Filter by date
    if (searchDate.HasValue)
    {
        ...
    }

    ...
}
```

###

- [x] Implement at least one of the standard .NET interfaces (IEnumerable, IComparable, IComparer, IEquatable, IEnumerator, etc.)

ChatModel.cs
```csharp
public record struct MessageData : IComparable<MessageData>
{
    ...
    public int CompareTo(MessageData other) // Descending order
    {
        return other.Timestamp.CompareTo(this.Timestamp);
    }
}
```