using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpravaSmluv.Data;
using SpravaSmluv.Models;

namespace SpravaSmluv.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ContractManagmentContext _context;
        public ContractsController(ContractManagmentContext context)
        {
            _context = context;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(string sortOrder, string searchString, string filterString)
        {
            var contracts = from c in _context.Contracts.Include("Client").Include("Advisors").Include("ContractManager") select c;
            ViewBag.FilterList = new List<string>() {"Evidenční číslo", "Instituce", "Jméno klienta", "Jméno správce smlouvy" };

            if (!string.IsNullOrEmpty(searchString))
            {
                contracts = GetSearchItems(searchString, filterString, contracts);
            }

            IQueryable<Contract> orderdContracts = GetOrderedList(sortOrder, contracts);
            return View(await orderdContracts.ToListAsync());
        }

        private static IQueryable<Contract> GetSearchItems(string searchString, string filterString, IQueryable<Contract> contracts)
        {
            contracts = filterString switch
            {
                "Evidenční číslo" => contracts.Where(c => c.EvidenceNumber.Contains(searchString)),
                "Instituce" => contracts.Where(c => c.Institution.Contains(searchString)),
                "Jméno klienta" => contracts.Where(c => c.Client.FullName.Contains(searchString)),
                "Jméno správce smlouvy" => contracts.Where(c => c.ContractManager.FullName.Contains(searchString)),
                _ => contracts.Where(c => c.EvidenceNumber.Contains(searchString) || c.Institution.Contains(searchString) || c.Client.FullName.Contains(searchString) || c.ContractManager.FullName.Contains(searchString)),
            };
            return contracts;
        }

        private IQueryable<Contract> GetOrderedList(string sortOrder, IQueryable<Contract> contracts)
        {
            string sortFormat = string.Empty;
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

            ViewBag.EvidenceNumberSortParm = !(sortFormat == "desc") ? "evidencenumber_desc" : "evidencenumber_asc";
            ViewBag.InstitutionSortParm = !(sortFormat == "desc") ? "institution_desc" : "institution_asc";
            ViewBag.ClientSortParm = !(sortFormat == "desc") ? "client_desc" : "client_asc";
            ViewBag.ContractManagerSortParm = !(sortFormat == "desc") ? "contractmanager_desc" : "contractmanager_asc";

            // ViewBag.ClosureDateSortParm = !(sortFormat == "desc") ? "closuredate_desc" : "closuredate_asc";
            // ViewBag.ExpirationDateSortParm = !(sortFormat == "desc") ? "expirationdate_desc" : "expirationdate_asc";
            // ViewBag.TerminationDateSortParm = !(sortFormat == "desc") ? "terminationdate_desc" : "terminationdate_asc";
            contracts = sortOrder switch
            {
                "evidencenumber_desc" => contracts.OrderByDescending(c => c.EvidenceNumber),
                "institution_desc" => contracts.OrderByDescending(c => c.Institution),
                "client_desc" => contracts.OrderByDescending(c => c.Client.FullName),
                "contractmanager_desc" => contracts.OrderByDescending(c => c.ContractManager),
                "evidencenumber_asc" => contracts.OrderBy(c => c.EvidenceNumber),
                "institution_asc" => contracts.OrderBy(c => c.Institution),
                "client_asc" => contracts.OrderBy(c => c.Client.FullName),
                "contractmanager_asc" => contracts.OrderBy(c => c.ContractManager),

                // "closuredate_desc" => contracts.OrderByDescending(c => c.ClosureDate),
                // "expirationdate_desc" => contracts.OrderByDescending(c => c.ExpirationDate),
                // "terminationdate_desc" => contracts.OrderByDescending(c => c.TerminationDate),
                // "closuredate_asc" => contracts.OrderBy(c => c.ClosureDate),
                // "expirationdate_asc" => contracts.OrderBy(c => c.ExpirationDate),
                // "terminationdate_asc" => contracts.OrderBy(c => c.TerminationDate),
                // "Date" => contracts.OrderBy(c => c.ClosureDate),
                _ => contracts.OrderBy(c => c.EvidenceNumber),
            };
            return contracts;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public FileResult Export()
        {

            List<object> contracts = (from contract in _context.Contracts.Include("Client").Include("Advisors").Include("ContractManager").ToList()
                                      select new[] {contract.Id.ToString(),
                                                      contract.EvidenceNumber,
                                                      contract.Institution,
                                                      contract.ClientId.ToString(),
                                                      contract.Client.FullName,
                                                      contract.ContractManagerId.ToString(),
                                                      contract.ContractManager.FullName,
                                                      //contract.Advisors.Where(c => c.Id != contract.ContractManagerId).ToList().ForEach(x => x.FullName),
                                                      contract.ClosureDate.ToShortDateString(),
                                                      contract.ExpirationDate.ToShortDateString(),
                                                      contract.TerminationDate.ToString() }).ToList<object>();
            contracts.Insert(0, new string[10] { "Id smlouvy", "Evidencni cislo", "Instituce", "Id klienta", "Jmeno klienta", "Id spravce smlouvy", "Jméno spravce smlouvy", "Datum uzavreni smlouvy", "Datum vyprseni platnosti", "Datum ukonceni smlouvy" });
            StringBuilder sb = new();
            for (int i = 0; i < contracts.Count; i++)
            {
                string[] contract = (string[])contracts[i];
                for (int j = 0; j < contract.Length; j++)
                {
                    sb.Append(contract[j] + ',');
                }

                sb.Append("\r\n");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Contracts.csv");
        }


        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts.Include("Client").Include("Advisors").Include("ContractManager")
                .FirstOrDefaultAsync(m => m.Id == id);
            //contract.Client = _context.Clients.Find(contract.ClientId);
            //contract.ContractManager = _context.Advisors.Find(contract.ContractManagerId);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            ViewData["Advisors"] = _context.Advisors.ToList(); 
            ViewData["Clients"] = _context.Clients.ToList();
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EvidenceNumber,Institution,ClientId,ClosureDate,ExpirationDate,TerminationDate,ContractManagerId")] Contract contract, List<int> advisorIds)
        {
            if (ModelState.IsValid)
            {

                if (!(contract.ClosureDate < contract.ExpirationDate))
                {
                    ViewData["Advisors"] = _context.Advisors.ToList();
                    ViewData["Clients"] = _context.Clients.ToList();
                    this.ModelState.AddModelError("ExpirationDate", "Datum vypršení platnosti musí být později než datum uzavření!");
                    return View(contract);
                }
                if (contract.TerminationDate != null)
                {
                    if (!(contract.TerminationDate > contract.ClosureDate && contract.TerminationDate < contract.ExpirationDate))
                    {
                        ViewData["Advisors"] = _context.Advisors.ToList();
                        ViewData["Clients"] = _context.Clients.ToList();
                        this.ModelState.AddModelError("TerminationDate", "Datum ukončení smlouvy musí být mezi datumem uzavření smlouvy a datumem vypršení platnosti!!");
                        return View(contract);
                    }
                }
                if(contract.Advisors == null)
                {
                    contract.Advisors = new List<Advisor>();
                }
                contract.Advisors.Add(_context.Advisors.Find(contract.ContractManagerId));

                List<Advisor> advisorsFromDB = _context.Advisors.Where(adv => advisorIds.Contains(adv.Id) && adv.Id != contract.ContractManagerId).ToList();

                foreach (var item in advisorsFromDB)
                {
                    contract.Advisors.Add(item);
                }

                _context.Add(contract);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include("Client")
                .Include("Advisors")
                .Include("ContractManager")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contract == null)
            {
                return NotFound();
            }
            ViewData["Advisors"] = _context.Advisors.ToList();
            ViewData["Clients"] = _context.Clients.ToList();
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(include:"Id,EvidenceNumber,Institution,ClientId,ClosureDate,ExpirationDate,ContractManagerId,TerminationDate")] Contract contract, List<int> advisorIds)
        {
            if (id != contract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!(contract.ClosureDate < contract.ExpirationDate))
                    {
                        ViewData["Advisors"] = _context.Advisors.ToList();
                        ViewData["Clients"] = _context.Clients.ToList();
                        this.ModelState.AddModelError("ExpirationDate", "Datum vypršení platnosti musí být později než datum uzavření!");
                        return View(contract);
                    }
                    if (contract.TerminationDate != null)
                    {
                        if (!(contract.TerminationDate > contract.ClosureDate && contract.TerminationDate < contract.ExpirationDate))
                        {
                            ViewData["Advisors"] = _context.Advisors.ToList();
                            ViewData["Clients"] = _context.Clients.ToList();
                            this.ModelState.AddModelError("TerminationDate", "Datum ukončení smlouvy musí být mezi datumem uzavření smlouvy a datumem vypršení platnosti!!");
                            return View(contract);
                        }
                    }
                    
                    _context.Entry(contract).State = EntityState.Modified;
                    //_context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.Id))
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
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include("Client")
                .Include("Advisors")
                .Include("ContractManager")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ContractExists(int id)
        {
            return _context.Contracts.Any(e => e.Id == id);
        }
    }
}
