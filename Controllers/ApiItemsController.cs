﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiItemsController : ControllerBase
    {
        private readonly ApiContext _context;

        public ApiItemsController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/ApiItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiItem>>> GetApiItems()
        {
            return await _context.ApiItems.ToListAsync();
        }

        // GET: api/ApiItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiItem>> GetApiItem(int id)
        {
            var apiItem = await _context.ApiItems.FindAsync(id);

            if (apiItem == null)
            {
                return NotFound();
            }

            //text file append

            StreamWriter sw = new StreamWriter("Response.txt", true);
            sw.WriteLine(apiItem);
            sw.Dispose();

            return apiItem;
        }

        // PUT: api/ApiItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApiItem(int id, ApiItem apiItem)
        {
            if (id != apiItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(apiItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApiItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApiItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiItem>> PostApiItem(ApiItem apiItem)
        {
            _context.ApiItems.Add(apiItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetApiItem), new { id = apiItem.Id }, apiItem); 
        }

        // DELETE: api/ApiItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApiItem(int id)
        {
            var apiItem = await _context.ApiItems.FindAsync(id);
            if (apiItem == null)
            {
                return NotFound();
            }

            _context.ApiItems.Remove(apiItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApiItemExists(int id)
        {
            return _context.ApiItems.Any(e => e.Id == id);
        }

        
    }
}
