using Cidean.GatherHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Data
{
    public static class SeedData
    {
        public static void SeedDatabase(HubContext context)
        {
            if(!context.CourseCategories.Any())
            {
                var courseCategories = new List<CourseCategory>()
                {
                    new CourseCategory("Arts and Crafts"),
                    new CourseCategory("Category B"),
                    new CourseCategory("Category C")
                };
                context.CourseCategories.AddRange(courseCategories);
            }

            if(!context.Courses.Any())
            {
                var courses = new List<Course>()
                {
                    new Course("CALLIGRAPHY FOR BEGINNERS","Calligraphy is a skill that anyone can learn. With individualized instruction, regular practice and dedication, you will be able to reach your goals. Whether you are looking to address invitations for that special day or send a greeting card, calligraphy will enhance the beauty and impact of all your correspondences. This class provides a birds eye view into the world of Calligraphy, its history and the resources for you to continue perfecting your new skill. Learning the Chancery script will provide a great foundation and will help you if you choose to explore additional scripts. ","Flying Dolphin Studio","MWF 1-2",1),
                    new Course("Course B","","Main Office","MWF 1-2",1),
                    new Course("Course C","","Main Office","MWF 1-2",2),
                    new Course("Course D","","Main Office","MWF 1-2",3)
                };
                context.Courses.AddRange(courses);
            }

            context.SaveChanges();

        }
    }
}
