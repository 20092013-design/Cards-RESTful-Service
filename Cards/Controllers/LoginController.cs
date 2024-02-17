using Cards.Data;
using Cards.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Configuration;
using System.Data;
using NuGet.Versioning;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Cards.Services;

namespace Cards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDBContext _dbContext;
        // SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings("DBConn").ConnectionStrings);
        IDbConnection conn = DBClient.GetInstance();

        public LoginController(UserManager<ApplicationUser>userManager,RoleManager<IdentityRole>roleManager,IConfiguration configuration, ApplicationDBContext dbcontext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            _dbContext = dbcontext;
        }





        [HttpPost("Login")]
        public IActionResult Login(LoginModel model)
        {
         
            try
            {
                if (ModelState.IsValid)
                {
                    GeneralResultModel genericResult = new GeneralResultModel();

                    var userLogin = new LoginService().UserAuth(model);

                    if (userLogin.Code == ConstantVal.Success)
                    {
                        TokenDetails tokenDetails = new TokenDetails();
                        tokenDetails.Email = model.Email;
                        tokenDetails.RoleID = userLogin.Data.RoleID;

                        string token = CreateToken(tokenDetails);

                        StateViewModel<UserAuthDetails> _stateView = new StateViewModel<UserAuthDetails>();

                        UserAuthDetails userAuthDetails = new UserAuthDetails();
                        userAuthDetails.Email = model.Email;
                        userAuthDetails.RoleID = userLogin.Data.RoleID;
                        userAuthDetails.Token = token;
                        

                        _stateView.Code = ConstantVal.Success;
                        _stateView.Status = ConstantVal.SuccessMsg;
                        _stateView.Message = ConstantVal.SuccessMsg;
                        _stateView.Data = userAuthDetails;


                        return Ok(_stateView);


                       
                    }
                    else
                    {
                        genericResult.Code = ConstantVal.Error;
                        genericResult.Status = ConstantVal.ErrorMsg;
                        genericResult.Message = "Wrong Username or Password";


                        return StatusCode(401, genericResult);

                    }
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    var dt = new
                    {
                        msg = message,
                        code = "10001"
                    };

                   

                    return StatusCode(401, dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [NonAction]
        public string CreateToken(TokenDetails details)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, details.Email),
                new Claim(ClaimTypes.Role, details.RoleID),
            
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }



      

      
    }
}
