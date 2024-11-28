using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czytnik_Model.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book> OriginalBooks { get; set; }
        public ICollection<Book> EditionBooks { get; set; }
    }
}
