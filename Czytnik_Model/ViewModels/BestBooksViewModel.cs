using Czytnik_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czytnik_Model.ViewModels
{
    public class BestBooksViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Cover { get; set; }
        public List<string> Authors { get; set; }
    }
}
