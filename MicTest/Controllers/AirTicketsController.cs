using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MicTest.Data;
using MicTest.Models;

namespace MicTest.Controllers
{
    public class AirTicketsController : Controller
    {
        private readonly TicketContext _context;

        public AirTicketsController(TicketContext context)
        {
            _context = context;
        }

        // GET: AirTickets
        public async Task<IActionResult> Index(string sort, DateTime sortDate)
        {
            ViewData["FromSortParm"] = sort == "From" ? "from_desc" : "From";
            ViewData["ToSortParm"] = sort == "To" ? "to_desc" : "To";
            ViewData["ProviderSortParm"] = sort == "Provider" ? "provider_desc" : "Provider";
            ViewData["DepartureSortParm"] = sort == "Departure" ? "departure_desc" : "Departure";
            ViewData["ArrivalSortParm"] = sort == "Arrival" ? "arrival_desc" : "Arrival";
            ViewData["RegistrationSortParm"] = sort == "Registration" ? "registration_desc" : "Registration";

            var airTickets = from s in _context.AirTicket
                            select s;

            switch (sort)
            {
                case "from_desc":
                    airTickets = airTickets.OrderByDescending(s => s.From);
                    break;
                case "From":
                    airTickets = airTickets.OrderBy(s => s.From);
                    break;
                case "to_desc":
                    airTickets = airTickets.OrderByDescending(s => s.To);
                    break;
                case "To":
                    airTickets = airTickets.OrderBy(s => s.To);
                    break;
                case "provider_desc":
                    airTickets = airTickets.OrderByDescending(s => s.Provider);
                    break;
                case "Provider":
                    airTickets = airTickets.OrderBy(s => s.Provider);
                    break;
                case "departure_desc":
                    airTickets = airTickets.OrderByDescending(s => s.Departure);
                    break;
                case "Departure":
                    airTickets = airTickets.OrderBy(s => s.Departure);
                    break;
                case "arrival_desc":
                    airTickets = airTickets.OrderByDescending(s => s.Arrival);
                    break;
                case "Arrival":
                    airTickets = airTickets.OrderBy(s => s.Arrival);
                    break;
                case "registration_desc":
                    airTickets = airTickets.OrderByDescending(s => s.Registration);
                    break;
                case "Registration":
                    airTickets = airTickets.OrderBy(s => s.Registration);
                    break;
            }
            return View(await airTickets.ToListAsync());
        }

        // GET: AirTickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airTicket = await _context.AirTicket
                .Include(a => a.Document)
                .ThenInclude(b => b.Passenger)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (airTicket == null)
            {
                return NotFound();
            }

            return View(airTicket);
        }

        // GET: AirTickets/Create
        public IActionResult Create()
        {
            ViewData["DocumentId"] = new SelectList(_context.Document, "Id", "Id");
            return View();
        }

        // POST: AirTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,From,To,Provider,Departure,Arrival,Registration,DocumentId")] AirTicket airTicket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(airTicket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DocumentId"] = new SelectList(_context.Document, "Id", "Id", airTicket.DocumentId);
            return View(airTicket);
        }

        // GET: AirTickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airTicket = await _context.AirTicket.FindAsync(id);
            if (airTicket == null)
            {
                return NotFound();
            }
            ViewData["DocumentId"] = new SelectList(_context.Document, "Id", "Id", airTicket.DocumentId);
            return View(airTicket);
        }

        // POST: AirTickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,From,To,Provider,Departure,Arrival,Registration,DocumentId")] AirTicket airTicket)
        {
            if (id != airTicket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airTicket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirTicketExists(airTicket.Id))
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
            ViewData["DocumentId"] = new SelectList(_context.Document, "Id", "Id", airTicket.DocumentId);
            return View(airTicket);
        }

        // GET: AirTickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airTicket = await _context.AirTicket
                .Include(a => a.Document)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (airTicket == null)
            {
                return NotFound();
            }

            return View(airTicket);
        }

        // POST: AirTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airTicket = await _context.AirTicket.FindAsync(id);
            _context.AirTicket.Remove(airTicket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirTicketExists(int id)
        {
            return _context.AirTicket.Any(e => e.Id == id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
