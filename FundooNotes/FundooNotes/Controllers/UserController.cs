using BusinessLayer.Interface;
using BusinessLayer.Service;
using CommonLayer.Model;
using Experimental.System.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;
      
        public   UserController(IUserBusiness userBusiness )
        {
            this.userBusiness = userBusiness;
          
        }

        [HttpPost]
        [Route ("Register")]
        public IActionResult UserRegister(UserRegModel userRegModel) //IActionResult return any type of value(sting, float, value etc.)
        {
            try
            {
                var result = userBusiness.UserRegister(userRegModel);
                if( result != null)
                {
                  return  this.Ok(new {success = true, message = "Registration Succesfull", data = result});
                }
                else
                {
                   return this.BadRequest(new { success = false, message = "Registration UnSuccesfull"});
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [Route ("Login")]
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {

            try
            {
                //JWT token
                var result = userBusiness.UserLogin(userLoginModel);
                if (result != null)
                { 
                    return this.Ok(new { success = true, message = "Login Succesful", data = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Login Unsuccesfull" });     
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route ("ForgotPassword")]

        public IActionResult  ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                var result = userBusiness.ForgotPassword(forgotPasswordModel);
                if(result != null)
                {
                    return this.Ok(new { success = true, Message = "mailSend", data = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, Message = "mailNotSend" });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("ResetPassword")]
        
        public IActionResult ResetPassword(ResetModel resetModel)
        {
            try
            {
                string email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                bool result = userBusiness.ResetPassword(resetModel, email);
                if(result)
                {
                    return this.Ok(new { success = true, Message = "Password Reset Succesfull" });
                }
                else
                {
                    return this.BadRequest(new { success = false, Message = "New Password not matched with Confirm Password" });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
