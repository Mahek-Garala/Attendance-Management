<%@ Page Title="" Language="C#" MasterPageFile="~/Student.Master" AutoEventWireup="true" CodeBehind="Home_Student.aspx.cs"
    Inherits="Attendance_Management.Home_Student" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
        background-color: #f4f4f4; /* Light gray background for the whole page */
    }

    .info-container {
        width: 300px;
        margin: 50px auto;
        padding: 20px;
        border: 1px solid #ccc;
        border-radius: 10px;
        background-color: #ffffff; /* White background for the teacher info container */
       
    }

    .info-label {
        font-weight: bold;
        margin-bottom: 5px;
    }

    .info-value {
        margin-bottom: 15px;
        color: #333;
    }


    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="info-container">
        <div class="info-label">Student ID:</div>
        <asp:Label ID="lblSid" CssClass="info-value" runat="server" />

        <div class="info-label">Name:</div>
        <asp:Label ID="lblName" CssClass="info-value" runat="server" />

        <div class="info-label">Email:</div>
        <asp:Label ID="lblEmail" CssClass="info-value" runat="server" />

        <div class="info-label">Mobile:</div>
        <asp:Label ID="lblMobile" CssClass="info-value" runat="server" />

        <div class="info-label">Sem:</div>
        <asp:Label ID="lblSem" CssClass="info-value" runat="server" />

        <div class="info-label">Date of Birth(Password):</div>
        <asp:Label ID="lblDob" CssClass="info-value" runat="server" />

        <div class="info-label">Branch:</div>
        <asp:Label ID="lblbranch" CssClass="info-value" runat="server" />
    </div>
</asp:Content>

