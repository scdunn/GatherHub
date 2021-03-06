﻿using Cidean.GatherHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Data
{

    public class CourseRepository: Repository<Course>, IRepository<Course>
    {
        public CourseRepository(DbContext dbContext): base(dbContext)
        {
         
        }

        public IQueryable<Course> GetMemberCourses(int memberId)
        {
            return _dbSet.Where(m => m.CourseMembers.Count(mbr=> mbr.MemberId==memberId)>0).AsQueryable();
        }
    }


    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly DbContext _dbContext;
        protected DbSet<T> _dbSet;

        public Repository(DbContext dbContext)
        {
            this._dbContext = dbContext;
            this._dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public Task<T> GetById(int id)
        {
            return _dbSet.FindAsync(id);
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry<T>(entity).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            T entity = _dbSet.FindAsync(id).Result;
            Delete(entity);
        }
        public virtual void Delete(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

    }
}
