using Cards.Data;
using Cards.Models;
using Cards.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using Sieve.Models;
using Sieve.Services;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Cards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        IDbConnection conn = DBClient.GetInstance();

        private readonly IConfiguration _configuration;
        private readonly SieveProcessor _processor;

        public CardsController(IConfiguration configuration, SieveProcessor processor)
        {
            _configuration = configuration;
            _processor = processor;
        }

        [HttpPost("AddCards")]
        public IActionResult AddCards(CardModel model)
        {

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);


            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);


            string email = null;

            // Access claims from the token
            foreach (Claim claim in jwtSecurityToken.Claims)
            {
                if (claim.Type == ClaimTypes.Name) // Check if the claim is a role claim
                {
                    email = claim.Value;
                    break; // Exit the loop once the role claim is found
                }
            }

            try
            {
                if (ModelState.IsValid)
                {
                    GeneralResultModel genericResult = new GeneralResultModel();

                    var searchReults = new CardsService().CreateCard(model,email);

                    if (searchReults.Code.Equals(ConstantVal.Error))
                    {
                        genericResult.Code = ConstantVal.Error;
                        genericResult.Status = ConstantVal.ErrorMsg;
                        genericResult.Message = searchReults.Message;

                        return BadRequest(genericResult);
                    }
                    else
                    {
                        return Ok(searchReults);
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

                    return BadRequest(dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpGet("GetAllCards")]
        public IActionResult GetAllCards()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);


            string role = null;

            // Access claims from the token
            foreach (Claim claim in jwtSecurityToken.Claims)
            {
                if (claim.Type == ClaimTypes.Role) // Check if the claim is a role claim
                {
                    role = claim.Value;
                    break; // Exit the loop once the role claim is found
                }
            }
            try
            {
                if (ModelState.IsValid)
                {
                    GeneralResultModel genericResult = new GeneralResultModel();

                    var searchReults = new CardsService().GetAllcards(role);

                    if (searchReults.Code.Equals(ConstantVal.Error))
                    {
                        genericResult.Code = ConstantVal.Error;
                        genericResult.Status = ConstantVal.ErrorMsg;
                        genericResult.Message = searchReults.Message;

                        return BadRequest(genericResult);
                    }
                    else
                    {
                        return Ok(searchReults);
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

                    return BadRequest(dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }


        [HttpGet("GetCard")]
        public IActionResult GetCard([FromQuery] GetOneCardmodel model)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);


            string role = null;
            string email = null;

            // Access claims from the token
            foreach (Claim claim in jwtSecurityToken.Claims)
            {
                if (claim.Type == ClaimTypes.Role) // Check if the claim is a role claim
                {
                    role = claim.Value;
                    break; // Exit the loop once the role claim is found
                }
            }

            foreach (Claim claim in jwtSecurityToken.Claims)
            {
                if (claim.Type == ClaimTypes.Name) // Check if the claim is a role claim
                {
                    email = claim.Value;
                    break; // Exit the loop once the role claim is found
                }
            }

            try
            {
                if (ModelState.IsValid)
                {
                    GeneralResultModel genericResult = new GeneralResultModel();

                    var searchReults = new CardsService().GetOnecard(model,role,email);

                    if (searchReults.Code == ConstantVal.Success)
                    {

                        return Ok(searchReults);

                    }
                    else
                    {
                        genericResult.Code = ConstantVal.Error;
                        genericResult.Status = ConstantVal.ErrorMsg;
                        genericResult.Message = "No card found";

                        return BadRequest(genericResult);

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

                    return BadRequest(dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete("DeletCards")]
        public IActionResult DeletCards(GetOneCardmodel model)
        {

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);


            string role = null;
            string email = null;
            // Access claims from the token
            foreach (Claim claim in jwtSecurityToken.Claims)
            {
                if (claim.Type == ClaimTypes.Role) // Check if the claim is a role claim
                {
                    role = claim.Value;
                    break; // Exit the loop once the role claim is found
                }
            }
            foreach (Claim claim in jwtSecurityToken.Claims)
            {
                if (claim.Type == ClaimTypes.Name) // Check if the claim is a role claim
                {
                    email = claim.Value;
                    break; // Exit the loop once the role claim is found
                }
            }
            try
            {
                if (ModelState.IsValid)
                {
                    GeneralResultModel genericResult = new GeneralResultModel();

                    var searchReults = new CardsService().DeleteCard(model,role,email);

                    if (searchReults.Code.Equals(ConstantVal.Error))
                    {
                        genericResult.Code = ConstantVal.Error;
                        genericResult.Status = ConstantVal.ErrorMsg;
                        genericResult.Message = searchReults.Message;

                        return BadRequest(genericResult);
                    }
                    else
                    {
                        return Ok(searchReults);
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

                    return BadRequest(dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut("UpdateCard")]
        public IActionResult UpdateCard(CardOneModel model)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);


            string role = null;
            string email = null;

            // Access claims from the token
            foreach (Claim claim in jwtSecurityToken.Claims)
            {
                if (claim.Type == ClaimTypes.Role) // Check if the claim is a role claim
                {
                    role = claim.Value;

                    break; // Exit the loop once the role claim is found
                }
            }
            foreach (Claim claim in jwtSecurityToken.Claims)
            {
                if (claim.Type == ClaimTypes.Name) // Check if the claim is a role claim
                {
                    email = claim.Value;

                    break; // Exit the loop once the role claim is found
                }
            }

            try
            {
                if (ModelState.IsValid)
                {
                    GeneralResultModel genericResult = new GeneralResultModel();

                    var searchReults = new CardsService().UpdateCard(model,role,email);

                    if (searchReults.Code.Equals(ConstantVal.Error))
                    {
                        genericResult.Code = ConstantVal.Error;
                        genericResult.Status = ConstantVal.ErrorMsg;
                        genericResult.Message = searchReults.Message;

                        return BadRequest(genericResult);
                    }
                    else
                    {
                        return Ok(searchReults);
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

                    return BadRequest(dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    

        [HttpGet("SearchCards")]
        public IActionResult SearchCards([FromQuery] SearchCardModel model)
        {

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);


            string role = null;
            string email = null;

            // Access claims from the token
            foreach (Claim claim in jwtSecurityToken.Claims)
            {
                if (claim.Type == ClaimTypes.Role) // Check if the claim is a role claim
                {
                    role = claim.Value;

                    break; // Exit the loop once the role claim is found
                }
            }
            foreach (Claim claim in jwtSecurityToken.Claims)
            {
                if (claim.Type == ClaimTypes.Name) // Check if the claim is a name claim
                {
                    email = claim.Value;

                    break; // Exit the loop once the name claim is found
                }
            }



            try
            {
                if (ModelState.IsValid)
                {
                    GeneralResultModel genericResult = new GeneralResultModel();



                    var searchService = new CardsService();

                    if (model.SearchType.Equals('N'))
                    {

                        var searchResults = searchService.DetailsSearch<SearchCardResultModel>(model,role,email);

                        if (searchResults.Code.Equals(ConstantVal.Error))
                        {
                            genericResult.Code = ConstantVal.Error;
                            genericResult.Status = ConstantVal.ErrorMsg;
                            genericResult.Message = searchResults.Message;

                            return BadRequest(genericResult);
                        }
                        else
                        {
                            return Ok(searchResults);
                        }
                    }
                    else if (model.SearchType.Equals('C'))
                    {
                        var searchResults = searchService.DetailsSearch<SearchCardResultModel>(model, role, email);

                        if (searchResults.Code.Equals(ConstantVal.Error))
                        {
                            genericResult.Code = ConstantVal.Error;
                            genericResult.Status = ConstantVal.ErrorMsg;
                            genericResult.Message = searchResults.Message;

                            return BadRequest(genericResult);
                        }
                        else
                        {
                            return Ok(searchResults);
                        }
                    }
                    else if (model.SearchType.Equals('S'))
                    {
                        var searchResults = searchService.DetailsSearch<SearchCardResultModel>(model,  role, email);

                        if (searchResults.Code.Equals(ConstantVal.Error))
                        {
                            genericResult.Code = ConstantVal.Error;
                            genericResult.Status = ConstantVal.ErrorMsg;
                            genericResult.Message = searchResults.Message;

                            return BadRequest(genericResult);
                        }
                        else
                        {
                            return Ok(searchResults);
                        }
                    }
                    else if (model.SearchType.Equals('T'))
                    {
                        var searchResults = searchService.DetailsSearch<SearchCardResultModel>(model, role, email);

                        if (searchResults.Code.Equals(ConstantVal.Error))
                        {
                            genericResult.Code = ConstantVal.Error;
                            genericResult.Status = ConstantVal.ErrorMsg;
                            genericResult.Message = searchResults.Message;

                            return BadRequest(genericResult);
                        }
                        else
                        {
                            return Ok(searchResults);
                        }
                    }
                
                else
                {
                    genericResult.Code = ConstantVal.Error;
                    genericResult.Status = ConstantVal.ErrorMsg;
                    genericResult.Message = ConstantVal.UnKnownError;

                    return BadRequest(genericResult);
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


                    return BadRequest(dt);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
