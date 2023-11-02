using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicTest.Models
{
    public class AirTicket
    {
        public int Id { get; set; }
        public string From { get; set; } = "";
        public string To { get; set; } = "";
        public string Provider { get; set; } = "";
        public DateTime Departure { get; set; } //вылет
        public DateTime Arrival { get; set; } //прилёт
        public DateTime Registration { get; set; } //прилёт

        public int? DocumentId { get; set; }
        public Document Document { get; set; }

    }
}
