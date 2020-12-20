using MeetingWebsite.Controllers;
using MeetingWebsite.Models;
using MeetingWebsite.ModelsView.Messages;
using MeetingWebsite.Repositories;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MeetingWebsite.Tests.Controllers
{
    public class MessagesControllerTests
    {
        Mock<DBContext> dbContext;
        //Repos
        Mock<DialogsRepository> dialogsRepositoryMock;
        Mock<UsersRepository> usersRepositoryMock;
        Mock<MessagesRepository> messagesRepositoryMock;

        //DialogsApiController
        MessagesController messagesController;

        public MessagesControllerTests()
        {
            dbContext = new Mock<DBContext>();
            //Dialogs
            dialogsRepositoryMock = new Mock<DialogsRepository>(dbContext.Object);

            var dialogs = new List<Dialog>()
            {
                new Dialog()
                {
                     Id = 1,
                     DialogsUsers = new List<DialogsUsers>()
                     {
                         new DialogsUsers()
                         {
                              DialogId = 1,
                               UserId = -1
                         }
                     }
                }
            };
            dialogsRepositoryMock.Setup(repo => repo.GetList()).Returns(dialogs.AsQueryable().BuildMock().Object);
            dialogsRepositoryMock.Setup(repo => repo.Add(It.IsAny<Dialog>())).Returns(Task.FromResult(true));
            dialogsRepositoryMock.Setup(repo => repo.Update(It.IsAny<Dialog>())).Returns(Task.FromResult(true));
            dialogsRepositoryMock.Setup(repo => repo.Remove(It.IsAny<Dialog>())).Returns(Task.FromResult(true));
            dialogsRepositoryMock.Setup(repo => repo.Any(p => p.Id == 1)).Returns(Task.FromResult(true));

            //Users
            usersRepositoryMock = new Mock<UsersRepository>(dbContext.Object);

            var users = new List<User>() {
                new User() { Id = -1 },
                new User() { Id = 1 },
                new User() { Id = 2 },
                new User() { Id = 3 },
            };
            usersRepositoryMock.Setup(repo => repo.GetList()).Returns(users.AsQueryable().BuildMock().Object);

            //Messages 
            messagesRepositoryMock = new Mock<MessagesRepository>(dbContext.Object);
            messagesRepositoryMock.Setup(repo => repo.Add(It.IsAny<Message>())).Returns(Task.FromResult(true));
            messagesRepositoryMock.Setup(repo => repo.Update(It.IsAny<Message>())).Returns(Task.FromResult(true));
            messagesRepositoryMock.Setup(repo => repo.Remove(It.IsAny<Message>())).Returns(Task.FromResult(true));


            //Controller
            messagesController = new MessagesController(usersRepositoryMock.Object, messagesRepositoryMock.Object, dialogsRepositoryMock.Object);
        }

        /// <summary>
        /// Получение списка сообщений диалога
        /// </summary>
        [Fact]
        public void GetListMessages()
        {
            // Arrange

            // Act
            var result = messagesController.GetList(1).Result;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsType<List<MessageView>>(okResult.Value);
            Assert.Empty((okResult.Value as List<MessageView>));
        }

        /// <summary>
        /// Создание сообщения
        /// </summary>
        /// <param name="usersId">Идентификаторы пользователей</param>
        /// <param name="statusCode">Возвращаемый результат</param>
        [Theory]
        [InlineData(1, 200)]
        [InlineData(1000, 404)]
        public void CreateMessage(int dialogId, int statusCode)
        {
            // Arrange
            // Act
            var result = messagesController.Create(dialogId, "Message body").Result as ObjectResult;
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
        /// Изменение сообщения
        /// </summary>
        [Theory]
        [InlineData(1, 200, -1)]
        [InlineData(2, 404, -1)]
        [InlineData(1, 401, 1)]
        public void ChangeMessage(int messageId, int statusCode, int messageUserId)
        {
            // Arrange
            messagesRepositoryMock.Setup(r => r.Find(1)).Returns(Task.FromResult(new Message() { Id = 1, Text = "Old text", UserId = messageUserId, DateCreate = DateTime.Now }));

            // Act
            var result = messagesController.Update(0, messageId, "new body message").Result;

            // Assert
            if (statusCode == 200)
                Assert.IsType<OkResult>(result);
            else
            {
                Assert.NotNull(result);
                Assert.Equal(statusCode, (result as ObjectResult).StatusCode);
            }
        }

        /// <summary>
        /// Удаление сообщения
        /// </summary>
        [Theory]
        [InlineData(1, 401, 1)]
        [InlineData(1, 200, -1)]
        [InlineData(2, 200, -1)]
        public void DeleteMessage(int messageId, int statusCode, int messageUserId)
        {
            // Arrange
            messagesRepositoryMock.Setup(r => r.Find(1)).Returns(Task.FromResult(new Message() { Id = 1, Text = "Old text", UserId = messageUserId, DateCreate = DateTime.Now }));

            // Act
            var result = messagesController.Delete(1, 1).Result;

            // Assert
            if (statusCode == 200)
                Assert.IsType<OkResult>(result);
            else
            {
                Assert.NotNull(result);
                Assert.Equal(statusCode, (result as ObjectResult).StatusCode);
            }
        }
    }
}
