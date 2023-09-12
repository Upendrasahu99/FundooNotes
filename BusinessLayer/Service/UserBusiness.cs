using BusinessLayer.Interface;
using CommonLayer.Model;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepo userRepo;

        public UserBusiness(IUserRepo userRepo)   // Dependency injection
        {
            this.userRepo = userRepo;
        }

        public string  UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                return userRepo.UserLogin(userLoginModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UserEntity UserRegister(UserRegModel userRegModel)
        {
            try
            {
                
                return userRepo.UserRegister(userRegModel);

            }
            catch (Exception)
            {

                throw;
            }
         
        }

        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                return userRepo.ForgotPassword(forgotPasswordModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ResetPassword(ResetModel resetModel, string email)
        {
            try
            {
                return userRepo.ResetPassword(resetModel, email);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
