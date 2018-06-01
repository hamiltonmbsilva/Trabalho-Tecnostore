using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnostore.Model.DB.Model;

namespace Tecnostore.Model.DB.Repository
{
    public class ComentarioRepository : RepositoryBase<Comentario>
    {
        public ComentarioRepository(ISession session) : base(session)
        {
        }

        public void Deletar(Comentario comentario)
        {
            try
            {
                this.Session.Clear();
                // var transaction = this.Session.BeginTransaction();

                var stm = this.Session.CreateSQLQuery("Delete from comentario where Id = " + comentario.Id);
                stm.ExecuteUpdate();

                // transaction.Commit();

            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(Comentario)
                    + "\nErro:" + ex.Message);
            }
        }
    }
}
