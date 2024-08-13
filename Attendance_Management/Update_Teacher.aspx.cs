using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Attendance_Management
{
    public partial class Update_Teacher : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["tid"] != null)
                {
                    int teacherID = Convert.ToInt32(Session["tid"]);
                    LoadTeacherData(teacherID);
                }
                else
                {
                    Response.Redirect("Login_teacher.aspx");
                }
            }
        }

        private void LoadTeacherData(int teacherID)
        {
            string connString = ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "SELECT email, mobile FROM Teacher WHERE teacher_ID = @TeacherID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacherID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtUpdateEmail.Text = reader["email"].ToString();
                        txtUpdateMobile.Text = reader["mobile"].ToString();
                    }
                }
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            if (Session["tid"] == null)
            {
                Response.Redirect("Login_teacher.aspx");
                return;
            }

            int teacherID = Convert.ToInt32(Session["tid"]);
            string connString = ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "UPDATE Teacher SET email = @Email, mobile = @Mobile WHERE teacher_ID = @TeacherID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", txtUpdateEmail.Text);
                    cmd.Parameters.AddWithValue("@Mobile", txtUpdateMobile.Text);
                    cmd.Parameters.AddWithValue("@TeacherID", teacherID);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        Response.Redirect("Home_Teacher.aspx");
                        
                      
                    }
                    catch (Exception ex)
                    {
                        // Display or log the error message
                        Response.Write("An error occurred: " + ex.Message);
                    }
                }
            }
        }
    }
}
