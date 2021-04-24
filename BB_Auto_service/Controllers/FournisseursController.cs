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
    public class FournisseursController : ControllerBase
    {
        private readonly bbAutoServiceContext _context;
        private IConfiguration _config;
        public FournisseursController(bbAutoServiceContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/Fournisseurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fournisseur>>> GetFournisseur()
        {
            return await _context.Fournisseur.ToListAsync();
        }

        // GET: api/Fournisseurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fournisseur>> GetFournisseur(int id)
        {
            var fournisseur = await _context.Fournisseur.FindAsync(id);

            if (fournisseur == null)
            {
                return NotFound();
            }

            return fournisseur;
        }

        // PUT: api/Fournisseurs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFournisseur(int id, Fournisseur fournisseur)
        {
            if (id != fournisseur.Id)
            {
                return BadRequest();
            }

            _context.Entry(fournisseur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FournisseurExists(id))
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

        // POST: api/Fournisseurs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Fournisseur>> PostFournisseur(Fournisseur fournisseur)
        {
            _context.Fournisseur.Add(fournisseur);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FournisseurExists(fournisseur.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFournisseur", new { id = fournisseur.Id }, fournisseur);
        }

        // DELETE: api/Fournisseurs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Fournisseur>> DeleteFournisseur(int id)
        {
            var fournisseur = await _context.Fournisseur.FindAsync(id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            _context.Fournisseur.Remove(fournisseur);
            await _context.SaveChangesAsync();

            return fournisseur;
        }

        [HttpGet("GetMouvement/{id}")]
        public async Task<ActionResult> GetMouvement(int id)
        {

            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];
            string query = "select dateDocument as date, id, totalTtc, 'BR' as TypeDocument "
                    + " from BonDeReception where fournisseur = " + id
                    + " union select dateOperation as date, id, montant, 'RF' as TypeDocument"
                    + " from ReglementFournisseur where fournisseur = " + id
                    + "order by date";
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dataTable);
            conn.Close();
            da.Dispose();


            return Ok(dataTable);
        }


        private bool FournisseurExists(int id)
        {
            return _context.Fournisseur.Any(e => e.Id == id);
        }
    }
}
