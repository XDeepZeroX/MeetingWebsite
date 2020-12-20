using MeetingWebsite.Controllers;
using MeetingWebsite.Models;
using MeetingWebsite.Repositories;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MeetingWebsite.Tests.Controllers
{
    public class UsersControllerTests
    {
        Mock<DBContext> dbContext;
        //Repos
        Mock<UsersRepository> usersRepositoryMock;

        //DialogsApiController
        UsersApiController usersController;

        public UsersControllerTests()
        {
            dbContext = new Mock<DBContext>();
            //Users
            usersRepositoryMock = new Mock<UsersRepository>(dbContext.Object);

            var users = new List<User>() {
                new User() { Id = 100, Sex=Sex.Man, City="Волгоград" },
                new User() { Id = 1, Sex=Sex.Man, City="Волгоград" },
                new User() { Id = 2, Sex=Sex.Girl, City="Волгоград" },
                new User() { Id = 3, Sex=Sex.Man, City="Волгоград" },
            };
            usersRepositoryMock.Setup(repo => repo.GetList()).Returns(users.AsQueryable().BuildMock().Object);

            //Controller
            usersController = new UsersApiController(usersRepositoryMock.Object);
        }

        /// <summary>
        /// Получение списка сообщений диалога
        /// </summary>
        [Theory]
        [InlineData(1, 200)]
        [InlineData(1111, 404)]
        public void GetListMessages(int userId, int statusCode)
        {
            // Arrange

            // Act
            var result = usersController.UserInfo(userId).Result;

            // Assert
            if (statusCode == 200)
                Assert.IsType<OkObjectResult>(result);
            else
                Assert.IsType<ObjectResult>(result);
            Assert.Equal(statusCode, (result as ObjectResult).StatusCode);
        }

    }
}
