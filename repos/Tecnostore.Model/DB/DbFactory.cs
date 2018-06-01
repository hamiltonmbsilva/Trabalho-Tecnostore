using MySql.Data.MySqlClient;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tecnostore.Model.DB.Model;
using Tecnostore.Model.DB.Repository;

namespace Tecnostore.Model.DB
{
    public class DbFactory
    {
        private static DbFactory _instance = null;

        private ISessionFactory _sessionFactory;

        public UserRepository UserRepository { get; set; }
        public ComentarioRepository ComentarioRepository { get; set; }
        public EnderecoRepository EnderecoRepository { get; set; }
        public ProdutoRepository ProdutoRepository { get; set; }
        public TipoProdutoRepository TipoProdutoRepository { get; set; }
        public PesquisaRepository PesquisaRepository { get; set; }
        public ImagemRepository ImagemRepository { get; set; }
        public CarrinhoRepository CarrinhoRepository { get; set; }
        public CarrinhoProdutoRepository CarrinhoProdutoRepository { get; set; }
        public CategoriaRepository CategoriaRepository { get; set; }
        public DescontoRepository DescontoRepository { get; set; }
        public EstoqueRepository EstoqueRepository { get; set; }
        public VendaRepository VendaRepository { get; set; }
        public FormaPagamentoRepository FormaPagamentoRepository { get; set; }
        public ItemVendaRepository ItemVendaRepository { get; set; }


        private DbFactory()
        {
            Conectar();

            this.UserRepository = new UserRepository(this.Session);
            this.ComentarioRepository = new ComentarioRepository(this.Session);
            this.EnderecoRepository = new EnderecoRepository(this.Session);
            this.ProdutoRepository = new ProdutoRepository(this.Session);
            this.TipoProdutoRepository = new TipoProdutoRepository(this.Session);
            this.PesquisaRepository = new PesquisaRepository(this.Session);
            this.ImagemRepository = new ImagemRepository(this.Session);
            this.CarrinhoRepository = new CarrinhoRepository(this.Session);
            this.CarrinhoProdutoRepository = new CarrinhoProdutoRepository(this.Session);
            this.CategoriaRepository = new CategoriaRepository(this.Session);
            this.DescontoRepository = new DescontoRepository(this.Session);
            this.EstoqueRepository = new EstoqueRepository(this.Session);
            this.VendaRepository = new VendaRepository(this.Session);
            this.FormaPagamentoRepository = new FormaPagamentoRepository(this.Session);
            this.ItemVendaRepository = new ItemVendaRepository(this.Session);

        }

        //public static DbFactory Instance => _instance ?? (_instance = new DbFactory());

        public void Initialize(object obj)
        {
            NHibernateUtil.Initialize(obj);
        }

        public static DbFactory Instance  // me retortna a instancia do objeto
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DbFactory();
                }

                return _instance;
            }
        }

        private void Conectar()
        {
            try
            {
                var server = "localhost";
                var port = "3306";
                var dbName = "db_Tecnostore";
                var user = "root";
                var psw = "247845";

                //var stringConexao = "Persist Security Info=False;" +
                //                    "server=" + server +
                //                    ";port=" + port +
                //                    ";database=" + dbName +
                //                    ";uid=" + user +
                //                    ";pwd=" + psw;                                      
                //                    //";SslMode= + none;
                //                     //   ssl: true,

                var stringConexao = "Server="+ server + 
                                    ";Port="+ port +
                                    ";Database=" + dbName +
                                    ";Uid=" + user +
                                    ";Pwd="+ psw + ";SslMode=none;";

                var mySql = new MySqlConnection(stringConexao);

                try
                {
                    //var mysql = new MySqlConnection();
                    //mysql.Open(); 

                    //if (mysql.State == ConnectionState.Open)
                    //{
                    //    mysql.Close();
                    //}
                    mySql.Open();
                }

                catch
                {

                    CriarSchema(server, port, dbName, psw, user);
                }
                finally
                {
                    if (mySql.State == ConnectionState.Open)
                    {
                        mySql.Close();
                    }
                }

                ConfigurarNHibernate(stringConexao);
            }

            catch (Exception ex)
            {
                
                throw new Exception("Nao foi possivel conectar no banco de dados", ex);
            }
        }

        private void CriarSchema(string server, string port, string dbName, string psw, string user)
        {
            
            try
            {

                var stringConexao = "Server=" + server +
                                    ";Port=" + port + 
                                    ";Uid=" + user +
                                    ";Pwd=" + psw + ";SslMode=none;";

                var mySql = new MySqlConnection(stringConexao);
                // vai tentar conectar com o mysql, nao com um banco especifico
                var cmd = mySql.CreateCommand();

                mySql.Open();
                cmd.CommandText = "CREATE DATABASE IF NOT EXISTS `" + dbName + "`;";
                cmd.ExecuteNonQuery();
                mySql.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Não foi criar o banco de dados.", ex);
            }
        }

        private void ConfigurarNHibernate(string stringConexao)
        {
            //Cria a configuração com o NH
            var config = new Configuration();

            try
            {
                //var config = new Configuration(); // using nHibernate config

                //Exibição de SQL no console
                //ConfigureLog4Net();

                //Configuração do NHibernate

                config.DataBaseIntegration(i =>
                {
                    //Dialeto do banco de dados
                    i.Dialect<NHibernate.Dialect.MySQLDialect>();
                    // conexao string
                    i.ConnectionString = stringConexao;
                    // Drive de conexao com o banco
                    i.Driver<NHibernate.Driver.MySqlDataDriver>();
                    // Provedor de conexao
                    i.ConnectionProvider<NHibernate.Connection.DriverConnectionProvider>();
                    // GERA LOG DOS SQL EXECUTADOS NO CONSOLE
                    i.LogSqlInConsole = true;
                    // DESCONECTAR CASO QUEIRA VISUALIZAR O LOG DE SQL FORMATADO NO CONSOLE
                    i.LogFormattedSql = true;
                    // CRIA O SCHEMA DO BANCO DE DADOS SEMPRE QUE A CONFIGURATION FOR UTILIZADO
                    i.SchemaAction = SchemaAutoAction.Update;

                });

                //Realiza o mapeamento das classes, todas as classess que foram criadas vao para o NHibernate
                var maps = this.Mapeamento();
                config.AddMapping(maps);

                //Para verificar se a aplicação é Descktop ou Web
                // antes disso coloca a referencia no seu projeto de baixo
                // referencias - Add referencia - Assembly = Sistem.Web

                if (HttpContext.Current == null)
                {
                    config.CurrentSessionContext<ThreadStaticSessionContext>();
                }
                else
                {
                    config.CurrentSessionContext<WebSessionContext>();
                }

                this._sessionFactory = config.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel configurar o NHibernate", ex);
            }
        }

        private HbmMapping Mapeamento()
        {            
            try
            {
                var mapper = new ModelMapper();

                // Para o NHibernate com apenas um objeto(Uma classe, nesse cado so uma: Esporte ou outra)
                // mapper.AddMapping(EsporteMap);

                mapper.AddMappings(
                    Assembly.GetAssembly(typeof(UserMap)).GetTypes()
                );

                return mapper.CompileMappingForAllExplicitlyAddedEntities();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível realizar o mapeamento do modelo!!!", ex);
            }

        }

        public ISession Session // USING DO NHIBERNATE
        {
            get
            {
                try
                {
                    if (CurrentSessionContext.HasBind(_sessionFactory))
                        return _sessionFactory.GetCurrentSession();

                    //  ESSE VAR VAI DA ERRO E VOCE TEM QUE COLOCAR:
                    // private ISessionFactory _sessionFactory;
                    // LA ENCIMA NO DBFACTORY
                    var session = _sessionFactory.OpenSession();
                    session.FlushMode = FlushMode.Commit;

                    CurrentSessionContext.Bind(session);

                    return session;
                }
                catch (Exception ex)
                {
                    throw new Exception("Não foi possivel criar a Sessão!!!", ex);
                }
            }
        }

        public void ClearSession()
        {
            this.Session.Clear();
        }
       
    }
}
