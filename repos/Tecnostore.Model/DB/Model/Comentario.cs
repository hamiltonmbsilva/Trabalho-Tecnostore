using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;

namespace Tecnostore.Model.DB.Model
{
    public class Comentario
    {
        public virtual int Id { get; set; }
        public virtual String Texto { get; set; }    
        public virtual String Avaliacao { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual User Usuario { get; set; }
        public virtual Produto Produto { get; set; }
    }

    public class ComentarioMap : ClassMapping<Comentario>
    {
        public ComentarioMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Texto);
            Property(x => x.Avaliacao);
            Property(x => x.Data);

            ManyToOne(x => x.Usuario, m => {
                m.Cascade(Cascade.All);
                m.Column("user_id");
                m.Class(typeof(User));
            });

            ManyToOne(x => x.Produto, m => {
                m.Cascade(Cascade.None);
                m.Column("produto_id");
                m.Class(typeof(Produto));
            });
        }

    }
}