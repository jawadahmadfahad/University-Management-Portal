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
    public class FacultyService : IFacultyService
    {
        private readonly FacultyDAL _facultyDAL;

        public FacultyService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _facultyDAL = new FacultyDAL(connectionString);
        }

        public Task<List<Faculty>> GetAllFacultiesAsync() => _facultyDAL.GetAllFacultiesAsync();
        public Task AddFacultyAsync(Faculty faculty) => _facultyDAL.AddFacultyAsync(faculty);
        public Task UpdateFacultyAsync(Faculty faculty) => _facultyDAL.UpdateFacultyAsync(faculty);
        public Task DeleteFacultyAsync(int id) => _facultyDAL.DeleteFacultyAsync(id);
        public Task<Faculty?> GetFacultyByIdAsync(int id) => _facultyDAL.GetFacultyByIdAsync(id);
    }

}
