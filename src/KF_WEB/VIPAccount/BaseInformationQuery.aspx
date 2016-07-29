<%@ Page language="c#" Codebehind="BaseInformationQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.VIPAccount.BaseInformationQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BaseInformationQuery</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="VBScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script>
			//���ֵ���֤
			function valNum(ev)
			{
				var e = ev.keyCode;
				//������д�С���̵����֣����Ҽ���backspace, delete, Control + C, Control + V
				if(e != 48 && e != 49 && e != 50 && e != 51 && e != 52 && e != 53 && e != 54 && e != 55 && e != 56 && e != 57 && e != 96 && e != 97 && e != 98 && e != 99 && e != 100 && e != 101 && e != 102 && e != 103 && e != 104 && e != 105 && e != 37 && e != 39 && e != 13 && e != 8 && e != 46)
				{
					if(ev.ctrlKey == false)
					{
						//������ľ����!
						ev.returnValue = "";
					}
					else
					{
						//��֤��������������Ƿ�Ϊ����!
						valClip(ev);
					}
				}
			}
			//��֤��������������Ƿ�Ϊ����!
			function valClip(ev)
			{
				//�鿴�����������!
				var content = clipboardData.getData("Text");
				if(content != null)
				{
					try
					{
						var test = parseInt(content);
						var str = "" + test;
						if(isNaN(test) == true)
						{
							//����������ֽ��������!
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
						//��ճ��ִ������ʾ!
						alert("ճ�����ִ���!");
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
		            alert("���䲻����Ϊ��!");
		            return false;
		        }
		    }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server" style="FONT-FAMILY:����">
			<table style="POSITION: absolute; TOP: 5%; LEFT: 5%" id="Table1" border="1" cellSpacing="1"
				cellPadding="1" width="800">
				<tr style="BACKGROUND-COLOR: #e4e5f7">
					<td colspan="4"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;��Ա������Ϣ��ѯ</FONT>
					</td>
				</tr>
				<tr style="BACKGROUND-COLOR: #e4e5f7">
					<td colspan="4">&nbsp;&nbsp;�˺�&nbsp;&nbsp;<asp:textbox id="txtQQ" runat="server"></asp:textbox>
						<span style="WIDTH:30px"></span>
						<asp:button id="btnSearch" runat="server" width="80" text="�� ѯ"></asp:button></td>
				</tr>
				<tr>
					<td colspan="4">&nbsp;&nbsp;�˺�:&nbsp;&nbsp;<asp:Label ID="account" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td colspan="2">&nbsp;&nbsp;��Ա�ȼ�:&nbsp;&nbsp;<asp:Label ID="level" runat="server"></asp:Label></td>
					<td colspan="2">&nbsp;&nbsp;����ʱ��:&nbsp;&nbsp;<asp:Label ID="expiration" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td colspan="4">&nbsp;&nbsp;�Ƹ�ֵ:&nbsp;&nbsp;<asp:Label ID="balance" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td colspan="4">&nbsp;&nbsp;��Ա����:&nbsp;&nbsp;<asp:Label ID="vipType" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td colspan="4">&nbsp;&nbsp;�״ο�ͨSVIP��Ա�Ƹ�ֵ��ȡʱ��:&nbsp;&nbsp;<asp:Label ID="firstTime" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td colspan="4">&nbsp;&nbsp;ʵ����֤�Ƹ�ֵ��ȡʱ��:&nbsp;&nbsp;<asp:Label ID="realNameTime" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td colspan="4">&nbsp;&nbsp;һ��ͨ�Ƹ�ֵ��ȡʱ��:&nbsp;&nbsp;<asp:Label ID="yidiantongTime" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td colspan="4">&nbsp;&nbsp;֤��Ƹ�ֵ��ȡʱ��:&nbsp;&nbsp;<asp:Label ID="certificationTime" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td colspan="4">&nbsp;&nbsp;���һ��������ʱ��:&nbsp;&nbsp;<asp:Label ID="lastMissionTime" runat="server"></asp:Label></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
