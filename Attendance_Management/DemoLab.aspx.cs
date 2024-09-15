using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI.WebControls;

namespace Attendance_Management
{
    public partial class DemoLab : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["tid"] != null)
                {
                    LoadTeacherSubjectList();
                }
                else
                {
                    Response.Redirect("Login_Teacher.aspx");
                }
            }
        }

        private void LoadTeacherSubjectList()
        {
            string str_tid = Session["tid"].ToString();
            Int32 tid = Int32.Parse(str_tid);
            string department = Session["department"].ToString();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString;

            List<string> subjects = new List<string>();
            List<int> sid = new List<int>();

            string query = "SELECT subject_ID FROM Teaching WHERE teacher_ID = (@teacher_ID)";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@teacher_ID", tid);
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Int32 sub_id = reader.GetInt32(0);
                            sid.Add(sub_id);
                        }
                    }
                    else
                    {
                        Response.Write("Please Add Some Subjects First");
                    }
                    reader.Close();
                    foreach (int id in sid)
                    {
                        string query2 = "SELECT subject_name FROM Subject WHERE subject_ID = (@subject_ID) AND department = (@dep)";
                        using (SqlCommand cmd3 = new SqlCommand(query2))
                        {
                            cmd3.Parameters.AddWithValue("@subject_ID", id);
                            cmd3.Parameters.AddWithValue("@dep", department);
                            cmd3.Connection = con;
                            SqlDataReader reader2 = cmd3.ExecuteReader();
                            if (reader2.HasRows)
                            {
                                while (reader2.Read())
                                {
                                    string name = reader2.GetString(0);
                                    subjects.Add(name);
                                }
                            }
                            reader2.Close();
                        }
                    }
                    foreach (string name in subjects)
                    {
                        ListItem item = new ListItem(name);
                        ddlSubject.Items.Add(item);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }



        }
        private void LoadTotalLabs()
        {
            string subjectName = ddlSubject.SelectedValue;
            string phase = ddlPhase.SelectedValue;
            int sub_id = 0;

            if (!string.IsNullOrEmpty(subjectName) && !string.IsNullOrEmpty(phase))
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString))
                {
                    try
                    {
                        con.Open();

                        // Get subject ID
                        string query = "SELECT subject_ID FROM Subject WHERE subject_name = @sub";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@sub", subjectName);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    sub_id = reader.GetInt32(0);
                                }
                            }
                        }

                        if (sub_id > 0)
                        {
                            string updatedquery = "";
                            switch (phase)
                            {
                                case "1":
                                    updatedquery = "SELECT phase1_lab_total FROM Attend WHERE subject_ID = @sub_id";
                                    break;
                                case "2":
                                    updatedquery = "SELECT phase2_lab_total FROM Attend WHERE subject_ID = @sub_id";
                                    break;
                                case "3":
                                    updatedquery = "SELECT phase3_lab_total FROM Attend WHERE subject_ID = @sub_id";
                                    break;
                            }

                            if (!string.IsNullOrEmpty(updatedquery))
                            {
                                using (SqlCommand cmd = new SqlCommand(updatedquery, con))
                                {
                                    cmd.Parameters.AddWithValue("@sub_id", sub_id);
                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            txtTotalLab.Text = reader[0].ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            string subjectName = ddlSubject.SelectedValue;
            if (!string.IsNullOrEmpty(subjectName))
            {
                //Response.Write("on select index ");
                BindStudentData(subjectName); //function call
            }
        }
        protected void ddlPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            string phase = ddlPhase.SelectedValue;
            if (!string.IsNullOrEmpty(phase))
            {
                //Response.Write("on select index ");
                LoadTotalLabs();
            }
        }
        protected void btnIncrement_Click(object sender, EventArgs e)
        {
            UpdateLabCount(2);
        }

        protected void btnDecrement_Click(object sender, EventArgs e)
        {
            UpdateLabCount(-2);
        }

        private void UpdateLabCount(int increment)
        {
            string subjectName = ddlSubject.SelectedValue;
            string selectedPhase = ddlPhase.SelectedValue; // Get the selected phase

            if (!string.IsNullOrEmpty(subjectName) && !string.IsNullOrEmpty(selectedPhase))
            {
                int sub_id = 0;
                string columnName = ""; // To hold the column name for the selected phase

                // Determine which column to update based on the selected phase
                switch (selectedPhase)
                {
                    case "1":
                        columnName = "phase1_lab_total";
                        break;
                    case "2":
                        columnName = "phase2_lab_total";
                        break;
                    case "3":
                        columnName = "phase3_lab_total";
                        break;
                }

                if (!string.IsNullOrEmpty(columnName))
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString))
                    {
                        con.Open();

                        // Get the subject ID based on the subject name
                        string querySubjectID = "SELECT subject_ID FROM Subject WHERE subject_name = @subjectName";
                        using (SqlCommand cmdSubject = new SqlCommand(querySubjectID, con))
                        {
                            cmdSubject.Parameters.AddWithValue("@subjectName", subjectName);
                            object result = cmdSubject.ExecuteScalar();
                            if (result != null)
                            {
                                sub_id = Convert.ToInt32(result);
                            }
                        }

                        if (sub_id > 0)
                        {
                            // Update the lecture count
                            string queryUpdate = $"UPDATE Attend SET {columnName} = {columnName} + @increment WHERE subject_ID = @sub_id";

                            using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con))
                            {
                                cmdUpdate.Parameters.AddWithValue("@increment", increment);
                                cmdUpdate.Parameters.AddWithValue("@sub_id", sub_id);
                                cmdUpdate.ExecuteNonQuery();
                            }

                            // Reload the updated total lecture count
                            LoadTotalLabs();
                        }
                    }
                }
            }
        }

        private void BindStudentData(string subjectName)
        {
            // Response.Write("to fill data");
            // System.Diagnostics.Debug.WriteLine("This is a debug message.");
            string connString = ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "SELECT student_ID, name, semester, @subjectName AS SubjectName FROM Student WHERE semester = (SELECT semester FROM Subject WHERE subject_name = @subjectName)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@subjectName", subjectName);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    gvStudents.DataSource = dt;
                    gvStudents.DataBind();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string selectedPhase = ddlPhase.SelectedValue;

            if (!string.IsNullOrEmpty(selectedPhase))
            {
                foreach (GridViewRow row in gvStudents.Rows)
                {
                    CheckBox chkPresent = (CheckBox)row.FindControl("chkPresent");
                    string studentID = row.Cells[0].Text;
                    string subjectName = ddlSubject.SelectedValue;

                    if (chkPresent != null && chkPresent.Checked)
                    {
                        UpdateAttendanceInDatabase(studentID, subjectName, selectedPhase);
                    }
                }
                Response.Redirect("Home_Teacher.aspx");
            }
            else
            {
                // Handle case where no phase is selected
                Response.Write("Please select a phase.");
            }
        }

        private void UpdateAttendanceInDatabase(string studentID, string subjectName, string selectedPhase)
        {
            string connString = ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connString))
            {
                // Get the subject ID based on the subject name
                string querySubjectID = "SELECT subject_ID FROM Subject WHERE subject_name = @subjectName";
                int subjectID = 0;

                using (SqlCommand cmdSubject = new SqlCommand(querySubjectID, con))
                {
                    cmdSubject.Parameters.AddWithValue("@subjectName", subjectName);
                    con.Open();
                    subjectID = (int)cmdSubject.ExecuteScalar();
                    con.Close();
                }

                // Determine which phase to update
                string attendanceField = "";
                if (selectedPhase == "1")
                {
                    attendanceField = "phase1_lab_present";
                }
                else if (selectedPhase == "2")
                {
                    attendanceField = "phase2_lab_present";
                }
                else if (selectedPhase == "3")
                {
                    attendanceField = "phase3_lab_present";
                }

                // Update attendance for the student and subject
                string updateQuery = $"UPDATE Attend SET {attendanceField} = {attendanceField} + 2 WHERE student_ID = @studentID AND subject_ID = @subjectID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                {
                    cmd.Parameters.AddWithValue("@studentID", studentID);
                    cmd.Parameters.AddWithValue("@subjectID", subjectID);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

    }
}
