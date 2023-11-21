using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Project.Data;
using Project.Exceptions;
using Project.Models;
using Project.Repository;
using Xunit;

namespace ProjectTesting.Tests
{
    public class UserRepositoryUnitTests
    {
        [Fact]   ////CIA BISKI KAZKA BANDZIAU, BET JEI TIE TESTAI FAILINA NE DEL KODO O DEL MANO KLAIDOS TAI SORRY BISKI NZN KA DARAU
        public void GetUserByEmail_ValidEmail_ReturnsUser()
        {
            // Arrange
            var userEmail = "test@example.com";
            var mockContext = GetMockContext();
            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = userRepository.GetUserByEmail(userEmail);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userEmail, result?.Email);
        }

        [Fact]
        public void GetUserByEmail_InvalidEmail_ReturnsNull()
        {
            // Arrange
            var userEmail = "nonexistent@example.com";
            var mockContext = GetMockContext();
            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = userRepository.GetUserByEmail(userEmail);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateUser_ValidUser_ReturnsCreatedUser()
        {
            // Arrange
            var newUser = new User("John", "Doe", "newuser@example.com", "password123");
            var mockContext = GetMockContext();
            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = userRepository.CreateUser(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newUser.Email, result?.Email);
        }

        // Helper method to create a mock DbContext
        private static Mock<NoteBlendDbContext> GetMockContext()
        {
            // Use the DbContextOptionsBuilder to create an in-memory database
            var options = new DbContextOptionsBuilder<NoteBlendDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Create a mock DbContext
            var mockContext = new Mock<NoteBlendDbContext>(options);

            // Setup any specific DbSet interactions if needed...

            return mockContext;
        }
    }
}
