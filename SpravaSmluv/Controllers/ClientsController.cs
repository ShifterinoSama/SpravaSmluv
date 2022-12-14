using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpravaSmluv.Data;
using SpravaSmluv.Models;

namespace SpravaSmluv.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ContractManagmentContext _context;

        public ClientsController(ContractManagmentContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string sortOrder, string searchString, string filterString)
        {
            var clients = from c in _context.Clients select c;
            ViewBag.FilterList = new List<string>() { "Jméno", "Přijmení", "Email","Rodné číslo" };
            if (!string.IsNullOrEmpty(searchString))
            {
                clients = GetSearchItems(searchString, filterString, clients);
            }
            IQueryable<Client> orderedClients = GetOrderedList(sortOrder, clients);
            return View(await orderedClients.ToListAsync());
        }
        private static IQueryable<Client> GetSearchItems(string searchString, string filterString, IQueryable<Client> clients)
        {
            clients = filterString switch
            {
                "Jméno" => clients.Where(c => c.FirstName.Contains(searchString)),
                "Přijmení" => clients.Where(c => c.LastName.Contains(searchString)),
                "Email" => clients.Where(c => c.Email.Contains(searchString)),
                "Rodné číslo" => clients.Where(c => c.PersonalIdentificationNumber.Contains(searchString)),
            _ => clients.Where(c => c.FirstName.Contains(searchString) || c.LastName.Contains(searchString) || c.Email.Contains(searchString) || c.PersonalIdentificationNumber.Contains(searchString)),
            };
            return clients;
        }
        private IQueryable<Client> GetOrderedList(string sortOrder, IQueryable<Client> clients)
        {
            string sortFormat = "";
            if (sortOrder == null)
            {
                sortFormat = "desc";
            }
            else
            {
                if (sortOrder[^3] == 'a' && sortOrder != null)
                {
                    sortFormat = "asc";
                }
                else
                {
                    sortFormat = "desc";
                }
            }
            ViewBag.FirstNameSortParm = !(sortFormat == "desc") ? "firstname_desc" : "firstname_asc";
            ViewBag.LastNameSortParm = !(sortFormat == "desc") ? "lastname_desc" : "lastname_asc";
            ViewBag.EmailSortParm = !(sortFormat == "desc") ? "email_desc" : "email_asc";
            //ViewBag.PhoneNumberSortParm = !(sortFormat == "desc") ? "phonenumber_desc" : "phonenumber_asc";
            ViewBag.PersonalIdNumberSortParm = !(sortFormat == "desc") ? "personalidnumber_desc" : "personalidnumber_asc";
            //ViewBag.AgeSortParm = !(sortFormat == "desc") ? "age_desc" : "age_asc";

            clients = sortOrder switch
            {
                "firstname_desc" => clients.OrderByDescending(c => c.FirstName),
                "lastname_desc" => clients.OrderByDescending(c => c.LastName),
                "email_desc" => clients.OrderByDescending(c => c.Email),
                //"phonenumber_desc" => clients.OrderByDescending(c => c.PhoneNumber),
                "personalidnumber_desc" => clients.OrderByDescending(c => c.PersonalIdentificationNumber),
                //"age_desc" => clients.OrderByDescending(c => c.Age),
                "firstname_asc" => clients.OrderBy(c => c.FirstName),
                "lastname_asc" => clients.OrderBy(c => c.LastName),
                "email_asc" => clients.OrderBy(c => c.Email),
                //"phonenumber_asc" => clients.OrderBy(c => c.PhoneNumber),
                "personalidnumber_asc" => clients.OrderBy(c => c.PersonalIdentificationNumber),
                //"age_asc" => clients.OrderBy(c => c.Age),
                _ => clients.OrderBy(c => c.FirstName),
            };
            return clients;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public FileResult Export()
        {

            List<object> clients = (from client in _context.Clients.ToList()
                                      select new[] {client.Id.ToString(),
                                                      client.FirstName,
                                                      client.LastName,
                                                      client.Email,
                                                      client.PhoneNumber.ToString(),
                                                      client.PersonalIdentificationNumber,
                                                      client.Age.ToString()}).ToList<object>();
            clients.Insert(0, new string[7] { "Id klienta", "Jméno", "Přijmení", "Email", "Telefonní číslo", "Rodné číslo", "Věk"});
            StringBuilder sb = new();
            for (int i = 0; i < clients.Count; i++)
            {
                string[] client = (string[])clients[i];
                for (int j = 0; j < client.Length; j++)
                {
                    sb.Append(client[j] + ',');
                }

                sb.Append("\r\n");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Clients.csv");
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include("Contracts")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Email,PhoneNumber,PersonalIdentificationNumber,Age")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber,PersonalIdentificationNumber,Age")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include("Contracts")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
