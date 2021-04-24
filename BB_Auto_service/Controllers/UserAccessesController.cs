using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Infrastructure.Data;
using Gateway.Dtos.UserAccess;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserAccessesController : ControllerBase
    {
        private readonly IdentityDbContext _context;

        public UserAccessesController(IdentityDbContext context)
        {
            _context = context;
        }

        // GET: api/UserAccesses
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAccess(string id)
        {
            var _access = await _context.UserAccess.Where(c => c.IdUser == id).ToListAsync();
            //var access = new AccessItemDto
            //{
            //    Bank = 0,
            //    Host = 0,
            //    Magasin = 0,
            //    Merchant = 0,
            //    Terminal = 0,
            //    Tpe = 0
            //};

            //if (_access.Count > 0)
            //{
            //    foreach (var item in _access)
            //    {
            //        string caseSwitch = item.IdAccessView;
            //        switch (caseSwitch)
            //        {
            //            case "Bank":
            //                access.Bank = item.ValueUserAccess;
            //                break;
            //            case "Host":
            //                access.Host = item.ValueUserAccess;
            //                break;
            //            case "Magasin":
            //                access.Magasin = item.ValueUserAccess;
            //                break;
            //            case "Merchant":
            //                access.Merchant = item.ValueUserAccess;
            //                break;
            //            case "Terminal":
            //                access.Terminal = item.ValueUserAccess;
            //                break;
            //            case "Tpe":
            //                access.Tpe = item.ValueUserAccess;
            //                break;
            //        }
            //    }
            //}

          
            var access = new Dictionary<string, int>()
            {
                { "Bank", 0},
                { "Host", 0},
                { "Magasin", 0},
                { "Merchant", 0},
                { "Terminal", 0},
                { "Tpe", 0},
                { "Version", 0},
                { "Suivie", 0}
            };

            if (_access.Count < 1)
                return Ok(access);

            foreach (var item in _access)
            {
                access[item.IdAccessView] = item.ValueUserAccess;
            }
            return Ok(access);
        }

       

       


        // PUT: api/UserAccesses/5
        [HttpPost("{id}")]
        public async Task<IActionResult> PutUserAccess(string id, Dictionary<string,int> permissions)
        {
            var oldAccess = await _context.UserAccess.Where(c => c.IdUser == id).ToListAsync();

            try
            {
                if (oldAccess != null)
                {
                    _context.UserAccess.RemoveRange(oldAccess);
                    await _context.SaveChangesAsync();
                }

                var userAccess = new List<UserAccess>();
                foreach(KeyValuePair<string,int> permission in permissions)
                {
                    userAccess.Add(
                        new UserAccess
                        {
                            IdUser=id,
                            IdAccessView= char.ToUpper(permission.Key[0]) + permission.Key.Substring(1),
                            ValueUserAccess=permission.Value

                        }
                        );
                }
                
                await _context.UserAccess.AddRangeAsync(userAccess);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }

           
        }


    }
}
