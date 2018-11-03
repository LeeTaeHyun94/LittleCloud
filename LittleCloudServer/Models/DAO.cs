using LittleCloudModels.Models;
using LittleCloudServer.Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace LittleCloudServer.Models
{
    public static class DAO
    {
        public static Member Login(string id, string pw)
        {
            var db = DatabaseConnector.Instance;

            var loginCmd = new MySqlCommand("select userID, isLogin from member where userID = @id and passwd = @pw");
            loginCmd.Parameters.AddWithValue("@id", id);
            loginCmd.Parameters.AddWithValue("@pw", pw);

            var ds = db.ExecuteQuery(loginCmd);


            if (ds.Tables[0].Rows.Count == 0)
                throw new Exception("Login failed");

            var checkCmd = new MySqlCommand("select isLogin from member where userID = @id");
            checkCmd.Parameters.AddWithValue("@id", id);
            var ds2 = db.ExecuteQuery(checkCmd);
            if (ds2.Tables[0].Rows[0]["isLogin"].ToString() == "1") throw new Exception("Already Logined");

            var member = new Member()
            {
                UserID = ds.Tables[0].Rows[0]["userID"].ToString(),
                IsLogin = true
            };

            var updateCmd = new MySqlCommand("update member set isLogin = true where userID = @id");
            updateCmd.Parameters.AddWithValue("@id", id);
            db.ExecuteNonQuery(updateCmd);

            return member;
        }

        public static int[] Logout(string id)
        {
            var db = DatabaseConnector.Instance;
            var cmd = new MySqlCommand("select chatNum from chatinfo where userID = @id");
            cmd.Parameters.AddWithValue("@id", id);
            DataSet ds = db.ExecuteQuery(cmd);
            int[] result = new int[ds.Tables[0].Rows.Count];
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                result[i] = int.Parse(ds.Tables[0].Rows[i]["chatNum"].ToString());
            }

            cmd = new MySqlCommand("delete from chatinfo where userID = @id");
            cmd.Parameters.AddWithValue("@id", id);
            db.ExecuteNonQuery(cmd);
            cmd = new MySqlCommand("update member set isLogin = false where userID = @id");
            cmd.Parameters.AddWithValue("@id", id);
            db.ExecuteNonQuery(cmd);

            return result;
        }

        public static void ExitRoom(string id, int num)
        {
            var db = DatabaseConnector.Instance;
            var cmd = new MySqlCommand("delete from chatinfo where userID = @id and chatNum = @num");
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@num", num);
            db.ExecuteNonQuery(cmd);
        }

        public static ObservableCollection<Member> getFriends(string userID)
        {
            ObservableCollection<Member> result = new ObservableCollection<Member>();

            var db = DatabaseConnector.Instance;

            var cmd = new MySqlCommand("select userID, isLogin from member where userID <> @id");
            cmd.Parameters.AddWithValue("@id", userID);
            var ds = db.ExecuteQuery(cmd);

            for (int i = 0; i< ds.Tables[0].Rows.Count; i++)
            {
                result.Add(new Member()
                {
                    UserID = ds.Tables[0].Rows[i]["userID"].ToString(),
                    IsLogin = ds.Tables[0].Rows[i]["isLogin"].ToString() == "1"
                });
            }

            return result;
        }

        public static Member[] getLoginFriends()
        {
            var db = DatabaseConnector.Instance;
            var cmd = new MySqlCommand("select userID, isLogin from member where isLogin = 1");
            var ds = db.ExecuteQuery(cmd);
            Member[] result = new Member[ds.Tables[0].Rows.Count];
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                result[i] = new Member()
                {
                    UserID = ds.Tables[0].Rows[i]["userID"].ToString(),
                    IsLogin = ds.Tables[0].Rows[i]["isLogin"].ToString() == "1"
                };
            }
            return result;
        }

        public static int StartChat(ObservableCollection<Member> members)
        {
            var db = DatabaseConnector.Instance;

            MySqlCommand cmd = new MySqlCommand("select max(chatNum) from chatinfo;");
            DataSet ds = db.ExecuteQuery(cmd);
            int idx = int.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;
            for (int i = 0; i < members.Count; i++)
            {
                cmd = new MySqlCommand("insert into chatinfo(chatNum, userID) values(@idx, @id);");
                cmd.Parameters.AddWithValue("@idx", idx);
                cmd.Parameters.AddWithValue("@id", members[i].UserID);
                db.ExecuteNonQuery(cmd);
            }

            return idx;
        }

        public static void InviteChat(string id, int chatNum)
        {
            var db = DatabaseConnector.Instance;
            var cmd = new MySqlCommand("insert into chatinfo(chatNum, userID) values(@idx, @id);");
            cmd.Parameters.AddWithValue("@idx", chatNum);
            cmd.Parameters.AddWithValue("@id", id);
            db.ExecuteNonQuery(cmd);
        }

        public static Member[] getMembersInChatRoom(int chatNum)
        {
            var db = DatabaseConnector.Instance;


            MySqlCommand cmd = new MySqlCommand("select userID from chatinfo where chatNum = @chatNum");
            cmd.Parameters.AddWithValue("@chatNum", chatNum);
            DataSet ds = db.ExecuteQuery(cmd);
            Member[] result = new Member[ds.Tables[0].Rows.Count];
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                result[i] = new Member()
                {
                    UserID = ds.Tables[0].Rows[i]["userID"].ToString()
                };
            }

            return result;
        }

        public static ObservableCollection<CloudFileObject> getCloudFileList(string userID)
        {
            var db = DatabaseConnector.Instance;
            MySqlCommand cmd = new MySqlCommand("select userID, filename from cloud where userID = @userID;");
            cmd.Parameters.AddWithValue("@userID", userID);
            DataSet ds = db.ExecuteQuery(cmd);
            ObservableCollection<CloudFileObject> result = new ObservableCollection<CloudFileObject>();
            for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                result.Add(new CloudFileObject()
                {
                    Sender = ds.Tables[0].Rows[i]["userID"].ToString(),
                    FileName = ds.Tables[0].Rows[i]["filename"].ToString()
                });
            }
            return result;
        }

        public static void CloudFileSave(string userID, string path, string fileName)
        {
            var db = DatabaseConnector.Instance;
            MySqlCommand cmd = new MySqlCommand("insert into cloud(userID, filepath, filename) values(@userID, @path, @fileName);");
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@path", path);
            cmd.Parameters.AddWithValue("@fileName", fileName);
            db.ExecuteNonQuery(cmd);
        }

        public static void CloudFileDelete(string userID, string fileName)
        {
            var db = DatabaseConnector.Instance;
            MySqlCommand cmd = new MySqlCommand("delete from cloud where userID = @userID and fileName = @fileName");
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@fileName", fileName);
            db.ExecuteNonQuery(cmd);
        }

        public static Member[] getMembersNotInChatRoom(int chatNum)
        {
            var db = DatabaseConnector.Instance;
            MySqlCommand cmd = new MySqlCommand("select userID from member where isLogin = 1 and userID not in (select userID from chatinfo where chatNum = @chatNum);");
            cmd.Parameters.AddWithValue("@chatNum", chatNum);
            DataSet ds = db.ExecuteQuery(cmd);
            
            Member[] result = new Member[ds.Tables[0].Rows.Count];
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                result[i] = new Member()
                {
                    UserID = ds.Tables[0].Rows[i]["userID"].ToString(),
                    IsLogin = true
                };
            }
            return result;
        }
    }
}
