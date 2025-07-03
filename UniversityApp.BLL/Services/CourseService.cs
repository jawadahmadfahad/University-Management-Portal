using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using UniversityApp.BLL.Interfaces;
using UniversityApp.DAL.DataAccess;
using UniversityApp.Entities;

namespace UniversityApp.BLL.Services
{
    public class CourseService : ICourseService
    {
        private readonly CourseDAL _courseDAL;

        public CourseService(IConfiguration config)
        {
            string connectionString = config.GetConnectionString("DefaultConnection")!;
            _courseDAL = new CourseDAL(connectionString);
        }

        public Task<List<Course>> GetAllCoursesAsync() => _courseDAL.GetAllCoursesAsync();
        public Task AddCourseAsync(Course course) => _courseDAL.AddCourseAsync(course);
        public Task UpdateCourseAsync(Course course) => _courseDAL.UpdateCourseAsync(course);
        public Task DeleteCourseAsync(int id) => _courseDAL.DeleteCourseAsync(id);
        public Task<Course?> GetCourseByIdAsync(int id) => _courseDAL.GetCourseByIdAsync(id);
    }
}
