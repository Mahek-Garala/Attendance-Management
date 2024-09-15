    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    namespace Attendance_Management
    {
        public partial class ViewStudent : System.Web.UI.Page
        {
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    if (Session["tid"] != null)
                    {
                        LoadSubjects();
                    }
                    else
                    {
                        Response.Redirect("Login_teacher.aspx");
                    }
                }
            }

            private void LoadSubjects()
            {
                //session mathi teacherId & department
                Int32 tid = Int32.Parse(Session["tid"].ToString());
                string department = Session["department"].ToString();
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString); 
                using (con)
                {
                    con.Open();
                    string query = "SELECT subject_ID FROM Teaching WHERE teacher_ID = @teacher_ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@teacher_ID", tid);

                    List<int> subjectIDs = new List<int>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjectIDs.Add(reader.GetInt32(0));
                        }
                        reader.Close();
                    }
                
                    List<string> subjectNames = new List<string>();
                    foreach (int id in subjectIDs)
                    {
                        string query2 = "SELECT subject_name FROM Subject WHERE subject_ID = @subject_ID AND department = @department";
                        SqlCommand cmd2 = new SqlCommand(query2, con);
                        cmd2.Parameters.AddWithValue("@subject_ID", id);
                        cmd2.Parameters.AddWithValue("@department", department);

                        using (SqlDataReader reader2 = cmd2.ExecuteReader())
                        {
                            while (reader2.Read())
                            {
                                subjectNames.Add(reader2.GetString(0));
                            }
                            reader2.Close();
                        }
                    }

                    foreach (string name in subjectNames)
                    {
                        ddlSubject.Items.Add(new ListItem(name));
                    }
                }
            }

            protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
            {
                string subjectName = ddlSubject.SelectedValue;

                if (!string.IsNullOrEmpty(subjectName))
                {
                    PopulateStudentData(subjectName);
                }
            }

        private void PopulateStudentData(string subjectName)
        {
            DataTable dtStudents = new DataTable();
            dtStudents.Columns.Add("StudentID");
            dtStudents.Columns.Add("Name");
            dtStudents.Columns.Add("Semester");
            dtStudents.Columns.Add("SubjectAttendance", typeof(double));
            dtStudents.Columns.Add("TotalAttendance", typeof(double));

            Int32 tid = Int32.Parse(Session["tid"].ToString());
            string department = Session["department"].ToString();

            int selected_subId = 0;
            int sem = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["attendanceDatabase"].ConnectionString);
            using (con)
            {
                //student show krva mate subject prthi semester retrieve kari 
                //je subject teacher e select karyo hase 
                string query = "SELECT semester, subject_ID FROM Subject WHERE subject_name = (@sub_name)";
                try
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        //subjectName is a local var from 'ddlSubject_SelectedIndexChanged'
                        cmd.Parameters.AddWithValue("@sub_name", subjectName);
                        cmd.Connection = con;
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                sem = reader.GetInt32(0);
                                selected_subId = reader.GetInt32(1);
                            }
                        }
                        reader.Close();
                    }

                    string query1 = "SELECT student_ID, name, semester FROM Student WHERE semester = (@sem) AND branch = (@branch)";
                    using (SqlCommand cmd1 = new SqlCommand(query1))
                    {
                        cmd1.Parameters.AddWithValue("@sem", sem);
                        cmd1.Parameters.AddWithValue("@branch", department);
                        cmd1.Connection = con;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd1))
                        {
                            DataTable dtStudentsData = new DataTable();
                            da.Fill(dtStudentsData);

                            foreach (DataRow dataRow in dtStudentsData.Rows)
                            {
                                DataRow row = dtStudents.NewRow();
                                row["StudentID"] = dataRow["student_ID"];
                                row["Name"] = dataRow["name"];
                                row["Semester"] = dataRow["semester"];
                                row["SubjectAttendance"] = CalculateAttendance(con, dataRow["student_ID"].ToString(), selected_subId);
                                row["TotalAttendance"] = CalculateAttendance(con, dataRow["student_ID"].ToString());
                                dtStudents.Rows.Add(row);
                            }
                        }
                    }

                    gvStudent.DataSource = dtStudents;
                    gvStudent.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write($"Error: {ex.Message}");
                }
                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
        }

        private double CalculateAttendance(SqlConnection con, string studentID, int? subjectID = null)
            {
                int present = 0;
                int total = 0;

                string query = "SELECT phase1_lec_present, phase1_lec_total, phase1_lab_present, phase1_lab_total, " +
                               "phase2_lec_present, phase2_lec_total, phase2_lab_present, phase2_lab_total, " +
                               "phase3_lec_present, phase3_lec_total, phase3_lab_present, phase3_lab_total " +
                               "FROM Attend WHERE student_ID = @student_id";

                if (subjectID.HasValue)
                {
                    query += " AND subject_ID = @sub_id";
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@student_id", studentID);
                    if (subjectID.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@sub_id", subjectID.Value);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            present += reader.GetInt32(0) + reader.GetInt32(2) + reader.GetInt32(4) + reader.GetInt32(6) + reader.GetInt32(8) + reader.GetInt32(10);
                            total += reader.GetInt32(1) + reader.GetInt32(3) + reader.GetInt32(5) + reader.GetInt32(7) + reader.GetInt32(9) + reader.GetInt32(11);
                        }
                    }
                }

                if (total != 0)
                {
                    return Math.Round(((double)present / total) * 100, 2, MidpointRounding.ToEven);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
