using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnostore.Model.DB.Model
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sobrenome { get; set; }
        public virtual string Email { get; set; }
        public virtual String Login { get; set; }
        public virtual String Senha { get; set; }
        public virtual IList<Adm> Adms { get; set; }
        public virtual IList<Endereco> Enderecos { get; set; }
        public virtual IList<Comentario> Comentarios { get; set; }
        public virtual IList<Pesquisa> Pesquisas { get; set; }

        public virtual string GetNomeCompleto()
        {
            return string.Format("{0},{1}", Nome, Sobrenome);
        }

        public virtual Boolean isAdmin()
        {
            if(Adms != null)
            {
                foreach(var adm in Adms)
                {
                    if(adm.Nome == "Administrador")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }

    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Senha, m => {
                m.NotNullable(true);
                m.Unique(true);
            });
            Property(x => x.Email, m => {
                m.NotNullable(true);
                m.Unique(true);
            });
            Property(x => x.Nome);
            Property(x => x.Sobrenome);
            Property(x => x.Login);
          

            Bag<Adm>(x => x.Adms, collectionMapping =>
            {
                collectionMapping.Table("user_adm");
                collectionMapping.Cascade(Cascade.None);
                collectionMapping.Key(k => k.Column("user_id"));
                collectionMapping.Lazy(CollectionLazy.NoLazy);
            }
            , map => map.ManyToMany(p => p.Column("adm_id")));

            Bag<Comentario>(x => x.Comentarios, m =>
            {
                m.Cascade(Cascade.All);
                m.Key(k => k.Column("user_id"));
                m.Lazy(CollectionLazy.NoLazy);
                m.Inverse(true);
            },
            r => r.OneToMany());

            Bag<Pesquisa>(x => x.Pesquisas, m =>
            {
                m.Cascade(Cascade.None);
                m.Key(k => k.Column("user_id"));
                m.Lazy(CollectionLazy.NoLazy);
                m.Inverse(true);
            },
            r => r.OneToMany());
            Bag<Endereco>(x => x.Enderecos, m =>
            {
                m.Cascade(Cascade.None);
                m.Key(k => k.Column("user_id"));
                m.Lazy(CollectionLazy.NoLazy);
                m.Inverse(true);
            },
            r => r.OneToMany());
        }

    }
}
