<%@ Register TagPrefix="uc1" TagName="HeaderControl" Src="Control/HeaderControl.ascx" %>
<%@ Page language="c#" Codebehind="Top.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.Top" %>
<%@ Register TagPrefix="uc1" TagName="CFTHeader" Src="CFTHeader/CFTHeader.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Top</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="styles/ossstyle.css" type="text/css" rel="stylesheet">
		<LINK href="styles/local.css" type="text/css" rel="stylesheet">
		<script type="text/javascript">   
		    window.onload = function ()
		    {
		        setInterval(function () {
		            var iframe = document.getElementById("iframe_CheckLogin");
		            iframe.src = "CheckLogin.aspx";
		        }, 1000 * 60 * 2);
		    }
        </script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
        <iframe width="0" height="0" frameborder="0" style="display:none" id="iframe_CheckLogin"></iframe>
		<form id="Form1" method="post" runat="server">
			<uc1:CFTHeader id="CFTHeader1" runat="server"></uc1:CFTHeader>
			<hr>
		</form>
	</body>
</HTML>
