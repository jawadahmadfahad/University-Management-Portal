using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using UniversityApp.Entities;

namespace UniversityApp.DAL.DataAccess
{
    public class CourseDAL
    {
        private readonly string _connectionString;

        public CourseDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            var courses = new List<Course>();
            using SqlConnection connection = new(_connectionString);
            string query = "SELECT Id, Title, Code, CreditHours, CreatedAt FROM Courses";

            using SqlCommand command = new(query, connection);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                courses.Add(new Course
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Code = reader.GetString(2),
                    CreditHours = reader.GetInt32(3),
                    CreatedAt = reader.GetDateTime(4)
                });
            }

            return courses;
        }

        public async Task AddCourseAsync(Course course)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "INSERT INTO Courses (Title, Code, CreditHours, CreatedAt) VALUES (@Title, @Code, @CreditHours, @CreatedAt)";
            using SqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@Title", course.Title);
            command.Parameters.AddWithValue("@Code", course.Code);
            command.Parameters.AddWithValue("@CreditHours", course.CreditHours);
            command.Parameters.AddWithValue("@CreatedAt", course.CreatedAt);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateCourseAsync(Course course)
        {
            using SqlConnection connection = new(_connectionString);
            string query = @"
                UPDATE Courses 
                SET Title = @Title, Code = @Code, CreditHours = @CreditHours, CreatedAt = @CreatedAt
                WHERE Id = @Id";

            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@Title", course.Title);
            command.Parameters.AddWithValue("@Code", course.Code);
            command.Parameters.AddWithValue("@CreditHours", course.CreditHours);
            command.Parameters.AddWithValue("@CreatedAt", course.CreatedAt);
            command.Parameters.AddWithValue("@Id", course.Id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteCourseAsync(int id)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "DELETE FROM Courses WHERE Id = @Id";
            using SqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            using SqlConnection connection = new(_connectionString);
            string query = "SELECT Id, Title, Code, CreditHours, CreatedAt FROM Courses WHERE Id = @Id";

            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Course
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Code = reader.GetString(2),
                    CreditHours = reader.GetInt32(3),
                    CreatedAt = reader.GetDateTime(4)
                };
            }

            return null;
        }
    }
}
