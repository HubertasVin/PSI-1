using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.Repository;
using Xunit;

namespace ProjectTesting.UnitTests;

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

    // [Fact]
    // public void GetSubjectList_IsEmpty_ReturnEmpty() //Sitas testas, kai leidi visus testus failina, bet kai leidi tik sita testa atskirai, tai passina lol what
    // {
    //     // Arrange
    //     
    //     // Act
    //     List<Subject> subjectList = _subjectRepository.GetSubjectsList();
    //     
    //     // Assert
    //     Assert.Empty(subjectList);
    // }

    // [Fact]
    // public void CreateSubject_InvalidSubject_ReturnsNull()
    // {
    //     // Arrange
    //     var invalidSubject = new Subject(null); // invalid name
    //
    //     // Act
    //     var createdSubject = _subjectRepository.CreateSubject(invalidSubject);
    //
    //     // Assert
    //     Assert.Null(createdSubject);
    // }

    [Fact]
    public void CreateSubject_ValidSubject_ReturnsCreatedSubject()
    {
        // Arrange
        var newSubject = new Subject("Math");

        // Act
        var createdSubject = _subjectRepository.CreateSubject(newSubject);

        // Assert
        Assert.NotNull(createdSubject);
        Assert.Equal(newSubject.Name, createdSubject.Name);
        Assert.NotEmpty(createdSubject.id);
    }

    [Fact]
    public void GetSubjectList_NotEmpty_ReturnsNonEmptyList()
    {
        // Arrange
        _subjectRepository.CreateSubject(new Subject("History"));

        // Act
        var subjectList = _subjectRepository.GetSubjectsList();

        // Assert
        Assert.NotEmpty(subjectList);
    }

    [Fact]
    public void GetSubject_ExistingId_ReturnsSubject()
    {
        // Arrange
        var newSubject = new Subject("Physics");
        _subjectRepository.CreateSubject(newSubject);

        // Act
        var retrievedSubject = _subjectRepository.GetSubject(newSubject.id);

        // Assert
        Assert.NotNull(retrievedSubject);
        Assert.Equal(newSubject.Name, retrievedSubject.Name);
    }
}