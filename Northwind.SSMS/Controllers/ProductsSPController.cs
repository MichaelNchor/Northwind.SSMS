using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Northwind.SSMS.Models;

namespace Northwind.SSMS.Controllers
{
    [Route("apiSP/[controller]")]
    [ApiController]
    public class ProductsSPController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public ProductsSPController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/ProductsSP
        [HttpGet]
        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products
                .FromSqlRaw<Product>("GetProductList")
                .ToListAsync();
        }

        // GET: api/ProductsSP/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<Product>> GetProduct(int id)
        {
            var parameter = new SqlParameter("@ProductID", id);

            var productDetails = await Task.Run(() => _context.Products
                            .FromSqlRaw(@"exec GetProductByID @ProductID", parameter).ToListAsync());

            return productDetails;
        }

        // PUT: api/ProductsSP/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> PutProduct(Product product)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@ProductID", product.ProductId));
            parameter.Add(new SqlParameter("@ProductName", product.ProductName));
            parameter.Add(new SqlParameter("@SupplierID", product.SupplierId));
            parameter.Add(new SqlParameter("@CategoryID", product.CategoryId));
            parameter.Add(new SqlParameter("@QuantityPerUnit", product.QuantityPerUnit));
            parameter.Add(new SqlParameter("@UnitPrice", product.UnitPrice));
            parameter.Add(new SqlParameter("@UnitsInStock", product.UnitsInStock));
            parameter.Add(new SqlParameter("@UnitsOnOrder", product.UnitsOnOrder));
            parameter.Add(new SqlParameter("@ReorderLevel", product.ReorderLevel));
            parameter.Add(new SqlParameter("@Discontinued", product.Discontinued));

            var result = await Task.Run(() => _context.Database
           .ExecuteSqlRawAsync(@"exec UpdateProduct @ProductID, @ProductName, @SupplierID, @CategoryID, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued", parameter.ToArray()));

            return Ok(result);
        }

        // POST: api/ProductsSP
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddProduct")]
        public async Task<IActionResult> PostProduct(Product product)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@ProductName", product.ProductName));
            parameter.Add(new SqlParameter("@SupplierID", product.SupplierId));
            parameter.Add(new SqlParameter("@CategoryID", product.CategoryId));
            parameter.Add(new SqlParameter("@QuantityPerUnit", product.QuantityPerUnit));
            parameter.Add(new SqlParameter("@UnitPrice", product.UnitPrice));
            parameter.Add(new SqlParameter("@UnitsInStock", product.UnitsInStock));
            parameter.Add(new SqlParameter("@UnitsOnOrder", product.UnitsOnOrder));
            parameter.Add(new SqlParameter("@ReorderLevel", product.ReorderLevel));
            parameter.Add(new SqlParameter("@Discontinued", product.Discontinued));

            var result = await Task.Run(() => _context.Database
           .ExecuteSqlRawAsync(@"exec AddProduct @ProductName, @SupplierID, @CategoryID, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued", parameter.ToArray()));

            return Ok(result);
        }

        // DELETE: api/ProductsSP/5
        [HttpDelete("DeleteProduct")]
        public async Task<int> DeleteProduct(int id)
        {
            return await Task.Run(() => _context.Database.ExecuteSqlInterpolatedAsync($"DeleteProduct {id}"));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
