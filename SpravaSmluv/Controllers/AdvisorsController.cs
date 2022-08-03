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

namespace SpravaSmluv.Views
{
    public class AdvisorsController : Controller
    {
        private readonly ContractManagmentContext _context;

        public AdvisorsController(ContractManagmentContext context)
        {
            _context = context;
        }

        // GET: Advisors
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            var advisors = from c in _context.Advisors select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                advisors = advisors.Where(c => c.FirstName.Contains(searchString) || c.LastName.Contains(searchString) || c.Email.Contains(searchString) || c.PhoneNumber.Contains(searchString));
            }
            IQueryable<Advisor> orderedAdvisors = GetOrderedList(sortOrder, advisors);
            return View(await orderedAdvisors.ToListAsync());
        }

        private IQueryable<Advisor> GetOrderedList(string sortOrder, IQueryable<Advisor> advisors)
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

            advisors = sortOrder switch
            {
                "firstname_desc" => advisors.OrderByDescending(c => c.FirstName),
                "lastname_desc" => advisors.OrderByDescending(c => c.LastName),
                "email_desc" => advisors.OrderByDescending(c => c.Email),
                //"phonenumber_desc" => advisors.OrderByDescending(c => c.PhoneNumber),
                "personalidnumber_desc" => advisors.OrderByDescending(c => c.PersonalIdentificationNumber),
                //"age_desc" => advisors.OrderByDescending(c => c.Age),
                "firstname_asc" => advisors.OrderByDescending(c => c.FirstName),
                "lastname_asc" => advisors.OrderByDescending(c => c.LastName),
                "email_asc" => advisors.OrderByDescending(c => c.Email),
                //"phonenumber_asc" => advisors.OrderByDescending(c => c.PhoneNumber),
                "personalidnumber_asc" => advisors.OrderByDescending(c => c.PersonalIdentificationNumber),
                //"age_asc" => advisors.OrderByDescending(c => c.Age),
                _ => advisors.OrderBy(c => c.FirstName),
            };
            return advisors;
        }


        [Microsoft.AspNetCore.Mvc.HttpPost]
        public FileResult Export()
        {

            List<object> advisors = (from advisor in _context.Advisors.ToList()
                                    select new[] {advisor.Id.ToString(),
                                                      advisor.FirstName,
                                                      advisor.LastName,
                                                      advisor.Email,
                                                      advisor.PhoneNumber.ToString(),
                                                      advisor.PersonalIdentificationNumber,
                                                      advisor.Age.ToString()}).ToList<object>();
            advisors.Insert(0, new string[7] { "Id klienta", "Jméno", "Přijmení", "Email", "Telefonní číslo", "Rodné číslo", "Věk" });
            StringBuilder sb = new();
            for (int i = 0; i < advisors.Count; i++)
            {
                string[] advisor = (string[])advisors[i];
                for (int j = 0; j < advisor.Length; j++)
                {
                    sb.Append(advisor[j] + ',');
                }

                sb.Append("\r\n");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Advisors.csv");
        }
    

        // GET: Advisors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advisor = await _context.Advisors
                .Include("Contracts")
                .FirstOrDefaultAsync(a => a.Id == id);
            if (advisor == null)
            {
                return NotFound();
            }

            return View(advisor);
        }

        // GET: Advisors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Advisors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber,PersonalIdentificationNumber,Age")] Advisor advisor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(advisor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(advisor);
        }

        // GET: Advisors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advisor = await _context.Advisors.FindAsync(id);
            if (advisor == null)
            {
                return NotFound();
            }
            return View(advisor);
        }

        // POST: Advisors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber,PersonalIdentificationNumber,Age")] Advisor advisor)
        {
            if (id != advisor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advisor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvisorExists(advisor.Id))
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
            return View(advisor);
        }

        // GET: Advisors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advisor = await _context.Advisors
                .Include("Contracts")
                .FirstOrDefaultAsync(a => a.Id == id);
            if (advisor == null)
            {
                return NotFound();
            }

            return View(advisor);
        }

        // POST: Advisors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var advisor = await _context.Advisors.FindAsync(id);
                _context.Advisors.Remove(advisor);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                ViewBag.ContractManagerDelete = "Nelze smazat správce smlouvy!";
                var advisor = await _context.Advisors.Include("Contracts").FirstOrDefaultAsync(a => a.Id == id);
                return View(advisor);
                throw;
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool AdvisorExists(int id)
        {
            return _context.Advisors.Any(e => e.Id == id);
        }
    }
}
