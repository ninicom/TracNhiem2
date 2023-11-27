using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TracNhiem2.Data;
using TracNhiem2.Models;

namespace TracNhiem2.Controllers
{
    [ApiController]
    [Route("api/score")]
    public class SingleResultsController : Controller
    {
        private readonly TracNhiem2Context _context;

        public SingleResultsController(TracNhiem2Context context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            return _context.SingleResult != null ?
                        View(await _context.SingleResult.ToListAsync()) :  
                        Problem("Entity set 'QuizDbContext.Result'  is null.");
        }

        [HttpPost]
        public IActionResult SaveScore([FromBody] SingleResult data)
        {
            var playerSingle = new SingleResult();
            DateTime completionTime = DateTime.Now;
            playerSingle.Name = data.Name;
            playerSingle.IdQuestionGroup = data.IdQuestionGroup;
            // Các trường khác nếu cần thiết
            playerSingle.Score = data.Score;
            playerSingle.ComCompletionTime = completionTime;
            _context.SingleResult.Update(playerSingle);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Trả về 200 OK nếu lưu thành công
        }
    }
}
