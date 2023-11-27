using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.EntityFrameworkCore;
using TracNhiem2.Data;
using TracNhiem2.Models;

namespace TracNhiem2.Controllers
{
    public class RoomQuizViewController : Controller
    {
        private readonly TracNhiem2Context _context;

        public RoomQuizViewController(TracNhiem2Context context)
        {
            _context = context;
        }

        [HttpGet]
        // GET: RoomQuizViews
        public JsonResult getPlayerFromRoom(int roomId)
        {
            try
            {
                var players = _context.Player.Join(
                                _context.RoomQuizView,
                                player => player.Id,
                                roomQuizView => roomQuizView.PlayerId,
                                (player, roomQuizView) => new {Player = player, RoomQuizView = roomQuizView })
                                .Where(rp => rp.RoomQuizView.RoomId == roomId)
                                .Select(rp => new { rp.Player.Id, rp.Player.Name, rp.RoomQuizView.score})
                                .OrderByDescending(r => r.score)
                                .ToList();
                return Json(data: new { code = 200, players = players, msg = "Lấy dữ liệu thành công!" });
            }
            catch (Exception ex)
            {
                return Json(data: new {StatusCode = 500, msg = ex.Message});
            }
        }
        [HttpPut]
        public JsonResult addPlayerToRoom(int roomId, int playerId)
        {
            try
            {
                var player_room = _context.RoomQuizView.FirstOrDefault(p => p.RoomId == roomId && p.PlayerId == playerId);
                if (player_room == null)
                {
                    player_room = new RoomQuizView();
                    player_room.RoomId = roomId;
                    player_room.PlayerId = playerId;
                    _context.RoomQuizView.Add(player_room);
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
                else
                {
                    player_room.score = 0;
                    _context.RoomQuizView.Update(player_room);
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
            }
            catch (Exception ex)
            {
                return Json(data: new { code = 500, msg = "Thêm mới không thành công: " + ex.Message });
            }
        }
        [HttpPost]
        public JsonResult updateScore(int roomId, int playerId, int score)
        {
            try
            {
                var player_room = _context.RoomQuizView.FirstOrDefault(p => p.RoomId == roomId && p.PlayerId == playerId);
                if (player_room == null)
                {
                    player_room = new RoomQuizView();
                    player_room.RoomId = roomId;
                    player_room.PlayerId = playerId;
                    _context.RoomQuizView.Add(player_room);
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
                else
                {
                    player_room.score = score;
                    _context.RoomQuizView.Update(player_room);
                    int numberOfRecordsChanged = _context.SaveChanges();

                    if (numberOfRecordsChanged > 0)
                    {
                        return Json(data: new { code = 200, msg = "Sửa điểm thành công!" });
                    }
                    else
                    {
                        return Json(data: new { code = 400, msg = "Sửa điểm không thành công!" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(data: new { code = 500, msg = "update không thành công: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ResetScore(int roomId)
        {
            try
            {
                var player_room = _context.RoomQuizView.Where(p => p.RoomId == roomId).ToList();
                if (player_room != null)
                {
                    foreach (var player in player_room)
                    {
                        // Cập nhật điểm số
                        player.score = 0;
                        _context.Update(player);
                    }

                    // Lưu thay đổi
                    _context.SaveChanges();

                    return Json(data: new { code = 400, msg = "Cập nhật thành công!" });
                }
                return Json(data: new { code = 400, msg = "Cập nhật không thành công!" });
            }
            catch (Exception ex)
            {
                return Json(data: new { code = 500, msg = "update không thành công: " + ex.Message });
            }
        }

        [HttpDelete]
        public JsonResult DeletePlayer(int playerId)
        {
            try
            {
                var player = _context.RoomQuizView.Where(r => r.PlayerId == playerId).ToList();
                ;
                if (player == null)
                {
                    return Json(data: new { code = 200, msg = "không tồn tại người chơi" });
                }

                _context.RoomQuizView.RemoveRange(player);
                
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
            catch(Exception ex) 
            {
                return Json(data: new { code = 500, msg = "xóa thất bại" + ex.Message });
            }
        }

    }

}
