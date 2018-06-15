using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tecnostore.Model.DB;
using Tecnostore.Model.DB.Model;


namespace Tecnostore.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var est = DbFactory.Instance.EstoqueRepository.FindAll().Where(x => x.Quantidade > 0);
            return View(est);

        }

        public ActionResult Produtos()
        {
            var prods = DbFactory.Instance.ProdutoRepository.FindAll();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Sua página de descrição do Site.";

            return View();
        }

        public ActionResult Contato()
        {
            ViewBag.Message = "Pagina de Contato.";

            return View();
        }

        //LOGIN 
        public ActionResult Denied()
        {
            return View();
        }

        public ActionResult Authenticated()
        {
            var usr = DbFactory.Instance.UserRepository.isAuthenticated();
            if (usr != null)
            {
                if (usr.isAdmin())
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index");
            }

            return RedirectToAction("Denied");
        }

        [HttpPost]
        public ActionResult logar(FormCollection form)
        {
            string email = form["email"].ToString();
            string pass = form["pass"].ToString();

            
        if (DbFactory.Instance.UserRepository.Authenticate(email, pass))
            {
                return RedirectToAction("Authenticated");
            }
            return RedirectToAction("Denied");
        }

        //Get
        public ActionResult Login()
        {
            return View();
        }


        public ActionResult Logout()
        {
            DbFactory.Instance.UserRepository.Logout();
            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            User usr = new User();

            usr.Email = form["email"].ToString();
            usr.Senha = form["pass"].ToString();
            usr.Nome = form["nome"].ToString();
            usr.Sobrenome = form["sobrenome"].ToString();
            DbFactory.Instance.UserRepository.Save(usr);
            DbFactory.Instance.UserRepository.Authenticate(usr.Email, usr.Senha);
            return RedirectToAction("Index");
        }

        public ActionResult Jogos()
        {
            var est = DbFactory.Instance.EstoqueRepository.FindAll().Where(x => x.Produto.Categoria.Nome == "Jogos").Where(k => k.Quantidade > 0);
            return View(est);
        }
        public ActionResult Acessorios()
        {
            var est = DbFactory.Instance.EstoqueRepository.FindAll().Where(k => k.Quantidade > 0).Where(x => x.Produto.Categoria.Nome == "Acessórios");
            return View(est);
        }
        [HttpPost]
        public ActionResult Buscar(FormCollection form)
        {
            double precoMin;
            double precoMax;

            var min = form["min"];
            var max = form["max"];
            if (form["min"] != "")
            {
                precoMin = Convert.ToDouble(form["min"]);
            }
            else
            {
                precoMin = 0;
            }
            if (form["max"] != "")
            {
                precoMax = Convert.ToDouble(form["max"]);
            }
            else
            {
                precoMax = 0;
            }

            string nome = form["busca"].ToString();

            Pesquisa pesq = new Pesquisa()
            {
                Nome = nome,
                PrecoMaximo = precoMax,
                PrecoMinimo = precoMin,
                Data = DateTime.Now,
                Categoria = "Todos"
            };
            var usr = DbFactory.Instance.UserRepository.isAuthenticated();
            if (usr != null)
            {
                pesq.Usuario = usr;
            }
            DbFactory.Instance.PesquisaRepository.Save(pesq);
            var est = DbFactory.Instance.EstoqueRepository.FindAll().Where(k => k.Quantidade > 0).Where(x => x.Produto.Nome.ToUpper().Contains(form["busca"].ToString().ToUpper()));

            if (precoMax > 0)
            {
                est = est.Where(x => x.Preco >= precoMin).Where(x => x.Preco <= precoMax).OrderBy(x => x.Preco);
            }

            return View("Index", est);
        }

        [HttpPost]
        public ActionResult BuscarJogos(FormCollection form)
        {

            double precoMin;
            double precoMax;

            var min = form["min"];
            var max = form["max"];
            if (form["min"] != "")
            {
                precoMin = Convert.ToDouble(form["min"]);
            }
            else
            {
                precoMin = 0;
            }
            if (form["max"] != "")
            {
                precoMax = Convert.ToDouble(form["max"]);
            }
            else
            {
                precoMax = 0;
            }

            string nome = form["busca"].ToString();

            Pesquisa pesq = new Pesquisa()
            {
                Nome = nome,
                PrecoMaximo = precoMax,
                PrecoMinimo = precoMin,
                Data = DateTime.Now,
                Categoria = "Jogos"
            };
            var usr = DbFactory.Instance.UserRepository.isAuthenticated();
            if (usr != null)
            {
                pesq.Usuario = usr;
            }
            DbFactory.Instance.PesquisaRepository.Save(pesq);

            var est = DbFactory.Instance.EstoqueRepository.FindAll().Where(k => k.Quantidade > 0).Where(x => x.Produto.Categoria.Nome == "Jogos");
            est = est.Where(x => x.Produto.Nome.ToUpper().Contains(form["busca"].ToString().ToUpper()));

            if (precoMax > 0)
            {
                est = est.Where(x => x.Preco >= precoMin).Where(x => x.Preco <= precoMax).OrderBy(x => x.Preco);
            }

            return View("Jogos", est);
        }
        [HttpPost]
        public ActionResult BuscarAcessorios(FormCollection form)
        {
            double precoMin;
            double precoMax;

            var min = form["min"];
            var max = form["max"];
            if (form["min"] != "")
            {
                precoMin = Convert.ToDouble(form["min"]);
            }
            else
            {
                precoMin = 0;
            }
            if (form["max"] != "")
            {
                precoMax = Convert.ToDouble(form["max"]);
            }
            else
            {
                precoMax = 0;
            }

            string nome = form["busca"].ToString();

            Pesquisa pesq = new Pesquisa()
            {
                Nome = nome,
                PrecoMaximo = precoMax,
                PrecoMinimo = precoMin,
                Data = DateTime.Now,
                Categoria = "Acessorios"
            };
            var usr = DbFactory.Instance.UserRepository.isAuthenticated();
            if (usr != null)
            {
                pesq.Usuario = usr;
            }
            DbFactory.Instance.PesquisaRepository.Save(pesq);

            var est = DbFactory.Instance.EstoqueRepository.FindAll().Where(x => x.Produto.Categoria.Nome == "Acessorios").Where(k => k.Quantidade > 0);
            est = est.Where(x => x.Produto.Nome.ToUpper().Contains(form["busca"].ToString().ToUpper()));

            if (precoMax > 0)
            {
                est = est.Where(x => x.Preco >= precoMin).Where(x => x.Preco <= precoMax).OrderBy(x => x.Preco);
            }

            return View("Acessorios", est);
        }

        public ActionResult Details(int id)
        {
            Estoque e = DbFactory.Instance.EstoqueRepository.FindAll().Where(x => x.Id == id).FirstOrDefault();
            return View(e);
        }

        //Metotodo Carrinho
        public ActionResult Carrinho()
        {
            Carrinho car = new Carrinho();

            if (HttpContext.Session["cartID"] != null && HttpContext.Session["cartID"].ToString() != "")
            {
                var idCart = Convert.ToInt32(HttpContext.Session["cartID"].ToString());
                car = DbFactory.Instance.CarrinhoRepository.FindAll().Where(x => x.Id == idCart).FirstOrDefault();
            }
            else
            {
                car = DbFactory.Instance.CarrinhoRepository.Save(car);
                HttpContext.Session["cartID"] = car.Id;
            }


            car.CarrinhoProduto = DbFactory.Instance.CarrinhoProdutoRepository.FindAll().Where(x => x.Id == car.Id).ToList();

            return View(car);

        }
        public ActionResult AddToCart(FormCollection form)
        {
            //get the product id
            var estoqueId = Convert.ToInt32(form["estoqueId"].ToString());
            Carrinho car = new Carrinho();

            //Check if cart exists
            if (HttpContext.Session["cartID"] != null && HttpContext.Session["cartID"].ToString() != "")
            {
                var idCart = Convert.ToInt32(HttpContext.Session["cartID"].ToString());
                car = DbFactory.Instance.CarrinhoRepository.FindAll().Where(x => x.Id == idCart).FirstOrDefault();
                car.CarrinhoProduto = DbFactory.Instance.CarrinhoProdutoRepository.FindAll().Where(x => x.Carrinho.Id == car.Id).ToList();

                //Check if the product is already in cart

                foreach (var cps in car.CarrinhoProduto)
                {
                    if (cps.Estoque.Id == estoqueId)
                    {
                        cps.Quantidade++;
                        return View("Carrinho", car);
                    }
                }
            }
            else
            {
                car = DbFactory.Instance.CarrinhoRepository.Save(car);
            }


            Estoque est = DbFactory.Instance.EstoqueRepository.FindAll().Where(x => x.Id == estoqueId).FirstOrDefault();

            //Create new CP
            CarrinhoProduto cp = new CarrinhoProduto();
            cp = DbFactory.Instance.CarrinhoProdutoRepository.Save(cp);
            cp.Estoque = est;
            cp.Carrinho = car;
            cp.Quantidade = 1;

            cp = DbFactory.Instance.CarrinhoProdutoRepository.Save(cp);

            car.CarrinhoProduto = DbFactory.Instance.CarrinhoProdutoRepository.FindAll().Where(x => x.Id == car.Id).ToList();

            HttpContext.Session["cartID"] = car.Id;

            return View("Carrinho", car);
        }


        //Auth Needed
        public ActionResult SaveComment(FormCollection form)
        {
            if (this.CheckLogIn())
            {

                Comentario com = new Comentario();
                com.Avaliacao = form["Avaliacao"].ToString();
                com.Texto = form["texto"].ToString();
                com.Produto = DbFactory.Instance.ProdutoRepository.FindAll().Where(x => x.Id == Convert.ToInt32(form["produtoID"])).FirstOrDefault();
                com.Usuario = DbFactory.Instance.UserRepository.isAuthenticated();
                com.Data = DateTime.Now;
                DbFactory.Instance.ComentarioRepository.Save(com);

                var est = DbFactory.Instance.EstoqueRepository.FindAll().Where(x => x.Produto.Id == com.Produto.Id).FirstOrDefault();
                return View("Details", est);
            }
            return RedirectToAction("Denied");

        }

        public Boolean CheckLogIn()
        {
            var usr = DbFactory.Instance.UserRepository.isAuthenticated();
            if (usr != null)
            {
                return true;
            }
            return false;
        }

        public ActionResult AumentarProduto(FormCollection form)
        {
            var cpId = Convert.ToInt32(form["carrinhoProdutoID"].ToString());
            CarrinhoProduto cp = DbFactory.Instance.CarrinhoProdutoRepository.FindAll().Where(x => x.Id == cpId).FirstOrDefault();

            try
            {
                cp.Quantidade = Convert.ToInt32(form["quantidade"]);
                DbFactory.Instance.CarrinhoProdutoRepository.Save(cp);
            }
            catch (Exception)
            {
                //  throw;
            }

            return RedirectToAction("Carrinho");
        }
        public ActionResult DeletarProduto(FormCollection form)
        {
            var cpId = Convert.ToInt32(form["carrinhoProdutoID"].ToString());
            //CarrinhoProduto cp = DbConfig.Instance.CarrinhoProdutoRepository.FindAll().Where(x => x.Id == cpId).FirstOrDefault();
            DbFactory.Instance.CarrinhoProdutoRepository.Deletar(cpId);

            return RedirectToAction("Carrinho");
        }
        public ActionResult CupomDesconto(FormCollection form)
        {
            //Check if the code is valid
            var desc = DbFactory.Instance.DescontoRepository.FindAll().Where(x => x.Codigo == form["desconto"].ToString()).FirstOrDefault();
            if (desc != null)
            {
                //Attaching discount to cart
                var cart = DbFactory.Instance.CarrinhoRepository.FindAll().Where(x => x.Id == Convert.ToInt32(form["carrinhoId"].ToString())).FirstOrDefault();
                cart.Desconto = desc;
                DbFactory.Instance.CarrinhoRepository.Save(cart);
            }
            return RedirectToAction("Carrinho");
        }

    }
}