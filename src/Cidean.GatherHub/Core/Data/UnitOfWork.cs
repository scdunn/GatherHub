using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cidean.GatherHub.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cidean.GatherHub.Core.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly HubContext _hubContext;
        public ILogger Logger { get; }

        public IRepository<Course> Courses { get; private set; }
        public IRepository<CourseCategory> CourseCategories { get; private set; }
        public IRepository<AdminUser> AdminUsers { get; private set; }
        public IRepository<Member> Members { get; private set; }
        public IRepository<CourseMember> CourseMembers { get; private set; }
        public IRepository<ActivityLogItem> ActivityLogItems { get; private set; }

        public UnitOfWork(HubContext hubContext, ILogger logger)
        {
            //build repositories
            this._hubContext = hubContext;
            Courses = new Repository<Course>(_hubContext);
            CourseCategories = new Repository<CourseCategory>(_hubContext);
            AdminUsers = new Repository<AdminUser>(_hubContext);
            Members = new Repository<Member>(_hubContext);
            CourseMembers = new Repository<CourseMember>(_hubContext);
            ActivityLogItems = new Repository<ActivityLogItem>(_hubContext);

            Logger = logger;

        }



        public async Task Save()
        {
            await _hubContext.SaveChangesAsync();
        }
        


        private bool disposed = false;
        
        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    _hubContext.Dispose();
                }
            }

            this.disposed = true;

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
