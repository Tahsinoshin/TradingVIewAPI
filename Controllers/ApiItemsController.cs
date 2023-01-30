using System.Globalization;
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
        [HttpGet("alert")]
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
        [HttpPost("/receivenotification", Name = "ReceiveNotification")]
        public async void ReceiveNotification([FromBody]ApiItem apiItem)
        {
            StreamWriter sw = null!;

            try
            {
                sw = new StreamWriter("Messages/DetailedMessage.txt", true);
                await sw.WriteLineAsync(apiItem.time.ToString(CultureInfo.InvariantCulture) + "," + apiItem.message);
            }
            catch(IOException ex)
            {
                sw = new StreamWriter("Messages/ErrorMessage.txt", true);
                await sw.WriteLineAsync(ex.Message);
            }
            catch (Exception ex)
            {
                sw = new StreamWriter("Messages/ErrorMessage.txt", true);
                await sw.WriteLineAsync(ex.Message);
            }
            finally
            {
                await sw.DisposeAsync();
            }
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
