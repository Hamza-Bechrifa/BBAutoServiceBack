using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Extensions.Configuration;
using BB_Auto_service.BBAutoServiceModels;
using System.Data.SqlClient;

namespace BB_Auto_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoituresController : ControllerBase
    {
        private readonly bbAutoServiceContext _context;
        private IConfiguration _config;
        public VoituresController(bbAutoServiceContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        // GET: api/Voitures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voiture>>> GetVoiture()
        {
            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];
            string query = "select * from voiture v inner join client c on  v.client = c.id";
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dataTable);
            conn.Close();
            da.Dispose();
            return Ok(dataTable);
        }

        // GET: api/Voitures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Voiture>> GetVoiture(int id)
        {
            var voiture = await _context.Voiture.FindAsync(id);

            if (voiture == null)
            {
                return NotFound();
            }

            return voiture;
        }

        // PUT: api/Voitures/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoiture(int id, Voiture voiture)
        {
            if (id != voiture.Id)
            {
                return BadRequest();
            }

            _context.Entry(voiture).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoitureExists(id))
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

        // POST: api/Voitures
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Voiture>> PostVoiture(Voiture voiture)
        {
            _context.Voiture.Add(voiture);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVoiture", new { id = voiture.Id }, voiture);
        }

        // DELETE: api/Voitures/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Voiture>> DeleteVoiture(int id)
        {
            var voiture = await _context.Voiture.FindAsync(id);
            if (voiture == null)
            {
                return NotFound();
            }

            _context.Voiture.Remove(voiture);
            await _context.SaveChangesAsync();

            return voiture;
        }

        [HttpGet("getByClient/{id}")]
        public async Task<ActionResult<List<Voiture>>> GetVoituresByClient(int id)
        {
            var voiture =  _context.Voiture.Where(x=> x.Client == id).ToList();

            if (voiture == null)
            {
                return NotFound();
            }

            return voiture;
        }

        private bool VoitureExists(int id)
        {
            return _context.Voiture.Any(e => e.Id == id);
        }
    }
}
