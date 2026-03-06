using Checkpoint_14.Data;
using Checkpoint_14.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Checkpoint_14.Data.DBContext;

namespace Checkpoint_14.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollegeController : ControllerBase
    {
        private readonly DBContext.Checkpoint_14DBContext _context;
        public CollegeController(Checkpoint_14DBContext context)
        {
            _context = context;
        }
        [HttpGet("students/{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound($"Студент с ID {id} не найден.");
            }
            return Ok(student);
        }

        [HttpGet("teachers/{id}")]
        public async Task<IActionResult> GetTeacher(int id)
        {
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(s => s.TeacherId == id);
            if (teacher == null)
            {
                return NotFound($"Преподаватель с ID {id} не найден.");
            }
            return Ok(teacher);
        }
        [HttpGet("courses/{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(s => s.StudentCourses)
                .FirstOrDefaultAsync(s => s.CourseId == id);
            if (course == null)
            {
                return NotFound($"Курс с ID {id} не найден.");
            }
            return Ok(course);
        }

        [HttpPost("student")]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }

        [HttpPost("teacher")]
        public async Task<IActionResult> CreateTeacher([FromBody] Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.TeacherId }, teacher);
        }
        [HttpPost("course")]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, course);
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _context.Students
                .Include(u => u.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .ToListAsync();

            return Ok(students);
        }

        [HttpGet("teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var teachers = await _context.Teachers
                .Include(u => u.Courses)
                .ToListAsync();

            return Ok(teachers);
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _context.Courses
                .Include(u => u.StudentCourses)
                .ThenInclude(sc => sc.Student)
                .ToListAsync();

            return Ok(courses);
        }
        [HttpPut("students/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            var existingStudent = await _context.Students
                .Include(u => u.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(u => u.StudentId == id);
            if (existingStudent == null) return NotFound();

            existingStudent.Name = student.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("teachers/{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] Teacher teacher)
        {
            var existingTeacher = await _context.Teachers
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.TeacherId == id);
            if (existingTeacher == null) return NotFound();

            existingTeacher.Name = teacher.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("courses/{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course course)
        {
            var existingCourse = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseId == id);
            if (existingCourse == null) return NotFound();

            existingCourse.Title = course.Title;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("students/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound($"Студент с ID {id} не найден.");

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return Ok("Студент успешно удален");
        }

        [HttpDelete("teachers/{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound($"Преподаватель с ID {id} не найден.");

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return Ok("Преподаватель успешно удален");
        }

        [HttpDelete("courses/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound($"Курс с ID {id} не найден.");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return Ok("Курс успешно удален");
        }

        [HttpPost("enrollStudent")]
        public async Task<IActionResult> EnrollStudent(int studentId, int courseId)
        {
            if (!await _context.Students.AnyAsync(s => s.StudentId == studentId))
                return NotFound($"Студент с ID {studentId} не найден.");

            if (!await _context.Courses.AnyAsync(c => c.CourseId == courseId))
                return NotFound($"Курс с ID {courseId} не найден.");

            var exists = await _context.StudentCourses
                .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (exists)
                return BadRequest($"Студент с ID {studentId} уже записан на курс с ID {courseId}.");
            var studentCourse = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId
            };

            _context.StudentCourses.Add(studentCourse);
            await _context.SaveChangesAsync();

            return Ok($"Студент с ID {studentId} успешно записан на курс с ID {courseId}");
        }

        [HttpPost("assignTeacher")]
        public async Task<IActionResult> assignTeacher(int teacherId, int courseId)
        {
            if (!await _context.Teachers.AnyAsync(s => s.TeacherId == teacherId))
                return NotFound($"Преподаватель с ID {teacherId} не найден.");
            if (!await _context.Courses.AnyAsync(c => c.CourseId == courseId))
                return NotFound($"Курс с ID {courseId} не найден.");

            var course = await _context.Courses.FindAsync(courseId);
            if (course.TeacherId == teacherId) return BadRequest($"Преподаватель с ID {teacherId} уже назначен на курс с ID {courseId}.");

            course.TeacherId = teacherId;
            await _context.SaveChangesAsync();
            return Ok($"Преподаватель с ID {teacherId} успешно назначен на курс с ID {courseId}");
        }
    }
}

