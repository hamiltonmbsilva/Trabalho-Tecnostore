using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tecnostore.Model.DB;
using Tecnostore.Model.DB.Model;

namespace Tecnostore.Controllers
{
    public class UserController : Controller
    {
        public User User { get; set; }

        public UserController()
        {
            this.User = DbFactory.Instance.UserRepository.isAuthenticated();
        }
        // GET: User
        public ActionResult Index()
        {
            if (this.User == null)
                return RedirectToAction("Denied", "Home");

            if (User.Enderecos == null)
            {
                User.Enderecos = new List<Endereco>();
            }

            IList<Endereco> enderecosValidos = new List<Endereco>();
            enderecosValidos = this.User.Enderecos.Where(x => x.Status > 0).ToList();

            IList<Venda> vendas = new List<Venda>();
            vendas = DbFactory.Instance.VendaRepository.FindAll().Where(x => x.Endereco.Usuario.Id == this.User.Id).ToList();

            ViewBag.vendas = vendas;
            ViewBag.enderecosValidos = enderecosValidos;
            return View(this.User);

        }
        [HttpPost]
        public ActionResult AddEndereco(FormCollection form)
        {
            if (this.User == null)
                return RedirectToAction("Denied", "Home");

            Endereco end = new Endereco()
            {
                Descricao = form["descricao"].ToString(),
                Logradouro = form["logradouro"].ToString(),
                CEP = form["cep"].ToString(),
                Bairro = form["bairro"].ToString(),
                Numero = form["numero"].ToString(),
                Complemento = form["complemento"].ToString(),
                Cidade = form["cidade"].ToString(),
                Estado = form["estado"].ToString(),
                Pais = form["pais"].ToString(),
                Usuario = this.User,
                Status = 1
            };
            DbFactory.Instance.EnderecoRepository.Save(end);
            this.User.Enderecos.Add(end);
            return RedirectToAction("Index");
        }
        public ActionResult Endereco(int id)
        {
            if (this.User == null)
                return RedirectToAction("Denied", "Home");

            var end = DbFactory.Instance.EnderecoRepository.FindAll().Where(x => x.Id == id).FirstOrDefault();
            return View("_PartialEnderecos", end);
        }
        public ActionResult InserirEndereco()
        {
            if (this.User == null)
                return RedirectToAction("Denied", "Home");

            return View("_AddEndereco");
        }
        public ActionResult DeleteEndereco(FormCollection form)
        {
            if (this.User == null)
                return RedirectToAction("Denied", "Home");

            var end = DbFactory.Instance.EnderecoRepository.FindAll().Where(x => x.Id == Convert.ToInt32(form["enderecoID"].ToString())).FirstOrDefault();
            end.Status = 0;
            DbFactory.Instance.EnderecoRepository.Save(end);
            return RedirectToAction("Index");
        }
        public ActionResult SalvarEndereco(FormCollection form)
        {
            int id = Convert.ToInt32(form["enderecoID"].ToString());
            Endereco end = DbFactory.Instance.EnderecoRepository.FindAll().Where(x => x.Id == id).FirstOrDefault();

            end.Descricao = form["descricao"];
            end.Logradouro = form["logradouro"];
            end.Numero = form["numero"];
            end.Bairro = form["bairro"];
            end.CEP = form["cep"];
            end.Complemento = form["complemento"];

            DbFactory.Instance.EnderecoRepository.Save(end);

            return RedirectToAction("Index");
        }
    }
}