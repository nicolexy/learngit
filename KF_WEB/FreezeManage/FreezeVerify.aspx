<%@ Page language="c#" Codebehind="FreezeVerify.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.FreezeManage.FreezeVerify1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FreezeVerify</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.tbx_beginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.tbx_beginDate.value=returnValue;
					}
					function openModeEnd()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.tbx_endDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.tbx_endDate.value=returnValue;
					}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table border="1" cellSpacing="1" cellPadding="1" width="1100">
				<TR>
					<TD style="WIDTH: 443px; HEIGHT: 20px" bgColor="#e4e5f7" colSpan="2"><FONT color="red" face="宋体"><IMG src="../IMAGES/Page/post.gif" width="20" height="16"><asp:label id="lb_pageTitle" Runat="server">风控解冻审核</asp:label></FONT></TD>
					<td style="HEIGHT: 20px"></FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></td>
				</TR>
				<tr>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">财付通帐号：</label><asp:textbox id="tbx_payAccount" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">用户提交证件号码：</label><asp:textbox id="tbx_cerNO" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">用户提交绑定手机：</label><asp:textbox id="tbx_userSubBindMobile" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 130px; HEIGHT: 20px; VERTICAL-ALIGN: middle">最后一次使用的地址：</label><asp:textbox id="tbx_lastAddr" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
				<tr>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">注册姓名：</label><asp:textbox id="tbx_userName" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">注册证件号码：</label><asp:textbox id="tbx_regCreNO" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">绑定手机：</label><asp:textbox id="tbx_bindMobile" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 130px; HEIGHT: 20px; VERTICAL-ALIGN: middle">近期安装数字证书：</label><asp:textbox id="tbx_DC" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
				<tr>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">账户余额：</label><asp:textbox id="tbx_restFin" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">联系邮箱：</label><asp:textbox id="tbx_email" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">联系电话：</label><asp:textbox id="tbx_phoneNo" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
			</table>
			<br>
			<br>
			<table id="table_images" border="1" cellSpacing="1" cellPadding="1" width="1200" runat="server">
				<tr bgColor="#ffffff">
					<td vAlign="top" colSpan="3">
						<p>
							<table style="WIDTH: 100%; HEIGHT: 248px" border="0" cellSpacing="1" cellPadding="0">
								<tr align="center">
									<td bgColor="#eeeeee" height="20">身份证扫描件</td>
									<td bgColor="#eeeeee">银行卡扫描件</td>
									<td id="td_pic1" bgColor="#eeeeee" height="20" runat="server">网上支付截图1</td>
									<td id="td_pic2" bgColor="#eeeeee" height="20" runat="server">网上支付截图2</td>
								</tr>
								<tr>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image1" runat="server" Width="200px" Height="150px"></asp:image></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image2" runat="server" Width="200px" Height="150px"></asp:image></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image3" runat="server" Width="200" Height="150px"></asp:image></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image4" runat="server" Width="200" Height="150px"></asp:image></td>
								</tr>
							</table>
							<table style="Z-INDEX: 0; WIDTH: 100%; DISPLAY: none; HEIGHT: 248px" id="table_bPics" border="0"
								cellSpacing="1" cellPadding="0" runat="server">
								<tr align="center">
									<td id="td_bpic1" bgColor="#eeeeee" height="20" width="20%" runat="server">备用图1</td>
									<td id="td_bpic2" bgColor="#eeeeee" height="20" width="20%" runat="server">备用图2</td>
									<td id="td_bpic3" bgColor="#eeeeee" height="20" width="20%" runat="server">备用图3</td>
								</tr>
								<tr>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image5" runat="server" Width="200" Height="150px" Visible="False"></asp:image></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image6" runat="server" Width="200" Height="150px" Visible="False"></asp:image></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image7" runat="server" Width="200" Height="150px" Visible="False"></asp:image></td>
								</tr>
							</table>
						</p>
					</td>
				</tr>
			</table>
			<table border="1" cellSpacing="1" cellPadding="1" width="1200">
				<tr>
					<td bgColor="#eeeeee" height="20" width="50%">用户补充问题描述</td>
					<td bgColor="#eeeeee" height="20" width="50%">客服处理结果</td>
				</tr>
				<tr>
					<td bgColor="#eeeeee" width="50%"><asp:textbox id="tbx_userQA" Runat="server" Width="400" Enabled="false" Height="82" TextMode="MultiLine"></asp:textbox></td>
					<td bgColor="#eeeeee" width="50%"><asp:textbox id="tbx_handleResult" Runat="server" Width="430" Height="82" TextMode="MultiLine"></asp:textbox><br>
						<label style="MARGIN-BOTTOM: 50px">快捷回复：</label>
						<asp:dropdownlist id="ddl_fastReply1" Runat="server" Width="350" AutoPostBack="True"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:button id="btn_manageFastReply" Runat="server" Text="管理快捷回复" onclick="btn_manageFastReply_Click"></asp:button><br>
					</td>
				</tr>
				<tr>
					<td bgColor="#ffffff" height="10" colSpan="2"></td>
				</tr>
				<tr>
					<td bgColor="#ffffff" colSpan="5" align="center"><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_hangUp" runat="server" Width="100" Text="挂 起" onclick="btn_hangUp_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_Finish1" runat="server" Width="100" Text="结单（已解冻）" onclick="btn_Finish1_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_Finish2" runat="server" Width="100" Text="结单（未解冻）" onclick="btn_Finish2_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_Del" runat="server" Width="100" Text="作 废" onclick="btn_Del_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_addRecord" runat="server" Width="100" Text="补充处理结果" onclick="btn_addRecord_Click"></asp:button></span></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
