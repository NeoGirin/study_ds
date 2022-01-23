using dsExam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace dsExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TicketList()
        {
            //var dt = new DataTable();
            //var list = new List<TicketModel>();
            //foreach(DataRow row in dt.Rows)
            //{
            //    var ticket = new TicketModel();
            //    ticket.Ticket_id = Convert.ToInt32(row["ticket_id"]);
            //    ticket.Title = row["title"] as string;
            //    ticket.Status = row["status"] as string;
            //    list.Add(ticket);
            //}
            //ViewData["ticketList"] = list;
            //ViewData["dt"] = dt;

            string status = "In Progress";
            return View(TicketModel.GetList(status));
        }

        public IActionResult TicketChange(int ticket_id, string title)
        {
            var model = new TicketModel();
            model.Ticket_id = ticket_id;
            model.Title = title;
            model.Update();
            
            return Redirect("/home/ticketList");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult BoardList(string search)
        {
            return View(BoardModel.GetList(search));
        }

        public IActionResult BoardView(uint idx)
        {
            return View(BoardModel.Get(idx));
        }

        [Authorize]
        public IActionResult BoardWrite()
        {
            return View();
        }

        [Authorize]
        public IActionResult BoardWrite_Input(string title, string contents)
        {
            var model = new BoardModel();

            model.Title = title;
            model.Contents = contents;
            model.Reg_User = Convert.ToUInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.Reg_Username = User.Identity.Name;
            model.Insert();
            return Redirect("/home/boardlist");
        }

        [Authorize]
        public IActionResult BoardEdit(uint idx, string type)
        {
            var model = BoardModel.Get(idx);
            var userSeq = Convert.ToUInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if(model.Reg_User != userSeq)
            {
                throw new Exception("수정할 수 없습니다.");
            }
            if(type == "U")
            {
                return View(model);
            } 
            else if(type == "D")
            {
                model.Delete();
                return Redirect("/home/boardList");
            }
            throw new Exception("잘못된 요청입니다.");
        }

        [Authorize]
        public IActionResult BoardEdit_Input(uint idx, string title, string contents)
        {
            var model = BoardModel.Get(idx);
            var userSeq = Convert.ToUInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (model.Reg_User != userSeq)
            {
                throw new Exception("수정할 수 없습니다.");
            }
            model.Title = title;
            model.Contents = contents;
            model.Update();
            return Redirect("/home/boardlist");
        }
    }
}
