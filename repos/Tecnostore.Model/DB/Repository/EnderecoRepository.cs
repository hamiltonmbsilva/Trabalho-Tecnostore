using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnostore.Model.DB.Model;

namespace Tecnostore.Model.DB.Repository
{
    public class EnderecoRepository : RepositoryBase<Endereco>
    {
        public EnderecoRepository(ISession session) : base(session) { }
       

        public void Delete(int id)
        {
            try
            {
                this.Session.Clear();
                // var transaction = this.Session.BeginTransaction();

                var stm = this.Session.CreateSQLQuery("Delete from endereco where Id = " + id);
                stm.ExecuteUpdate();

                // transaction.Commit();

            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(Endereco)
                    + "\nErro:" + ex.Message);
            }
        }
        public void getActives()
        {
            this.FindAll().Where(x => x.Status > 0);
        }
    }
}
