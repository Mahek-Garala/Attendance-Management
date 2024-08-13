using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace Attendance_Management
{
    public partial class Update_Student : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["sid"] != null)
                {
                    string studentId = Session["sid"].ToString();
                    LoadStudentData(studentId);
                }
                else
                {
                    Response.Redirect("Login_student.aspx");
                }
            }
        }

        private void LoadStudentData(string studentId)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString))
            {
                try
                {
                    con.Open();
                    int sem = 0;
                    string query = "SELECT semester FROM Student WHERE student_ID = @stu_id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@stu_id", studentId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                sem = reader.GetInt32(0);
                            }
                            reader.Close();
                        }
                    }

                    if (sem != 8)
                    {
                        sem++;
                        ListItem listItem = new ListItem("Semester " + sem.ToString(), sem.ToString());
                        ddlUpdateSemester.Items.Add(listItem);
                    }

                    string query1 = "SELECT email, mobile FROM Student WHERE student_ID = @StudentID";
                    using (SqlCommand cmd = new SqlCommand(query1, con))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", studentId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtUpdateEmail.Text = reader["email"].ToString();
                                txtUpdateMobile.Text = reader["mobile"].ToString();
                            }
                            reader.Close ();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            if (Session["sid"] == null)
            {
                Response.Redirect("Login_student.aspx");
                return;
            }

            string studentId = Session["sid"].ToString();
            string connString = ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                try
                {
                    // Update email if not empty
                    if (!string.IsNullOrEmpty(txtUpdateEmail.Text))
                    {
                        string query = "UPDATE Student SET email = @Email WHERE student_ID = @StudentID";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Email", txtUpdateEmail.Text);
                            cmd.Parameters.AddWithValue("@StudentID", studentId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Update mobile if not empty
                    if (!string.IsNullOrEmpty(txtUpdateMobile.Text))
                    {
                        string query = "UPDATE Student SET mobile = @Mobile WHERE student_ID = @StudentID";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Mobile", txtUpdateMobile.Text);
                            cmd.Parameters.AddWithValue("@StudentID", studentId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Update semester if not empty
                    if (!string.IsNullOrEmpty(ddlUpdateSemester.SelectedValue))
                    {
                        int updatedSem = int.Parse(ddlUpdateSemester.SelectedValue);
                        string query = "UPDATE Student SET semester = @Semester WHERE student_ID = @StudentID";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Semester", updatedSem);
                            cmd.Parameters.AddWithValue("@StudentID", studentId);
                            cmd.ExecuteNonQuery();
                        }

                        // Insert new subjects into Attend table
                        List<int> subjectIds = new List<int>();
                        string querySubjects = "SELECT subject_ID FROM Subject WHERE semester = @Semester";
                        using (SqlCommand cmd = new SqlCommand(querySubjects, con))
                        {
                            cmd.Parameters.AddWithValue("@Semester", updatedSem);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    subjectIds.Add(reader.GetInt32(0));
                                }
                            }
                        }

                        string insertQuery = "INSERT INTO Attend (student_ID, subject_ID) VALUES (@StudentID, @SubjectID)";
                        foreach (int subjectId in subjectIds)
                        {
                            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                            {
                                cmd.Parameters.AddWithValue("@StudentID", studentId);
                                cmd.Parameters.AddWithValue("@SubjectID", subjectId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    Response.Redirect("Home_Student.aspx");
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
