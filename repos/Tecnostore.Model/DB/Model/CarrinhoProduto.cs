using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;

namespace Tecnostore.Model.DB.Model
{
    public class CarrinhoProduto
    {
        public virtual int Id { get; set; }     
        public virtual int Quantidade { get; set; }
        public virtual Estoque Estoque { get; set; }
        public virtual Carrinho Carrinho { get; set; }

        public virtual Double getValor()
        {
            return this.Estoque.getPreco();
        }
        public virtual double getValorQuantidade()
        {
            return this.Estoque.getPreco() * this.Quantidade;
        }
    }

    public class CarrinhoProdutoMap : ClassMapping<CarrinhoProduto>
    {
        public CarrinhoProdutoMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Quantidade);

            ManyToOne(x => x.Estoque, m => {
                m.Cascade(Cascade.None);
                m.Column("estoque_id");
                m.Class(typeof(Estoque));
                m.Lazy(LazyRelation.NoLazy);
            });
            ManyToOne(x => x.Carrinho, m => {
                m.Cascade(Cascade.None);
                m.Column("carrinho_id");
                m.Class(typeof(Carrinho));
                m.Lazy(LazyRelation.NoLazy);
            });
        }
    }

}