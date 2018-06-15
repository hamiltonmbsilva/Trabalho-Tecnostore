using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;

namespace Tecnostore.Model.DB.Model
{
    public class Desconto
    {
        public virtual Guid Id { get; set; }
        public virtual String Codigo { get; set; }
        public virtual int Tipo { get; set; }
        public virtual IList<Produto> Produtos { get; set; }
    }

    public class DescontoMap : ClassMapping<Desconto>
    {
        public DescontoMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Codigo);
            Property(x => x.Tipo);


            
        }
        
    }
 }

