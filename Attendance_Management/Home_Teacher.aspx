<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home_Teacher.aspx.cs" Inherits="Attendance_Management.Home_Teacher"
    MasterPageFile="~/Teacher.Master"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style>
    body {
        background-color: #f4f4f4; /* Light gray background for the whole page */
    }

    .teacher-info-container {
        width: 300px;
        margin: 0 auto;
        padding: 20px;
        border: 1px solid #ccc;
        border-radius: 10px;
        background-color: #ffffff; /* White background for the teacher info container */
      
    }

    .teacher-info-label {
        font-weight: bold;
        margin-bottom: 5px;
    }

    .teacher-info-value {
        margin-bottom: 15px;
        color: #333;
    }

    .teacher-info-subjects {
        font-style: italic;
        color: #666;
    }

</style>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="teacher-info-container">
        <div class="teacher-info-label">Teacher ID:</div>
        <asp:Label ID="lblTid" CssClass="teacher-info-value" runat="server" />

        <div class="teacher-info-label">Name:</div>
        <asp:Label ID="lblName" CssClass="teacher-info-value" runat="server" />

        <div class="teacher-info-label">Email:</div>
        <asp:Label ID="lblEmail" CssClass="teacher-info-value" runat="server" />

        <div class="teacher-info-label">Mobile:</div>
        <asp:Label ID="lblMobile" CssClass="teacher-info-value" runat="server" />

        <div class="teacher-info-label">Date of Birth:</div>
        <asp:Label ID="lblDob" CssClass="teacher-info-value" runat="server" />

        <div class="teacher-info-label">Department:</div>
        <asp:Label ID="lbldepartment" CssClass="teacher-info-value" runat="server" />

        <div class="teacher-info-label">Subjects:</div>
        <asp:Label ID="lblsubject" CssClass="teacher-info-subjects" runat="server" />
    </div>
</asp:Content>

