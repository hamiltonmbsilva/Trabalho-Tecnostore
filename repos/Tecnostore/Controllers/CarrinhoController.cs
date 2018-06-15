using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Tecnostore.Model.DB;
using Tecnostore.Model.DB.Model;

namespace Tecnostore.Controllers
{
    public class CarrinhoController : Controller
    {
        // GET: Carrinho
        public ActionResult Index()
        {
            //check authentication
            if (this.CheckLogIn())
            {
                //Check if cart exists
                if (HttpContext.Session["cartID"] != null && HttpContext.Session["cartID"].ToString() != "")
                {
                    //getting cart from session
                    var idCart = Convert.ToInt32(HttpContext.Session["cartID"].ToString());
                    Carrinho cart = DbFactory.Instance.CarrinhoRepository.FindAll().Where(x => x.Id == idCart).FirstOrDefault();

                    //adding shipment to cart
                    cart.Entrega = 10;

                    //attaching user to cart
                    cart.Usuario = DbFactory.Instance.UserRepository.isAuthenticated();
                    DbFactory.Instance.CarrinhoRepository.Save(cart);

                    //sending cart to view Carrinho, and valid adresses
                    IList<Endereco> enderecosValidos = new List<Endereco>();
                    if (cart.Usuario.Enderecos != null)
                    {
                        enderecosValidos = cart.Usuario.Enderecos.Where(x => x.Status > 0).ToList();
                    }
                    ViewBag.enderecosValidos = enderecosValidos;
                    return View(cart);
                }
            }
            //Not Authenticated
            return RedirectToAction("Denied", "Home");
        }

        //Details Venda, Payment.
        public ActionResult Venda(int id)
        {
            Venda v = DbFactory.Instance.VendaRepository.FindAll().Where(x => x.Id == id).FirstOrDefault();
            return View(v);
        }

        //Client with new addres
        public ActionResult Adicionar(FormCollection form)
        {
            if (this.CheckLogIn())
            {
                //getting cart
                var idCart = Convert.ToInt32(HttpContext.Session["cartID"].ToString());
                Carrinho cart = DbFactory.Instance.CarrinhoRepository.FindAll().Where(x => x.Id == idCart).FirstOrDefault();

                //creating new Address
                var usr = DbFactory.Instance.UserRepository.isAuthenticated();
                Endereco end = new Endereco();
                end.Descricao = form["descricao"].ToString();
                end.Logradouro = form["logradouro"].ToString();
                end.CEP = form["cep"].ToString();
                end.Bairro = form["bairro"].ToString();
                end.Numero = form["numero"].ToString();
                end.Complemento = form["complemento"].ToString();
                end.Cidade = form["cidade"].ToString();
                end.Estado = form["estado"].ToString();
                end.Pais = form["pais"].ToString();
                end.Usuario = usr;
                end.Status = 1;
                DbFactory.Instance.EnderecoRepository.Save(end);


                //creating new Venda
                Venda venda = new Venda();
                venda.Endereco = end;
                venda.Carrinho = cart;
                venda.Data = DateTime.Now;
                venda.ValorTotal = cart.getValorTotal();

                //Clear Session's Cart
                HttpContext.Session["cartID"] = null;

                DbFactory.Instance.VendaRepository.CriarVenda(venda);

                return RedirectToAction("Venda", new { id = venda.Id });
            }
            //If not Authenticated
            return RedirectToAction("Denied", "Home");
        }

        //Client with Adress selected
        public ActionResult SelecionarEndereco(FormCollection form)
        {
            //check auth
            if (this.CheckLogIn())
            {
                //getting cart
                var idCart = Convert.ToInt32(HttpContext.Session["cartID"].ToString());
                Carrinho cart = DbFactory.Instance.CarrinhoRepository.FindAll().Where(x => x.Id == idCart).FirstOrDefault();

                var enderecoId = Convert.ToInt32(form["enderecoId"].ToString());
                var end = DbFactory.Instance.EnderecoRepository.FindAll().Where(x => x.Id == enderecoId).FirstOrDefault();

                //validating address
                if (end.Status > 0)
                {
                    //creating new Venda
                    Venda venda = new Venda();
                    venda.Endereco = end;
                    venda.Carrinho = cart;
                    venda.Data = DateTime.Now;
                    venda.ValorTotal = cart.getValorTotal();
                    DbFactory.Instance.VendaRepository.CriarVenda(venda);
                    //Clear Session's Cart
                    HttpContext.Session["cartID"] = null;

                    return RedirectToAction("Venda", new { id = venda.Id });
                }
                else
                {
                    ViewBag.error = "Endereço inválido";
                    return RedirectToAction("Index");
                }
            }
            //If not Authenticated
            return RedirectToAction("Denied", "Home");

        }

        //PAYMENT METHODS
        public ActionResult Cartao(FormCollection form)
        {
            var vendaId = Convert.ToInt32(form["venda_id"].ToString());
            var venda = DbFactory.Instance.VendaRepository.FindAll().Where(x => x.Id == vendaId).FirstOrDefault();

            var NumeroCartao = form["numeroCartao"].ToString();
            var Codigo = Convert.ToInt32(form["codigo"].ToString());
            var Validade = form["validade_ano"].ToString() + form["validade_mes"].ToString();
            var Valor = venda.ValorTotal;
            var Parcelas = Convert.ToInt32(form["parcelas"].ToString());
            var Nome = form["nomeCliente"].ToString();

            var NomeEmpresa = form["nomeEmpresa"].ToString(); 
            var CNPJEmpresa = Convert.ToInt32(form["cnpj"].ToString());

            ServiceReference1.tDadosCartao td = new ServiceReference1.tDadosCartao();
            td.NumeroCartao = NumeroCartao;
            td.Parcelas = Parcelas;
            td.Validade = Validade;
            td.Valor = Valor;
            td.Codigo = Codigo;
            td.NomeEmpresa = NomeEmpresa;
            td.CNPJEmpresa = CNPJEmpresa;
            td.NomeCliente = Nome;

            ServiceReference1.CardPortTypeClient cpt = new ServiceReference1.CardPortTypeClient();


            try
            {
                var result = cpt.ValidarCartao(td);
                venda.Status = 1;
                venda.FormaPagamento = DbFactory.Instance.FormaPagamentoRepository.FindAll().Where(x => x.Id == 1).FirstOrDefault();
                venda.DataPagamento = DateTime.Now;
                venda.Parcelas = Parcelas;
                DbFactory.Instance.VendaRepository.Save(venda);

                return RedirectToAction("Venda", new { id = venda.Id });
            }
            catch (Exception)
            {

                //var result = cpt.ValidarCartao(td);
                venda.Status = 1;
                venda.FormaPagamento = DbFactory.Instance.FormaPagamentoRepository.FindAll().Where(x => x.Id == 1).FirstOrDefault();
                venda.DataPagamento = DateTime.Now;
                venda.Parcelas = Parcelas;
                DbFactory.Instance.VendaRepository.Save(venda);

                return RedirectToAction("Venda", new { id = venda.Id });

                //return RedirectToAction("CartaoNegado", new { id = venda.Id });
                //throw;
            }

        }

        public ActionResult CartaoNegado(int id)
        {
            Venda v = DbFactory.Instance.VendaRepository.FindAll().Where(x => x.Id == id).FirstOrDefault();
            return View(v);
        }

        public ActionResult NotaFiscal(FormCollection form)
        {
            if (!this.CheckLogIn())
            {
                return RedirectToAction("Denied", "Home");
            }

            var id = Convert.ToInt32(form["venda_Id"]);
            Venda v = DbFactory.Instance.VendaRepository.FindAll().Where(x => x.Id == id).FirstOrDefault();
            return View(v);
        }

        /*   public ActionResult Boleto(int id)
        {
            var Venda = DbConfig.Instance.VendaRepository.FindAll().Where(x => x.Id == id).FirstOrDefault();
            
        }
        */


        public Boolean CheckLogIn()
        {
            var usr = DbFactory.Instance.UserRepository.isAuthenticated();
            if (usr != null)
            {
                return true;
            }
            return false;
        }
    }
}