using MyWeb.Lib.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dsExam.Models
{
    public class TicketModel
    {
        public int Ticket_id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }


        public static List<TicketModel> GetList(string status)
        {
            using (var db = new MySqlDapperHelper())
            {
                string sql = @"
SELECT
    A.ticket_id
,   A.title
,   A.status
FROM
    t_ticket A
WHERE
    A.status = @status
";
                return db.Query<TicketModel>(sql, new { status = status });
            }
        }

        public int Update()
        {
            using (var db = new MySqlDapperHelper())
            {
                string sql = @"
UPDATE t_ticket
SET
    title = @title
WHERE
    ticket_id = @ticket_id
";

                //// 트랜잭션을 사용하면 이런식으로
                //db.BeginTransaction();
                //try
                //{
                //    int r = 0;
                //    r += db.Execute(sql, this);
                //    r += db.Execute(sql, this);
                //    r += db.Execute(sql, this);

                //    db.Commit();
                //}
                //catch(Exception ex)
                //{
                //    db.Rollback();
                //    throw;
                //}

                return db.Execute(sql, this);
            }
        }
    }

}
