using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCManukauTech.Models.DB;
using Newtonsoft.Json;

namespace MVCManukauTech.Controllers
{
    public class ProductsController : Controller
    {
        private readonly XSpy4CoreContext _context;

        public ProductsController(XSpy4CoreContext context)
        {
            _context = context;
        }

        //// GET: Products
        //public async Task<IActionResult> Index()
        //{
        //    var xSpy4CoreContext = _context.Products.Include(p => p.Category);
        //    return View(await xSpy4CoreContext.ToListAsync());
        //}

        // 20180911 LJB Searchbox
        public IActionResult Index(string searchString)
        {

            string sql = "SELECT * FROM Products WHERE Name LIKE @p0";
            searchString = "%" + searchString + "%";

            string page = Request.Query["page"];

            var products = _context.Products.FromSql(sql, searchString).ToList();
            return View(products.ToList());

            //return View(products);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Category)
                .SingleOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,CategoryId,Name,ImageFileName,UnitCost,Description,IsDownload,DownloadFileName")] Products products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", products.CategoryId);
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.SingleOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", products.CategoryId);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProductId,CategoryId,Name,ImageFileName,UnitCost,Description,IsDownload,DownloadFileName")] Products products)
        {
            if (id != products.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", products.CategoryId);
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Category)
                .SingleOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var products = await _context.Products.SingleOrDefaultAsync(m => m.ProductId == id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(string id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
