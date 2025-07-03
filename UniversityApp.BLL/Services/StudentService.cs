using UniversityApp.BLL.Interfaces;
using UniversityApp.DAL.DataAccess;
using UniversityApp.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversityApp.BLL.Services
{
    public class StudentService : IStudentService
    {
        private readonly StudentDAL _studentDAL;

        public StudentService(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            _studentDAL = new StudentDAL(connectionString);
        }

        public Task<List<Student>> GetAllStudentsAsync()
        {
            return _studentDAL.GetAllStudentsAsync();
        }

        public Task AddStudentAsync(Student student)
        {
            return _studentDAL.AddStudentAsync(student);
        }

        public Task UpdateStudentAsync(Student student)
        {
            return _studentDAL.UpdateStudentAsync(student);
        }

        public Task DeleteStudentAsync(int id)
        {
            return _studentDAL.DeleteStudentAsync(id);
        }

        public Task<Student?> GetStudentByIdAsync(int id)
        {
            return _studentDAL.GetStudentByIdAsync(id);
        }

        public Task<List<Student>> SearchStudentsAsync(string keyword)
        {
            return _studentDAL.SearchStudentsAsync(keyword);
        }

    }
}
