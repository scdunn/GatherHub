using Cidean.GatherHub.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Data
{
   
    public static class SeedData {

        public class DataToSeed
        {
            public List<CourseCategory> Categories;
            public List<Course> Courses;

        }


        public static void SeedDatabase(IHostingEnvironment env,HubContext context)
        {
            DataToSeed data;

            using (StreamReader r = new StreamReader(Path.Combine(env.WebRootPath, "data/seed.json")))
            {
                var json = r.ReadToEnd();
                data = JsonConvert.DeserializeObject<DataToSeed>(json);
                
            }

            if(!context.AdminUsers.Any())
            {
                
                
                context.AdminUsers.Add(new AdminUser("admin","password"));
            }
            if (!context.Members.Any())
            {


                context.Members.Add(new Member("chris@cidean.com","Chris","Dunn", "password"));
                context.Members.Add(new Member("megan@cidean.com", "Megan", "Dunn", "password"));
                context.Members.Add(new Member("abby@cidean.com", "Abby", "Dunn", "password"));
            }

            
            if (!context.CourseCategories.Any())
            {
                context.CourseCategories.AddRange(data.Categories);
            }

            context.SaveChanges();


            if (!context.Courses.Any())
            {
                /*
                foreach(var course in data.Courses)
                {
                    foreach(var member in context.Members)
                    {
                        course.AddMember(member.Id);
                    }
                }
                */

                context.Courses.AddRange(data.Courses);
            }

            context.SaveChanges();


          

        }
    }
}
