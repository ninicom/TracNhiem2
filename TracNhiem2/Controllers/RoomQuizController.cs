using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TracNhiem2.Data;
using TracNhiem2.Models;

namespace TracNhiem2.Controllers
{
    public class RoomQuizController : Controller
    {
        private readonly TracNhiem2Context _context;

        public RoomQuizController(TracNhiem2Context context)
        {
            _context = context;
        }

        // GET: RoomQuizs
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult JoinRoom()
        {
            return View();
        }
        public IActionResult CreateRoom()
        {
            return View();
        }
        public IActionResult GameStart()
        {
            return View();
        }


        [HttpGet]
        public JsonResult IsAdminRoom(int roomId, int playerId)
        {
            return Json(data: new { code = 200, isAdmin = _context.RoomQuiz.Any(r => r.RoomId == roomId && r.AdminId == playerId) });
        }

        [HttpGet]
        public JsonResult GetRoomById(int roomId)
        {
            try
            {
                var room = _context.RoomQuiz.FirstOrDefault(q => q.RoomId == roomId);
                if (room != null)
                {
                    return Json(data: new { code = 200, room, msg = "Lấy dữ liệu thành công!" });
                }
                return Json(data: new { code = 400, room, msg = roomId + " không tồn tại!" });
            }
            catch
            {
                return Json(data: new { code = 500, msg = "Lấy dữ liệu thất bại!" });
            }
        }



        [HttpPut]
        public JsonResult CreateRoom(int roomId, int playerId, int groupId)
        {
            try
            {
                var room = new RoomQuiz();
                room.RoomId = roomId;
                room.AdminId = playerId;
                room.GroupId = groupId;
                _context.RoomQuiz.Add(room);
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
            catch (Exception ex)
            {
                return Json(data: new { code = 500, msg = "Thêm mới không thành công: " + ex.Message });
            }
        }

        public bool RemoveAllPersonByRoomId(int roomId)
        {
            try
            {
                var roomQuizViews = _context.RoomQuizView.Where(r => r.RoomId == roomId).ToList();
                _context.RoomQuizView.RemoveRange(roomQuizViews);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        [HttpDelete]
        public JsonResult DeleteRoom(int roomId)
        {
            try
            {
                if (RemoveAllPersonByRoomId(roomId))
                {
                    var rooms = _context.RoomQuiz.Where(r => r.RoomId == roomId);
                    _context.RoomQuiz.RemoveRange(rooms);
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
                return Json(data: new { code = 500, msg = "Xóa không thành công!" });
            }
            catch
            {
                return Json(data: new { code = 500, msg = "Xóa không thành công!" });
            }
        }
    }
}
