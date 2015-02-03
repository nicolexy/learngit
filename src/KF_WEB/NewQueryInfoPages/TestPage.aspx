<%@ Register TagPrefix="uc1" TagName="UserAppealCheckControl" Src="../Control/UserAppealCheckControl.ascx" %>
<%@ Page language="c#" Codebehind="TestPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.TestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>TestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script type="text/javascript">
			var tip = 1;
			function showTips()
			{
				//alert("fired");

				
				if(tip)
				{
					document.getElementById("div_1").style.display = "";
					document.getElementById("cb_2").checked = true;
					tip = 0;
				}
				else
				{
					document.getElementById("div_1").style.display = "none";
					document.getElementById("cb_2").checked = false;
					tip = 1;
				}
			}
			
			function Init()
			{
				document.getElementById("rbtn_pass").checked=true;
			}
			
			function enterPress(e)
					{
						if(window.event) // IE
						{
							if(e.keyCode == 13)
								alert("enter press");
						}
					}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" onkeydown="enterPress(event)">
		<form id="Form1" method="post" runat="server">
			<table>
				<tr>
					<td>
						<input type="radio" name="rbtn" value="通过" id="rbtn_pass">通过 <input type="radio" name="rbtn" id="rbtn_reject">拒绝
						<asp:CheckBoxList Runat="server" id="CheckBoxList1">
							<asp:ListItem Value="1"></asp:ListItem>
							<asp:ListItem Value="2"></asp:ListItem>
							<asp:ListItem Value="3"></asp:ListItem>
							<asp:ListItem Value="4"></asp:ListItem>
							<asp:ListItem Value="5"></asp:ListItem>
						</asp:CheckBoxList>
					</td>
					<td><input runat="server" type="checkbox" id="cb_2" value="cb2" onclick="showTips()"><asp:Label Runat="server" Text="value" id="Label1"></asp:Label></td>
					<td><asp:CheckBox runat="server" ID="cb_1" Text="cb1" /></td>
					<td>
						<div id="div_1" style="DISPLAY:none">
							<asp:CheckBoxList Runat="server" ID="cbxl_1">
								<asp:ListItem Value="1_1"></asp:ListItem>
								<asp:ListItem Value="1_2"></asp:ListItem>
								<asp:ListItem Value="1_3"></asp:ListItem>
								<asp:ListItem Value="1_4"></asp:ListItem>
							</asp:CheckBoxList>
						</div>
					</td>
				</tr>
			</table>
			<table>
				<tr>
					<td><uc1:UserAppealCheckControl runat="server" ID="Userappealcheckcontrol1" NAME="Userappealcheckcontrol2"></uc1:UserAppealCheckControl></td>
				</tr>
				<tr>
					<td><uc1:UserAppealCheckControl runat="server" ID="Userappealcheckcontrol2" NAME="Userappealcheckcontrol2"></uc1:UserAppealCheckControl></td>
				</tr>
				<tr>
					<td><uc1:UserAppealCheckControl runat="server" ID="Userappealcheckcontrol3" NAME="Userappealcheckcontrol2"></uc1:UserAppealCheckControl></td>
				</tr>
				<tr>
					<td><asp:Button Runat="server" Text="提交" id="btn_submit" onclick="btn_submit_Click" /></td>
				</tr>
				<tr>
					<td><asp:Label runat="Server" ID="lb_text1"></asp:Label></td>
				</tr>
				<tr>
					<td><a href='https://www.tenpay.com/certificates/tenpay_tel31.shtml?listid=4385931'> test</a></td>
				</tr>
				<tr>
					<td>
						<p style="PADDING-BOTTOM:10px !important; MARGIN:0px; PADDING-LEFT:0px !important; PADDING-RIGHT:0px !important; PADDING-TOP:10px !important">您于2012年05月04日提交的更换关联手机的申诉表已通过审核，请<A href="https://www.tenpay.com/certificates/tenpay_tel31.shtml?listid=4385931 rel="
								target="_blank">点此激活您的新手机</A>
							<BR>
							温馨提示: 此链接地址有效期为72小时，请您在有效期内完成新手机帐号的绑定，如果您无法正常打开链接地址，请您使用IE浏览器登录<A href="www.tenpay.com">财付通主站</A>后，再重新点击邮件链接地址，谢谢！"<a rel="nofollow" target="_blank" style="DISPLAY:none">点此激活您的新手机
							</a>
							<br>
							https://www.tenpay.com/certificates/tenpay_tel31.shtml?listid=4385931<br>
							温馨提示：此链接地址有效期为72小时，请您在有效期内完成新手机帐号的绑定，如果您无法正常打开链接地址，请您使用IE浏览器登录<a href='www.tenpay.com'>财付通主站</a>后，再重新点击邮件链接地址，谢谢！
						</p>
					</td>
				</tr>
			</table>
			<p style="PADDING-BOTTOM:10px !important; MARGIN:0px; PADDING-LEFT:0px !important; PADDING-RIGHT:0px !important; PADDING-TOP:10px !important"><br>
				https://www.tenpay.com/certificates/tenpay_tel31.shtml?listid=4385931<br>
				温馨提示：此链接地址有效期为72小时<br>
				如果您无法正常打开链接地址</p>
			<p><asp:TextBox Runat="server" ID="tbx_1" TextMode="MultiLine"></asp:TextBox></p>
			<p><asp:DropDownList Runat="server" ID="ddl_1" AutoPostBack="True"></asp:DropDownList>
			<asp:Button Runat="server" ID="btn_addFastReply" Text="添加快捷回复" onclick="btn_addFastReply_Click"></asp:Button>
			<asp:Button Runat="server" ID="btn_updateConfigFile" Text="修改配置文件" onclick="btn_updateConfigFile_Click"></asp:Button>
			</p>
		</form>
	</body>
</HTML>
