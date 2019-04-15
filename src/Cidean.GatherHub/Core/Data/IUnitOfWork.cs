using Cidean.GatherHub.Core.Models;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Data
{
    public interface IUnitOfWork
    {
        CourseRepository Courses { get; }
        IRepository<CourseCategory> CourseCategories { get; }
        IRepository<AdminUser> AdminUsers { get; }
        IRepository<Member> Members { get; }
        IRepository<CourseMember> CourseMembers { get; }
        IRepository<ActivityLogItem> ActivityLogItems { get; }

        ILogger Logger { get; }

        Task Save();
        void Dispose();
        
    }
}