using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BB_Auto_service.Dto;
using System.Data;
using Microsoft.Extensions.Configuration;
using BB_Auto_service.BBAutoServiceModels;
using System.Data.SqlClient;

namespace BB_Auto_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdreDeReparationsController : ControllerBase
    {
        private readonly bbAutoServiceContext _context;
        private IConfiguration _config;

        public OrdreDeReparationsController(bbAutoServiceContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<DataTable>> GetOrdreDeReparation()
        {

            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];//"Server=TEC-HAMZAB\\SQLExpress;DataBase=bbAutoService;User Id =MssUsr; Password=abc.123";

            string query = "" +
                "SELECT ors.id,cli.nomPrenom client,voiture.matricule voiture,dateDocument,dateCreation, ors.kilometrage, totalHt, totalTtc, resteAPaye " +
                "FROM OrdreDeReparation ors " +
                "LEFT JOIN Client cli on   ors.client = cli.id " +
                "LEFT JOIN Voiture voiture ON ORS.voiture = VOITURE.id";

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



        // GET: api/OrdreDeReparations
        [HttpPost("list")]
        public async Task<ActionResult<DataTable>> GetOrdreDeReparation(FiltresDTO filtres)
        {
            if (string.IsNullOrEmpty(filtres.DateDebut))
                filtres.DateDebut = "0000-00-00";
         
            if (string.IsNullOrEmpty(filtres.DateFin))
                filtres.DateFin = "9999-99-99";


            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];//"Server=TEC-HAMZAB\\SQLExpress;DataBase=bbAutoService;User Id =MssUsr; Password=abc.123";

            string query = "" +
                "SELECT ors.id,cli.nomPrenom client,voiture.matricule voiture,dateDocument,dateCreation, ors.kilometrage, totalHt, totalTtc, resteAPaye " +
                "FROM OrdreDeReparation ors "+
                "LEFT JOIN Client cli on   ors.client = cli.id " +
                "LEFT JOIN Voiture voiture ON ORS.voiture = VOITURE.id " +
                "where SUBSTRING (ors.dateCreation,1,10) between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "' ";

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

        // GET: api/OrdreDeReparations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetOrdreDeReparation(int id)
        {

            return Ok(_context.OrdreDeReparation.Find(id));
            DataTable dataTable = new DataTable();
            //string connString = _config["ConnectionStrings:bbAutoServiceConnection"];//"Server=TEC-HAMZAB\\SQLExpress;DataBase=bbAutoService;User Id =MssUsr; Password=abc.123";

            //string query = "" +
            //    "SELECT ors.id,cli.id client,cli.nomPrenom clientNomPrenom,voiture.id voiture,voiture.matricule matriculeVoiture,dateDocument,dateCreation, ors.kilometrage, totalHt, totalTtc, resteAPaye " +
            //    "FROM OrdreDeReparation ors " +
            //    "LEFT JOIN Client cli on   ors.client = cli.id " +
            //    "LEFT JOIN Voiture voiture ON ORS.voiture = VOITURE.id where ors.id = " + id;

            //SqlConnection conn = new SqlConnection(connString);
            //SqlCommand cmd = new SqlCommand(query, conn);
            //conn.Open();

            //SqlDataAdapter da = new SqlDataAdapter(cmd);

            //da.Fill(dataTable);
            //conn.Close();
            //da.Dispose();

            //if (dataTable.Rows.Count == 0)
            //{
            //    return NotFound();
            //}

            //return dataTable;
        }

        // PUT: api/OrdreDeReparations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrdreDeReparation(int id, OrdreDeReparationDTO ordreDeReparationDTO)
        {
            if (id != ordreDeReparationDTO.Id)
            {
                return BadRequest();
            }
            var ordreDeReparation = await _context.OrdreDeReparation.FindAsync(id);

            if (ordreDeReparation == null)
            {
                return NotFound();
            }
            //update LastClient solde
            Client LastClient = await _context.Client.FindAsync(ordreDeReparation.Client);
            if (LastClient != null)
            {
                LastClient.Solde += ordreDeReparation.TotalTtc;
                _context.Entry(LastClient).State = EntityState.Modified;
                _context.SaveChanges();
            }

            ordreDeReparation.Client = ordreDeReparationDTO.Client;
            ordreDeReparation.Voiture = ordreDeReparationDTO.Voiture;
            ordreDeReparation.DateCreation = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            ordreDeReparation.DateDocument = ordreDeReparationDTO.dateDocument;
            ordreDeReparation.Kilometrage = ordreDeReparationDTO.Kilometrage;
            ordreDeReparation.CommentaireInterne = ordreDeReparationDTO.CommentaireInterne;
            ordreDeReparation.CommentaireExterne = ordreDeReparationDTO.CommentaireExterne;
            _context.Entry(ordreDeReparation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdreDeReparationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var Todelete = _context.DetailleOr.Where(
                o => o.OrdreDeReparation == id);
            foreach (DetailleOr detailleOr in Todelete)
            {
                Article article = await _context.Article.FindAsync(detailleOr.Article);
                article.StockReel += detailleOr.Quantite;
                _context.Entry(article).State = EntityState.Modified;
                _context.DetailleOr.Remove(detailleOr);
            }
            double totalTTC = 0;
            double TotalHt = 0;
            foreach (var detailleOr in ordreDeReparationDTO.DetailleOr)
            {
                DetailleOr d = new DetailleOr();
                d.OrdreDeReparation = ordreDeReparation.Id;
                d.Article = detailleOr.Article;
                d.PrixHt = detailleOr.PrixHt;
                d.Quantite = detailleOr.Quantite;
                d.Remise = detailleOr.Remise;
                d.Tva = detailleOr.Tva;
                d.TotalTtc = detailleOr.TotalTtc;
                TotalHt += (double)detailleOr.PrixHt * (double)detailleOr.Quantite;
                totalTTC += (double)detailleOr.TotalTtc;
                _context.DetailleOr.Add(d);
                Article article = await _context.Article.FindAsync(detailleOr.Article);
                article.StockReel -= detailleOr.Quantite;
                _context.Entry(article).State = EntityState.Modified;

            }
            ordreDeReparation.TotalHt = TotalHt;
            ordreDeReparation.TotalTtc = totalTTC;
            _context.Entry(ordreDeReparation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            //update NewClient solde
            Client NewClient = await _context.Client.FindAsync(ordreDeReparation.Client);
            if (NewClient != null)
            {
                NewClient.Solde -= ordreDeReparation.TotalTtc;
                _context.Entry(NewClient).State = EntityState.Modified;
                _context.SaveChanges();
            }
            //update kilometrage voiture 
            Voiture voiture = _context.Voiture.Find(ordreDeReparation.Voiture);
            if (voiture != null && ordreDeReparation.Kilometrage > voiture?.Kilometrage)
                voiture.Kilometrage = ordreDeReparation.Kilometrage;

            return NoContent();
        }

        // POST: api/OrdreDeReparations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<OrdreDeReparation>> PostOrdreDeReparation(OrdreDeReparationDTO ordreDeReparationDTO)
        {
            OrdreDeReparation o = new OrdreDeReparation();
            o.Client = ordreDeReparationDTO.Client;
            o.Voiture = ordreDeReparationDTO.Voiture;
            o.DateCreation = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            o.DateDocument = ordreDeReparationDTO.dateDocument;
            o.Kilometrage = ordreDeReparationDTO.Kilometrage;
            o.CommentaireInterne = ordreDeReparationDTO.CommentaireInterne;
            o.CommentaireExterne = ordreDeReparationDTO.CommentaireExterne;
            _context.OrdreDeReparation.Add(o);
            await _context.SaveChangesAsync();
            foreach (DetailleOr detailleOr in ordreDeReparationDTO.DetailleOr)
            {
                o.TotalHt += (double)detailleOr.PrixHt * (double)detailleOr.Quantite;
                o.TotalTtc += (double)detailleOr.TotalTtc;
                detailleOr.OrdreDeReparation = o.Id;
                Article article = await _context.Article.FindAsync(detailleOr.Article);
                article.StockReel -= detailleOr.Quantite;
                _context.Entry(article).State = EntityState.Modified;
                _context.DetailleOr.Add(detailleOr);

            }
            _context.Entry(o).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            //update Client solde
            Client client = await _context.Client.FindAsync(o.Client);
            client.Solde -= o.TotalTtc;
            _context.Entry(client).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            o.ClientNavigation = null;

            return Ok(o);
        }

        // DELETE: api/OrdreDeReparations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrdreDeReparation>> DeleteOrdreDeReparation(int id)
        {
            var ordreDeReparation = await _context.OrdreDeReparation.FindAsync(id);
            if (ordreDeReparation == null)
            {
                return NotFound();
            }
            var Todelete = _context.DetailleOr.Where(
                o => o.OrdreDeReparation == id);
            foreach (DetailleOr detailleOr in Todelete)
            {
                Article article = await _context.Article.FindAsync(detailleOr.Article);
                article.StockReel += detailleOr.Quantite;
                _context.Entry(article).State = EntityState.Modified;
                _context.DetailleOr.Remove(detailleOr);
            }
            _context.OrdreDeReparation.Remove(ordreDeReparation);
            await _context.SaveChangesAsync();

            //update Client solde
            Client client = await _context.Client.FindAsync(ordreDeReparation.Client);
            client.Solde += ordreDeReparation.TotalTtc;
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return ordreDeReparation;
        }

        [HttpGet("GetByArticle/{id}")]
        public async Task<ActionResult> GetByArticle(int id)
        {

            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];
            string query = "select * from DetailleOr DOR inner join OrdreDeReparation Ordr on  DOR.ordreDeReparation = Ordr.id";
            query += " Where article = " + id;
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            return Ok(dataTable);
        }

        private bool OrdreDeReparationExists(int id)
        {
            return _context.OrdreDeReparation.Any(e => e.Id == id);
        }
    }
}
