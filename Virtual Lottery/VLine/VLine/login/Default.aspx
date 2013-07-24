<%@ Page Language="C#" Title="Index" AutoEventWireup="true" CodeBehind="Default.aspx.cs" MasterPageFile="~/WebForms/Site1.Master" Inherits="VLine.WebForms.Index" %>

<%@ Register Src="~/User Controls/LoginForm.ascx" TagPrefix="uc1" TagName="LoginForm" %>
<%@ Register Src="~/User Controls/SessionHandler.ascx" TagPrefix="uc1" TagName="SessionHandler" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%-- <meta charset="utf-8">--%>
    
   <%-- <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link href="../../Content/bootstrap.css" rel="stylesheet">--%>
<%--    <style type="text/css">
        body {
            background: #b8e1fc; /* Old browsers */
            background: -moz-linear-gradient(left, #b8e1fc 0%, #a9d2f3 10%, #90bae4 25%, #90bcea 37%, #6ba8e5 46%, #90bff0 58%, #a2daf5 83%, #bdf3fd 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, right top, color-stop(0%,#b8e1fc), color-stop(10%,#a9d2f3), color-stop(25%,#90bae4), color-stop(37%,#90bcea), color-stop(46%,#6ba8e5), color-stop(58%,#90bff0), color-stop(83%,#a2daf5), color-stop(100%,#bdf3fd)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(left, #b8e1fc 0%,#a9d2f3 10%,#90bae4 25%,#90bcea 37%,#6ba8e5 46%,#90bff0 58%,#a2daf5 83%,#bdf3fd 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(left, #b8e1fc 0%,#a9d2f3 10%,#90bae4 25%,#90bcea 37%,#6ba8e5 46%,#90bff0 58%,#a2daf5 83%,#bdf3fd 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(left, #b8e1fc 0%,#a9d2f3 10%,#90bae4 25%,#90bcea 37%,#6ba8e5 46%,#90bff0 58%,#a2daf5 83%,#bdf3fd 100%); /* IE10+ */
            background: linear-gradient(to right, #b8e1fc 0%,#a9d2f3 10%,#90bae4 25%,#90bcea 37%,#6ba8e5 46%,#90bff0 58%,#a2daf5 83%,#bdf3fd 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#b8e1fc', endColorstr='#bdf3fd',GradientType=1 ); /* IE6-9 */
            padding-top: 40px;
            padding-bottom: 40px;
            background-color: #f5f5f5;
        }

        .form-signin {
            max-width: 300px;
            padding: 19px 29px 29px;
            margin: 0 auto 20px;
            background-color: #fff;
            border: 1px solid #e5e5e5;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.05);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.05);
            box-shadow: 0 1px 2px rgba(0,0,0,.05);
        }

            .form-signin .form-signin-heading,
            .form-signin .checkbox {
                margin-bottom: 10px;
            }

            .form-signin input[type="text"],
            .form-signin input[type="password"] {
                font-size: 16px;
                height: auto;
                margin-bottom: 15px;
                padding: 7px 9px;
            }
    </style>--%>

    <link href="../../Content/bootstrap-responsive.css" rel="stylesheet">

    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
    <script src="../../Scripts/html5shiv.js"></script>
    <![endif]-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:SessionHandler runat="server" ID="SessionHandler" />
    <div class="container">
        
        <%-- 
      <form id="form1" runat="server" action="/WebForms/Index.aspx" method="post" style="margin-top: 250px" enctype="multipart/form-data" class="form-signin">
        <h2 class="form-signin-heading">VTix Login</h2>
          <asp:TextBox ID="txtUname" CssClass="input-block-level" placeholder="Username" runat="server"></asp:TextBox>
          <asp:TextBox ID="txtPassword" CssClass="input-block-level" TextMode="Password" placeholder="Password" runat="server"></asp:TextBox>
          <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
          <asp:Button ID="btnLogin" runat="server" OnClick="SignIn" CssClass="btn btn-large btn-primary" Text="Sign In" />
      </form>

            <br />
            <br />--%>
        <form id="Form1" runat="server">
            <uc1:LoginForm runat="server" ID="LoginForm" />
        </form>
        <br />
        <ul class="LinkMenu">
            <li>
                <a href="/register/" title="Register for a new account">Create a new account</a>
            </li>
            <li>
                <a href="/recover/" title="Look up an account">Forgot username / password?</a>
            </li>
        </ul>
    </div>
    <!-- /container -->

</asp:Content>

