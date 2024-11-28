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
    public class LanguageService : ILanguageService
    {
        private readonly AppDbContext _dbContext;
        public LanguageService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ListLanguagesViewModel>> GetAll()
        {

            var languagesQuery = _dbContext.Languages.Select(l => new ListLanguagesViewModel
            {
                Id = l.Id,
                Name = l.Name
            }).OrderBy(l => l.Name);
            var languages = await languagesQuery.ToListAsync();
            return languages;
        }
    }
}
