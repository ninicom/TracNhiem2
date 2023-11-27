using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using TracNhiem2.Data;
using TracNhiem2.Models;

namespace TracNhiem2.Controllers
{
    public class PlayerController : Controller
    {
        private readonly TracNhiem2Context _context;
        public PlayerController(TracNhiem2Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult PlayerExists(int Id)
        {
            try
            {
                bool result = _context.Player.Any(q => q.Id == Id);
                if (result)
                {
                    var player = _context.Player.FirstOrDefault(q => q.Id == Id);
                    return Json(data: new { code = 200, PlayerExists = result, player = player, msg = "lấy dữ liệu thành công" });
                }
                return Json(data: new { code = 200, PlayerExists = result, msg = "lấy dữ liệu thành công" });

            }
            catch
            {
                return Json(data: new { code = 500, PlayerExists = false, msg = "lấy dữ liệu thất bại" });
            }
        }
        [HttpPut]
        public JsonResult CreatePlayer(string Name)
        {
            try
            {
                var player = new Player();
                player.Name = Name;
                _context.Player.Add(player);
                int numberOfRecordsChanged = _context.SaveChanges();

                if (numberOfRecordsChanged > 0)
                {
                    return Json(data: new { code = 200,Id = player.Id, msg = "Thêm mới thành công!" });
                }
                else
                {
                    return Json(data: new { code = 400, msg = "Thêm mới không thành công!" });
                }
            }
            catch
            {
                return Json(data: new { code = 500, msg = "thêm người chơi thất bại" });
            }
        }
        [HttpPost]
        public JsonResult UpdatePlayer(int id, string name)
        {
            try
            {
                var player = new Player();
                player.Id = id;
                player.Name = name;
                _context.Player.Update(player);
                int numberOfRecordsChanged = _context.SaveChanges();

                if (numberOfRecordsChanged > 0)
                {
                    return Json(data: new { code = 200, Id = player.Id, msg = "Đổi tên thành công!" });
                }
                else
                {
                    return Json(data: new { code = 400, msg = "Đổi tên không thành công!" });
                }
            }
            catch(Exception ex)
            {
                return Json(data: new { code = 500, msg = "Sửa thất bại" + ex});
            }
            
        }
        
    }
}
