using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
       public List<Student> GetAllStdents()
        {
            //string[]  studendName = new string[] { "Abhay", "Pandey" }; 
            //return Ok(studendName);
            var students = new List<Student>
            {
                new Student
                {
                    Id=1,
                    Name="Abhay",
                    LastName="Pandey"
                }
            };
            return students;
        }
        
    }
}
