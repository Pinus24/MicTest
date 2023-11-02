using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicTest.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Patronymic { get; set; } = "";
        public int DocumentId { get; set; }
        public Document Document { get; set; }

    }
}
