using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;

namespace Tecnostore.Model.DB.Model
{
    public class Categoria
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
    }

    public class CategoriaMap : ClassMapping<Categoria>
    {
        public CategoriaMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));

            Property(x => x.Nome);
        }
    }
}