using DAL;
using DAL.Repo;
using Isopoh.Cryptography.Argon2;
using LoginModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LoginModule.Logic
{
    public class LoginLogic
    {
        public LogInDtls VerifyLogin(LoginModel model)
        {
            UserRepo userRep = new UserRepo();
            LogInDtls dtls = new LogInDtls();
            User user = new User();
            
            if (Argon2.Verify(userRep.GetPassword(model.Email), model.Password))
            {
                user = userRep.GetCredentials(model.Email);
                dtls.UserID = user.UserID;
                dtls.UserName = user.Name;
                dtls.Verify = true;
            }
            else
            {
                dtls.Verify = false;
            }

            return dtls;
        }

        public void ClearCookies(HttpCookie LoginCookie)
        {
            if (LoginCookie != null)
            {
                LoginCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(LoginCookie);
            }
        }

        public bool CheckLogin(bool sessionStatus, HttpCookie LoginCookie)
        {
            bool LoginStatus = false;
            LoginStatus = sessionStatus;
            HttpCookie userName = HttpContext.Current.Request.Cookies["UserName"];

            if (LoginCookie != null)
            {
                LoginStatus = Convert.ToBoolean(LoginCookie.Value);
                LoginCookie.Expires = DateTime.Now.AddMinutes(10);
                userName.Expires = DateTime.Now.AddMinutes(10);
                HttpContext.Current.Response.Cookies.Add(LoginCookie);
                HttpContext.Current.Response.Cookies.Add(userName);
            }
            else
            {
                HttpContext.Current.Request.Cookies.Clear();
                LoginStatus = false;
            }

            return LoginStatus;
        }

        public string GenerateHash(string input)
        {
            string hash = Argon2.Hash(input);
            return hash;
        }
    }
}
