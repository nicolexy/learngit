<%@ Page language="c#" Codebehind="ChildrenOrderFromQueryNew.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ChildrenOrderFromQueryNew" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ChildrenOrderFromQueryNew</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="1" cellPadding="1" width="900" align="center" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;子帐户订单查询(新)</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 83px" align="right"><asp:label id="Label2" runat="server">订单号</asp:label></TD>
					<td style="WIDTH: 150px" align="left"><asp:textbox id="tbFlistid" runat="server" Width="320px"></asp:textbox></td>
					<td style="WIDTH: 150px" align="left">账户类型：
						<asp:DropDownList Runat="server" id="dd_curType">
							<asp:ListItem Value="1">主帐户</asp:ListItem>
							<asp:ListItem Value="2">基金</asp:ListItem>
							<asp:ListItem Value="80" Selected="True">游戏</asp:ListItem>
							<asp:ListItem Value="81">返利积分</asp:ListItem>
							<asp:ListItem Value="82">直通车</asp:ListItem>
						</asp:DropDownList>
					</td>
					<TD align="center" colSpan="2"><asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button></TD>
				</TR>
			</table>
			<table cellSpacing="1" cellPadding="1" width="900" align="center" border="1">
				<TR>
					<TD style="WIDTH: 154px" width="154" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="24">&nbsp; 交易单号:</TD>
					<TD style="WIDTH: 129px; HEIGHT: 2px" width="243" bgColor="#ffffff" height="24">&nbsp;<span class="style4">
							<asp:label id="LB_Flistid" runat="server" ForeColor="Black" Width="194px"></asp:label></span></TD>
					<TD width="152" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="24">&nbsp;交易状态:</TD>
					<TD bgColor="#ffffff" colSpan="2" height="24">&nbsp;
						<asp:dropdownlist id="DropDownList2_tradeState" runat="server" ForeColor="Black">
							<asp:ListItem Value="1" Selected="True">支付中</asp:ListItem>
							<asp:ListItem Value="2">支付成功</asp:ListItem>
							<asp:ListItem Value="3">确认收货</asp:ListItem>
							<asp:ListItem Value="4">转入退款</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
							<asp:ListItem Value="13">未定义</asp:ListItem>
							<asp:ListItem Value="14">未定义</asp:ListItem>
							<asp:ListItem Value="99">作废</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<tr>
					<TD style="WIDTH: 154px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18"><FONT face="宋体">&nbsp;买家帐号:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp; </FONT>
						<asp:label id="lb_buyerId" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18">
						<P><FONT face="宋体">&nbsp;卖家帐号:</FONT></P>
					</TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_sellerID" runat="server"></asp:label></FONT></TD>
				</tr>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18"><FONT face="宋体">&nbsp;是否调帐标志:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp; </FONT>
						<asp:label id="lbAdjustFlag" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18">
						<P><FONT face="宋体">&nbsp;交易类型:</FONT></P>
					</TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lbTradeType" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18"><FONT face="宋体">&nbsp;给银行订单号:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="LB_Fbank_listid" runat="server"></asp:label></FONT></TD>
					<TD style="HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18"><FONT face="宋体">&nbsp;银行返回订单号:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="LB_Fbank_backid" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 17px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="17"><FONT face="宋体">&nbsp;机构代码名称(发起):</FONT></TD>
					<TD style="HEIGHT: 17px" bgColor="#ffffff" height="17"><FONT face="宋体">&nbsp;
							<asp:label id="LB_Fspid" runat="server"></asp:label></FONT></TD>
					<TD style="HEIGHT: 17px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="17"><FONT face="宋体">&nbsp;交易单的状态:</FONT></TD>
					<TD style="HEIGHT: 17px" width="132" bgColor="#ffffff" height="17"><FONT face="宋体">&nbsp;
							<asp:label id="LB_Flstate" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18">&nbsp;币种代码:</TD>
					<TD style="WIDTH: 129px; HEIGHT: 18px" bgColor="#ffffff" height="18">&nbsp;
						<asp:label id="LB_Fcurtype" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18">&nbsp;支付类型:</TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">
							<asp:label id="LB_Fpay_type" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18"><FONT face="宋体">&nbsp;最后修改交易单的IP:</FONT></TD>
					<TD style="WIDTH: 129px; HEIGHT: 15px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="LB_Fip" runat="server"></asp:label></FONT></TD>
					<TD style="HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18"><FONT face="宋体">&nbsp;最后修改时间(本地)</FONT></TD>
					<TD style="HEIGHT: 15px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
							<asp:label id="LB_Fmodify_time" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18"><FONT face="宋体">&nbsp;创建时间:</FONT></TD>
					<TD style="WIDTH: 129px; HEIGHT: 15px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="LB_Fcreate_time" runat="server"></asp:label></FONT></TD>
					<TD style="HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18"><FONT face="宋体">&nbsp;渠道编号</FONT></TD>
					<TD style="HEIGHT: 15px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
							<asp:label id="LB_Fchannel_id" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;中介ID:</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Fmediuid" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;中介余额:</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Fmedinum" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;手续费账户:</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Fchargeuid" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;手续费:</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Fchargenum" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;总金额:</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Ftotalnum" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;买家已支付总金额:</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Fbuyerpaytotal" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;买家已退款总金额:</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Fbuyerrefundtotal" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;卖家已收到总金额:</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Fsellerpaytotal" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;卖家已退款总金额:</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Fsellerrefundtotal" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;参与方数量:</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Frolenum" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;内部ID(0):</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Fuid0" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;预计金额(0):</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Fplanpaynum0" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;实际金额(0):</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Fpaynum0" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;退款金额(0):</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Frefund0" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;内部ID(1):</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Fuid1" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;预计金额(1):</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Fplanpaynum1" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;实际金额(1):</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Fpaynum1" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;退款金额(1):</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Frefund1" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;内部ID(2):</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Fuid2" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;预计金额(2):</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Fplanpaynum2" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;实际金额(2):</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Fpaynum2" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;退款金额(2):</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Frefund2" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;内部ID(3):</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Fuid3" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;预计金额(3):</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Fplanpaynum3" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;实际金额(3):</TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">&nbsp;<FONT face="宋体">&nbsp;</FONT>
						<asp:label id="LB_Fpaynum3" runat="server"></asp:label></TD>
					<TD style="HEIGHT: 19px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="19">&nbsp;退款金额(3):</TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">&nbsp;
							<asp:label id="LB_Frefund3" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18"><FONT face="宋体">&nbsp;后台备注:</FONT></TD>
					<TD bgColor="#ffffff" colSpan="4" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="LB_Fexplain" runat="server"></asp:label></FONT><FONT face="宋体">&nbsp;</FONT><FONT face="宋体">&nbsp;</FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18"><FONT face="宋体">&nbsp;备注:</FONT></TD>
					<TD bgColor="#ffffff" colSpan="4" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="LB_FMemo" runat="server"></asp:label></FONT><FONT face="宋体">&nbsp;</FONT><FONT face="宋体">&nbsp;</FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 154px; HEIGHT: 15px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="18"><FONT face="宋体">&nbsp;业务处理描述:</FONT></TD>
					<TD bgColor="#ffffff" colSpan="4" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="LB_Fcatch_desc" runat="server"></asp:label></FONT><FONT face="宋体">&nbsp;</FONT><FONT face="宋体">&nbsp;</FONT></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
