using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;

namespace Tecnostore.Model.DB.Model
{
    public class Imagem
    {
        public virtual int Id { get; set; }
        public virtual string Img { get; set; }
        public virtual Produto Produto { get; set; }
    }

    public class ImagemMap : ClassMapping<Imagem>
    {
        public ImagemMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Img, m => m.NotNullable(true));

            ManyToOne(x => x.Produto, m => {
                m.Cascade(Cascade.All);
                m.Column("produto_id");
                m.Class(typeof(Produto));
                m.Lazy(LazyRelation.NoLazy);
            });
        }
    }
}