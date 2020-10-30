using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MyBundle.Data;
using MyBundle.Models;

namespace MyBundle.Controllers
{
    public class ProductsController : ApiController
    {
        private MyBundleContext db = new MyBundleContext();

        // get all active products
        // GET: apo/Products      
        // get all active products which name contains 'CU'
        // GET: apo/Products?name=CU     
        // get all acitve products ordered by Price asc/desc
        // GET: apo/Products?orderbyPrice=asc 
        // get all active products which name contains 'CU' ordered by price asc/desc
        // GET: apo/Products?name=CU&orderbyPrice=asc
        public IQueryable<Product> GetProducts(string name =null, string orderByPrice = null)   
        {
            
            if (name == null && orderByPrice == "asc")
            {
                return db.Products.Where(tt => tt.Active == 1).OrderBy(tt => tt.Price);
            }
            else if (name == null && orderByPrice == "desc")
            {
                return db.Products.Where(tt => tt.Active == 1).OrderByDescending(tt => tt.Price);
            }
            else if (name != null && orderByPrice == null)
            {
                return db.Products.Where(tt => tt.Active == 1).Where(tt => tt.Name.Contains(name));
            }
            else if (name != null && orderByPrice == "asc")
            {
                return db.Products.Where(tt => tt.Active == 1).Where(tt => tt.Name.Contains(name)).OrderBy(tt => tt.Price);
            }
            else if (name != null && orderByPrice == "desc")
            {
                return db.Products.Where(tt => tt.Active == 1).Where(tt => tt.Name.Contains(name)).OrderByDescending(tt => tt.Price);
            }
            else
            {
                return db.Products.Where(tt => tt.Active == 1);
            }
            
            
        }

        // get all active products with code = 1
        // GET: apo/Products?code=1
        public IQueryable<Product> GetProducts(int code)
        {
                return db.Products.Where(tt => tt.Code == code && tt.Active == 1);
            
        }
        // get all active products with price = 10.0
        // GET: apo/Products?price=10.0
        public IQueryable<Product> GetProducts(decimal price)
        {
            return db.Products.Where(tt => tt.Price.Equals(price) && tt.Active == 1);

        }




        // deactivate product by name
        // PUT: api/Products?deactByName=CU
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(string deactByName, Product product)
        {
            // if Name doesn't exist throw Not Found
            if (db.Products.Any(tt => tt.Name == deactByName) == false)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           var UpdatedProduct = db.Products.Where(tt => tt.Name == deactByName).First<Product>();
          
           
            UpdatedProduct.Active = 0 ;
           
            var id = UpdatedProduct.Id;
         
            db.Entry(UpdatedProduct).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
         
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //Check if Product Name exists
            if (db.Products.Where(tt => tt.Name == product.Name).Cast<Product>().Count()!=0)
            {
                ModelState.AddModelError("Product Name", "Product Name already exists");
                return BadRequest(ModelState);
            }

            //Check if Product Code exists
            if (db.Products.Where(tt => tt.Code == product.Code).Cast<Product>().Count() != 0)
            {
                ModelState.AddModelError("Product Code", "Product Code already exists");
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}