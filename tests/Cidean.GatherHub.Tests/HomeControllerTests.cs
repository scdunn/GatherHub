using Cidean.GatherHub.Controllers;
using Cidean.GatherHub.Core.Data;
using Cidean.GatherHub.Core.Helpers;
using Cidean.GatherHub.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cidean.GatherHub.Tests
{
    public class HomeControllerTests
    {



        public HomeControllerTests()
        {

        }



        private UnitOfWork GetUnitOfWork(string dbname)
        {
            var hubOptions = new DbContextOptionsBuilder<HubContext>()
              .UseInMemoryDatabase(databaseName: $"HUB_{dbname}")
              .Options;
            var logOptions = new DbContextOptionsBuilder<ActivityContext>()
               .UseInMemoryDatabase(databaseName: $"LOG_{dbname}")
               .Options;

            var work = new UnitOfWork(new HubContext(hubOptions), new ActivityLogger(new ActivityContext(logOptions)));


            work.CourseCategories.Insert(new Core.Models.CourseCategory() { Title = "Arts and Crafts", Id = 1 });
            work.Members.Insert(new Core.Models.Member() { EmailAddress = "user@domain.com", FirstName = "John", LastName = "Doe", Id = 1 });
            work.Courses.Insert(new Core.Models.Course() { Title = "Test Course", CourseCategoryId = 1, InstructorId = 1 });
            work.Save().Wait();


            return work;
        }

        [Fact]
        public void SomeMockTest()
        {
            var member = new Mock<Member>();

            member.SetupGet(m => m.FullName).Returns("Jimmy Hendrix");

            Assert.Equal("Jimmy Hendrix", member.Object.FullName);

        }


        [Fact]
        public void About_GetViewDataMessage_IsAboutUs()
        {
            //Arrange       
            using (UnitOfWork work = GetUnitOfWork(nameof(this.About_GetViewDataMessage_IsAboutUs)))
            { 
                var controller = new HomeController(work);

                //Act
                var result = controller.About() as ViewResult;

                //Assert
                Assert.Equal("About Us",result.ViewData["Message"].ToString()  );
            }


        }

        [Fact]
        public void Index_Get_OnlyModelIsTestCourse()
        {
            //Arrange
            using (UnitOfWork work = GetUnitOfWork(nameof(this.Index_Get_OnlyModelIsTestCourse)))
            {
                var controller = new HomeController(work);

                //Act
                var result = controller.Index() as ViewResult;
                IQueryable<Course> model = (IQueryable<Course>)result.Model;

                //Assert
                Assert.True(model.Count() == 1);
                Assert.True(model.First().Title == "Test Course");

            }
            
        }

    }
}
