using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnostore.Model.DB.Model;

namespace Tecnostore.Model.DB.Repository
{
    public class CategoriaRepository : RepositoryBase<Categoria>
    {
        public CategoriaRepository(ISession session) : base(session)
        {
        }
    }
}
