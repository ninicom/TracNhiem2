using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TracNhiem2.Data;
using TracNhiem2.Models;

namespace TracNhiem2.Controllers
{
    
    public class QuestionGroupController : Controller
    {
        private readonly TracNhiem2Context _context;

        public QuestionGroupController(TracNhiem2Context context)
        {
            _context = context;
        }

        // GET: QuestionGroups
        [HttpGet]
        public JsonResult GetQuestionGroups()
        {
            try
            {
                // Lấy tất cả các nhóm câu hỏi từ cơ sở dữ liệu
                List<QuestionGroup> questionGroups = _context.QuestionGroup.ToList();

                // Trả về dữ liệu dưới dạng JSON
                return Json(data: new { code = 200, questionGroups, msg = "Lấy dữ liệu thành công!" });
            }
            catch
            {
                return Json(data: new { code = 500, msg = "Lấy dữ liệu thất bại!" });
            }
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return _context.QuestionGroup != null ?
                        View(await _context.QuestionGroup.ToListAsync()) :
                        Problem("Entity set 'TracNhiem2Context.QuestionGroup'  is null.");
        }

        public bool QuestionGroupExists(int Id)
        {
            bool result = _context.QuestionGroup.Any(q => q.Id == Id);
            return result;
        }
        [HttpGet]
        public JsonResult GetQuestionGroupById(int Id)
        {
            try
            {
                var questionGroup = _context.QuestionGroup.FirstOrDefault(q => q.Id == Id);
                return Json(data: new { code = 200, questionGroup, msg = "Lấy dữ liệu thành công!" });
            }
            catch
            {
                return Json(data: new { code = 500, msg = "Lấy dữ liệu thất bại!" });
            }
        }
        // tạo mới câu hỏi
        [HttpPost]
        public JsonResult CreateQuestionGroup(string Name, string Description)
        {
            try
            {
                var questionGroup = new QuestionGroup();
                questionGroup.Name = Name;
                questionGroup.Description = Description;
                _context.QuestionGroup.Add(questionGroup);
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
        public JsonResult UpdateQuestionGroup(int Id, string Name, string Description)
        {
            try
            {
                var questionGroup = new QuestionGroup();
                questionGroup.Id = Id;
                questionGroup.Name = Name;
                questionGroup.Description = Description;
                if (!QuestionGroupExists(questionGroup.Id))
                {
                    return Json(data: new { code = 400, msg = "Nhóm câu hỏi không tồn tại!" });
                }
                _context.QuestionGroup.Update(questionGroup);
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
        public JsonResult DeleteQuestionGroup(int Id)
        {
            try
            {
                if (!QuestionGroupExists(Id))
                {
                    return Json(data: new { code = 400, msg = "Nhóm câu hỏi không tồn tại!" });
                }
                var questionGroup = _context.QuestionGroup.Find(Id);
                _context.QuestionGroup.Remove(questionGroup);
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
