using MudBlazor.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MudBlazor.CategoryTypes;


namespace V_Lib.Data
{
    public class Vservice
    {
        private String connectionString = "Data Source=LASITHA\\SQLEXPRESS;Initial Catalog=video_lib;Integrated Security=True";
        
        
        public Task<List<Video>> GetAllVideos()
        {
            List<Video> list = new List<Video>();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string procedure = "GetAllVideos";
               

                // SqlCommand qr = new SqlCommand("select * from Video", conn);
                SqlCommand cmd = new SqlCommand(procedure, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader vrd = cmd.ExecuteReader();
                
                while (vrd.Read())
                {
                    Video vid = new Video();
                    vid.VideoId = vrd.GetInt32(0);
                    vid.Vname = vrd.GetString(1);
                    vid.Vdes = vrd.GetString(2);
                    try
                    {
                        vid.borrowed_id = vrd.GetInt32(3);
                    }
                    catch (Exception ex)
                    {
                        vid.borrowed_id = 0;
                    }
                    list.Add(vid);
                }
                conn.Close();
            }
            return Task.FromResult(list);
        }

        public Task<List<Member>> GetAllMembers()
        {
            List<Member> list = new List<Member>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string procedure = "GetAllUsers";
                conn.Open();
                SqlCommand cmd = new SqlCommand(procedure, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //SqlCommand qr = new SqlCommand("select * from member", conn);

                SqlDataReader mrd = cmd.ExecuteReader();


                while (mrd.Read())
                {
                    Member mem = new Member();
                    mem.Id = mrd.GetInt32(0);
                    mem.Fname = mrd.GetString(1);
                    mem.Lname = mrd.GetString(2);
                    mem.Tel = mrd.GetString(3);
                    list.Add(mem);
                }
                conn.Close();
            }
            return Task.FromResult(list);
        }


        public Task<List<ReserveVideo>> GetAllResVideos()
        {
            List<ReserveVideo> list = new List<ReserveVideo>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string procedure = "GetAllResVideo";
                conn.Open();
                //SqlCommand qr = new SqlCommand("select * from reserved_video", conn);
                SqlCommand cmd = new SqlCommand(procedure, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader vrd = cmd.ExecuteReader();

                while (vrd.Read())
                {
                    ReserveVideo vid = new ReserveVideo();
                    vid.Rid = vrd.GetInt32(0);
                    vid.ResVid = vrd.GetInt32(1);
                    vid.ResMid = vrd.GetInt32(2);
                    vid.ResFromDate=vrd.GetDateTime(3);
                    vid.ResToDate=vrd.GetDateTime(4);
                    
                    list.Add(vid);
                }
                conn.Close();
            }
            return Task.FromResult(list);
        }

        public Task<List<Video>> GetBorrowedVideos()
        {
            List<Video> list = new List<Video>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string procedure = "GetBorrowedVideo";
                conn.Open();
                //SqlCommand qr = new SqlCommand("select * from Video where borrowed_id IS NOT NULL", conn);
                SqlCommand cmd = new SqlCommand(procedure, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader vrd = cmd.ExecuteReader();
                while (vrd.Read())
                {
                    Video vid = new Video();
                    vid.VideoId = vrd.GetInt32(0);
                    vid.Vname = vrd.GetString(1);
                    vid.Vdes = vrd.GetString(2);
                    
                    list.Add(vid);
                }
            }
            return Task.FromResult(list);
        }




        public bool InsertVideo(Video video)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string procedure = "InsertVideo";
                conn.Open();
                //SqlCommand qr = new SqlCommand("insert into Video (vname,vdes) values ('" + video.Vname + "','" + video.Vdes + "')", conn);
                SqlCommand cmd = new SqlCommand(procedure, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@vname",
                    Value = video.Vname,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@vdes",
                    Value = video.Vdes,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input
                });

                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

                
            }
        }

        public bool InsertUser(Member member)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string procedure = "InsertUser";
                conn.Open();
                SqlCommand cmd = new SqlCommand(procedure, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //SqlCommand qr = new SqlCommand("insert into member (first_name,last_name,tel) values ('" + member.Fname + "','" + member.Lname + "','" + member.Tel + "')", conn);
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@fname",
                    Value = member.Fname,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@lname",
                    Value = member.Lname,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@tel",
                    Value = member.Tel,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input
                });
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool isBorrowed(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string procedure = "IsBorrowed";
                //SqlCommand qr = new SqlCommand("select borrowed_id from Video where video_id = " + id, conn);
                SqlCommand cmd = new SqlCommand(procedure, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //SqlCommand qr = new SqlCommand("insert into member (first_name,last_name,tel) values ('" + member.Fname + "','" + member.Lname + "','" + member.Tel + "')", conn);
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int borrowed_id;
                    try
                    {
                        borrowed_id = dr.GetInt32(0);
                    }
                    catch (Exception e)
                    {
                        borrowed_id = 0;
                    }
                    if (borrowed_id > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool BorrowVideo(int VId, int MId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string procedure = "BorrowVideo";
                SqlCommand cmd = new SqlCommand(procedure, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@vid",
                    Value = VId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@mid",
                    Value = MId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });
                //SqlCommand qr = new SqlCommand("update Video set borrowed_id = " + MId + " where video_id = " + VId, conn);
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool isReserved(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string procedure = "isReserved";
                SqlCommand cmd = new SqlCommand(procedure, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });
                //SqlCommand qr = new SqlCommand("select res_mid from reserved_video where res_vid = " + id, conn);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int borrowed_id;
                    try
                    {
                        borrowed_id = dr.GetInt32(0);
                    }
                    catch (Exception e)
                    {
                        borrowed_id = 0;
                    }
                    if (borrowed_id > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
    public bool InsertReserve(ReserveVideo RV)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string procedure = "InsertReserve";
            SqlCommand cmd = new SqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
               
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@vid",
                    Value = RV.ResVid,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@mid",
                    Value = RV.ResMid,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@fromD",
                    Value = RV.ResFromDate,
                    SqlDbType = SqlDbType.Date,
                    Direction = ParameterDirection.Input
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@toD",
                    Value = RV.ResToDate,
                    SqlDbType = SqlDbType.Date,
                    Direction = ParameterDirection.Input
                });
                //SqlCommand qr = new SqlCommand("insert into reserved_video (res_vid,res_mid,res_fromDate,res_toDate) values ('" + RV.ResVid + "','" + RV.ResMid + "','" + RV.ResFromDate.ToIsoDateString() + "','" + RV.ResToDate.ToIsoDateString() + "')", conn);
                int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

        public bool DeleteVideo(int vid)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string procedure = "DeleteVideo";
                    SqlCommand cmd = new SqlCommand(procedure, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@vid",
                        Value =vid,
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input
                    });
                    //SqlCommand qr = new SqlCommand("delete from Video where video_id = " + vid , conn);

                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                   
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool CheckedIn(int vid)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string procedure = "CheckedIn";
                    SqlCommand cmd = new SqlCommand(procedure, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@vid",
                        Value = vid,
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input
                    });
                    //SqlCommand qr = new SqlCommand("update Video set borrowed_id =  NULL  where video_id = " + vid, conn);

                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool DeleteMember(int mid)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string procedure = "DeleteMember";
                    SqlCommand cmd = new SqlCommand(procedure, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@mid",
                        Value = mid,
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input
                    });
                    //SqlCommand qr = new SqlCommand("delete from member where member_id =" + mid, conn);

                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool DeleteRV(int rvid)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string procedure = "DeleteRV";
                    SqlCommand cmd = new SqlCommand(procedure, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@rvid",
                        Value = rvid,
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input
                    });
                    //SqlCommand qr = new SqlCommand("delete from reserved_video where rid =" + rvid, conn);

                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }





    }
}
