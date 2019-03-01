using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Product.WebApi.Services;
using Product.WebApi.Extentions;
using System;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.Collections.Generic;
using System.Net;

namespace Product.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductsService _service { get; set; }
        private readonly IMapper _mapper;

        public ProductsController(IProductsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET api/products
        [HttpGet, Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var productList = await _service.GetAll();
                var result = _mapper.Map<IEnumerable<Models.Product>, IEnumerable<Models.ProductDto>>(productList);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} exception");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error");
            }
        }

        // GET api/products/id
        //[HttpGet("{id}", Name = "GetProduct"), Authorize]
        [HttpGet("{id:int:min(1)}", Name = "GetProduct"), Authorize]
        public async Task<IActionResult> GetById([Required]int id)
        {
            try
            {
                var item = await _service.Find(id);

                if (item.IsObjectNull())
                {
                    return NotFound();
                }
                var result = _mapper.Map<Models.ProductDto>(item);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} exception");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error");
            }
        }

        // CREATE api/products POST
        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] Models.ProductDto item)
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

                var currentItem = _mapper.Map<Models.Product>(item);

                await _service.Add(currentItem);

                return CreatedAtRoute("GetProduct", new { Controller = "Products", id = item.ProductId }, item);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name} exception");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error");
            }
        }

        // UPDATE api/products/5 PUT
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] Models.ProductDto item)
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

                var currentItem = _mapper.Map<Models.Product>(item);

                await _service.Update(currentItem);

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
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error");
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
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error");
            }
        }
    }
}