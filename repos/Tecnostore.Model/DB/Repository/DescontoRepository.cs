using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnostore.Model.DB.Model;

namespace Tecnostore.Model.DB.Repository
{
    public class DescontoRepository : RepositoryBase<Desconto>
    {
        public DescontoRepository(ISession session) : base(session)
        {
        }
        public void Deletar(Guid id)
        {
            try
            {
                this.Session.Clear();
                var transaction = this.Session.BeginTransaction();

                Desconto d = FindAll().Where(x => x.Id == id).FirstOrDefault();

                this.Session.Delete(d);

                transaction.Commit();

            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(CarrinhoProduto)
                    + "\nErro:" + ex.Message);
            }
        }
    }
}
