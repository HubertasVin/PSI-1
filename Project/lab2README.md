Laboratory assignment 2 objectives:
- [x] Relational database is used for storing data;
- [ ] Create generic method, event or delegate; define at least 2 generic constraints;

```csharp

```

###

- [ ] Delegates usage;

```csharp

```

###

- [ ] Create at least 1 exception type and throw it; meaningfully deal with it; (most of the exceptions are logged to a file or a server);

```csharp

```

###

- [x] Lambda expressions usage;

[In Program.cs](Program.cs#L33) Line 33-42
```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
            builder
                .WithOrigins("https://localhost:44465") // Updated with your React app's URL
                .AllowAnyMethod()
                .AllowAnyHeader()
    );
});
```

###

- [ ] Usage of threading via Thread class;

```csharp

```

###

- [x] Usage of async/await;

[In ConspectsController.cs](Controllers/ConspectsController.cs#L32) Line 32-35
```csharp
public async Task<IActionResult> ShowSearchResults(string searchPhrase)
{
    return View("Index", await _context.Conspect.Where(j => j.name.Contains(searchPhrase)).ToListAsync());
}
```

###

- [ ] Use at least 1 concurrent collection or Monitor;

```csharp

```

###

- [x] Regex usage;

[In UserContents.cs](Contents/UserContents.cs#L104) Line 104
```csharp
Regex regex = new(@"[\w.+-]+@\[?[\w-]+\.[\w.-]+\]?");
```

###

- [ ] No instances are created using 'new' keyword, dependency injection is used everywhere;
- [ ] Unit and integration tests coverage at least 20%.