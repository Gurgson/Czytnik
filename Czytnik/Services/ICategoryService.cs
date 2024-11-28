using Czytnik_Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czytnik.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<ListCategoriesViewModel>> GetAll();
    }
}
