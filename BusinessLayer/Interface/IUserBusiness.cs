using CommonLayer.Model;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBusiness
    {
        public UserEntity UserRegister(UserRegModel userRegModel);

        public string UserLogin(UserLoginModel userLoginModel);

        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel);

        public bool ResetPassword(ResetModel resetModel, string email);
    }
}
