using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;

namespace Tecnostore.Model.DB.Model
{
    public class Estoque
    {
        public virtual int Id { get; set; }        
        public virtual int Quantidade { get; set; }
        public virtual double Preco { get; set; }
        public virtual Produto Produto { get; set; }

        //  public virtual IList<CarrinhoProduto> CarrinhoProduto { get; set; }
        private double precoCusto;

        public virtual double PrecoCusto
        {
            get
            {
                return Math.Round(precoCusto, 2);
            }
            set
            {
                precoCusto = value;
            }
        }


        public virtual double getPreco()
        {
            return Math.Round(Despesa.PrecoFinal(this.CustoMedio()), 2);
        }

        public virtual Double CustoMedio()
        {
            return Math.Round(this.precoCusto / Quantidade, 2);
        }

        /*  public virtual List<CarrinhoProduto> VendaMes(int mes)
          {
              if (this.CarrinhoProduto != null && this.CarrinhoProduto.Count() > 0)
              return this.CarrinhoProduto.Where(x => x.Carrinho.Venda != null).Where(x => x.Carrinho.Venda.Data.Month == mes).ToList();

              return null;
          }
          */
        /*public virtual int QtdVendaMes(int mes)
        {
            int qtd = 0;
            if (this.CarrinhoProduto != null && this.CarrinhoProduto.Count() > 0)
            {
                foreach (var cps in this.CarrinhoProduto.Where(x => x.Carrinho.Venda != null).Where(x => x.Carrinho.Venda.Data.Month == mes))
                {
                    qtd += cps.Quantidade;
                }
            }
                
            return qtd;
        }
        public virtual double TotalVendaMes(int mes)
        {
            double venda = 0;

            if (this.CarrinhoProduto != null && this.CarrinhoProduto.Count() > 0)
            {
                foreach (var cps in this.CarrinhoProduto.Where(x => x.Carrinho.Venda != null).Where(x => x.Carrinho.Venda.Data.Month == mes))
                {
                    //Venda gets the Avarage Price
                    venda += (cps.Estoque.getPreco() * cps.Quantidade);
                }
            }

            return venda;
        }

    */
    }
    public class EstoqueMap : ClassMapping<Estoque>
    {
        public EstoqueMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Quantidade);
            Property(x => x.PrecoCusto);


            ManyToOne(x => x.Produto, m => {
                m.Cascade(Cascade.All);
                m.Column("produto_id");
                m.Class(typeof(Produto));
                m.Lazy(LazyRelation.NoLazy);
            });

            /*  Bag<CarrinhoProduto>(x => x.CarrinhoProduto, m =>
              {
                  m.Cascade(Cascade.All);
                  m.Key(k => k.Column("estoque_id"));
                  m.Lazy(CollectionLazy.NoLazy);
                  m.Inverse(true);
              },
              r => r.OneToMany());*/
        }

    }

}