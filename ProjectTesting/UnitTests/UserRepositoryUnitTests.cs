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

namespace ProjectTesting.UnitTests
{
    public class UserRepositoryUnitTests
    {
        
        private readonly NoteBlendDbContext _context;
        private readonly UserRepository _userRepository;
        private readonly DbContextOptions<NoteBlendDbContext> _options;
    
        public UserRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<NoteBlendDbContext>()
                .UseInMemoryDatabase(databaseName: "UserDB")
                .Options;
            _context = new NoteBlendDbContext(_options);
            _userRepository = new UserRepository(_context);
            
            //adding user for first test
            var user = new User("Johnny", "Bonnie", "test@testing.com", "password");
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        
        [Fact]
        public void CreateUser_ValidUser_ReturnsCreatedUser()
        {
            // Arrange
            var newUser = new User("John", "Doe", "newuser@example.com", "password123");

            // Act
            var result = _userRepository.CreateUser(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newUser.Email, result?.Email);
        }
        
        [Fact]   ////CIA BISKI KAZKA BANDZIAU, BET JEI TIE TESTAI FAILINA NE DEL KODO O DEL MANO KLAIDOS TAI SORRY BISKI NZN KA DARAU
        public void GetUserByEmail_ValidEmail_ReturnsUser()
        {
            // Arrange
            var userEmail = "test@testing.com";

            // Act
            var result = _userRepository.GetUserByEmail(userEmail);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userEmail, result?.Email);
        }

        [Fact]
        public void GetUserByEmail_InvalidEmail_ReturnsNull()
        {
            // Arrange
            var userEmail = "nonexistent@example.com";

            // Act
            var result = _userRepository.GetUserByEmail(userEmail);

            // Assert
            Assert.Null(result);
        }
    }
}
