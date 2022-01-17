using Microsoft.AspNetCore.Mvc;

namespace TestController
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private TestService ps;
        private TestDbContext db;
        
        public TestController(TestService ps, TestDbContext db)
        {
            this.ps = ps;
            this.db = db;
        }

        [HttpGet("Version")]
        public object Version()
        {
            return $"Plugin Controller v 1.0 {ps.Test()}";
        }

        [HttpGet("Add/{text}")]
        public IActionResult Add(string text)
        {
            System.Diagnostics.Debug.WriteLine("SIEMAA: "+User?.Identity?.IsAuthenticated );
            System.Diagnostics.Debug.WriteLine("eLOO: "+ HttpContext.User.Identity?.IsAuthenticated);
            db.Add(new DisplayedText { Text = text });
            db.SaveChanges();
            return Ok();
        }

        [HttpGet("All")]
        public DisplayedText[] All()
        {
            return db.DisplayedTexts.ToArray();
        }

    }
}