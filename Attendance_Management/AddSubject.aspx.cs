using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Attendance_Management
{
    public partial class AddSubject : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["tid"] == null)
            {
                Response.Redirect("Login_teacher.aspx");
            }
        }

        protected void btnAddSubject_Click(object sender, EventArgs e)
        {
            //je subject select karyo hase te database ma update krse('teaching' table ma)
            string subject = ddlSubject.SelectedValue;
            Console.WriteLine(subject);

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString;
            string query1 = "SELECT subject_ID  FROM Subject WHERE subject_name = (@sub_name)";
            try
            {
                using (con)
                {
                    using (SqlCommand cmd = new SqlCommand(query1))
                    {
                        //Response.Write(subject);
                        cmd.Parameters.AddWithValue("@sub_name", subject);
                        cmd.Connection = con;
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        Int32 subject_id = 20;
                        while (reader.Read())
                        {
                            subject_id = reader.GetInt32(0);
                        }

                        con.Close();
                        Int32 tid = Int32.Parse(Session["tid"].ToString());

                        //corner case : subject is already exist
                        string query3 = "SELECT * From Teaching Where subject_ID = (@sub_id) AND teacher_ID = (@teacher_id) ";
                        using(SqlCommand cmd3 = new SqlCommand(query3))
                        {
                            cmd3.Parameters.AddWithValue("@sub_id", subject_id);
                            cmd3.Parameters.AddWithValue("@teacher_id", tid);
                            cmd3.Connection = con;
                            con.Open();
                            SqlDataReader reader1 = cmd3.ExecuteReader();
                            if (reader1.HasRows) // If the query returns any rows, it means the subject is already added.
                            {
                                // Display an alert box with the message
                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Subject is already added.');", true);
                                reader1.Close();
                                con.Close();

                                // Terminate the function to prevent further execution
                                return;
                                //Response.Redirect("Home_Teacher.aspx");
                            }
                            con.Close();
                        }
                        string query2 = "INSERT INTO Teaching (teacher_ID, subject_ID) VALUES(@teacher_id, @subject_id)";
                        
                        
                        Response.Write("Subject ID : " + subject_id);
                        Response.Write("Teacher ID : " + tid);
                        //con.Close();
                        using (SqlCommand cmd2 = new SqlCommand(query2))
                        {
                            cmd2.Parameters.AddWithValue("@subject_id", subject_id);
                            cmd2.Parameters.AddWithValue("@teacher_id", tid);
                            cmd2.Connection = con;
                            con.Open();
                            cmd2.ExecuteNonQuery();
                            con.Close();
                        }
                        Response.Redirect("Home_Teacher.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        protected void ddlSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            //semester select karse etle aema jetla subject avta hase te batavse
            ddlSubject.Items.Clear();
            ddlSubject.Items.Add(new ListItem("Select Subject", ""));
            int selectedSem = ddlSemester.SelectedIndex;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString;
            //string query = "SELECT subject_ID  FROM Subject WHERE subject_name = (@sem)";
            string query = "SELECT subject_name FROM Subject WHERE semester = (@sem)";
            try
            {
                using (con)
                {
                    using(SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@sem", selectedSem);
                        cmd.Connection = con;
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        string subName;
                        while (reader.Read())
                        {
                            subName = reader.GetString(0);
                            ddlSubject.Items.Add(new ListItem(subName, subName));
                        }
                        con.Close() ;
                    }
                }
            }catch(Exception ex)
            {
                Response.Write(ex);
            }
        }
    }
}