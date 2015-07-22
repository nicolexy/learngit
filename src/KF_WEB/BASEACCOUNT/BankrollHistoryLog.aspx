<%@ Page language="c#" Codebehind="BankrollHistoryLog.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.BankrollHistoryLog" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BankrollHistoryLog</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
			function openModeBegin()
			{
				var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
				if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
			}

			function openModeEnd()
			{
				var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxEndDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
				if(returnValue != null) Form1.TextBoxEndDate.value=returnValue;
			}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体"></FONT>
			<br>
			<table cellSpacing="1" cellPadding="0" width="80%" align="center" bgColor="#666666" border="0">
				<tr>
					<td width="90%" background="../IMAGES/Page/bg_bl.gif" colSpan="2"><IMG height="16" src="../IMAGES/Page/post.gif" width="20"><font color="#ff0000">
							用户查询</font>
					</td>
				</tr>
				<tr>
					<td width="70%" bgColor="#ffffff">输入：
						<asp:textbox id="TextBox1_InputQQ" runat="server"></asp:textbox>&nbsp;
						<asp:label id="Label2" runat="server">开始日期</asp:label><asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton><asp:label id="Label5" runat="server">结束日期</asp:label><asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></td>
					<td bgColor="#ffffff"><asp:button id="Button1" runat="server" Text="查 询" onclick="Button1_Click"></asp:button></td>
				</tr>
			</table>
			<br>
			<TABLE cellSpacing="1" cellPadding="0" width="80%" align="center" border="0" bgColor="#666666">
				<TR>
					<TD background="../IMAGES/Page/bg_bl.gif" colSpan="5"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
						<FONT color="#ff0000">用户账户资料</FONT>
					</TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 3px" width="20%" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="3">&nbsp;QQ 账号:</TD>
					<TD style="HEIGHT: 3px" bgColor="#ffffff" height="3">&nbsp;<span class="style2">
							<asp:label id="Label1_Acc" runat="server"></asp:label></span></TD>
					<TD style="HEIGHT: 8px" width="11%" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7"
						height="8"><asp:label id="labQQstate" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 3px" width="24%" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="3">&nbsp;<FONT face="宋体">真实姓名:</FONT></TD>
					<TD style="HEIGHT: 3px" bgColor="#ffffff" height="3">&nbsp;<FONT face="宋体">
							<asp:label id="Label14_Ftruename" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 3px" width="20%" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="3">EMAIL账号:</TD>
					<TD style="HEIGHT: 3px" bgColor="#ffffff" height="3">&nbsp;<span class="style2">
							<asp:label id="labEmail" runat="server"></asp:label></span></TD>
					<TD style="HEIGHT: 8px" width="11%" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7"
						height="8"><asp:label id="labEmailState" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 3px" width="24%" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="3">&nbsp;<FONT face="宋体">内部ID:</FONT></TD>
					<TD style="HEIGHT: 3px" bgColor="#ffffff" height="3">&nbsp;<FONT face="宋体">
							<asp:label id="lbInnerID" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 13px" width="20%" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="13"><FONT face="宋体">&nbsp;手机帐号:</FONT></TD>
					<TD style="HEIGHT: 13px" bgColor="#ffffff" height="13"><FONT face="宋体">&nbsp;
							<asp:label id="labMobile" runat="server"></asp:label></FONT></TD>
					<TD style="HEIGHT: 8px" width="11%" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7"
						height="8"><asp:label id="labMobileState" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 13px" width="24%" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="13"><FONT face="宋体">&nbsp;余额支付状态:</FONT></TD>
					<TD style="HEIGHT: 13px" bgColor="#ffffff" height="13"><FONT face="宋体">&nbsp;</FONT>
						<asp:label id="lbLeftPay" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 8px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="8"><FONT face="宋体">&nbsp;帐户状态:</FONT></TD>
					<TD style="HEIGHT: 8px" bgColor="#ffffff" colSpan="2" height="8"><FONT face="宋体">&nbsp;
							<asp:label id="Label12_Fstate" runat="server"></asp:label></FONT><FONT face="宋体"></FONT></TD>
					<TD style="HEIGHT: 8px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="8"><FONT face="宋体">&nbsp;帐户类型:</FONT></TD>
					<TD style="HEIGHT: 8px" bgColor="#ffffff" height="8"><FONT face="宋体">&nbsp;
							<asp:label id="Label13_Fuser_type" runat="server"></asp:label></FONT><FONT face="宋体"></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="15">&nbsp;<FONT face="宋体">可用余额</FONT>:</TD>
					<TD style="HEIGHT: 15px" bgColor="#ffffff" colSpan="2" height="15">&nbsp;
						<asp:label id="Label15_Useable" runat="server" ForeColor="Red" Width="180px"></asp:label><FONT face="宋体"></FONT></TD>
					<TD style="HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="15">&nbsp;冻结金额:</TD>
					<TD style="HEIGHT: 15px" bgColor="#ffffff" height="15">&nbsp;
						<asp:label id="Label4_Freeze" runat="server" Width="180px"></asp:label><FONT style="BACKGROUND-COLOR: #ffffff" face="宋体"></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 4px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="4">&nbsp;昨日余额:</TD>
					<TD style="HEIGHT: 4px" bgColor="#ffffff" colSpan="2" height="4">&nbsp;
						<asp:label id="Label5_YestodayLeft" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 4px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="4">&nbsp;帐户余额<FONT face="宋体">:</FONT></TD>
					<TD style="HEIGHT: 4px" bgColor="#ffffff" height="4">&nbsp;<FONT face="宋体">
							<asp:label id="Label3_LeftAcc" runat="server" Width="180px"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 4px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="4"><FONT face="宋体">&nbsp;币种类型:</FONT></TD>
					<TD style="HEIGHT: 4px" bgColor="#ffffff" colSpan="2" height="4"><FONT face="宋体">&nbsp;</FONT>
						<asp:label id="Label2_Type" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 4px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="4"><FONT face="宋体">&nbsp;注册时间:</FONT></TD>
					<TD style="HEIGHT: 4px" bgColor="#ffffff" height="4"><FONT face="宋体">&nbsp;
							<asp:label id="lblLoginTime" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 4px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="4"><FONT face="宋体">&nbsp;当日已付:</FONT></TD>
					<TD style="HEIGHT: 4px" bgColor="#ffffff" colSpan="2" height="4"><FONT face="宋体">&nbsp;
							<asp:label id="Label16_Fapay" runat="server"></asp:label></FONT></TD>
					<TD style="HEIGHT: 13px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;单笔交易限额:</TD>
					<TD style="HEIGHT: 13px" bgColor="#ffffff" height="13">&nbsp;
						<asp:label id="Label7_SingleMax" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 13px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="13">&nbsp;单日支付限额:</TD>
					<TD style="HEIGHT: 13px" bgColor="#ffffff" colSpan="2" height="13">&nbsp;
						<asp:label id="Label8_PerDayLmt" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 13px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="宋体">&nbsp;当日提现金额:</FONT></TD>
					<TD style="HEIGHT: 13px" bgColor="#ffffff" height="13"><FONT face="宋体">&nbsp;
							<asp:label id="lbFetchMoney" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 13px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="13"><FONT face="宋体">&nbsp;当日已充值金额:</FONT></TD>
					<TD style="HEIGHT: 13px" bgColor="#ffffff" colSpan="2" height="13"><FONT face="宋体">&nbsp;
							<asp:label id="lbSave" runat="server"></asp:label></FONT></TD>
					<TD style="HEIGHT: 6px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="6">&nbsp;最近存款日期:</TD>
					<TD style="HEIGHT: 6px" bgColor="#ffffff" height="6">&nbsp;
						<asp:label id="Label9_LastSaveDate" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 6px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="6">&nbsp;最近提款日期:</TD>
					<TD style="HEIGHT: 6px" bgColor="#ffffff" colSpan="2" height="6">&nbsp;
						<asp:label id="Label10_Drawing" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 14px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="14"><FONT face="宋体">&nbsp;最后登陆IP地址:</FONT></TD>
					<TD style="HEIGHT: 14px" bgColor="#ffffff" height="14"><FONT face="宋体">&nbsp;
							<asp:label id="Label17_Flogin_ip" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 14px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="14"><FONT face="宋体">&nbsp;最后修改时间:</FONT></TD>
					<TD style="HEIGHT: 14px" bgColor="#ffffff" colSpan="2" height="14"><FONT face="宋体">&nbsp;</FONT>
						<asp:label id="Label6_LastModify" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 14px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="14"><FONT face="宋体">&nbsp;产品属性:</FONT></TD>
					<TD style="HEIGHT: 14px" bgColor="#ffffff" height="14"><FONT face="宋体">&nbsp;</FONT>
						<asp:label id="Label18_Attid" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="10">&nbsp;备注:</TD>
					<TD bgColor="#ffffff" colSpan="4" height="10">&nbsp;
						<asp:label id="Label11_Remark" runat="server"></asp:label></TD>
				</TR>
			</TABLE>
			<br>
			<TABLE cellSpacing="1" cellPadding="0" width="80%" align="center" border="0">
				<TR>
					<TD background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7" colSpan="3"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
						<asp:label id="Label3" runat="server" ForeColor="red">用户资金流水</asp:label>
                        &nbsp;<asp:LinkButton runat="server" ID="lbtnQueryOld" OnClick="lbtnQueryOld_Click">新版</asp:LinkButton>
					</TD>
				</TR>
				<tr>
					<td><iframe id=iframe0 name=iframe0 marginWidth=0 marginHeight=230 src="<%=iFramePath%>" frameBorder=0 width="100%" height=50></iframe>
					</td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
