using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TracNhiem2.Data;
using TracNhiem2.Models;

namespace TracNhiem2.Controllers
{
    
    public class QuestionController : Controller
    {
        private readonly TracNhiem2Context _context;

        public QuestionController(TracNhiem2Context context)
        {
            _context = context;
        }
        
        public bool QuestionExists(int Id)
        {
            bool questionExists = _context.Question.Any(q => q.Id == Id);
            return questionExists;
        }
        // GET: Questions
        [HttpGet]
        public JsonResult GetQuestion()
        {
            try
            {
                // Lấy tất cả các nhóm câu hỏi từ cơ sở dữ liệu
                List<Question> question = _context.Question.ToList();

                // Trả về dữ liệu dưới dạng JSON
                return Json(data: new { code = 200, question, msg = "Lấy dữ liệu thành công!" });
            }
            catch
            {
                return Json(data: new { code = 500, msg = "Lấy dữ liệu thất bại!" });
            }
        }
        [HttpGet]
        public JsonResult GetQuestionByGroup(int groupId)
        {
            try
            {
                // Lấy tất cả các nhóm câu hỏi từ cơ sở dữ liệu
                List<Question> questions = _context.Question.Where(q => q.GroupId == groupId).ToList();

                // Trả về dữ liệu dưới dạng JSON
                return Json(data: new { code = 200, question = questions, msg = "Lấy dữ liệu thành công!" });
            }
            catch
            {
                return Json(data: new { code = 500, msg = "Lấy dữ liệu thất bại!" });
            }
        }

        [HttpGet]
        public JsonResult GetQuestionById(int Id)
        {
            try
            {
                var question = _context.Question.FirstOrDefault(q => q.Id == Id);
                return Json(data: new { code = 200, question, msg = "Lấy dữ liệu thành công!" });
            }
            catch
            {
                return Json(data: new { code = 500, msg = "Lấy dữ liệu thất bại!" });
            }
        }

        [HttpGet]
        public JsonResult GetQuestionRandomByGroupRandom()
        {
            try
            {

                // Lấy tất cả các nhóm câu hỏi từ cơ sở dữ liệu
                List<Question> questionrd = _context.Question
               .OrderBy(q => Guid.NewGuid())
               .ToList();

                // Trả về dữ liệu dưới dạng JSON
                return Json(data: new { code = 200, question = questionrd, msg = "Lấy dữ liệu thành công!" });
            }
            catch
            {
                return Json(data: new { code = 500, msg = "Lấy dữ liệu thất bại!" });
            }
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public IActionResult CRUD_Question()
        {
            return View();
        }
        // tạo mới câu hỏi
        [HttpPost]
        public JsonResult CreateQuestion(string Content, string Answer1, string Answer2, string Answer3, string Answer4, string CorrectAnswer, int GroupId)
        {
            try
            {
                var question = new Question();
                question.Content = Content;
                question.Answer1 = Answer1;
                question.Answer2 = Answer2;
                question.Answer3 = Answer3;
                question.Answer4 = Answer4;
                question.CorrectAnswer = CorrectAnswer;
                question.GroupId = GroupId;
                _context.Question.Add(question);
                int numberOfRecordsChanged = _context.SaveChanges();

                if (numberOfRecordsChanged > 0)
                {
                    return Json(data: new { code = 200, msg = "Thêm mới thành công!" });
                }
                else
                {
                    return Json(data: new { code = 400, msg = "Thêm mới không thành công!" });
                }

            }
            catch
            {
                return Json(data: new { code = 500, msg = "Thêm mới không thành công!" });
            }
        }

        // sửa câu hỏi
        [HttpPut]
        public JsonResult UpdateQuestion(int Id, string Content, string Answer1, string Answer2, string Answer3, string Answer4, string CorrectAnswer, int GroupId)
        {
            try
            {
                var question = new Question();
                question.Id = Id;
                question.Content = Content;
                question.Answer1 = Answer1;
                question.Answer2 = Answer2;
                question.Answer3 = Answer3;
                question.Answer4 = Answer4;
                question.CorrectAnswer = CorrectAnswer;
                question.GroupId = GroupId;
                if (!QuestionExists(question.Id))
                {
                    return Json(data: new { code = 400, msg = "Câu hỏi không tồn tại!" });
                }
                _context.Question.Update(question);
                int numberOfRecordsChanged = _context.SaveChanges();

                if (numberOfRecordsChanged > 0)
                {
                    return Json(data: new { code = 200, msg = "Sửa thành công!" });
                }
                else
                {
                    return Json(data: new { code = 400, msg = "Sửa không thành công!" });
                }
            }
            catch
            {
                return Json(data: new { code = 500, msg = "Sửa thất bại!" });
            }
        }

        [HttpDelete]
        public JsonResult DeleteQuestion(int Id)
        {
            try
            {
                if (!QuestionExists(Id))
                {
                    return Json(data: new { code = 400, msg = "Câu hỏi không tồn tại!" });
                }
                var question = _context.Question.Find(Id);
                _context.Question.Remove(question);
                int numberOfRecordsChanged = _context.SaveChanges();

                if (numberOfRecordsChanged > 0)
                {
                    return Json(data: new { code = 200, msg = "Xóa thành công!" });
                }
                else
                {
                    return Json(data: new { code = 400, msg = "Xóa không thành công!" });
                }
            }
            catch
            {
                return Json(data: new { code = 500, msg = "Xóa thất bại!" });
            }
        }
    }
}
