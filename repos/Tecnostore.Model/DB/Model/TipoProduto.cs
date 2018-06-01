using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnostore.Model.DB.Model
{
    public class TipoProduto
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }       

    }

   
    public class TipoProdutoMap : ClassMapping<TipoProduto>
    {
        public TipoProdutoMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));

            Property(x => x.Nome);
        }
    }
}
