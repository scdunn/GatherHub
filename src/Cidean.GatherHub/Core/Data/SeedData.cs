using Cidean.GatherHub.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Cidean.GatherHub.Core.Data
{
   
    public static class SeedData {

        public class DataToSeed
        {
            public List<CourseCategory> Categories;
            public List<Member> Members;
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

                foreach (var member in data.Members)
                    member.SetPassword("password");

                context.Members.AddRange(data.Members);
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
                
                XDocument doc = new XDocument();
                var dataFile = Path.Combine(env.WebRootPath, "data/data.xml");
                if(File.Exists(dataFile))
                { 
                    doc = XDocument.Load(dataFile);
                    foreach (var courseXml in doc.Descendants("Link"))
                    {
                        var course = new Course();
                        course.Description = courseXml.Element("Description").Value;
                        course.Title = courseXml.Element("Title").Value;
                        course.Location = courseXml.Element("Address").Value;
                        course.ImageUrl = "/images/courses/" + courseXml.Element("Photo").Value;
                        course.InstructorId = 1;
                        course.CourseCategoryId = 1;
                        context.Courses.Add(course);
                    }
                }

            }
            


            context.SaveChanges();


          

        }
    }
}
