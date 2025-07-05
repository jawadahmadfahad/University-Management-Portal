﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http.Json;
using UniversityApp.Entities;

namespace UniversityApp.BLL.Services.Api
{
    public class CourseApiService
    {
        private readonly HttpClient _http;

        public CourseApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Course>> GetCoursesAsync()
            => await _http.GetFromJsonAsync<List<Course>>("api/courses");

        public async Task<Course?> GetCourseByIdAsync(int id)
            => await _http.GetFromJsonAsync<Course>($"api/courses/{id}");

        public async Task AddCourseAsync(Course course)
            => await _http.PostAsJsonAsync("api/courses", course);

        public async Task UpdateCourseAsync(Course course)
            => await _http.PutAsJsonAsync($"api/courses/{course.Id}", course);

        public async Task DeleteCourseAsync(int id)
            => await _http.DeleteAsync($"api/courses/{id}");
    }
}
