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
using BB_Auto_service.Dto;

namespace BB_Auto_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonDeReceptionsController : ControllerBase
    {
        private readonly bbAutoServiceContext _context;
        private IConfiguration _config;

        public BonDeReceptionsController(bbAutoServiceContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        // GET: api/BonDeReceptions
        [HttpGet]
        public async Task<ActionResult<DataTable>> GetBonDeReception()
        {
            var data = await _context.OrdreDeReparation.ToListAsync();
            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];//"Server=TEC-HAMZAB\\SQLExpress;DataBase=bbAutoService;User Id =MssUsr; Password=abc.123";

            string query = "" +
                "select * from BonDeReception brs left join Fournisseur F on brs.fournisseur = f.id";

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

        // GET: api/BonDeReceptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BonDeReception>> GetBonDeReception(int id)
        {
            var bonDeReception = _context.BonDeReception.Find(id);

            if (bonDeReception == null)
            {
                return NotFound();
            }

            return Ok(bonDeReception);
        }

        // PUT: api/BonDeReceptions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBonDeReception(int id, BonDeReceptionDTO bonDeReceptionDTO)
        {
            if (id != bonDeReceptionDTO.Id)
            {
                return BadRequest();
            }
            var bonDeReception = await _context.BonDeReception.FindAsync(id);

            if (bonDeReception == null)
            {
                return NotFound();
            }
            bonDeReception.Code = bonDeReceptionDTO.Code;
            bonDeReception.DateDocument = bonDeReceptionDTO.Date;
            bonDeReception.Fournisseur = bonDeReceptionDTO.Fournisseur;
            bonDeReception.CommentaireInterne = bonDeReceptionDTO.CommentaireInterne;
            bonDeReception.CommentaireExterne = bonDeReceptionDTO.CommentaireExterne;
            _context.Entry(bonDeReception).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BonDeReceptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var Todelete = _context.DetailleBr.Where(
               o => o.BonDeReception == id);
            foreach (DetailleBr detaillebr in Todelete)
            {
                Article article = await _context.Article.FindAsync(detaillebr.Article);
                article.StockReel -= detaillebr.Quantite;
                _context.Entry(article).State = EntityState.Modified;
                _context.DetailleBr.Remove(detaillebr);
            }
            double totalTTC = 0;
            double TotalHt = 0;
            foreach (var detailleBr in bonDeReceptionDTO.DetailleBR)
            {

                detailleBr.BonDeReception = id;
                Article article = await _context.Article.FindAsync(detailleBr.Article);
                article.StockReel += detailleBr.Quantite;
                TotalHt += (double)detailleBr.PrixHt * (double)detailleBr.Quantite;
                totalTTC += (double)detailleBr.TotalTtc;
                _context.Entry(article).State = EntityState.Modified;
                _context.DetailleBr.Add(detailleBr);

            }

            bonDeReception.TotalHt = TotalHt;
            bonDeReception.TotalTtc = totalTTC;
            _context.Entry(bonDeReception).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/BonDeReceptions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BonDeReception>> PostBonDeReception(BonDeReceptionDTO bonDeReceptionDTO)
        {
            BonDeReception bonDeReception = new BonDeReception()
            {
                Code = bonDeReceptionDTO.Code,
                CommentaireInterne = bonDeReceptionDTO.CommentaireInterne,
                CommentaireExterne = bonDeReceptionDTO.CommentaireExterne,
                DateDocument = bonDeReceptionDTO.Date,
                DateCreation = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                Fournisseur = bonDeReceptionDTO.Fournisseur,
                TotalHt = bonDeReceptionDTO.TotalHt,
                TotalTtc = bonDeReceptionDTO.TotalTtc
            };
            _context.BonDeReception.Add(bonDeReception);
            await _context.SaveChangesAsync();
            foreach (DetailleBr detailleBr in bonDeReceptionDTO.DetailleBR)
            {
                bonDeReception.TotalHt += (double)detailleBr.PrixHt * (double)detailleBr.Quantite;
                bonDeReception.TotalTtc += (double)detailleBr.TotalTtc;
                detailleBr.BonDeReception = bonDeReception.Id;
                Article article = await _context.Article.FindAsync(detailleBr.Article);
                article.StockReel += detailleBr.Quantite;
                _context.Entry(article).State = EntityState.Modified;
                _context.DetailleBr.Add(detailleBr);
            }
            _context.Entry(bonDeReception).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            updateFournisseurSolde((int)bonDeReceptionDTO.Fournisseur, bonDeReception.TotalTtc* -1);
            bonDeReception.FournisseurNavigation = null;
            return Ok(bonDeReception);
        }

        // DELETE: api/BonDeReceptions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BonDeReception>> DeleteBonDeReception(int id)
        {
            var bonDeReception = await _context.BonDeReception.FindAsync(id);
            if (bonDeReception == null)
            {
                return NotFound();
            }
            var Todelete = _context.DetailleBr.Where(
               o => o.BonDeReception == id);
            foreach (DetailleBr detaillebr in Todelete)
            {
                Article article = await _context.Article.FindAsync(detaillebr.Article);
                article.StockReel -= detaillebr.Quantite;
                _context.Entry(article).State = EntityState.Modified;
                _context.DetailleBr.Remove(detaillebr);
            }
            _context.BonDeReception.Remove(bonDeReception);
            await _context.SaveChangesAsync();

            return bonDeReception;
        }

        [HttpGet("GetByArticle/{id}")]
        public async Task<ActionResult> GetByArticle(int id)
        {

            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];
            string query = "SELECT   DBR.id, DBR.bonDeReception, DBR.article, DBR.prixHt, DBR.quantite, DBR.remise, DBR.tva, DBR.totalTtc, BR.dateCreation, BR.dateDocument FROM   DetailleBR AS DBR INNER JOIN     BonDeReception AS BR ON DBR.bonDeReception = BR.id";
            query += " Where DBR.article = " + id;
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dataTable);
            conn.Close();
            da.Dispose();


            return Ok(dataTable);
        }

        private bool BonDeReceptionExists(int id)
        {
            return _context.BonDeReception.Any(e => e.Id == id);
        }
        [NonAction]
        public async void updateFournisseurSolde(int idFournisseur, double signedAmount)
        {           
            var fournisseur =  _context.Fournisseur.Find(idFournisseur);
            fournisseur.Solde += signedAmount;
            _context.Entry(fournisseur).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
    }

}
