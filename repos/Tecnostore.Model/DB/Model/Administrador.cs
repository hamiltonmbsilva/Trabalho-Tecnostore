using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnostore.Model.DB.Model
{
    public class Administrador
    {
        public virtual Guid Id { get; set; }
        public virtual String Nome { get; set; }
        public virtual DateTime DtNascimento { get; set; }
        public virtual bool inativo { get; set; }
        public virtual User Usuario { get; set; }
    }

    public class AdministradorMap : ClassMapping<Administrador>
    {
        public AdministradorMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));
            Property(x => x.Nome);
            Property(x => x.DtNascimento);
            Property(x => x.inativo);

            ManyToOne(x => x.Usuario, m =>
            {
                m.Column("idUsuario");
                m.Unique(true);
                m.Lazy(LazyRelation.NoLazy);
                m.Cascade(Cascade.Persist);
            });
        }
    }
}
