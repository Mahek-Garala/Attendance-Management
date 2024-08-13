<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher.Master" AutoEventWireup="true" CodeBehind="MarkAttendance.aspx.cs" Inherits="Attendance_Management.MarkAttendance" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <style>
        .container {
            width: 80%;
            margin: 0 auto;
        }

        .form-container {
            margin-top: 20px;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f9f9f9;
        }

        .form-label {
            font-weight: bold;
            margin-bottom: 10px;
        }

        .form-dropdown {
            width: 100% !important;
            padding: 10px !important;
            border-radius: 5px !important;
            border: 1px solid #ccc !important;
            margin-bottom: 20px !important;
        }

        .table-container {
            margin-top: 20px;
        }

        .student-table {
            width: 100%;
            border-collapse: collapse;
        }

        .student-table th,
        .student-table td {
            padding: 10px;
            border: 1px solid #ccc;
            text-align: center;
        }

        .student-table th {
            background-color: #f2f2f2;
        }

        .add-attendance-link {
            color: #007bff;
            text-decoration: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h2>Mark Attendance</h2>
        <div class="form-container">
            <div class="form-group">
                <label for="ddlSubject" class="form-label">Select Subject:</label>
                <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                    <asp:ListItem Text="Select Subject" Value="" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="table-container">
            <asp:GridView ID="gvStudents" runat="server" AutoGenerateColumns="False" CssClass="student-table" EmptyDataText="No students found for the selected subject.">
                <Columns>
                    <asp:BoundField DataField="student_ID" HeaderText="Student ID" />
                    <asp:BoundField DataField="name" HeaderText="Name" />
                    <asp:BoundField DataField="semester" HeaderText="Semester" />
                    <asp:TemplateField HeaderText="Add Attendance">
                        <ItemTemplate>
                            <a href='AddAttendance.aspx?studentID=<%# Eval("student_ID") %>&subjectName=<%# Eval("SubjectName") %>'>Add</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    
</asp:Content>