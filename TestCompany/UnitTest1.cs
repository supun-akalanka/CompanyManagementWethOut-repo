using CompanyManagement.Controllers;
using CompanyManagement.Data;
using CompanyManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestCompany
{
    public class UnitTest1
    {
        [Fact]
        public async Task Add_Valid_Company_Return_ToActionResult()
        {
            //Arrange
            var addCompanyViewModel = new AddCompanyViewModel
            {
                Name = "Test",
                Address = "Test Address",
                Contact = "1234567890",
                Email = "test@example.com"
            };
            var companyDBContextMock = new Mock<CompanyDBContext>();
            var companyDbSetMock = new Mock<DbSet<Company>>();

            companyDBContextMock.Setup(x => x.Company).Returns(companyDbSetMock.Object);
            //companyDBContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var controller = new CompanyController(companyDBContextMock.Object);

            //Act
            var result = await controller.Add(addCompanyViewModel);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}