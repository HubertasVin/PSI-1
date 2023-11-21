using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ProjectTesting.Tests
{
    public class TopicRepositoryUnitTests
    {
        private readonly DbContextOptions<NoteBlendDbContext> _options;
        private readonly NoteBlendDbContext _dbContext;
        private readonly TopicRepository _topicRepository;
        private readonly SubjectRepository _subjectRepository;

        public TopicRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<NoteBlendDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new NoteBlendDbContext(_options);
            _topicRepository = new TopicRepository(_dbContext);
            _subjectRepository = new SubjectRepository(_dbContext);
        }
        
        [Fact]
        public void GetTopicsList_ReturnsCorrectTopics()
        {
            // Arrange
            var subject = new Subject("Math");
            _subjectRepository.Add(subject);

            var topic1 = new Topic("Algebra", subject);
            var topic2 = new Topic("Geometry", subject);
            _topicRepository.Add(topic1);
            _topicRepository.Add(topic2);
            _dbContext.SaveChanges();

            // Act
            var result = _topicRepository.GetTopicsList(subject.id);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.Name == "Algebra");
            Assert.Contains(result, t => t.Name == "Geometry");
        }

        [Fact]
        public void CreateTopic_CreatesNewTopic()
        {
            // Arrange
            var subject = new Subject("Physics");
            _subjectRepository.Add(subject);
            _dbContext.SaveChanges();
            
            var jsonElement = JsonDocument.Parse("{\"topicName\":\"Mechanics\",\"subjectId\":\"" + subject.id + "\"}").RootElement;

            // Act
            var result = _topicRepository.CreateTopic(jsonElement);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Mechanics", result.Name);
            Assert.Equal(subject.id, result.Subject.id);
        }
    }
}