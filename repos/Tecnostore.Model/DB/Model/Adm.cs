using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnostore.Model.DB.Model
{
    public class Adm
    {
        public virtual int Id { get; set; }
        public virtual String Nome { get; set; }
    }

    public class AdmMap : ClassMapping<Adm>
    {
        public AdmMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Nome);
        }

    }
}
