using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Npgsql;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Controllers;
using WebApplication4.Models;
using System.Web.Routing;

using System.Data;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Grpc.Core;

namespace WebApplication4.Controllers.Tests
{
    public class NpgsqlConnectionWrapper : MockDatabaseConnection
    {
        private readonly NpgsqlConnection _connection;

        public NpgsqlConnectionWrapper(string connectionString)
        {
            var connstring = "Host=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;password=Ffgtte??;";
            _connection = new NpgsqlConnection(connstring);
        }

        public void Open()
        {
            _connection.Open();
        }

        public IDbCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }
    }

    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void SignUp_Valid()
        {
            var mockConnection = new Mock<MockDatabaseConnection>();
            mockConnection.Setup(c => c.Open());
            mockConnection.Setup(c => c.CreateCommand()).Returns(() =>
            {
                var mockCommand = new Mock<IDbCommand>();
                mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(1);
                mockCommand.Setup(c => c.Parameters.Add(new NpgsqlParameter("email", It.IsAny<string>())));
                mockCommand.Setup(c => c.Parameters.Add(new NpgsqlParameter("password", It.IsAny<string>())));
                mockCommand.Setup(c => c.Parameters.Add(new NpgsqlParameter("name", It.IsAny<string>())));
                mockCommand.Setup(c => c.Parameters.Add(new NpgsqlParameter("username", It.IsAny<string>())));
                return mockCommand.Object;
            });

            var userModel = new UserModel
            {
                email = "ethanjm001@gmail.com",
                password = "password1",
                name = "Ethan",
                username = "ethanm44"

            };
            var signupcontroller = new UserController(mockConnection.Object);
            var request = new Mock<HttpRequestBase>();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            signupcontroller.ControllerContext = new ControllerContext { HttpContext = context.Object };


            var result = signupcontroller.SignUp(userModel) as ViewResult;

            Assert.IsNotNull(result);
            
        }

        [TestMethod()]
        public void AddToReviewTest()
        {
            var mockConnection = new Mock<MockDatabaseConnection>();
            mockConnection.Setup(c => c.Open());
            mockConnection.Setup(c => c.CreateCommand()).Returns(() =>
            {
                var mockCommand = new Mock<IDbCommand>();
                mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(1);
              

                return mockCommand.Object;
            });

            var httpContext = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            httpContext.Setup(x => x.Session).Returns(session.Object);
            var reviewcontroller = new UserController(mockConnection.Object) { ControllerContext = new ControllerContext { HttpContext = httpContext.Object } };
            var request = new Mock<HttpRequestBase>();
            var context = new Mock<HttpContextBase>();
            session.Setup(s => s["username"]).Returns("testuser");
            reviewcontroller.Session["username"] = "testuser";


           
            var result = reviewcontroller.AddToReview("testtrack", "testreview") as JsonResult;
            string json = new JavaScriptSerializer().Serialize(result.Data).Replace("\"", string.Empty); ;
            
            Assert.IsNotNull(result);
            Assert.AreEqual("{result:ok}", json);
            Debug.WriteLine("Json: " + json);


        }

        [TestMethod()]
        public void AddToRatingTest()
        {
            var mockConnection = new Mock<MockDatabaseConnection>();
            mockConnection.Setup(c => c.Open());
            mockConnection.Setup(c => c.CreateCommand()).Returns(() =>
            {
                var mockCommand = new Mock<IDbCommand>();
                mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(1);
               


                return mockCommand.Object;
            });

            var httpContext = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            httpContext.Setup(x => x.Session).Returns(session.Object);
            var ratecontroller = new UserController(mockConnection.Object) { ControllerContext = new ControllerContext { HttpContext = httpContext.Object } };
            var request = new Mock<HttpRequestBase>();
            var context = new Mock<HttpContextBase>();
            session.Setup(s => s["username"]).Returns("testuser");
            ratecontroller.Session["username"] = "testuser";


            // Act
            var result = ratecontroller.AddToRating("testtrack", 4) as JsonResult;
            string json = new JavaScriptSerializer().Serialize(result.Data).Replace("\"", string.Empty); ;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("{result:ok}", json);
            Debug.WriteLine("Json: " + json);


        }

        [TestMethod()]
        public void LogoutTest()
        {
            var httpContext = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s["username"]).Returns("User");
            session.Setup(s => s["Login"]).Returns("LoggedIn");
            httpContext.SetupGet(x => x.Session).Returns(session.Object);
            var logoutcontroller = new UserController() { ControllerContext = new ControllerContext { HttpContext = httpContext.Object } };
            var result = logoutcontroller.Logout() as RedirectToRouteResult;


            Assert.IsNotNull(result);
            Assert.AreEqual("SignUp", result.RouteValues["action"]); //ensures that the redirect happens 

        }

        [TestMethod()]
        public void TestFunctionTest()
        {
            var mockConnection = new Mock<MockDatabaseConnection>();
            var mockServer = new Mock<HttpServerUtilityBase>();
            mockServer.Setup(x => x.MapPath(It.IsAny<string>())).Returns("C:\\Users\\ethan\\OneDrive\\Documents\\GitHub\\FinalYearProject-ethan\\WebApplication4\\WebApplication4\\defaultPic\\default-user.png");
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.Server).Returns(mockServer.Object);
            mockConnection.Setup(c => c.Open());
            mockConnection.Setup(c => c.CreateCommand()).Returns(() =>
            {
                var mockCommand = new Mock<IDbCommand>();
                mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(1);
                
                return mockCommand.Object;
            });

            var defaultPicture = System.IO.File.ReadAllBytes(mockServer.Object.MapPath("~/defaultPic/default-user.png"));

            var playlistModel = new PlaylistModel
            {
                username = "",
                trackname = "",
                playlistname = "",
                genre = "",
                artist = "",
                img = "",
                trackid = ""
                

            };

            var httpContext = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            httpContext.Setup(x => x.Session).Returns(session.Object);
            var controller = new UserController(mockConnection.Object) { ControllerContext = new ControllerContext { HttpContext = httpContext.Object } };
            var request = new Mock<HttpRequestBase>();
            mockServer.Setup(s => s.MapPath("~/defaultPic/default-user.png")).Returns("C:\\defaultPic\\default-user.png"); 
            var context = new Mock<HttpContextBase>();
            session.Setup(s => s["username"]).Returns("testuser");
            controller.Session["username"] = "testuser";


            // Act
            var result = controller.TestFunction("playlist1", "tracktest", "rock", "testartist", "img1", "trackid", playlistModel) as JsonResult;
            string json = new JavaScriptSerializer().Serialize(result.Data).Replace("\"", string.Empty); ;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("{result:ok}", json);
            Debug.WriteLine("Json: " + json);
        }
    }
}

namespace WebApplication4.tests
{


    public class NpgsqlConnectionWrapper : MockDatabaseConnection
    {
        private readonly NpgsqlConnection _connection;

        public NpgsqlConnectionWrapper(string connectionString)
        {
            var connstring = "Host=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;password=Ffgtte??;";
            _connection = new NpgsqlConnection(connstring);
        }

        public void Open()
        {
            _connection.Open();
        }

        public IDbCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }
    }

    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void Login_Valid()
        {
            
            var mockConnection = new Mock<MockDatabaseConnection>();
            mockConnection.Setup(c => c.Open());
            mockConnection.Setup(c => c.CreateCommand()).Returns(() =>
            {
                var mockCommand = new Mock<IDbCommand>();
                mockCommand.Setup(c => c.ExecuteScalar()).Returns(1);
                mockCommand.Setup(c => c.Parameters.Add(new NpgsqlParameter("username", It.IsAny<string>())));
                mockCommand.Setup(c => c.Parameters.Add(new NpgsqlParameter("password", It.IsAny<string>())));
                return mockCommand.Object;
            });

            var userModel = new UserModel
            {
                username = "ethanm33",
                password = "password1"
            };

            var logincontroller = new UserController(mockConnection.Object);
            var request = new Mock<HttpRequestBase>();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            logincontroller.ControllerContext = new ControllerContext { HttpContext = context.Object };


            //calls the login controller and gets the redirect back. This will only happen if the user exists
            var result = logincontroller.Login(userModel) as RedirectToRouteResult;

            Assert.IsNotNull(result); //ensures there is a response
            Assert.AreEqual("HomePage", result.RouteValues["action"]); //ensures that the redirect happens 
        }
    }
}