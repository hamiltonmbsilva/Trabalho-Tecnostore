using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Tecnostore.Model.DB.Model;

namespace Tecnostore.Model.DB.Repository
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(ISession session) : base(session)
        {
        }
        public User findByEmail(string email)
        {
            return this.Session.Query<User>().FirstOrDefault(f => f.Email == email);
        }

        public bool Authenticate(string email, string senha)
        {
            var query = (User)this.Session.Query<User>().FirstOrDefault(f => f.Email == email);
            if (query == null)
            {
                return false;
            }
            else if (query.Senha == senha)
            {
                FormsAuthentication.SetAuthCookie(email, true);
                //if (query.isAdmin())
                //{
                //    query.Senha = this.GerarSenha();
                //    this.Save(query);
                //}
                return true;
            }
            return false;
        }
        public User isAuthenticated()
        {
            var login = "";
            if (HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                login = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            }

            if (login == "")
            {
                return null;
            }
            else
            {
                return this.Session.Query<User>().FirstOrDefault(x => x.Email == login);
            }

        }
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
        public string GerarSenha()
        {
            var dia = DateTime.Now.DayOfWeek;
            string diaSemana;

            switch (dia)
            {
                case DayOfWeek.Sunday:
                    diaSemana = "dom";
                    break;
                case DayOfWeek.Monday:
                    diaSemana = "seg";
                    break;
                case DayOfWeek.Tuesday:
                    diaSemana = "ter";
                    break;
                case DayOfWeek.Wednesday:
                    diaSemana = "qua";
                    break;
                case DayOfWeek.Thursday:
                    diaSemana = "qui";
                    break;
                case DayOfWeek.Friday:
                    diaSemana = "sex";
                    break;
                case DayOfWeek.Saturday:
                    diaSemana = "sab";
                    break;
                default:
                    diaSemana = "dom";
                    break;
            }


            var month = DateTime.Now.Month;
            var mes = "";
            if (month < 10)
            {
                mes = String.Format("0{0}", month);
            }
            else
            {
                mes = String.Format("{0}", month);
            }
            var day = DateTime.Now.Day;
            var hour = DateTime.Now.Hour;

            return string.Format("{0}{1}{2}", diaSemana, mes, day + hour);

        }
    }
}
