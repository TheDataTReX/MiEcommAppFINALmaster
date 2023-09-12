using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiEcommApp.Models;
using MiEcommApp.Helpers;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace MiEcommApp.Controllers
{
    [Authorize("999")]
    public class CustomersController : Controller
    {
        private readonly NorthwindContext _context;

        public CustomersController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: Customers
        
        public async Task<IActionResult> Index()
        {
              return _context.Customers != null ? 
                          View(await _context.Customers.ToListAsync()) :
                          Problem("Entity set 'NorthwindContext.Customers'  is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        /*
        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Generate CustomerId from CompanyName
                if (customer.CompanyName.Length >= 5)
                {
                    customer.CustomerId = customer.CompanyName.Substring(0, 5).ToUpper();
                }
                else
                {
                    // If CompanyName is less than 5 letters, pad it with X's or handle it as you see fit
                    customer.CustomerId = customer.CompanyName.PadRight(5, 'X').ToUpper();
                }

                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        */

        /*
        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Generate CustomerId from CompanyName
                customer.CustomerId = IdHelper.GenerateCustomerId(customer.CompanyName);

                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        */

        /*
        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Generate CustomerId from CompanyName
                customer.CustomerId = IdHelper.GenerateCustomerId(customer.CompanyName);

                _context.Add(customer);
                _context.SaveChanges(); // I've changed this line from async to sync for simplicity. You may use async version if you prefer.
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        */

        
        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Generate CustomerId from CompanyName
                customer.CustomerId = IdHelper.GenerateCustomerId(customer.CompanyName);

                _context.Add(customer);
                _context.SaveChanges(); // I've changed this line from async to sync for simplicity. You may use async version if you prefer.
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // This will output model validation errors to your debug console
                var errors = ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .Select(x => new { x.Key, x.Value.Errors })
                            .ToArray();

                foreach (var error in errors)
                {
                    Debug.WriteLine($"Key: {error.Key}, Errors: {string.Join(",", error.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            return View(customer);
        }
        

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CustomerId,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'NorthwindContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Este punto de anlace de API toma un nombre de empresa como parametro de consulta y utiliza IdHelper para generar el CustomerID correspondiente
        //Si el CompanyName no se proporciona o esta vacio, devolvera un estado BadRequest. Si todo va bien, devolverá el CustomerId generado.
        [HttpGet]
        public IActionResult GetCustomerId(string companyName)
        {
            if (string.IsNullOrEmpty(companyName))
            {
                return BadRequest("CompanyName should not be empty.");
            }

            string customerId = IdHelper.GenerateCustomerId(companyName);
            return Ok(customerId);
        }


        private bool CustomerExists(string id)
        {
          return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
