using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BB_Auto_service.BBAutoServiceModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BB_Auto_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReglementFournisseursController : ControllerBase
    {
        private readonly bbAutoServiceContext _context;
        private IConfiguration _config;

        public ReglementFournisseursController(bbAutoServiceContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }


        // GET: api/ReglementFournisseurs
        [HttpGet]
        public async Task<ActionResult<DataTable>> GetReglementFournisseur()
        {
            //return await _context.ReglementFournisseur.ToListAsync();
            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];//"Server=TEC-HAMZAB\\SQLExpress;DataBase=bbAutoService;User Id =MssUsr; Password=abc.123";

            string query =
                @"select * from [ReglementFournisseur] RF
                LEFT JOIN fournisseur f on   RF.fournisseur = f.id ";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            //    var x = await _context.OrdreDeReparation.Include(c=>c.Client).ToListAsync();
            return dataTable;
        }

        // GET: api/ReglementFournisseurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReglementFournisseur>> GetReglementFournisseur(int id)
        {
            var reglementFournisseur = await _context.ReglementFournisseur.FindAsync(id);

            if (reglementFournisseur == null)
            {
                return NotFound();
            }

            return reglementFournisseur;
        }

        // PUT: api/ReglementFournisseurs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReglementFournisseur(int id, ReglementFournisseur reglementFournisseur)
        {
            if (id != reglementFournisseur.Id)
            {
                return BadRequest();
            }

            _context.Entry(reglementFournisseur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReglementFournisseurExists(id))
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

        // POST: api/ReglementFournisseurs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ReglementFournisseur>> PostReglementFournisseur(ReglementFournisseur reglementFournisseur)
        {
            reglementFournisseur.DateOperation = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            if (string.IsNullOrEmpty(reglementFournisseur.DateReglement))
                reglementFournisseur.DateReglement = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            _context.ReglementFournisseur.Add(reglementFournisseur);
            await _context.SaveChangesAsync();
            updateFournisseurSolde((int)reglementFournisseur.Fournisseur, reglementFournisseur.Montant);
            return CreatedAtAction("GetReglementFournisseur", new { id = reglementFournisseur.Id }, reglementFournisseur);
        }

        // DELETE: api/ReglementFournisseurs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ReglementFournisseur>> DeleteReglementFournisseur(int id)
        {
            var reglementFournisseur = await _context.ReglementFournisseur.FindAsync(id);
            if (reglementFournisseur == null)
            {
                return NotFound();
            }

            _context.ReglementFournisseur.Remove(reglementFournisseur);
            await _context.SaveChangesAsync();

            return reglementFournisseur;
        }

        private bool ReglementFournisseurExists(int id)
        {
            return _context.ReglementFournisseur.Any(e => e.Id == id);
        }
        [NonAction]
        public async void updateFournisseurSolde(int idFournisseur, double signedAmount)
        {
            var fournisseur = _context.Fournisseur.Find(idFournisseur);
            fournisseur.Solde += signedAmount;
            _context.Entry(fournisseur).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
    }
}
