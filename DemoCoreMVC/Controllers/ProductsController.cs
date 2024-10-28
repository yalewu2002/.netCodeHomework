using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoCoreMVC.Models;
using Serilog;

namespace DemoCoreMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AdventureWorksLT2022Context _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AdventureWorksLT2022Context context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            Log.Information("Products/Index");
            try
            {
                var adventureWorksLT2022Context = _context.Product.Include(p => p.ProductCategory).Include(p => p.ProductModel);
                return View(await adventureWorksLT2022Context.ToListAsync());

            }
            catch (Exception ex)
            {
                Log.Error(ex, "取得產品清單發生錯誤！");
                return StatusCode(500, "取得產品清單發生錯誤！");
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Log.Information($"Products/Details/{id}");
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _context.Product
                    .Include(p => p.ProductCategory)
                    .Include(p => p.ProductModel)
                    .FirstOrDefaultAsync(m => m.ProductID == id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"取得產品明細發生錯誤！{id}");
                return StatusCode(500, $"取得產品明細發生錯誤！{id}");
            }
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            Log.Information("Products/Create");

            ViewData["ProductCategoryID"] = new SelectList(_context.ProductCategory, "ProductCategoryID", "Name");
            ViewData["ProductModelID"] = new SelectList(_context.ProductModel, "ProductModelID", "Name");
            return View();

        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,rowguid,ModifiedDate")] Product product)
        {
            Log.Information("Products/Create");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"建立新產品發生錯誤！");
                    return StatusCode(500, $"建立新產品發生錯誤！");
                }
            }
            ViewData["ProductCategoryID"] = new SelectList(_context.ProductCategory, "ProductCategoryID", "Name", product.ProductCategoryID);
            ViewData["ProductModelID"] = new SelectList(_context.ProductModel, "ProductModelID", "Name", product.ProductModelID);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Log.Information($"Products/Edit/{id}");
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductCategoryID"] = new SelectList(_context.ProductCategory, "ProductCategoryID", "Name", product.ProductCategoryID);
            ViewData["ProductModelID"] = new SelectList(_context.ProductModel, "ProductModelID", "Name", product.ProductModelID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,rowguid,ModifiedDate")] Product product)
        {
            Log.Information($"Products/Edit/{id}");
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Log.Error(ex, "產品資料更新時發生錯誤");
                    if (!ProductExists(product.ProductID))
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
            ViewData["ProductCategoryID"] = new SelectList(_context.ProductCategory, "ProductCategoryID", "Name", product.ProductCategoryID);
            ViewData["ProductModelID"] = new SelectList(_context.ProductModel, "ProductModelID", "Name", product.ProductModelID);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Log.Information($"Products/Delete/{id}");
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductModel)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Log.Information($"Products/DeleteConfirmed/{id}");

            try
            {
                var product = await _context.Product.FindAsync(id);
                if (product != null)
                {
                    _context.Product.Remove(product);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"刪除產品發生錯誤：{id}");   
                return StatusCode(500, $"刪除產品發生錯誤：");
            }
        }

        private bool ProductExists(int id)
        {
            Log.Information($"ProductExists Product {id}");
            return _context.Product.Any(e => e.ProductID == id);
        }
    }
}
