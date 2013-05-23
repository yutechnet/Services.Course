using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Entities;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Linq;
using NHibernate.Stat;
using NHibernate.Type;

namespace BpeProducts.Services.Course.Domain.Repositories
{
	public class Repository:IRepository
	{
		private readonly ISession _session;

		public Repository(ISession session)
		{
			_session = session;
		}

		public void Dispose()
		{
			_session.Dispose();
		}

		public ISessionStatistics Statistics
		{
			get { return _session.Statistics; }
		}

		public ITransaction Transaction
		{
			get { return _session.Transaction; }
		}

		public bool DefaultReadOnly
		{
			get { return _session.DefaultReadOnly; }
			set { _session.DefaultReadOnly = value; }
		}

		public bool IsConnected
		{
			get { return _session.IsConnected; }
		}

		public bool IsOpen
		{
			get { return _session.IsOpen; }
		}

		public IDbConnection Connection
		{
			get { return _session.Connection; }
		}

		public ISessionFactory SessionFactory
		{
			get { return _session.SessionFactory; }
		}

		public CacheMode CacheMode
		{
			get { return _session.CacheMode; }
			set { _session.CacheMode = value; }
		}

		public FlushMode FlushMode
		{
			get { return _session.FlushMode; }
			set { _session.FlushMode = value; }
		}

		public EntityMode ActiveEntityMode
		{
			get { return _session.ActiveEntityMode; }
		}

		public ISession GetSession(EntityMode entityMode)
		{
			return _session.GetSession(entityMode);
		}

		public IMultiCriteria CreateMultiCriteria()
		{
			return _session.CreateMultiCriteria();
		}

		public ISessionImplementor GetSessionImplementation()
		{
			return _session.GetSessionImplementation();
		}

		public ISession SetBatchSize(int batchSize)
		{
			return _session.SetBatchSize(batchSize);
		}

		public IMultiQuery CreateMultiQuery()
		{
			return _session.CreateMultiQuery();
		}

		public void DisableFilter(string filterName)
		{
			_session.DisableFilter(filterName);
		}

		public IFilter GetEnabledFilter(string filterName)
		{
			return _session.GetEnabledFilter(filterName);
		}

		public IFilter EnableFilter(string filterName)
		{
			return _session.EnableFilter(filterName);
		}

		public string GetEntityName(object obj)
		{
			return _session.GetEntityName(obj);
		}

		public T Get<T>(object id, LockMode lockMode)
		{
			return _session.Get<T>(id, lockMode);
		}

		public T Get<T>(object id)
		{
			return _session.Get<T>(id);
		}

		public object Get(string entityName, object id)
		{
			return _session.Get(entityName, id);
		}

		public object Get(Type clazz, object id, LockMode lockMode)
		{
			return _session.Get(clazz, id, lockMode);
		}

		public object Get(Type clazz, object id)
		{
			return _session.Get(clazz, id);
		}

		public void Clear()
		{
			_session.Clear();
		}

		public ISQLQuery CreateSQLQuery(string queryString)
		{
			return _session.CreateSQLQuery(queryString);
		}

		public IQuery GetNamedQuery(string queryName)
		{
			return _session.GetNamedQuery(queryName);
		}

		public IQuery CreateFilter(object collection, string queryString)
		{
			return _session.CreateFilter(collection, queryString);
		}

		public IQuery CreateQuery(string queryString)
		{
			return _session.CreateQuery(queryString);
		}

		public IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class
		{
			return _session.QueryOver(entityName, alias);
		}

		public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
		{
			return _session.QueryOver<T>(entityName);
		}

		public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
		{
			return _session.QueryOver(alias);
		}

		public IQueryOver<T, T> QueryOver<T>() where T : class
		{
			return _session.QueryOver<T>();
		}

		public ICriteria CreateCriteria(string entityName, string alias)
		{
			return _session.CreateCriteria(entityName, alias);
		}

		public ICriteria CreateCriteria(string entityName)
		{
			return _session.CreateCriteria(entityName);
		}

		public ICriteria CreateCriteria(Type persistentClass, string alias)
		{
			return _session.CreateCriteria(persistentClass, alias);
		}

		public ICriteria CreateCriteria(Type persistentClass)
		{
			return _session.CreateCriteria(persistentClass);
		}

		public ICriteria CreateCriteria<T>(string alias) where T : class
		{
			return _session.CreateCriteria<T>(alias);
		}

		public ICriteria CreateCriteria<T>() where T : class
		{
			return _session.CreateCriteria<T>();
		}

		public ITransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			return _session.BeginTransaction(isolationLevel);
		}

		public ITransaction BeginTransaction()
		{
			return _session.BeginTransaction();
		}

		public LockMode GetCurrentLockMode(object obj)
		{
			return _session.GetCurrentLockMode(obj);
		}

		public void Refresh(object obj, LockMode lockMode)
		{
			_session.Refresh(obj, lockMode);
		}

		public void Refresh(object obj)
		{
			_session.Refresh(obj);
		}

		public void Lock(string entityName, object obj, LockMode lockMode)
		{
			_session.Lock(entityName, obj, lockMode);
		}

		public void Lock(object obj, LockMode lockMode)
		{
			_session.Lock(obj, lockMode);
		}

		public int Delete(string query, object[] values, IType[] types)
		{
			return _session.Delete(query, values, types);
		}

		public int Delete(string query, object value, IType type)
		{
			return _session.Delete(query, value, type);
		}

		public int Delete(string query)
		{
			return _session.Delete(query);
		}

		public void Delete(string entityName, object obj)
		{
			_session.Delete(entityName, obj);
		}

		public void Delete(object obj)
		{
			_session.Delete(obj);
		}

		public object SaveOrUpdateCopy(object obj, object id)
		{
			return _session.SaveOrUpdateCopy(obj, id);
		}

		public object SaveOrUpdateCopy(object obj)
		{
			return _session.SaveOrUpdateCopy(obj);
		}

		public void Persist(string entityName, object obj)
		{
			_session.Persist(entityName, obj);
		}

		public void Persist(object obj)
		{
			_session.Persist(obj);
		}

		public T Merge<T>(string entityName, T entity) where T : class
		{
			return _session.Merge(entityName, entity);
		}

		public T Merge<T>(T entity) where T : class
		{
			return _session.Merge(entity);
		}

		public object Merge(string entityName, object obj)
		{
			return _session.Merge(entityName, obj);
		}

		public object Merge(object obj)
		{
			return _session.Merge(obj);
		}

		public void Update(string entityName, object obj)
		{
			_session.Update(entityName, obj);
		}

		public void Update(object obj, object id)
		{
			_session.Update(obj, id);
		}

		public void Update(object obj)
		{
			_session.Update(obj);
		}

		public void SaveOrUpdate(string entityName, object obj)
		{
			_session.SaveOrUpdate(entityName, obj);
		}

		public void SaveOrUpdate(object obj)
		{
			_session.SaveOrUpdate(obj);
		}

		public object Save(string entityName, object obj)
		{
			return _session.Save(entityName, obj);
		}

		public void Save(object obj, object id)
		{
			_session.Save(obj, id);
		}

		public object Save(object obj)
		{
			return _session.Save(obj);
		}

		public void Replicate(string entityName, object obj, ReplicationMode replicationMode)
		{
			_session.Replicate(entityName, obj, replicationMode);
		}

		public void Replicate(object obj, ReplicationMode replicationMode)
		{
			_session.Replicate(obj, replicationMode);
		}

		public void Load(object obj, object id)
		{
			_session.Load(obj, id);
		}

		public object Load(string entityName, object id)
		{
			return _session.Load(entityName, id);
		}

		public T Load<T>(object id)
		{
			return _session.Load<T>(id);
		}

		public T Load<T>(object id, LockMode lockMode)
		{
			return _session.Load<T>(id, lockMode);
		}

		public object Load(Type theType, object id)
		{
			return _session.Load(theType, id);
		}

		public object Load(string entityName, object id, LockMode lockMode)
		{
			return _session.Load(entityName, id, lockMode);
		}

		public object Load(Type theType, object id, LockMode lockMode)
		{
			return _session.Load(theType, id, lockMode);
		}

		public void Evict(object obj)
		{
			_session.Evict(obj);
		}

		public bool Contains(object obj)
		{
			return _session.Contains(obj);
		}

		public object GetIdentifier(object obj)
		{
			return _session.GetIdentifier(obj);
		}

		public void SetReadOnly(object entityOrProxy, bool readOnly)
		{
			_session.SetReadOnly(entityOrProxy, readOnly);
		}

		public bool IsReadOnly(object entityOrProxy)
		{
			return _session.IsReadOnly(entityOrProxy);
		}

		public bool IsDirty()
		{
			return _session.IsDirty();
		}

		public void CancelQuery()
		{
			_session.CancelQuery();
		}

		public IDbConnection Close()
		{
			return _session.Close();
		}

		public void Reconnect(IDbConnection connection)
		{
			_session.Reconnect(connection);
		}

		public void Reconnect()
		{
			_session.Reconnect();
		}

		public IDbConnection Disconnect()
		{
			return _session.Disconnect();
		}

		public void Flush()
		{
			_session.Flush();
		}

		public  IQueryable<T> Query<T>()
		{
			return _session.Query<T>();
		}

		
	}

	public interface IRepository:ISession
	{
		IQueryable<T> Query<T>();
		//object Save(object obj);
		//IQueryOver<T, T> QueryOver<T>();
	}
}
