using System.Collections.Generic;

namespace Czytnik_Model.Models
{
    public class Translator
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? Surname { get; set; }

        public ICollection<BookTranslator> BookTranslators { get; set; }

    }
}
