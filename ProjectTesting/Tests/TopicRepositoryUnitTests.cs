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
    public class TopicRepositoryUnitTests : IDisposable
    {
        private readonly DbContextOptions<NoteBlendDbContext> _options;
        private readonly NoteBlendDbContext _dbContext;

        public TopicRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<NoteBlendDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new NoteBlendDbContext(_options);
            _dbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }


        [Fact]
        public void GetTopicsList_ReturnsCorrectTopics()
        {
            // Arrange
            var subject = new Subject("Math");
            var topicRepository = new TopicRepository(_dbContext);
            _dbContext.Subjects.Add(subject);
            _dbContext.SaveChanges();

            var topic1 = new Topic("Algebra", subject);
            var topic2 = new Topic("Geometry", subject);
            topicRepository.Add(topic1);
            topicRepository.Add(topic2);
            _dbContext.SaveChanges();

            // Act
            var result = topicRepository.GetTopicsList(subject.id);

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
            _dbContext.Subjects.Add(subject);
            _dbContext.SaveChanges();

            var topicRepository = new TopicRepository(_dbContext);
            var jsonElement = JsonDocument.Parse("{\"topicName\":\"Mechanics\",\"subjectId\":\"" + subject.id + "\"}").RootElement;

            // Act
            var result = topicRepository.CreateTopic(jsonElement);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Mechanics", result.Name);
            Assert.Equal(subject.id, result.Subject.id);
        }
    }
}