using dsExam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
            var dt = new DataTable();
            using (var conn = new MySqlConnection("Server=127.0.0.1;Port=3306;Database=myweb;Uid=root;Pwd=123;"))
            {
                conn.Open();
                using (var cmd = new MySqlCommand())
                {
                    string status = "In Progress";
                    cmd.Connection = conn;
                    cmd.CommandText = @"
SELECT
	A.ticket_id
,	A.title
,	A.status
FROM
	t_ticket A
WHERE 
	A.status = @status
";
                    cmd.Parameters.AddWithValue("status", status);
                    var reader = cmd.ExecuteReader();
                    dt.Load(reader);

                    //cmd.ExecuteNonQuery();
                }
            }
            var list = new List<TicketModel>();
            foreach(DataRow row in dt.Rows)
            {
                var ticket = new TicketModel();
                ticket.Ticket_id = Convert.ToInt32(row["ticket_id"]);
                ticket.Title = row["title"] as string;
                ticket.Status = row["status"] as string;
                list.Add(ticket);
            }
            ViewData["ticketList"] = list;
            ViewData["dt"] = dt;
            return View();
        }

        public IActionResult TicketChange(int ticket_id, string title)
        {
            using (var conn = new MySqlConnection("Server=127.0.0.1;Port=3306;Database=myweb;Uid=root;Pwd=123;"))
            {
                conn.Open();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
UPDATE t_ticket
SET
	title = @title
WHERE 
	ticket_id = @ticket_id
";
                    cmd.Parameters.AddWithValue("ticket_id", ticket_id);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.ExecuteNonQuery();
                    //cmd.ExecuteNonQuery();
                }
            }
            //return Json(new { msg = "OK" });
            return Redirect("/home/ticketList");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}
