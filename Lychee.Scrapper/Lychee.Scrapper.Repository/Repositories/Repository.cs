using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Lychee.Scrapper.Repository.Interfaces;

namespace Lychee.Scrapper.Repository.Repositories
{
    public class Repository<T> : IRepository<T> 
        where T: class
    {
        private readonly bool _sharedContext;
        private readonly DbContext _context;

        private IDbSet<T> _dbset;
        public virtual IDbSet<T> DbSet
        {
            get => _dbset ?? _context.Set<T>();
            set => _dbset = value;
        }

        public Repository(DbContext context, bool sharedContext = false)
        {
            _context = context;
            _sharedContext = sharedContext;
        }

        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public virtual IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression);
        }

        public virtual T FirstOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return DbSet.FirstOrDefault(expression);
        }

        public virtual T Add(T entity)
        {
            DbSet.Add(entity);

            if (!_sharedContext)
                _context.SaveChanges();

            return entity;
        }

        public virtual void Update(T entity)
        {
            DbSet.AddOrUpdate(entity);
            if (!_sharedContext)
                _context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
            if (!_sharedContext)
                _context.SaveChanges();
        }
    }
}
