using CompanyManagement.Data;
using CompanyManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Controllers
{
    public class CompanyController:Controller
    {
        private readonly CompanyDBContext companyDBContext;
        public CompanyController(CompanyDBContext companyDBContext)
        {
            this.companyDBContext = companyDBContext;   
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var companies = await companyDBContext.Company.ToListAsync();
            return View(companies);
        }

        [HttpGet]
        public IActionResult Add() 
        {
            return View(); 
        }   

        [HttpPost]
        public async Task<IActionResult> Add(AddCompanyViewModel addCompanyViewModel)
        {
            var company = new Company() 
            {
                Id = Guid.NewGuid(),
                Name = addCompanyViewModel.Name,
                Address = addCompanyViewModel.Address,
                Contact = addCompanyViewModel.Contact,
                Email = addCompanyViewModel.Email,
            };
            await companyDBContext.Company.AddAsync(company);  
            await companyDBContext.SaveChangesAsync(); 
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id) 
        {
            var company = await companyDBContext.Company.FirstOrDefaultAsync(x=>x.Id == id);

            if(company != null)
            {
                var updateviewModel = new UpdateCompanyViewModel()
                {
                    Id = company.Id,
                    Name = company.Name,
                    Address = company.Address,
                    Contact = company.Contact,
                    Email = company.Email
                };
            
                return await Task.Run(()=>View("View", updateviewModel));

            }

            return RedirectToAction("Index");   
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateCompanyViewModel updateVM)
        {
            var company = await companyDBContext.Company.FindAsync(updateVM.Id);

            if(company != null) 
            {
                company.Name = updateVM.Name;
                company.Address = updateVM.Address;
                company.Contact = updateVM.Contact;
                company.Email = updateVM.Email;

                await companyDBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]  
        public async Task<IActionResult> Delete(UpdateCompanyViewModel updateVM)
        {
            var company = await companyDBContext.Company.FindAsync(updateVM.Id);
            if(company != null) 
            { 
                companyDBContext.Company.Remove(company);
                await companyDBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
