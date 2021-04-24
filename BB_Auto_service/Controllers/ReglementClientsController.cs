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
    public class ReglementClientsController : ControllerBase
    {
        private readonly bbAutoServiceContext _context;
        private IConfiguration _config;

        public ReglementClientsController(bbAutoServiceContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        // GET: api/ReglementClients
        [HttpGet]
        public async Task<ActionResult<DataTable>> GetReglementClient()
        {
            //return await _context.ReglementClient.ToListAsync();

            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];//"Server=TEC-HAMZAB\\SQLExpress;DataBase=bbAutoService;User Id =MssUsr; Password=abc.123";

            string query =
                @"select * from ReglementClient rc
                LEFT JOIN Client cli on   rc.client = cli.id ";

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

        // GET: api/ReglementClients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReglementClient>> GetReglementClient(int id)
        {
            var reglementClient = await _context.ReglementClient.FindAsync(id);

            if (reglementClient == null)
            {
                return NotFound();
            }

            return reglementClient;
        }

        // PUT: api/ReglementClients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReglementClient(int id, ReglementClient reglementClient)
        {
            if (id != reglementClient.Id)
            {
                return BadRequest();
            }
            var lastreglementClient = await _context.ReglementClient.FindAsync(id);
            if (reglementClient.DateOperation == null)
                reglementClient.DateOperation = lastreglementClient.DateOperation;
           
            _context.Entry(reglementClient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReglementClientExists(id))
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

        // POST: api/ReglementClients
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ReglementClient>> PostReglementClient(ReglementClient reglementClient)
        {
            reglementClient.DateOperation = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            if (string.IsNullOrEmpty(reglementClient.DateReglement))
            reglementClient.DateReglement = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            _context.ReglementClient.Add(reglementClient);
            var client = _context.Client.Find(reglementClient.Client);
            client.Solde += reglementClient.Montant;
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReglementClient", new { id = reglementClient.Id }, reglementClient);
        }

        // DELETE: api/ReglementClients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ReglementClient>> DeleteReglementClient(int id)
        {
            var reglementClient = await _context.ReglementClient.FindAsync(id);
            if (reglementClient == null)
            {
                return NotFound();
            }

            _context.ReglementClient.Remove(reglementClient);
            await _context.SaveChangesAsync();

            return reglementClient;
        }

        private bool ReglementClientExists(int id)
        {
            return _context.ReglementClient.Any(e => e.Id == id);
        }
    }
}
