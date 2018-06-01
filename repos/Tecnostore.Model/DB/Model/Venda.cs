using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnostore.Model.DB.Model
{
    public class Venda
    {
        public virtual int Id { get; set; }        
        public virtual Double ValorTotal { get; set; }
        public virtual Double ValorFrete { get; set; }
        public virtual int Status { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual DateTime DataPagamento { get; set; }
        public virtual int Parcelas { get; set; }
        public virtual Carrinho Carrinho { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual Endereco Endereco { get; set; }
        
        

        public virtual IList<ItemVenda> ItemVendas { get; set; }

        public virtual double GetParcelas(int vezes)
        {
            double parcela = 0;

            parcela = (ValorTotal / vezes);

            if (vezes > 3)
            {
                parcela += parcela * 0.1;
            }

            return parcela;
        }
    }
    public class VendaMap : ClassMapping<Venda>
    {
        public VendaMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));

            //{"The type is incompatible; expected assignable to TropicalBears.Model.DataBase.Model.Venda\r\nNome do parâmetro: entityType"}
            Property(x => x.ValorTotal);
            Property(x => x.Status);
            Property(x => x.Data);
            Property(x => x.DataPagamento);
            Property(x => x.ValorFrete);
            Property(x => x.Parcelas);

            ManyToOne(x => x.Carrinho, m => {
                m.Cascade(Cascade.None);
                m.Column("carrinho_id");
                m.Class(typeof(Carrinho));
            });

            ManyToOne(x => x.Endereco, m => {
                m.Cascade(Cascade.None);
                m.Column("endereco_id");
                m.Class(typeof(Endereco));
            });

            ManyToOne(x => x.FormaPagamento, m => {
                m.Cascade(Cascade.None);
                m.Column("formapagamento_id");
                m.Class(typeof(FormaPagamento));
            });            

            Bag<ItemVenda>(x => x.ItemVendas, m =>
            {
                m.Cascade(Cascade.None);
                m.Key(k => k.Column("venda_id"));
                m.Lazy(CollectionLazy.NoLazy);
                m.Inverse(true);
            },
            r => r.OneToMany());

        }
    }
}
