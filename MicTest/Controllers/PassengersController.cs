using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MicTest.Data;
using MicTest.Models;

namespace MicTest.Controllers
{
    public class PassengersController : Controller
    {
        private readonly TicketContext _context;

        public PassengersController(TicketContext context)
        {
            _context = context;
        }

        // GET: Passengers
        public async Task<IActionResult> Index(string sort, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sort) ? "name_desc" : "";
            ViewData["SurnameSortParm"] = sort == "Surname" ? "surname_desc" : "Surname";
            ViewData["PatronymicSortParm"] = sort == "Patronymic" ? "patronymic_desc" : "Patronymic";
            ViewData["CurrentFilter"] = searchString;

            var students = from s in _context.Passenger
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name.Contains(searchString));
            }
            switch (sort)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.Name);
                    break;
                case "Surname":
                    students = students.OrderBy(s => s.Surname);
                    break;
                case "surname_desc":
                    students = students.OrderByDescending(s => s.Surname);
                    break;
                case "Patronymic":
                    students = students.OrderBy(s => s.Patronymic);
                    break;
                case "patronymic_desc":
                    students = students.OrderByDescending(s => s.Patronymic);
                    break;
                default:
                    students = students.OrderBy(s => s.Name);
                    break;
            }
            return View(await students.AsNoTracking().ToListAsync());
        }

        // GET: Passengers/Details/5
        public async Task<IActionResult> Details(int? id,DateTime searchDateFrom, DateTime searchDateTo)
        {
            ViewData["CurrentFilterFrom"] = searchDateFrom;
            ViewData["CurrentFilterTo"] = searchDateTo;

            if (id == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passenger
                .Include(x => x.Document)
                .ThenInclude(x => x.AirTickets)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if ( searchDateFrom != DateTime.MinValue || searchDateTo != DateTime.MinValue)
            {
               //passenger.Document.AirTickets = passenger.Document.AirTickets.Where(a => (a.Arrival.Date >= searchDateFrom.Date) && (a.Arrival.Date <= searchDateFrom.Date)).ToList();

               var sortlist = new List<AirTicket>();
               foreach (var ticket in passenger.Document.AirTickets)
                {
                    if (ticket.Arrival.Date >= searchDateFrom.Date)
                    {
                        if (ticket.Arrival.Date <= searchDateTo.Date)
                        {
                            sortlist.Add(ticket);
                        }
                    }
                }
                passenger.Document.AirTickets = sortlist;
            }
            
            

            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // GET: Passengers/Create
        public IActionResult Create()
        {
            ViewData["PassengerId"] = new SelectList(_context.Passenger, "Id", "Id");

            return View();
        }

        // POST: Passengers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Passenger passenger)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(passenger);
                    //await _context.SaveChangesAsync();
                    passenger.Document.Passenger = passenger;
                    passenger.Document.PassengerId = passenger.Id;
                    passenger.Document.Id = passenger.Document.Id;
                    _context.Document.Add(passenger.Document);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(passenger);
        }


        // GET: Passengers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passenger.FindAsync(id);
            if (passenger == null)
            {
                return NotFound();
            }
            return View(passenger);
        }

        // POST: Passengers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Patronymic")] Passenger passenger)
        {
            if (id != passenger.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passenger);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassengerExists(passenger.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(passenger);
        }

        // GET: Passengers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passenger
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // POST: Passengers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passenger = await _context.Passenger.FindAsync(id);
            _context.Passenger.Remove(passenger);
            var document = await _context.Document.FindAsync(passenger.DocumentId);
            _context.Document.Remove(document);
            var airTickets = await _context.AirTicket.Where(i => i.DocumentId == id)
.ToListAsync();
            if (airTickets != null)
            {
                foreach (var at in airTickets)
                {
                    _context.AirTicket.Remove(at);
                    await _context.SaveChangesAsync();
                }

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassengerExists(int id)
        {
            return _context.Passenger.Any(e => e.Id == id);
        }
    }
}
