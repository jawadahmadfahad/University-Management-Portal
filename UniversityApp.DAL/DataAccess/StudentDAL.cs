using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UniversityApp.Entities;

namespace UniversityApp.DAL.DataAccess
{
    public class StudentDAL
    {
        private readonly string _connectionString;

        public StudentDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var students = new List<Student>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Name, Email, Department, CreatedAt FROM Students";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            students.Add(new Student
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
            }
            return students;
        }

        public async Task AddStudentAsync(Student student)
        {
            try
            {
                Debug.WriteLine("Using connection string: " + _connectionString);
                Debug.WriteLine($"Inserting student: {student.Name}, {student.Email}, {student.Department}");

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO Students (Name, Email, Department, CreatedAt) VALUES (@Name, @Email, @Department, @CreatedAt)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", student.Name ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Email", student.Email ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Department", student.Department ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CreatedAt", student.CreatedAt);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("🔥 ERROR while adding student: " + ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }


        public async Task UpdateStudentAsync(Student student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE Students
                    SET Name = @Name, Email = @Email, Department = @Department, CreatedAt = @CreatedAt
                    WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", student.Name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", student.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Department", student.Department ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedAt", student.CreatedAt);
                    command.Parameters.AddWithValue("@Id", student.Id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteStudentAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Students WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Student>> SearchStudentsAsync(string keyword)
        {
            var students = new List<Student>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT Id, Name, Email, Department, CreatedAt 
            FROM Students 
            WHERE Name LIKE @Keyword OR Email LIKE @Keyword OR Department LIKE @Keyword";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            students.Add(new Student
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
            }
            return students;
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Name, Email, Department, CreatedAt FROM Students WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Student
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Department = reader.GetString(3),
                                CreatedAt = reader.GetDateTime(4)
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
