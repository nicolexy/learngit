<%@ Page language="c#" Codebehind="QQReclaim.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.QQReclaim" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QQReclaim</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script>
		
			//数字的验证
			function valNum(ev)
			{
				var e = ev.keyCode;
				//允许的有大、小键盘的数字，左右键，backspace, delete, Control + C, Control + V
				if(e != 48 && e != 49 && e != 50 && e != 51 && e != 52 && e != 53 && e != 54 && e != 55 && e != 56 && e != 57 && e != 96 && e != 97 && e != 98 && e != 99 && e != 100 && e != 101 && e != 102 && e != 103 && e != 104 && e != 105 && e != 37 && e != 39 && e != 13 && e != 8 && e != 46)
				{
					if(ev.ctrlKey == false)
					{
						//不允许的就清空!
						ev.returnValue = "";
					}
					else
					{
						//验证剪贴板里的内容是否为数字!
						valClip(ev);
					}
				}
			}
			//验证剪贴板里的内容是否为数字!
			function valClip(ev)
			{
				//查看剪贴板的内容!
				var content = clipboardData.getData("Text");
				if(content != null)
				{
					try
					{
						var test = parseInt(content);
						var str = "" + test;
						if(isNaN(test) == true)
						{
							//如果不是数字将内容清空!
							clipboardData.setData("Text","");
						}
						else
						{
							if(str != content)
								clipboardData.setData("Text", str);
						}
					}
					catch(e)
					{
						//清空出现错误的提示!
						alert("粘贴出现错误!");
					}
				}
			}

		    function CheckEmail()
		    {
		        var txtEmail = document.getElementById("txtEmail");
		        
		        if(txtEmail.value.replace( /^\s*/, "").replace( /\s*$/, "").length == 0)
		        {
		            txtEmail.focus();
		            txtEmail.select();
		            alert("邮箱不允许为空!");
		            return false;
		        }
		    }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="POSITION: absolute; TOP: 5%; LEFT: 5%" id="Table1" border="1" cellSpacing="1"
				cellPadding="1" width="800">
				<TR>
					<TD style="WIDTH: 800px" bgColor="#e4e5f7" colSpan="4"><FONT face="宋体"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;QQ帐号回收</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></TD>
				</TR>
				<TR>
					<TD width="150" align="right"><asp:label id="Label2" runat="server">QQ号</asp:label></TD>
					<TD width="250"><asp:textbox onkeydown="valNum(event);" id="txtQQ" onpaste="clip(event);" runat="server"></asp:textbox></TD>
					<TD colSpan="2" align="center"><FONT face="宋体"><asp:radiobutton id="rbtnQQ" Text="普通账号" Runat="server" GroupName="GroupQQType"></asp:radiobutton><asp:radiobutton id="rbtnBeauty" Text="QQ靓号" Runat="server" GroupName="GroupQQType"></asp:radiobutton></FONT></TD>
				</TR>
				<TR>
					<TD colSpan="4" align="center"><FONT face="宋体"><asp:button id="btnSearch" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
				<tr>
					<TD width="150" align="right"><asp:label id="Label4" runat="server">最后金额(元)</asp:label></TD>
					<TD width="250"><asp:label id="lblBalance" runat="server"></asp:label></TD>
					<TD width="150" align="right"><asp:label id="Label5" runat="server">回收时间</asp:label></TD>
					<TD width="250"><asp:label id="lblReclaimTime" runat="server"></asp:label></TD>
				</tr>
				<tr>
					<TD align="right"><asp:label id="Label3" runat="server">邮箱</asp:label></TD>
					<TD><asp:textbox id="txtEmail" runat="server"></asp:textbox></TD>
					<TD colSpan="2" align="center"><FONT face="宋体"><asp:button id="btnSend" runat="server" Width="80px" Text="发 送" Enabled="False" onclick="btnSend_Click"></asp:button></FONT></TD>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
