using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnostore.Model.DB.Model
{
    public class Endereco
    {
       // public static List<Endereco> Esportes = new List<Endereco>();
        public virtual int Id { get; set; }
        public virtual String Logradouro { get; set; }
        public virtual String Numero { get; set; }
        public virtual String Complemento { get; set; }
        public virtual string Descricao { get; set; }
        public virtual String Bairro { get; set; }
        public virtual String Cidade { get; set; }
        public virtual String Estado { get; set; }
        public virtual String CEP { get; set; }
        public virtual String Pais { get; set; }
        public virtual int Status { get; set; }
        public virtual User Usuario { get; set; }       

    }

    public class EnderecoMap : ClassMapping<Endereco>
    {
        public EnderecoMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));

            Property(x => x.Logradouro, m => m.NotNullable(true));
            Property(x => x.Complemento, m => m.NotNullable(true));
            Property(x => x.Descricao, m => m.NotNullable(true));
            Property(x => x.Numero, m => m.NotNullable(true));
            Property(x => x.Bairro, m => m.NotNullable(true));
            Property(x => x.CEP, m => m.NotNullable(true));
            Property(x => x.Cidade, m => m.NotNullable(true));
            Property(x => x.Estado, m => m.NotNullable(true));
            Property(x => x.Pais, m => m.NotNullable(true));
            Property(x => x.Status, m => { m.NotNullable(true); });

            ManyToOne(x => x.Usuario, m => {
                m.Cascade(Cascade.All);
                m.Column("user_id");
                m.Class(typeof(User));
                m.NotNullable(true);
            });
        }
    }

}
