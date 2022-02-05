using BB_Auto_service.BBAutoServiceModels;
using BB_Auto_service.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BB_Auto_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevisClientsController : ControllerBase
    {
        private readonly bbAutoServiceContext _context;

        public DevisClientsController(bbAutoServiceContext context)
        {
            _context = context;
        }

        // GET: api/DevisClients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DevisClient>>> GetDevisClient()
        {
            return await _context.DevisClient.ToListAsync();
        }

        // GET: api/DevisClients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DevisClient>> GetDevisClient(int id)
        {
            var devisClient = await _context.DevisClient.FindAsync(id);

            if (devisClient == null)
            {
                return NotFound();
            }

            return devisClient;
        }

        // PUT: api/DevisClients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevisClient(int id, DevisClientDTO devisClientDTO)        
        {
            if (id != devisClientDTO.Id)
            {
                return BadRequest();
            }
            var devisClient = await _context.OrdreDeReparation.FindAsync(id);

            if (devisClient == null)
            {
                return NotFound();
            }
            //update LastClient solde
            Client LastClient = await _context.Client.FindAsync(devisClient.Client);
            if (LastClient != null)
            {
                LastClient.Solde += devisClient.TotalTtc;
                _context.Entry(LastClient).State = EntityState.Modified;
                _context.SaveChanges();
            }

            devisClient.Client = devisClientDTO.Client;
            devisClient.Voiture = devisClientDTO.Voiture;
            devisClient.DateCreation = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            devisClient.DateDocument = devisClientDTO.dateDocument;
            devisClient.Kilometrage = devisClientDTO.Kilometrage;
            devisClient.CommentaireInterne = devisClientDTO.CommentaireInterne;
            devisClient.CommentaireExterne = devisClientDTO.CommentaireExterne;
            _context.Entry(devisClient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DevisClientExists(id))
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
            foreach (var detaille in devisClientDTO.Detaille)
            {
                DetailleDevisClient d = new DetailleDevisClient();
                d.DevisClient = devisClient.Id;
                d.Article = detaille.Article;
                d.PrixHt = detaille.PrixHt;
                d.Quantite = detaille.Quantite;
                d.Remise = detaille.Remise;
                d.Tva = detaille.Tva;
                d.TotalTtc = detaille.TotalTtc;
                TotalHt += (double)detaille.PrixHt * (double)detaille.Quantite;
                totalTTC += (double)detaille.TotalTtc;
                _context.DetailleDevisClient.Add(d);
               

            }
            devisClient.TotalHt = TotalHt;
            devisClient.TotalTtc = totalTTC;
            _context.Entry(devisClient).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/DevisClients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DevisClient>> PostDevisClient(DevisClientDTO devisClient)
        {
            DevisClient data = new DevisClient();
            data.Client = devisClient.Client;
            data.Voiture = devisClient.Voiture;
            data.DateCreation = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            data.DateDocument = devisClient.dateDocument;
            data.Kilometrage = devisClient.Kilometrage;
            data.CommentaireInterne = devisClient.CommentaireInterne;
            data.CommentaireExterne = devisClient.CommentaireExterne;
            _context.DevisClient.Add(data);
            await _context.SaveChangesAsync();
            foreach (DetailleDevisClient detaille in devisClient.Detaille)
            {
                data.TotalHt += (double)detaille.PrixHt * (double)detaille.Quantite;
                data.TotalTtc += (double)detaille.TotalTtc;
                detaille.DevisClient = data.Id;                
                _context.DetailleDevisClient.Add(detaille);

            }
            _context.Entry(data).State = EntityState.Modified;
            await _context.SaveChangesAsync();                        
            return Ok(data);
        }

        // DELETE: api/DevisClients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevisClient(int id)
        {
            var devisClient = await _context.DevisClient.FindAsync(id);
            if (devisClient == null)
            {
                return NotFound();
            }

            _context.DevisClient.Remove(devisClient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("GetDetaille/{id}")]
        public async Task<ActionResult<IEnumerable<DetailleDevisClient>>> GetDetailleDevis(int id)
        {
            var detailleDevisClient = _context.DetailleDevisClient.Where(
                d => d.DevisClient == id
                );

            if (detailleDevisClient == null)
            {
                return NotFound();
            }

            return detailleDevisClient.ToList();
        }

        private bool DevisClientExists(int id)
        {
            return _context.DevisClient.Any(e => e.Id == id);
        }
    }
}
