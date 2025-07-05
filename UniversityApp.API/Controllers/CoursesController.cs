using Microsoft.AspNetCore.Mvc;
using UniversityApp.BLL.Interfaces;
using UniversityApp.BLL.Services;
using UniversityApp.Entities;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
    {
        return await _courseService.GetAllCoursesAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourse(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        return course == null ? NotFound() : Ok(course);
    }

    [HttpPost]
    public async Task<ActionResult> AddCourse(Course course)
    {
        await _courseService.AddCourseAsync(course);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCourse(int id, Course course)
    {
        if (id != course.Id)
            return BadRequest();

        await _courseService.UpdateCourseAsync(course);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCourse(int id)
    {
        await _courseService.DeleteCourseAsync(id);
        return NoContent();
    }
}
