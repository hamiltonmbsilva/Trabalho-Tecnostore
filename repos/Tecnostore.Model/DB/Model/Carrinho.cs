using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnostore.Model.DB.Model
{
    public class Carrinho
    {
        public virtual int Id { get; set; }
        public virtual User Usuario { get; set; }
        public virtual Boolean Status { get; set; }
        public virtual double Entrega { get; set; }
        public virtual IList<CarrinhoProduto> CarrinhoProduto { get; set; }
        public virtual Desconto Desconto { get; set; }

        public virtual double getValorTotal()
        {
            //Valor do produto
            double valor = 0;
            foreach (var prod in CarrinhoProduto)
            {

                for (int i = 0; i < prod.Quantidade; i++)
                {
                    valor += prod.getValor();
                }

            }

            if (Desconto != null)
            {
                //type of discount
                switch (Desconto.Tipo)
                {
                    case 1:
                        valor = valor - (valor * 0.1);
                        break;
                    case 2:
                        valor = valor - (valor * 0.2);
                        break;
                    case 3:
                        valor = valor - 20;
                        break;
                    default:
                        break;
                }
            }

            if (Entrega != 0)
            {
                valor = valor + Entrega;
            }

            return valor;
        }

    }

    public class CarrinhoMap : ClassMapping<Carrinho>
    {
        public CarrinhoMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Status);

            ManyToOne(x => x.Usuario, m =>
            {
                m.Cascade(Cascade.All);
                m.Column("user_id");
                m.Class(typeof(User));
            });

            ManyToOne(x => x.Desconto, m =>
            {
                m.Cascade(Cascade.All);
                m.Column("desconto_id");
                m.Class(typeof(Desconto));
            });

            Bag<CarrinhoProduto>(x => x.CarrinhoProduto, m =>
            {
                m.Cascade(Cascade.None);
                m.Key(k => k.Column("carrinho_id"));
                m.Lazy(CollectionLazy.NoLazy);
                m.Inverse(true);
            },
            r => r.OneToMany());



            /*   ManyToOne(x => x.Venda, m =>
               {
                   m.Cascade(Cascade.All);
                   m.Column("venda_id");
                   m.Class(typeof(Desconto));
               });*/


            Property(x => x.Entrega);
        }

    }
}
