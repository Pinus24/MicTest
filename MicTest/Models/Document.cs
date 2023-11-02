using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicTest.Models
{
    public enum Type // P - passport, C - birth certificate, I - international passport
    {
        P, C, I
    }
    public class Document
    {
        [Key]
        [ForeignKey("Passenger")]
        public int Id { get; set; }
        public Type? Type { get; set; }
        public int Number { get; set; }
        public int PassengerId { get; set; }
        public Passenger Passenger { get; set;}

        public virtual ICollection<AirTicket> AirTickets { get; set; }


    }
}
