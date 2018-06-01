using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnostore.Model.DB.Model;

namespace Tecnostore.Model.DB.Repository
{
    public class ImagemRepository : RepositoryBase<Imagem>
    {
        public ImagemRepository(ISession session) : base(session) { }

        public virtual void Deletar(Imagem img)
        {
            try
            {
                this.Session.Clear();
                var transaction = this.Session.BeginTransaction();

                this.Session.Delete(img);

                transaction.Commit();

            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(Imagem)
                    + "\nErro:" + ex.Message);
            }
        }
    }
}
