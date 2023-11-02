using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MicTest.Data;
using MicTest.Models;

namespace MicTest.Controllers
{
	public class DocumentsController : Controller
	{
		private readonly TicketContext _context;

		public DocumentsController(TicketContext context)
		{
			_context = context;
		}

		// GET: Documents
		public async Task<IActionResult> Index(string sort)
		{
            ViewData["TypeSortParm"] = String.IsNullOrEmpty(sort) ? "type_desc" : "";

            var documents = from s in _context.Document
                            select s;

			switch (sort)
			{
				case "type_desc":
					documents = documents.OrderByDescending(s => s.Type);
					break;
				default:
					documents = documents.OrderBy(s => s.Type);
					break;
			}
                    return View(await documents.AsNoTracking().ToListAsync());
		}

		// GET: Documents/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var document = await _context.Document
				.Include(d => d.Passenger)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id);
			if (document == null)
			{
				return NotFound();
			}

			return View(document);
		}

		// GET: Documents/Create
		public IActionResult Create()
		{
			ViewData["PassengerId"] = new SelectList(_context.Passenger, "Id", "Id");

            return View();
		}

		// POST: Documents/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Type,Number,PassengerId")] Document document)
		{
			try
			{

				if (ModelState.IsValid)
				{
					_context.Add(document);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				//ViewData["PassengerId"] = new SelectList(_context.Passenger, "Id", "Id", document.PassengerId);
				
			}
			catch
			{
				//Log the error (uncomment ex variable name and write a log.
				ModelState.AddModelError("", "Unable to save changes. " +
					"Try again, and if the problem persists " +
					"see your system administrator.");
			}
			return View(document);
		}

		// GET: Documents/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var document = await _context.Document.FindAsync(id);
			if (document == null)
			{
				return NotFound();
			}
			//ViewData["PassengerId"] = new SelectList(_context.Passenger, "Id", "Id", document.PassengerId);
			return View(document);
		}

		// POST: Documents/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Number,PassengerId")] Document document)
		{
			if (id != document.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(document);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!DocumentExists(document.Id))
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
			//ViewData["PassengerId"] = new SelectList(_context.Passenger, "Id", "Id", document.PassengerId);
			return View(document);
		}

		// GET: Documents/Delete/5
		public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
		{
			if (id == null)
			{
				return NotFound();
			}
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            var document = await _context.Document
				.AsNoTracking()
				.Include(d => d.Passenger)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (document == null)
			{
				return NotFound();
			}
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }


            return View(document);
		}

		// POST: Documents/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
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
                var document = await _context.Document.FindAsync(id);
				_context.Document.Remove(document);
				await _context.SaveChangesAsync();
			}
			catch (DataException)
			{
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
			return RedirectToAction(nameof(Index));
		}

		private bool DocumentExists(int id)
		{
			return _context.Document.Any(e => e.Id == id);
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
