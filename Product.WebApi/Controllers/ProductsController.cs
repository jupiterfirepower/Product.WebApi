using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Product.WebApi.Services;
using Product.WebApi.Extentions;
using System;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Product.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductsService _service { get; set; }

        public ProductsController(IProductsService service)
        {
            _service = service;
        }

        // GET api/products
        [HttpGet, Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var contactList = await _service.GetAll();
                return Ok(contactList);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} exception");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET api/products/id
        [HttpGet("{id}", Name = "GetProduct"), Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _service.Find(id);

                if (item.IsObjectNull())
                {
                    return NotFound();
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} exception");
                return StatusCode(500, "Internal server error");
            }
        }

        // CREATE api/products POST
        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] Models.Product item)
        {
            try
            {
                if (item.IsObjectNull())
                {
                    return BadRequest("Product object is null.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid product object sent from client.");
                }

                await _service.Add(item);

                return CreatedAtRoute("GetProduct", new { Controller = "Products", id = item.ProductId }, item);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} exception");
                return StatusCode(500, "Internal server error");
            }
        }

        // UPDATE api/products/5 PUT
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] Models.Product item)
        {
            try
            {
                if (item.IsObjectNull())
                {
                    return BadRequest("Product object is null.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid product object sent from client.");
                }

                var product = await _service.Find(id);

                if (product.IsObjectNull())
                {
                    return NotFound();
                }

                await _service.Update(item);

                //return NoContent();
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} exception");
                return StatusCode(500, "Internal server error");
            }
            
        }

        // REMOVE api/products/5 DELETE
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _service.Find(id);

                if (product.IsObjectNull())
                {
                    return NotFound();
                }

                await _service.Remove(id);

                //return NoContent();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} exception");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}