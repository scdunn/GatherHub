using Cidean.GatherHub.Core.Models;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Data
{
    public interface IUnitOfWork
    {
        IRepository<Course> Courses { get; }
        IRepository<CourseCategory> CourseCategories { get; }
        IRepository<AdminUser> AdminUsers { get; }
        IRepository<Member> Members { get; }
        IRepository<CourseMember> CourseMembers { get; }

        Task Save();
        void Dispose();
    }
}