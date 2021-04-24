using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccessViewsController : ControllerBase
    {
        private readonly IdentityDbContext _context;

        public AccessViewsController(IdentityDbContext context)
        {
            _context = context;
        }

        // GET: api/AccessViews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccessView>>> GetAccessView()
        {
            return await _context.AccessView.ToListAsync();
        }

        // GET: api/AccessViews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccessView>> GetAccessView(string id)
        {
            var accessView = await _context.AccessView.FindAsync(id);

            if (accessView == null)
            {
                return NotFound();
            }

            return accessView;
        }

        // PUT: api/AccessViews/5
        [HttpPost("{id}")]
        public async Task<IActionResult> PutAccessView(string id, AccessView accessView)
        {
            if (id != accessView.IdAccessView)
            {
                return BadRequest();
            }

            _context.Entry(accessView).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccessViewExists(id))
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

        // POST: api/AccessViews
        [HttpPost]
        public async Task<ActionResult<AccessView>> PostAccessView(AccessView accessView)
        {
            _context.AccessView.Add(accessView);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccessView", new { id = accessView.IdAccessView }, accessView);
        }

        // DELETE: api/AccessViews/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AccessView>> DeleteAccessView(string id)
        {
            var accessView = await _context.AccessView.FindAsync(id);
            if (accessView == null)
            {
                return NotFound();
            }

            _context.AccessView.Remove(accessView);
            await _context.SaveChangesAsync();

            return accessView;
        }

        private bool AccessViewExists(string id)
        {
            return _context.AccessView.Any(e => e.IdAccessView == id);
        }
    }
}
