<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher.Master" AutoEventWireup="true" CodeBehind="ViewStudent.aspx.cs" Inherits="Attendance_Management.ViewStudent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

        .low-attendance {
            background-color: #ff5555; /* Red background color */
            color: #ffffff; /* White text color */
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h2>View Student</h2>
        <div class="form-container">
            <div class="form-group">
                <label for="ddlSubject" class="form-label">Select Subject:</label>
                <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                    <asp:ListItem Text="Select Subject" Value="" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="table-container">
            <asp:GridView ID="gvStudent" runat="server" CssClass="student-table" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="StudentID" HeaderText="Student ID" />
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Semester" HeaderText="Semester" />
                    <asp:BoundField DataField="SubjectAttendance" HeaderText="Subject Attendance" />
                    <asp:BoundField DataField="TotalAttendance" HeaderText="Total Attendance" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
