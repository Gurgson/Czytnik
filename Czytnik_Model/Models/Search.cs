using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czytnik_Model.Models
{
    public class Search
    {
        public int? CategoryId { get; set; }
        public int? LanguageId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? StartPrice { get; set; }
        public int? EndPrice { get; set; }
        public string? Sort { get; set; }
        public string? SearchText { get; set; }
        public int? Page { get; set; }
    }
}
