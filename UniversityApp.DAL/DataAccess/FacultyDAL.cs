using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UniversityApp.Entities;


namespace UniversityApp.DAL.DataAccess
{
    public class FacultyDAL
    {
        private readonly string _connectionString;

        public FacultyDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Faculty>> GetAllFacultiesAsync()
        {
            var list = new List<Faculty>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Name, Email, Department, CreatedAt FROM Faculties";
                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        list.Add(new Faculty
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Department = reader.GetString(3),
                            CreatedAt = reader.GetDateTime(4)
                        });
                    }
                }
            }
            return list;
        }

        public async Task AddFacultyAsync(Faculty faculty)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "INSERT INTO Faculties (Name, Email, Department, CreatedAt) VALUES (@Name, @Email, @Department, @CreatedAt)";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", faculty.Name ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Email", faculty.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Department", faculty.Department ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CreatedAt", faculty.CreatedAt);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateFacultyAsync(Faculty faculty)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"UPDATE Faculties SET Name = @Name, Email = @Email, Department = @Department, CreatedAt = @CreatedAt WHERE Id = @Id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", faculty.Id);
            command.Parameters.AddWithValue("@Name", faculty.Name ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Email", faculty.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Department", faculty.Department ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CreatedAt", faculty.CreatedAt);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteFacultyAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Faculties WHERE Id = @Id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<Faculty?> GetFacultyByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT Id, Name, Email, Department, CreatedAt FROM Faculties WHERE Id = @Id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Faculty
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Department = reader.GetString(3),
                    CreatedAt = reader.GetDateTime(4)
                };
            }
            return null;
        }
    }

}
