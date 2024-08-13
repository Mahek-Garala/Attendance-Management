using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Attendance_Management
{
    public partial class AddAttendance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["studentID"] != null && Request.QueryString["subjectName"] != null)
            {
                string studentID = Request.QueryString["studentID"];
                string subjectName = Request.QueryString["subjectName"];
                stu_id.Value = studentID; ;
                sub_name.Value = subjectName;

                // Now you have the StudentID and SubjectID, you can use them as needed
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string student_id = stu_id.Value;
            string subjectName = sub_name.Value;
            int attendedLecture = Int32.Parse(txtAttendedLecture.Text);
            int totalLecture = Int32.Parse(txtTotalLecture.Text);
            int attendedLab = Int32.Parse(txtAttendedLab.Text);
            int totalLab = Int32.Parse(txtTotalLab.Text);
            string phase = ddlPhase.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString;
            int sub_id = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Retrieve subject ID
                string query = "SELECT subject_ID FROM Subject WHERE subject_name = @sub";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@sub", subjectName);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        sub_id = reader.GetInt32(0);
                    }
                    reader.Close();
                }
                con.Close();
                // Update attendance

                if (sub_id > 0)
                {
                    string updateQuery = "";
                    switch (phase)
                    {
                        case "Phase 1":
                            updateQuery = "UPDATE Attend SET phase1_lec_present = @attendedLecture, phase1_lec_total = @totalLecture, phase1_lab_present = @attendedLab, phase1_lab_total = @totalLab WHERE student_ID = @student_id AND subject_ID = @subject_id";
                            break;
                        case "Phase 2":
                            updateQuery = "UPDATE Attend SET phase2_lec_present = @attendedLecture, phase2_lec_total = @totalLecture, phase2_lab_present = @attendedLab, phase2_lab_total = @totalLab WHERE student_ID = @student_id AND subject_ID = @subject_id";
                            break;
                        case "Phase 3":
                            updateQuery = "UPDATE Attend SET phase3_lec_present = @attendedLecture, phase3_lec_total = @totalLecture, phase3_lab_present = @attendedLab, phase3_lab_total = @totalLab WHERE student_ID = @student_id AND subject_ID = @subject_id";
                            break;
                        default:
                            break;
                    }

                    if (!string.IsNullOrEmpty(updateQuery))
                    {
                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@attendedLecture", attendedLecture);
                            cmd.Parameters.AddWithValue("@totalLecture", totalLecture);
                            cmd.Parameters.AddWithValue("@attendedLab", attendedLab);
                            cmd.Parameters.AddWithValue("@totalLab", totalLab);
                            cmd.Parameters.AddWithValue("@student_id", student_id);
                            cmd.Parameters.AddWithValue("@subject_id", sub_id);
                            //cmd.ExecuteNonQuery();
                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new Exception("No rows affected.");
                            }
                            con.Close();
                        }
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Attendance Added Successfully');", true);
                    }
                    else
                    {
                        Response.Write("Invalid phase selected.");
                    }
                }
                else
                {
                    Response.Write("Subject not found.");
                }
            }
        }

    }
}