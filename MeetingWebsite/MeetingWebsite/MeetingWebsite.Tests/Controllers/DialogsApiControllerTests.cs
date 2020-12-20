using MeetingWebsite.Controllers;
using MeetingWebsite.Models;
using MeetingWebsite.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using MockQueryable.Moq;
using MeetingWebsite.ModelsView.Dialogs;
using System.Threading.Tasks;

namespace MeetingWebsite.Tests.Controllers
{
    public class DialogsApiControllerTests
    {
        Mock<DBContext> dbContext;
        //Repos
        Mock<DialogsRepository> dialogsRepositoryMock;
        Mock<UsersRepository> usersRepositoryMock;

        //DialogsApiController
        DialogsApiController dialogsController;

        public DialogsApiControllerTests()
        {
            dbContext = new Mock<DBContext>();
            //Dialogs
            dialogsRepositoryMock = new Mock<DialogsRepository>(dbContext.Object);

            dialogsRepositoryMock.Setup(repo => repo.GetList()).Returns(new List<Dialog>().AsQueryable().BuildMock().Object);
            dialogsRepositoryMock.Setup(repo => repo.Add(It.IsAny<Dialog>())).Returns(Task.FromResult(true));
            dialogsRepositoryMock.Setup(repo => repo.Update(It.IsAny<Dialog>())).Returns(Task.FromResult(true));
            dialogsRepositoryMock.Setup(repo => repo.Remove(It.IsAny<Dialog>())).Returns(Task.FromResult(true));

            //Users
            usersRepositoryMock = new Mock<UsersRepository>(dbContext.Object);

            var users = new List<User>() {
                new User() { Id = -1 },
                new User() { Id = 1 },
                new User() { Id = 2 },
                new User() { Id = 3 },
            };
            usersRepositoryMock.Setup(repo => repo.GetList()).Returns(users.AsQueryable().BuildMock().Object);

            //Controller
            dialogsController = new DialogsApiController(dialogsRepositoryMock.Object, usersRepositoryMock.Object);
        }

        [Fact]
        public void GetListDialogs()
        {
            // Arrange

            // Act
            var test = dialogsController.Dialogs();
            var result = dialogsController.Dialogs().Result;
            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsType<List<DialogView>>(okResult.Value);
            Assert.Empty((okResult.Value as List<DialogView>));
        }

        /// <summary>
        /// Данные для теста CreateDialog
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> GetDataForTest_CreateDialog()
        {
            yield return new object[] { new List<int>() { }, 400 };
            yield return new object[] { new List<int>() { 1, 3, 4 }, 400 };
            yield return new object[] { new List<int>() { 2 }, 200 };
            yield return new object[] { new List<int>() { 1, 2 }, 200 };
        }

        /// <summary>
        /// Создание диалога
        /// </summary>
        /// <param name="usersId">Идентификаторы пользователей</param>
        /// <param name="statusCode">Возвращаемый результат</param>
        [Theory]
        [MemberData(nameof(GetDataForTest_CreateDialog))]
        public void CreateDialog(List<int> usersId, int statusCode)
        {
            // Arrange
            // Act
            var result = dialogsController.Create("Title", usersId).Result as ObjectResult;
            // Assert
            Assert.Equal(statusCode, result.StatusCode);
            if (statusCode == 200)
                Assert.IsType<int>(result.Value);
            else
            {
                Assert.IsType<string>(result.Value);
                Assert.NotEmpty(result.Value as string);
            }
        }

        /// <summary>
        /// Изменение название диалога
        /// </summary>
        [Fact]
        public void ChangeTitle()
        {
            // Arrange
            dialogsRepositoryMock.Setup(r => r.Find(It.IsAny<int>())).Returns(Task.FromResult(new Dialog() { Id = 1, Title = "Title" }));

            // Act
            var result = dialogsController.ChangeTitle(1, "Title new").Result;

            // Assert
            Assert.IsType<OkResult>(result);
        }

        /// <summary>
        /// Изменение название несуществующего диалога
        /// </summary>
        [Fact]
        public void ChangeTitleNotExistDialog()
        {
            // Arrange

            // Act
            var result = dialogsController.ChangeTitle(2, "Title new").Result;

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objResult = result as ObjectResult;
            Assert.Equal(404, objResult.StatusCode);
        }


        /// <summary>
        /// Удаление диалога
        /// </summary>
        [Fact]
        public void DeleteDialog()
        {
            // Arrange
            dialogsRepositoryMock.Setup(r => r.Find(It.IsAny<int>())).Returns(Task.FromResult(new Dialog() { Id = 1, Title = "Title", CreateUserId = -1 }));

            // Act
            var result = dialogsController.Delete(1).Result;

            // Assert
            Assert.IsType<OkResult>(result);
        }

        /// <summary>
        /// Удаление несуществующего диалога
        /// </summary>
        [Fact]
        public void DeleteNotExistDialog()
        {
            // Arrange

            // Act
            var result = dialogsController.Delete(2).Result;

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objResult = result as ObjectResult;
            Assert.Equal(404, objResult.StatusCode);
        }
        /// <summary>
        /// Удаление диалога не имея к нему прав 
        /// </summary>
        [Fact]
        public void DeleteDialog_NotRigths()
        {
            // Arrange
            dialogsRepositoryMock.Setup(r => r.Find(It.IsAny<int>())).Returns(Task.FromResult(new Dialog() { Id = 1, Title = "Title" }));

            // Act
            var result = dialogsController.Delete(1).Result;

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objResult = result as ObjectResult;
            Assert.Equal(401, objResult.StatusCode);
        }
    }
}
