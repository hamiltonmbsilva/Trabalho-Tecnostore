using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnostore.Model.DB.Model
{
    public static class Despesa
    {
        public static double DespesasFixa = 3;
        public static double MargemLucro = 20;

        //Despesas Variáveis
        public static double Transporte = 1;
        public static double LogisticaArmazenagem = 2;
        public static double Comissao = 0.3;
        public static double DespesaBancaria = 0.3;
        public static double Inadimplencia = 0.5;
        public static double Propaganda = 0.5;
        public static double Servidores = 0.5;
        public static double RMA = 0.5;

        //Impostos
        public static double PIS = 1.65;
        public static double COFINS = 7.60;
        public static double ICMS = 12;
        public static double IPI = 15;

        public static double PrecoFinal(double preco)
        {
            //Impostos Recuperáveis, deduzir impostos bitributaveis
            double impostos = preco * ((PIS + COFINS + ICMS + IPI) / 100);
            double precoReal = preco - impostos;

            //Despesas e Margem de lucro
            double despesasVariaveis = precoReal * ((Transporte + LogisticaArmazenagem + Comissao + DespesaBancaria + Inadimplencia + Propaganda + Servidores + RMA) / 100);
            double despesasFixa = precoReal * (DespesasFixa / 100);
            double lucro = precoReal * (MargemLucro / 100);

            precoReal += (despesasVariaveis + despesasFixa + lucro + impostos);

            return precoReal;
        }
    }
}
