using CommonLayer.Model;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RepoLayer.Interface
{
    public interface IUserRepo
    {
        public UserEntity UserRegister(UserRegModel userRegModel);

        public string UserLogin(UserLoginModel userLoginModel);

        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel);

        public bool ResetPassword(ResetModel resetModel, string email);
    }
}
