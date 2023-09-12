using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepoLayer.Service
{
    public class UserRepo : IUserRepo
    {
        private readonly FundooContext fundooContext; // Instance (object) of FunddooContext
        private readonly IConfiguration configuration; // IConfiguration is interface is part of ASP.NET  Cor's congifuration system and provides access of configuration setting defined in Various configuration source like 'appsettings.json'


        public UserRepo(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }

        //User Registraion method
        public UserEntity UserRegister(UserRegModel userRegModel)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = userRegModel.FirstName;
                userEntity.LastName = userRegModel.LastName;
                userEntity.Email = userRegModel.Email;
                userEntity.Password = EncryptPassword(userRegModel.Password);
                fundooContext.Users.Add(userEntity);
                fundooContext.SaveChanges();
                if (userEntity != null)
                {
                    return userEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // User Login method 
        public string UserLogin(UserLoginModel userLoginModel)
        {
            try
            {


                UserEntity userEntity = fundooContext.Users.FirstOrDefault(u => u.Email == userLoginModel.Email && u.Password == EncryptPassword(userLoginModel.Password)); // It will return First Values which satisfly the condition.
                if (userEntity != null)
                {
                    string jwtToken = GenerateJwtToken(userEntity.Email, userEntity.UserId);
                    return jwtToken;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //JWT Implementaion
        public string GenerateJwtToken(string Email, long UserID)

        {
            try
            {
                //Claims are an essential part of the token payload.
                //Create list of claims. Claims is peach of information in a token.
                var claims = new List<Claim>
            {
                new Claim("UserID", UserID.ToString()),// adding userID as claims, "UserID" is custom calim type.
                new Claim(ClaimTypes.Email, Email), // adding Email as claims, Email is Predefine claim.
            };
                // SymertricSecurityKey using the secret key obtained from configuration. This key will be used to sign the JWT token.
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));

                //Create SingingCredentials Thast encapsulated the key and algorithms(Hash-based Message Authenticaiton Code SHA-256) used for signing the token. 
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                // write the JWT token with specified parameters.
                var token = new JwtSecurityToken(
                    issuer: "JwtSettings:Issuer", // The issuer of the token (usally application)
                    audience: "JwtSettings:Audience", // The audience of the token (usually the intended recipient)
                    claims: claims, // The calims created earlier
                    notBefore: DateTime.Now, // Tge earliest time ate which token is valid(starting time)
                    expires: DateTime.Now.AddHours(1), // The Expiration time of the token ( 1 hour from now )
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {

                throw;
            }
        }


        // For Encryption

        public string EncryptPassword(string password)
        {
            if (password != null)
            {
                //ASCIIEncoding.ASCII refers to a class and an encoding provided by the .NET Framework for working with ASCII (American Standard Code for Information Interchange) character encoding. 
                byte[] storePassword = ASCIIEncoding.ASCII.GetBytes(password);
                string encyptedPassword = Convert.ToBase64String(storePassword);
                return encyptedPassword;
            }
            else
            {
                return null;
            }
        }

        public string DecryptPassword(string password)
        {
            if (password != null)
            {
                byte[] encryptedPassword = Convert.FromBase64String(password);
                string decryptedPassword = ASCIIEncoding.ASCII.GetString(encryptedPassword);
                return decryptedPassword;
            }
            else
            {
                return null;
            }
        }
       
        //Forgot Password
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                UserEntity user = fundooContext.Users.FirstOrDefault(u => u.Email == forgotPasswordModel.Email);

                if (user != null)
                {
                    string token = GenerateJwtToken(user.Email, user.UserId);
                    MsmqModel msmqModel = new MsmqModel();
                    msmqModel.MessageSender(token);
                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Reset Password 

        public bool ResetPassword(ResetModel resetModel, string email)
        {
            try
            {
                var resetPassword = fundooContext.Users.FirstOrDefault(u => u.Email == email);
                if (resetPassword != null && resetModel.NewPassword == resetModel.ConfirmPassword)
                {
                    resetPassword.Password = EncryptPassword(resetPassword.Password);
                    fundooContext.SaveChanges();
                    return true;
                }
                else { return false; }
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
