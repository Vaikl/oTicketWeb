using Moq;
using System.Collections.Generic;
using Xunit;
using Clinic.Interfaces;
using Clinic.Models;
using System.Linq;
using Clinic.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            Mock<IServiceRepository> mock = new Mock<IServiceRepository>();
            mock.Setup(m => m.Services).Returns(new Service[] {
                new Service {ServiceId = 1, Name = "P1"},
                new Service {ServiceId = 2, Name = "P2"},
                new Service {ServiceId = 3, Name = "P3"},
            }.AsQueryable<Service>());

            AdminController target = new AdminController(mock.Object);

            Service[] result = GetViewModel<IEnumerable<Service>>(target.Index())?.ToArray();

            Assert.Equal(3, result.Length);
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);
        }

        [Fact]
        public void Can_Edit_Product()
        {
            Mock<IServiceRepository> mock = new Mock<IServiceRepository>();
            mock.Setup(m => m.Services).Returns(new Service[] {
                new Service {ServiceId = 1, Name = "P1"},
                new Service {ServiceId = 2, Name = "P2"},
                new Service {ServiceId = 3, Name = "P3"},
            }.AsQueryable<Service>());

            AdminController target = new AdminController(mock.Object);

            Service p1 = GetViewModel<Service>(target.Edit(1));
            Service p2 = GetViewModel<Service>(target.Edit(2));
            Service p3 = GetViewModel<Service>(target.Edit(3));

            Assert.Equal(1, p1.ServiceId);
            Assert.Equal(2, p2.ServiceId);
            Assert.Equal(3, p3.ServiceId);
        }

        [Fact]
        public void Cannot_Edit_Nonexistent_Product()
        {
            Mock<IServiceRepository> mock = new Mock<IServiceRepository>();
            mock.Setup(m => m.Services).Returns(new Service[] {
                new Service {ServiceId = 1, Name = "P1"},
                new Service {ServiceId = 2, Name = "P2"},
                new Service {ServiceId = 3, Name = "P3"},
            }.AsQueryable<Service>());

            AdminController target = new AdminController(mock.Object);
            Service result = GetViewModel<Service>(target.Edit(4));
            Assert.Null(result);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            Mock<IServiceRepository> mock = new Mock<IServiceRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };

            Service service = new Service { Name = "Test" };
            IActionResult result = target.Edit(service);
            mock.Verify(m => m.SaveService(service));

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IServiceRepository> mock = new Mock<IServiceRepository>();
            AdminController target = new AdminController(mock.Object);
            Service service = new Service { Name = "Test" };
            target.ModelState.AddModelError("error", "error");

            IActionResult result = target.Edit(service);
            mock.Verify(m => m.SaveService(It.IsAny<Service>()), Times.Never());
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Products()
        {
            Service service = new Service { ServiceId = 2, Name = "Test" };

            Mock<IServiceRepository> mock = new Mock<IServiceRepository>();
            mock.Setup(m => m.Services).Returns(new Service[] {
                new Service {ServiceId = 1, Name = "P1"},
                service,
                new Service {ServiceId = 3, Name = "P3"},
            }.AsQueryable<Service>());

            AdminController target = new AdminController(mock.Object);
            target.Delete(service.ServiceId);
            mock.Verify(m => m.DeleteService(service.ServiceId));
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
