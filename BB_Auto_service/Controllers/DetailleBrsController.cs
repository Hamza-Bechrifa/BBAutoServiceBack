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
    public class DetailleBrsController : ControllerBase
    {
        private readonly bbAutoServiceContext _context;

        public DetailleBrsController(bbAutoServiceContext context)
        {
            _context = context;
        }

        // GET: api/DetailleBrs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<DetailleBr>>> GetDetailleBr(int id)
        {
            var detailleBr = _context.DetailleBr.Where(
                d=>d.BonDeReception == id
                );

            if (detailleBr == null)
            {
                return NotFound();
            }

            return detailleBr.ToList();
        }

    }
}
