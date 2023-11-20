using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.Repository;

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
}