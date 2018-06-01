using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnostore.Model.DB.Repository
{
    public class RepositoryBase<T> where T : class
    {
        public ISession Session;

        public RepositoryBase(ISession session)
        {
            this.Session = session;
        }

        public T FindFirstById(int id)
        {
            return this.Session.CreateCriteria<T>()
                        .Add(Restrictions.Eq("Id", id))
                        .SetMaxResults(1)
                        .List<T>()
                        .FirstOrDefault();
        }

        public T FindFirstOrDefault()
        {
            return this.Session.Query<T>().FirstOrDefault();
        }


        public void Delete(T entity) //porque entity??
        {
            try
            {
                this.Session.Clear();

                var transacao = this.Session.BeginTransaction();
                // essa transicao seria abrindo um caminho para o objeto?

                this.Session.Delete(entity);

                transacao.Commit(); // por que o commit??
            }
            catch (Exception ex)
            {
                throw new Exception("Nao foi possivel excluir" + typeof(T) + "\nErro:" + ex.Message);
            }
        }

        public virtual T SaveOrUptade(T entity) // entity é uma variavel global?
        {
            try
            {
                this.Session.Clear();

                var transacao = this.Session.BeginTransaction();

                this.Session.SaveOrUpdate(entity); // nao entendi esse entity passando
                                              // sei que ele vai entender qual tabela vou atuaizar

                transacao.Commit();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Nao foi possivel salvar" + typeof(T) + "\nErro:" + ex.Message);
            }
        }

        // esse metoto mais o debaixo, fazem a mesma coisa?, um busca pelo Id??

        public virtual IList<T> FindAll()
        {
           
                // selection para qualquer tabela
                return this.Session.CreateCriteria(typeof(T)).List<T>();
                       
        }


        public T FindById(Guid id)
        {
            try
            {
                // selection para qualquer tabela
                return Session.Get<T>(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Não achei esse cara", ex);
            }
        }

        public virtual T Save(T entity)
        {
            try
            {
                this.Session.Clear();

                var transacao = this.Session.BeginTransaction();

                this.Session.Save(entity);

                transacao.Commit();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível salvar " + typeof(T) + "\nErro:" + ex.Message);
            }
        }

        public void Delete(T entity, string id)
        {
            try
            {
                this.Session.CreateQuery(String.Format("delete from {0} where id = {1}", typeof(T).Name, id)).ExecuteUpdate();

                this.Session.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(T) + "\nErro:" + ex.Message);
            }
        }

        public void DeleteAll(List<T> entity)
        {
            try
            {
                this.Session.Clear();

                var transacao = this.Session.BeginTransaction();

                this.Session.Delete(entity);

                transacao.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(T) + "\nErro:" + ex.Message);
            }
        }

        public void Clear()
        {
            if (this.Session != null)
                this.Session.Clear();
        }

        public virtual T UnProxy(T entity)
        {
            try
            {
                return (T)this.Session.GetSessionImplementation().PersistenceContext.Unproxy(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível salvar " + typeof(T) + "\nErro:" + ex.Message);
            }
        }

        public virtual T Update(T entity)
        {
            try
            {
                this.Session.Clear();

                var transacao = this.Session.BeginTransaction();

                this.Session.Update(entity);

                transacao.Commit();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível editar " + typeof(T) + "\nErro:" + ex.Message);
            }
        }
    }
}
