using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BB_Auto_service.Dto;
using System.Data;
using BB_Auto_service.BBAutoServiceModels;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace BB_Auto_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly bbAutoServiceContext _context;
        private IConfiguration _config;

        public ReportController(bbAutoServiceContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }



        [HttpPost("GetTotalpieces")]
        public async Task<ActionResult<DataTable>> GetTotalpieces(FiltresDTO filtres)
        {
            if (string.IsNullOrEmpty(filtres.DateDebut))
                filtres.DateDebut = "0000-00-00";

            if (string.IsNullOrEmpty(filtres.DateFin))
                filtres.DateFin = "9999-99-99";


            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];//"Server=TEC-HAMZAB\\SQLExpress;DataBase=bbAutoService;User Id =MssUsr; Password=abc.123";

            string query = "" +
                "select sum(totalTtc) as 'totalPieces' from DetailleOr where (article in (select id from article a where a.type = 'true') ) " +
                "and ordreDeReparation in (select id from OrdreDeReparation ord where ord.dateCreation " +
                " between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "' )";

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


        [HttpPost("GetTotalMainDoeuvre")]
        public async Task<ActionResult<DataTable>> GetTotalMainDoeuvre(FiltresDTO filtres)
        {
            if (string.IsNullOrEmpty(filtres.DateDebut))
                filtres.DateDebut = "0000-00-00";

            if (string.IsNullOrEmpty(filtres.DateFin))
                filtres.DateFin = "9999-99-99";


            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];//"Server=TEC-HAMZAB\\SQLExpress;DataBase=bbAutoService;User Id =MssUsr; Password=abc.123";

            string query = "" +
                "select sum(totalTtc) as 'totalMainDoeuvre' from DetailleOr where (article in (select id from article a where a.type != 'true') ) " +
                "and ordreDeReparation in (select id from OrdreDeReparation ord where ord.dateCreation " +
                " between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "' )";

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


        [HttpPost("ImpayerClient")]
        public async Task<ActionResult<DataTable>> ImpayerClient(FiltresDTO filtres)
        {
            if (string.IsNullOrEmpty(filtres.DateDebut))
                filtres.DateDebut = "0000-00-00";

            if (string.IsNullOrEmpty(filtres.DateFin))
                filtres.DateFin = "9999-99-99";


            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];//"Server=TEC-HAMZAB\\SQLExpress;DataBase=bbAutoService;User Id =MssUsr; Password=abc.123";

            string query = "" +
                "select sum(solde) as 'impayeClient' from client where solde > 0.1 or solde < -0.1 ";

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


        [HttpPost("articleVenduNonAcheter")]
        public async Task<ActionResult<DataTable>> articleVenduNonAcheter(FiltresDTO filtres)
        {
            if (string.IsNullOrEmpty(filtres.DateDebut))
                filtres.DateDebut = "0000-00-00";

            if (string.IsNullOrEmpty(filtres.DateFin))
                filtres.DateFin = "9999-99-99";


            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];//"Server=TEC-HAMZAB\\SQLExpress;DataBase=bbAutoService;User Id =MssUsr; Password=abc.123";

            string query = "" +
                "select * from ( " +
                "SELECT *,(tab.[ttc achat] *(tab.[qte achat]- tab.[qte vente]))as col1 from ( " +
                "select *  from( " +
                "select a.id, a.reference, a.designation, " +
                "ISNULL((select sum(quantite) from detailleBr dbr where article = a.id and dbr.bonDeReception in (select id from BonDeReception where dateCreation between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "')),0) as 'qte achat',       " +
                "ISNULL((select sum(quantite) from DetailleOr dor where article = a.id and dor.ordreDeReparation in (select id from OrdreDeReparation where dateCreation between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "')),0) as 'qte vente', " +
                "ISNULL((select sum(totalTtc) from detailleBr dbr where article = a.id and dbr.bonDeReception in (select id from BonDeReception where dateCreation between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "')),0) as 'ttc achat',       " +
                "ISNULL((select sum(totalTtc) from DetailleOr dor where article = a.id and dor.ordreDeReparation in (select id from OrdreDeReparation where dateCreation between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "')),0) as 'ttc vente'  " +
                "from article a where type != 'true' ) " +
                "req where req.[qte achat] is not null or req.[qte vente] is not null " +
                ") as tab " +
                ") as tabb where tabb.[qte vente] - tabb.[qte achat] > 0 ";

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


        
        [HttpPost("qteAchatArticles")]
        public async Task<ActionResult<DataTable>> qteAchatArticles(FiltresDTO filtres)
        {
            if (string.IsNullOrEmpty(filtres.DateDebut))
                filtres.DateDebut = "0000-00-00";

            if (string.IsNullOrEmpty(filtres.DateFin))
                filtres.DateFin = "9999-99-99";


            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];//"Server=TEC-HAMZAB\\SQLExpress;DataBase=bbAutoService;User Id =MssUsr; Password=abc.123";

            string query = "" +
                "select *  from(" +
                "select a.id, a.reference, a.designation, " +
                " ISNULL((select sum(quantite) from detailleBr dbr where article = a.id and dbr.bonDeReception in (select id from BonDeReception where dateCreation  between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "')),0) as 'qte achat', " +
                " ISNULL((select sum(quantite) from DetailleOr dor where article = a.id and dor.ordreDeReparation in (select id from OrdreDeReparation where dateCreation  between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "')),0) as 'qte vente', " +
                " ISNULL((select sum(totalTtc) from detailleBr dbr where article = a.id and dbr.bonDeReception in (select id from BonDeReception where dateCreation  between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "')),0) as 'ttc achat', " +
                " ISNULL((select sum(totalTtc) from DetailleOr dor where article = a.id and dor.ordreDeReparation in (select id from OrdreDeReparation where dateCreation  between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "')),0) as 'ttc vente' " +
                " from article a where type != 'true') " +
                " req where req.[qte achat] is not null or req.[qte vente] is not null ";

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


          
        [HttpPost("AcheterNonVendu")]
        public async Task<ActionResult<DataTable>> AcheterNonVendu(FiltresDTO filtres)
        {
            if (string.IsNullOrEmpty(filtres.DateDebut))
                filtres.DateDebut = "0000-00-00";

            if (string.IsNullOrEmpty(filtres.DateFin))
                filtres.DateFin = "9999-99-99";


            DataTable dataTable = new DataTable();
            string connString = _config["ConnectionStrings:bbAutoServiceConnection"];//"Server=TEC-HAMZAB\\SQLExpress;DataBase=bbAutoService;User Id =MssUsr; Password=abc.123";

            string query = "" +
               "select * from ( " +
                "SELECT *,(tab.[ttc achat] *(tab.[qte achat]- tab.[qte vente]))as col1 from (  " +
                "select *  from(   " +
                "select a.id, a.reference, a.designation, " +
                "ISNULL((select sum(quantite) from detailleBr dbr where article = a.id and dbr.bonDeReception in (select id from BonDeReception where dateCreation between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "')),0) as 'qte achat',       " +
                "ISNULL((select sum(quantite) from DetailleOr dor where article = a.id and dor.ordreDeReparation in (select id from OrdreDeReparation where dateCreation between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "')),0) as 'qte vente', " +
                "ISNULL((select sum(totalTtc) from detailleBr dbr where article = a.id and dbr.bonDeReception in (select id from BonDeReception where dateCreation between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "')),0) as 'ttc achat',       " +
                "ISNULL((select sum(totalTtc) from DetailleOr dor where article = a.id and dor.ordreDeReparation in (select id from OrdreDeReparation where dateCreation between '" + filtres.DateDebut + "' and '" + filtres.DateFin + "')),0) as 'ttc vente'  " +
                "from article a where type != 'true' ) " +
                "req where req.[qte achat] is not null or req.[qte vente] is not null " +
                ") as tab " +
                ") as tabb where tabb.[qte vente] - tabb.[qte achat] < 0 ";

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



    }
}
