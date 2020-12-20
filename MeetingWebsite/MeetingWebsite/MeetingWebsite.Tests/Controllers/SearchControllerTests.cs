using MeetingWebsite.Controllers;
using MeetingWebsite.Models;
using MeetingWebsite.ModelsView.UserModels;
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
    public class SearchControllerTests
    {
        Mock<DBContext> dbContext;
        //Repos
        Mock<UsersRepository> usersRepositoryMock;

        //DialogsApiController
        SearchController searchController;

        public SearchControllerTests()
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
            searchController = new SearchController(usersRepositoryMock.Object);
        }

        /// <summary>
        /// Получение списка сообщений диалога
        /// </summary>
        [Theory]
        [InlineData("Волг", Sex.Girl, 1)]
        [InlineData("Волг", Sex.Man, 3)]
        [InlineData("Не известный город", Sex.Man, 0)]
        public void GetListMessages(string city, Sex sex, int countResult)
        {
            // Arrange

            // Act
            var result = searchController.SerachUsers(city, sex).Result;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsType<List<UserView>>(okResult.Value);
            Assert.Equal(countResult, (okResult.Value as List<UserView>).Count);
        }

    }
}
