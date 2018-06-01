using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;

namespace Tecnostore.Model.DB.Model
{
    public class ItemVenda
    {
        public virtual int Id { get; set; }        
        public virtual int Quantidade { get; set; }
        public virtual double Valor { get; set; }
        public virtual Venda Venda { get; set; }
        public virtual Produto Produto { get; set; }
    }

    public class ItemVendaMap : ClassMapping<ItemVenda>
    {
        public ItemVendaMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            
            Property(x => x.Quantidade);

            Property(x => x.Valor, m => m.NotNullable(true));

            ManyToOne(x => x.Produto, m =>
            {
                m.Cascade(Cascade.None);
                m.Column("produto_id");
                m.Class(typeof(Produto));
            });
            ManyToOne(x => x.Venda, m =>
            {
                m.Cascade(Cascade.None);
                m.Column("venda_id");
                m.Class(typeof(Venda));
            });
        }
    }
}