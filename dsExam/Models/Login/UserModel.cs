using MySqlConnector;
using MyWeb.Lib.DataBase;
using System;

namespace dsExam.Models.Login
{
    public class UserModel
    {
        public uint User_seq { get; set; }
        public string User_name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string ConvertPassword()
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(this.Password.Length.ToString());
            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.Password));

            return System.Convert.ToBase64String(hash);
        }

        internal int Register()
        {
            // 중복 user_name 체크
            // 중복 email 체크

            string sql = @"
INSERT INTO t_user (
    user_name
,   email
,   password
)
SELECT
    @user_name
,   @email
,   @password
";
            using (var db = new MySqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        internal UserModel GetLoginUser()
        {
            string sql = @"
SELECT 
    user_seq
,   user_name
,   email
,   password
FROM
    t_user
WHERE
    user_name = @user_name
";
            UserModel user;
            using (var db = new MySqlDapperHelper())
            {
                user = db.QuerySingle<UserModel>(sql, this);
            }

            if(user == null)
            {
                throw new Exception("사용자가 존재하지 않습니다");
            }

            if(user.Password != this.Password)
            {
                throw new Exception("비밀번호가 틀립니다");
                // 비밀번호 틀린 횟수 -- update
            }
            return user;
        }
    }
}