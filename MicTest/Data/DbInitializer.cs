using MicTest.Models;
using System;
using System.Linq;
using Type = MicTest.Models.Type;

namespace MicTest.Data
{
    public class DbInitializer
    {

        public static void Initialize(TicketContext context)
        {
            context.Database.EnsureCreated();

            // Look for any Passengers.
            if (context.Passenger.Any())
            {
                return;   // DB has been seeded
            }

            var documents = new Document[]
            {
            new Document{Type=Type.P,PassengerId=1, Number=1611},
            new Document{Type=Type.P,PassengerId=2, Number=1612},
            new Document{Type=Type.C,PassengerId=3, Number=1613},
            new Document{Type=Type.I,PassengerId=4, Number=1614},
            new Document{Type=Type.P,PassengerId=5, Number=1615},
            new Document{Type=Type.P,PassengerId=6, Number=1616},
            new Document{Type=Type.P,PassengerId=7, Number=1617}
            };

            foreach (Document e in documents)
            {
                context.Document.Add(e);
            }
            context.SaveChanges();

            var passengers = new Passenger[]
           {
            new Passenger{Name="Carson",Surname="Alexander",Patronymic="Harry",DocumentId=1},
            new Passenger{Name="Meredith",Surname="Alonso",Patronymic="Oliver",DocumentId=2},
            new Passenger{Name="Arturo",Surname="Anand",Patronymic="Jack",DocumentId=3},
            new Passenger{Name="Gytis",Surname="Barzdukas",Patronymic="Charlie",DocumentId=4},
            new Passenger{Name="Yan",Surname="Li",Patronymic="Thomas",DocumentId=5},
            new Passenger{Name="Peggy",Surname="Justice",Patronymic="Jacob",DocumentId=6},
            new Passenger{Name="Laura",Surname="Norman",Patronymic="Alfie",DocumentId=7}
           };

            foreach (Passenger s in passengers)
            {
                context.Passenger.Add(s);
            }
            context.SaveChanges();

            var airTickets = new AirTicket[]
  {
            new AirTicket{From="Arkhangelsk",To="Moscow",Provider="AeroFlot",Departure=new DateTime(2023, 1, 5, 8, 30, 0),Arrival=new DateTime(2023, 1, 21, 8, 30, 0),Registration=new DateTime(2023, 1, 5, 13, 40, 0),DocumentId=1},
            new AirTicket{From="Moscow",To="Arkhangelsk",Provider="AeroFlot",Departure=new DateTime(2023, 2, 5, 7, 0, 0),Arrival=new DateTime(2023, 2, 6, 6, 30, 0),Registration=new DateTime(2023, 1, 5, 13, 40, 0),DocumentId =1},
            new AirTicket{From="Moscow",To="Pyatigorsk",Provider="AeroFlot",Departure=new DateTime(2023, 4, 12, 18, 20, 0),Arrival=new DateTime(2023, 4, 12, 20, 0, 0),Registration=new DateTime(2023, 4, 1, 10, 0, 0),DocumentId=2},
            new AirTicket{From="Moscow",To="Pyatigorsk",Provider="AeroFlot",Departure=new DateTime(2023, 4, 27, 7, 40, 0),Arrival=new DateTime(2023, 4, 27, 15, 10, 0),Registration=new DateTime(2023, 4, 1, 10, 0, 0),DocumentId=2},
            new AirTicket{From="Pyatigorsk",To="Moscow",Provider="AeroFlot",Departure=new DateTime(2023, 4, 12, 18, 20, 0),Arrival=new DateTime(2023, 4, 12, 20, 0, 0),Registration=new DateTime(2023, 4, 1, 10, 0, 0),DocumentId=3},
            new AirTicket{From="Pyatigorsk",To="Moscow",Provider="AeroFlot",Departure=new DateTime(2023, 4, 27, 7, 40, 0),Arrival=new DateTime(2023, 4, 27, 15, 10, 0),Registration=new DateTime(2023, 4, 1, 10, 0, 0),DocumentId=3},
            new AirTicket{From="Nizhny Tagil",To="Moscow",Provider="S7",Departure=new DateTime(2023, 8, 28, 13, 45, 0),Arrival=new DateTime(2023, 8, 29, 1, 10, 0),Registration=new DateTime(2023, 8, 5, 10, 0, 0),DocumentId=4},
            new AirTicket{From="Nizhny Tagil",To="Moscow",Provider="Emirates",Departure=new DateTime(2023, 5, 7, 10, 0, 0),Arrival=new DateTime(2023, 5, 7, 12, 20, 0),Registration=new DateTime(2023, 4, 29, 18, 10, 0),DocumentId=5},
            new AirTicket{From="Sochi",To="Rostov-on-Don",Provider="Emirates",Departure=new DateTime(2023, 9, 3, 15, 40, 0),Arrival=new DateTime(2023, 9, 3, 22, 30, 0),Registration=new DateTime(2023, 9, 1, 19, 45, 0),DocumentId=6},
            new AirTicket{From="Moscow",To="Sochi",Provider="Emirates",Departure=new DateTime(2023, 12, 31, 10, 0, 0),Arrival=new DateTime(2023, 12, 31, 10, 0, 0),Registration=new DateTime(2023, 12, 17, 20, 0, 0),DocumentId=7},
            new AirTicket{From="Sochi",To="Moscow",Provider="S7",Departure=new DateTime(2024, 1, 10, 4, 0, 0),Arrival=new DateTime(2024, 1, 10, 23, 0, 0),Registration=new DateTime(2023, 12, 17, 20, 0, 0),DocumentId=7}

  };

            foreach (AirTicket c in airTickets)
            {
                context.AirTicket.Add(c);
            }
            context.SaveChanges();
        }
    }
}
