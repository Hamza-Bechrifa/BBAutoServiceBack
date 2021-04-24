using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BB_Auto_service.BBAutoServiceModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BB_Auto_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailleOrsController : ControllerBase
    {
        private readonly bbAutoServiceContext _context;

        public DetailleOrsController(bbAutoServiceContext context)
        {
            _context = context;
        }

        // GET: api/DetailleOrs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<DetailleOr>>> GetDetailleOr(int id)
        {
            var detailleOr = _context.DetailleOr.Where(
                d=>d.OrdreDeReparation == id
                );

            if (detailleOr == null)
            {
                return NotFound();
            }

            return detailleOr.ToList();
        }

    }
}
