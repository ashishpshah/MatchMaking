using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace MatchMaking.Infra
{
	public interface IRepositoryBase<T> where T : class
	{
		IEnumerable<T> GetAll();
		IEnumerable<T> GetByCondition(Expression<Func<T, bool>> expression);
		bool Any(Expression<Func<T, bool>> expression);
		T Add(T entity);
		void Add(List<T> entity);
		void Update(T entity);
		void Delete(T entity);
	}


	//public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
	public class RepositoryWrapper<T> : IRepositoryBase<T> where T : class
	{
		private readonly DataContext _context;

		public RepositoryWrapper(DataContext context) { _context = context; }

		public IEnumerable<T> GetAll() => _context.Set<T>().AsNoTracking().ToList();

		public IEnumerable<T> GetByCondition(Expression<Func<T, bool>> expression) =>
			_context.Set<T>().Where(expression).AsNoTracking().ToList();

		public bool Any(Expression<Func<T, bool>> expression) =>
			_context.Set<T>().Any(expression);

		// -----------------------------
		// ADD
		// -----------------------------
		public T Add(T entity)
		{
			_context.Set<T>().Add(entity);
			_context.SaveChanges();

			// Reload with DB values (identity / default values)
			_context.Entry(entity).Reload();

			// Detach to avoid tracking issues
			_context.Entry(entity).State = EntityState.Detached;

			return entity;
		}

		public void Add(List<T> entity)
		{
			_context.Set<T>().AddRange(entity);
			_context.SaveChanges();
		}

		// -----------------------------
		// UPDATE
		// -----------------------------
		public void Update(T entity)
		{
			_context.Attach(entity); // IMPORTANT in EF Core
			_context.Entry(entity).State = EntityState.Modified;

			_context.SaveChanges();
			_context.Entry(entity).State = EntityState.Detached;
		}

		// -----------------------------
		// DELETE
		// -----------------------------
		public void Delete(T entity)
		{
			_context.Attach(entity); // must attach if not tracked
			_context.Entry(entity).State = EntityState.Deleted;

			_context.SaveChanges();
			_context.Entry(entity).State = EntityState.Detached;
		}
	}


	public interface IRepositoryWrapper
	{
		IRepositoryBase<T> Using<T>() where T : class;
		IDbContextTransaction BeginTransaction();
	}


	public class RepositoryWrapper : IRepositoryWrapper
	{
		private readonly DataContext _context;

		public RepositoryWrapper(DataContext context) { _context = context; }

		public IRepositoryBase<T> Using<T>() where T : class => new RepositoryWrapper<T>(_context);
		public IDbContextTransaction BeginTransaction() { return _context.Database.BeginTransaction(); }

	}

}
