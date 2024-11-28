using Czytnik_DataAccess.Database;
using Czytnik_Model.Models;
using Czytnik_Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czytnik.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;
        public CategoryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ListCategoriesViewModel>> GetAll()
        {

            var categoriesQuery = _dbContext.Categories.Select(c => new ListCategoriesViewModel
            {
                Category = c,
                BookCount = c.Books.Count
            }).OrderBy(c=>c.Category.Name);
            var categories = await categoriesQuery.ToListAsync();
            return categories;
        }
    }
}
