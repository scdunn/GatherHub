﻿using Cidean.GatherHub.Core.Models;
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

            if (!context.CourseCategories.Any())
            {
                context.CourseCategories.AddRange(data.Categories);
            }

            if(!context.Courses.Any())
            {
                context.Courses.AddRange(data.Courses);
            }

            context.SaveChanges();

        }
    }
}