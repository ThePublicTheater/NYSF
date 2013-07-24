<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Nysf.UserControls.Default" %>
<%@ Register TagPrefix="nysf" TagName="LoginForm2" Src="~/usercontrols/LoginForm2.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<nysf:LoginForm2 runat="server" />
    </div>
    </form>
</body>
</html>
