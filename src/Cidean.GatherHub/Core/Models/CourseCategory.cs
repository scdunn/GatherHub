using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Models
{
    public class CourseCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public CourseCategory()
        {

        }

        public CourseCategory(string title)
        {
            Title = title;
        }
    }
}
