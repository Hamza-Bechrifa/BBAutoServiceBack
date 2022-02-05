using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Gateway.Dtos.Account;
using Gateway.Dtos.Organization;
using Gateway.Dtos.UserAccess;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly AppSettings _appSettings;
        private readonly IdentityDbContext _context;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<AppSettings> appSettings, IdentityDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSettings?.Value;
            _context = context;
        }



        //[HttpPost("[action]")]
        [HttpPost]
        [Route("Register")]
        [Authorize]
        //POST : /api/Account/Register
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            //var organization = new Organization();




            var applicationUser = new User
            {
                UserName = model?.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                LastConnection = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            using (var trans = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            {
                try
                {
                    model.Password = RandomPassword();//GenerateRandomPassword();
                    var result = await _userManager.CreateAsync(applicationUser, model.Password);


                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(applicationUser, model.Role);

                        // add organism and userorganism
                        var orgs = model.OrganizationsId;
                        foreach (string orgId in orgs)
                        {
                            if (orgId != "MSS" && orgId != "SMT")
                            {

                                var org = _context.Organization.Find(orgId);


                                if (org == null)
                                {
                                    org = new Organization
                                    {
                                        Id = orgId,
                                        OrganismTypeId = model.Type
                                    };
                                   
                                    _context.Organization.Add(org);
                                }
                            }


                            var userorg = new UserOrganism
                            {
                                OrganizationId = orgId,
                                UserId = applicationUser.Id

                            };
                            _context.UserOrganisms.Add(userorg);

                            await _context.SaveChangesAsync();

                        }// end add organism and userorganism


                        // send confirmation email
                        //1- confirmation link
                        //send mail username & password
                        var url = "";
                        if (model.Role == "MERCHANT" || model.Role == "MAGASIN")
                            url = _appSettings.UrlLoginMerchant;
                        else
                            url = _appSettings.UrlLogin;
                        var message = "<div style=''>Bonjour <strong>" + applicationUser.UserName + "</strong>," + " <p> Bienvenue sur la plateforme Gateway Solutions , un compte associé à votre adresse mail a été crée.</p> <p> Votre nom d'utilisateur est : <strong>" + applicationUser.UserName + " </strong><br> Votre mot de passe est : <strong>" + model.Password + "</strong></p>" + " <p><a href='" + url + "'>Cliquez ici pour login</a>.</p></div>";
                        var subject = "Activation de votre compte ";
                        try
                        {
                            SendEmail(applicationUser.Email, message, subject);

                            trans.Commit();

                        }
                        catch (Exception ex)
                        {
                            List<string> errors = new List<string>();

                            errors.Add("L'envoi d'emails n'est pas possible");
                            return BadRequest(new JsonResult(errors));
                        }
                        // end send mail
                        return Ok(new { username = applicationUser.UserName, email = applicationUser.Email, status = 1, message = " Registration Successfull" });
                    }
                    else
                    {
                        List<string> errors = new List<string>();
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                            errors.Add(error.Description);
                        }
                        return BadRequest(new JsonResult(errors));
                    }

                    //return Ok(result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [HttpPost]
        [Route("Login")]
        //POST : /api/Account/Login
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {

            try
            {
                //var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,false,false);
                //if (result.Succeeded)

                // var user = await _userManager.FindByNameAsync(model.UserName);
                var user = _userManager.Users.Include(c => c.UserRoles).ThenInclude(u => u.Role).Where(c => c.UserName == model.UserName).FirstOrDefault();
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {

                    if (!user.LockoutEnabled)
                        return BadRequest(new { message = "Compte Desactivé" });
                    List<OrganizationListDto> listorganizations = await _context.UserOrganisms.Where(c => c.UserId == user.Id).Include(c => c.Organization).Select(s => new OrganizationListDto {id = s.OrganizationId, name = s.Organization.Name }).ToListAsync();
                    // var ListorgsName = _context.Organization.Find(user.OrganizationId).Name;
                    
                    var token = await GenerateJwtToken(user, listorganizations);
                    // update last connection

                    if (!user.EmailConfirmed)
                        user.EmailConfirmed = true;

                    user.LastConnection = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                    // generate token
                    return token;
                }
                ModelState.AddModelError("", "Le nom d'utilisateur ou le mot de passe n'est pas juste!");
                return Unauthorized(new { message = "Le nom d'utilisateur ou le mot de passe n'est pas juste!" });
                //return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("Profile")]
        [Authorize]

        //POST : /api/Account/Profile
        public async Task<IActionResult> Profile()
        {
            var userId = User.GetUserId();
            if (userId == null)
                return BadRequest("Utilisateur introuvable!");
            // var _user = await _userManager.FindByNameAsync(userId);

            var user = await _userManager.Users
                 .Select(c => new UserDetailsDto
                 {
                     UserName = c.UserName,
                     FirstName = c.FirstName,
                     LastName = c.LastName,
                     Email = c.Email,
                     PhoneNumber = c.PhoneNumber,
                 })
                //.Include(c => c.UserRoles)
                //.ThenInclude(u => u.Role)
                .Where(c => c.UserName == userId)
                .FirstOrDefaultAsync();


            //var _role = await _userManager.GetRolesAsync(_user);
            //var user = new UserDetailsDto
            //{
            //    UserName = _user.UserName,
            //   FirstName= _user.FirstName,
            //   LastName=_user.LastName,
            //  // LastConnection= _user.LastConnection,
            //   Email=_user.Email,
            //    PhoneNumber = _user.PhoneNumber,

            //    //Role = _role.FirstOrDefault()

            //};

            return Ok(user);
        }

        [HttpGet]
        [Route("Users/{id}")]
        [Authorize]
        //POST : /api/Account/Users
        public async Task<IActionResult> GetAllUsers(string id)
        {
            try
            {
                var userName = User.GetUserId();

                if (id == null || userName == null)
                    return BadRequest();

                var org = _context.Organization.Find(id);
                if (org == null)
                    return BadRequest();
                List<User> _users = new List<User>();
                if (org.Id.ToLower() == "mss" || org.Id.ToLower() == "smt")
                {

                    _users = _userManager.Users.Include(e => e.UserRoles).ThenInclude(e => e.Role).Include(e => e.UserOrganizations).ThenInclude(e => e.Organization).Where(c => c.UserName != userName).OrderBy(c => c.LastName).ToList();
                }
                else
                {
                    _users = _userManager.Users.Include(e => e.UserRoles).ThenInclude(e => e.Role).Include(e => e.UserOrganizations).ThenInclude(e => e.Organization).Where(c => (c.UserOrganizations.Select(e => e.Organization.OrganismRefId).Contains(id)) && c.UserName != userName).OrderBy(c => c.LastName).ToList();

                }

                var users = new List<UserListDto>();
                foreach (var item in _users)
                {
                    var roles = await _userManager.GetRolesAsync(item);
                    var user = new UserListDto
                    {
                        Id = item.Id,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        PhoneNumber = item.PhoneNumber,
                        UserName = item.UserName,
                        Email = item.Email,
                        Role = roles.FirstOrDefault(),
                        EmailConfirmed = item.EmailConfirmed,
                        Organism = item.UserOrganizations?.FirstOrDefault()?.Organization?.Name,
                        LockoutEnabled = item.LockoutEnabled
                    };
                    users.Add(user);
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Superviseur")]
        //POST : /api/Account/Users
        public async Task<IActionResult> GetSuperviseur(string orgsStr)
        {
            var orgs = JsonConvert.DeserializeObject<List<OrganizationListDto>>(orgsStr);
            var userName = User.GetUserId();

            if (orgs == null)
                return BadRequest();

            //var org = _context.Organization.Find(id);
            //if (org == null)
            //    return BadRequest();

            //var _superviseurs = await _userManager.GetUsersInRoleAsync("SUPERVISEUR");

            //if (orgs.FirstOrDefault().name != null && orgs.FirstOrDefault().name.ToLower() == "mss")
            //{
            var superviseurs = await _userManager.Users
                                .Include(c => c.UserRoles)
                                .ThenInclude(u => u.Role)
                                .Where(c => c.UserRoles.FirstOrDefault().Role.NormalizedName == "SUPERVISEUR")
                                .Select(s => new
                                {
                                    Id = s.Id,
                                    FirstName = s.FirstName,
                                    LastName = s.LastName,
                                    UserName = s.UserName,

                                })
                                .ToListAsync();
            /*.Where(c => c.UserRoles.FirstOrDefault().Role.ToString() == "SUPERVISEUR").Select(s => new
             {
                 Id = s.Id,
                 FirstName = s.FirstName,
                 LastName = s.LastName,
                 UserName = s.UserName,

             }).ToListAsync();


         //}*/

            if (orgs.FirstOrDefault().name != null && orgs.FirstOrDefault().name.ToLower() != "mss")
            {
                superviseurs = await _userManager.Users
                               .Include(c => c.UserOrganizations)
                               .Include(c => c.UserRoles)
                               .ThenInclude(u => u.Role)
                               .Where(c => c.UserRoles.FirstOrDefault().Role.NormalizedName == "SUPERVISEUR" && c.UserOrganizations.FirstOrDefault().OrganizationId == orgs.FirstOrDefault().id)
                               .Select(s => new
                               {
                                   Id = s.Id,
                                   FirstName = s.FirstName,
                                   LastName = s.LastName,
                                   UserName = s.UserName,

                               }).ToListAsync();


            }
            return Ok(superviseurs);



        }


        //[HttpGet]
        //[Route("Users/{id}")]
        ////POST : /api/Account/Users
        //public IActionResult GetUsersByOrganization(int id=0)
        //{
        //    List<User> _users = new List<User>();

        //    _users= _userManager.Users.Where(c => c.OrganizationId == id).ToList();

        //    var users = new List<UserListDto>();
        //    _users.ForEach(item => users.Add(
        //        new UserListDto
        //        {
        //            Id = item.Id,
        //            FirstName = item.FirstName,
        //            LastName = item.LastName,
        //            PhoneNumber = item.PhoneNumber,
        //            UserName = item.UserName
        //        }
        //        ));
        //    return Ok(users);
        //}
        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize]
        //POST : /api/Account/delete
        //[Authorize(Roles = "Admin,Directeur technique,Responsable RH")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return BadRequest("Utilisateur introuvable!");
            try
            {
                var res = _userManager.DeleteAsync(user);
                return Ok(1);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpPost]
        [Route("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            var user = await _userManager.FindByNameAsync(User.GetUserId());
            if (user == null)
                return BadRequest("Utilisateur introuvable!");

            if (await _userManager.CheckPasswordAsync(user, model.OldPassword))
            {
                // compute the new hash string
                var newPassword = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
                user.PasswordHash = newPassword;
                var res = await _userManager.UpdateAsync(user);

                return Ok(res);
            }
            return BadRequest("Le mot de passe est incorrect");

        }
        [HttpPost]
        [Route("UpdateProfile")]
        [Authorize]
        //[Authorize]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto model)
        {
            var user = await _userManager.FindByNameAsync(User.GetUserId());
            if (user == null)
                return BadRequest("Utilisateur introuvable!");


            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            var res = await _userManager.UpdateAsync(user);

            return Ok(res);


        }
        [HttpPost]
        [Route("UpdateUser/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string id, UpdateProfileDto model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest("Utilisateur introuvable!");


            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            var res = await _userManager.UpdateAsync(user);

            return Ok(res);


        }
        [HttpPost]
        [Route("RecoverPassword")]
        [Authorize (Roles = "ADMIN ORGANISME")]
        //POST : /api/Account/RecoverPassword
        public async Task<IActionResult> RecoverPassword(string userName)
        {
            if (userName == null)
                return BadRequest("UserName ou Email est requis");
            User user = null;
            if (userName.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(userName);
            }
            else
            {
                user = await _userManager.FindByNameAsync(userName);

            }
            if (user == null)
                return BadRequest("Utilisateur introuvable!");

            // compute the new hash string
            var newPassword = "Stvcqjvd123+";
            var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);
            user.PasswordHash = newPasswordHash;
            var res = await _userManager.UpdateAsync(user);
            //var message = "<div style=''>Bonjour <strong>" + user.UserName + "</strong>," + " <p> Bienvenue sur la plateforme MSS Intranet , votre mot de passe a été changé.</p> <p> Votre nom d'utilisateur est : <strong>" + user.UserName + " </strong><br> Votre nouveau mot de passe  est : <strong>" + newPassword + "</strong></p>" + " <p><a href='" + _appSettings.UrlLogin + "'>Cliquez ici pour login</a>.</p></div>";
            //var subject = "Récupération mot de passe";
            //SendEmail(user.Email, message, subject);
            return Ok(res);

        }

        [HttpPost]
        [Route("Lockout")]
        //POST : /api/Account/RecoverPassword
        public async Task<IActionResult> Lockout(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest();


            user.LockoutEnabled = !user.LockoutEnabled;

            await _userManager.UpdateAsync(user);
            return Ok();

        }
        //==== access users========================================================
        [HttpPost]
        [Route("Access")]
        [Authorize]
        //POST : /api/Account/ChangePassword
        public async Task<IActionResult> UpdateRoles(UpdateRolesDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return BadRequest("Utilisateur introuvable!");

            IdentityResult res;
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count > 0)
                res = await _userManager.RemoveFromRolesAsync(user, roles);

            if (model.roles.Count > 0)
                res = await _userManager.AddToRolesAsync(user, model.roles);

            return Ok(model.roles);
        }
        [HttpGet]
        [Route("Roles")]
        //POST : /api/Account/Users
        public IActionResult GetAllRoles()
        {
            var _roles = _roleManager.Roles.ToList();
            var roles = new List<string>();
            foreach (var item in _roles)
            {
                roles.Add(
                    item.Name
                    );
            }
            return Ok(roles);
        }

        [HttpGet]
        [Route("Roles/{id}")]
        //POST : /api/Account/Users
        public async Task<IActionResult> GetRolesForUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }
        //
        #region HELPERS
        [NonAction]
        private async Task<IActionResult> GenerateJwtToken(User user, List<OrganizationListDto> listorganizations)
        {
            var _access = _context.UserAccess.Where(c => c.IdUser == user.Id).ToList();
             //var roles = await _userManager.GetRolesAsync(user);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret));
            double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);
           
                var accessList = new Dictionary<string, int>()
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

            if (_access.Count > 0)
            {
                foreach (var item in _access)
                {
                    accessList[item.IdAccessView] = item.ValueUserAccess;
                }
            }

          
            // token description
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
                        //new Claim("UserID",user.Id)
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Role, user.UserRoles.FirstOrDefault().Role.ToString()),
                        new Claim("LoggedOn", DateTime.Now.ToString()),
            }),
                //SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["ApplicationSettings:JWT_Secret"])), SecurityAlgorithms.HmacSha256Signature),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _appSettings.Site,
                Audience = _appSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return Ok(new { token = token, expiration = securityToken.ValidTo, username = user.UserName, lastConnection=user.LastConnection ,userRole = user.UserRoles.FirstOrDefault().Role.ToString(), organization= listorganizations, access= accessList, OrganisationType=_appSettings.OrganisationType });
        }
        // [Route("RandomPassword")]
        [NonAction]
        public string RandomPassword(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(2, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        [NonAction]
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generate a random string with a given size and case.   
        // If second parameter is true, the return string is lowercase  
        [NonAction]
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        /// <summary>
        /// Generates a Random Password
        /// respecting the given strength requirements.
        /// </summary>
        /// <param name="opts">A valid PasswordOptions object
        /// containing the password strength requirements.</param>
        /// <returns>A random password</returns>
        [NonAction]
        public string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = false,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
        "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
        "abcdefghijkmnopqrstuvwxyz",    // lowercase
        "0123456789",                   // digits
        "!@$?_-"                        // non-alphanumeric
          };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
        [NonAction]
        private void SendEmail(string email, string messageBody, string subject)
        {
            SmtpClient client = new SmtpClient(_appSettings.Host, _appSettings.EmailPort);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_appSettings.EmailSender, _appSettings.Password);
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(email);
            mailMessage.From = new MailAddress(_appSettings.EmailSender);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = messageBody;
            client.Send(mailMessage);
        }
        #endregion
    }
}