using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.Repository;
using Xunit;

namespace ProjectTesting;

public class SubjectRepositoryUnitTests
{
    private readonly NoteBlendDbContext _context;
    private readonly SubjectRepository _subjectRepository;
    private readonly DbContextOptions<NoteBlendDbContext> _options;
    
    public SubjectRepositoryUnitTests()
    {
        _options = new DbContextOptionsBuilder<NoteBlendDbContext>()
            .UseInMemoryDatabase(databaseName: "SubjectDB")
            .Options;
        _context = new NoteBlendDbContext(_options);
        _subjectRepository = new SubjectRepository(_context);
    }

    [Fact]
    public void GetSubjectList_IsEmpty_ReturnEmpty()
    {
        // Arrange
        
        // Act
        List<Subject> subjectList = _subjectRepository.GetSubjectsList();
        
        // Assert
        Assert.Empty(subjectList);
    }
    
    /*

    [Fact]
    public void GetSubjectList_NotEmpty_ReturnListWithSubjects()
    {
        // Arrange
        var subject = new Subject { Id = "1", Name = "Math" };
        _context.Subjects.Add(subject);
        _context.SaveChanges();

        // Act
        List<Subject> subjectList = _subjectRepository.GetSubjectsList();

        // Assert
        Assert.NotEmpty(subjectList);
        Assert.Contains(subject, subjectList);
    }

    [Fact]
    public void GetSubject_ExistingId_ReturnsSubject()
    {
        // Arrange
        var subject = new Subject { Id = "1", Name = "Physics" };
        _context.Subjects.Add(subject);
        _context.SaveChanges();

        // Act
        Subject retrievedSubject = _subjectRepository.GetSubject("1");

        // Assert
        Assert.NotNull(retrievedSubject);
        Assert.Equal(subject, retrievedSubject);
    }

    [Fact]
    public void GetSubject_NonExistingId_ReturnsNull()
    {
        // Arrange

        // Act
        Subject retrievedSubject = _subjectRepository.GetSubject("nonexistentId");

        // Assert
        Assert.Null(retrievedSubject);
    }

    [Fact]
    public void CreateSubject_ValidSubject_ReturnsCreatedSubject()
    {
        // Arrange
        var newSubject = new Subject { Id = "2", Name = "History" };

        // Act
        Subject createdSubject = _subjectRepository.CreateSubject(newSubject);

        // Assert
        Assert.NotNull(createdSubject);
        Assert.Equal(newSubject, createdSubject);
    }

    [Fact]
    public void CreateSubject_InvalidSubject_ReturnsNull()
    {
        // Arrange
        var invalidSubject = new Subject(); // This subject is invalid as it lacks required properties.

        // Act
        Subject createdSubject = _subjectRepository.CreateSubject(invalidSubject);

        // Assert
        Assert.Null(createdSubject);
    }*/
}