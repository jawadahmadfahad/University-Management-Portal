using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApp.Entities;


namespace UniversityApp.BLL.Interfaces
{
    public interface IFacultyService
    {
        Task<List<Faculty>> GetAllFacultiesAsync();
        Task AddFacultyAsync(Faculty faculty);
        Task UpdateFacultyAsync(Faculty faculty);
        Task DeleteFacultyAsync(int id);
        Task<Faculty?> GetFacultyByIdAsync(int id);
    }

}
