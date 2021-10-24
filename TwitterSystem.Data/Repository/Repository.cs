namespace TwitterSystem.Data.Repository
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext dbContext;
        private readonly IDbSet<TEntity> entitySet;

        public Repository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", "dbContext");
            }

            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<TEntity>();
        }

        public IDbSet<TEntity> EntitySet
        {
            get { return this.entitySet; }
        }

        public IQueryable<TEntity> All()
        {
            return this.entitySet;
        }

        public TEntity Find(object id)
        {
            return this.entitySet.Find(id);
        }

        public void Add(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Added);
        }

        public void Update(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        public void Remove(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
        }

        public void Remove(object id)
        {
            this.Remove(this.Find(id));
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        private void ChangeState(TEntity entity, EntityState state)
        {
            var entry = this.dbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.entitySet.Attach(entity);
            }

            entry.State = state;
        }
    }
}
