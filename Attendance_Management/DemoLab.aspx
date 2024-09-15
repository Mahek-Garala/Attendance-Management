<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher.Master" AutoEventWireup="true" CodeBehind="DemoLab.aspx.cs" Inherits="Attendance_Management.DemoLab" %>
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
        <h2>Mark Lab Attendance</h2>
        <div class="form-container">
            <div class="form-group">
                <label for="ddlSubject" class="form-label">Select Subject:</label>
                <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                    <asp:ListItem Text="Select Subject" Value="" />
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="ddlPhase" class="form-label">Select Phase:</label>
                <asp:DropDownList ID="ddlPhase" runat="server" CssClass="form-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlPhase_SelectedIndexChanged">
                    <asp:ListItem Text="Select Phase" Value="" />
                    <asp:ListItem Text="Phase 1" Value="1" />
                    <asp:ListItem Text="Phase 2" Value="2" />
                    <asp:ListItem Text="Phase 3" Value="3" />
                </asp:DropDownList>
            </div>

            <div class="form-group">
    <label for="txtTotalLab" class="form-label">Total Labs:</label>
        <div>
            <asp:TextBox ID="txtTotalLab" runat="server" CssClass="form-dropdown" ReadOnly="true"></asp:TextBox>
            <asp:Button ID="btnIncrement" runat="server" Text="+" OnClick="btnIncrement_Click" />
            <asp:Button ID="btnDecrement" runat="server" Text="-" OnClick="btnDecrement_Click" />
        </div>
</div>
        </div>

        <div class="table-container">
            <asp:GridView ID="gvStudents" runat="server" AutoGenerateColumns="False" CssClass="student-table" EmptyDataText="No students found for the selected subject.">
                <Columns>
                    <asp:BoundField DataField="student_ID" HeaderText="Student ID" />
                    <asp:BoundField DataField="name" HeaderText="Name" />
                    <asp:BoundField DataField="semester" HeaderText="Semester" />
                    <asp:TemplateField HeaderText="Present">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkPresent" runat="server" Checked="true" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />

        <asp:Button ID="btnSubmit" runat="server" Text="Submit Lab Attendance" OnClick="btnSubmit_Click" CssClass="btn btn-primary" />
    </div>
</asp:Content>
