using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czytnik_Model.Models
{
    public class BookTranslator
    {
        public int BookId { get; set; }
        public int TranslatorId { get; set; }

        public Book Book { get; set; }
        public Translator Translator { get; set; }

    }
}
