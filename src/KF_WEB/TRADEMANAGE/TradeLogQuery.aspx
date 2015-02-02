<%@ Page language="c#" Codebehind="TradeLogQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.TradeLogQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>TradeLogQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT>
			<br>
			<table cellSpacing="1" cellPadding="0" width="95%" align="center" bgColor="#666666" border="0">
				<tr bgColor="#e4e5f7">
					<td vAlign="middle" colSpan="2" height="20">
						<table height="90%" cellSpacing="0" cellPadding="1" width="100%" border="0">
							<tr>
								<td width="80%" background="../IMAGES/Page/bg_bl.gif" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										交易查询</font>
									<div align="right"><FONT face="Tahoma" color="#ff0000"></FONT></div>
								</td>
								<td width="20%" background="../IMAGES/Page/bg_bl.gif">操作员代码: <span class="style3">
										<asp:label id="Label_uid" runat="server">Label</asp:label></span></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td>
						<table height="100%" cellSpacing="0" cellPadding="1" width="100%" border="0">
							<tr>
								<td style="HEIGHT: 37px" width="78%">
									<P align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
										输入：&nbsp;
										<asp:textbox id="TextBox1_ListID" runat="server" Width="200px" BorderWidth="1px" BorderStyle="Solid"></asp:textbox><asp:radiobutton id="rbList" runat="server" Text="交易单号" Checked="True" GroupName="id" AutoPostBack="True"
											ToolTip="根据交易单号查询"></asp:radiobutton>&nbsp;<asp:radiobutton id="rbBank" runat="server" Text="银行返回定单号" GroupName="id" AutoPostBack="True" ToolTip="根据银行订单号查选"></asp:radiobutton>
										<asp:regularexpressionvalidator id="rfvNum" runat="server" Display="Dynamic" ValidationExpression="^[0-9 ][0-9 ][0-9 ][0-9 ]+"
											ControlToValidate="TextBox1_ListID" ErrorMessage="RegularExpressionValidator" Enabled="False">不为数字或单号过短</asp:regularexpressionvalidator><br>
										&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="lbEnter" runat="server" Width="38px" Visible="False">日期：</asp:label><asp:textbox id="txDate" runat="server" Width="77px" BorderStyle="Groove" Visible="False"></asp:textbox><asp:linkbutton id="lkTime" runat="server" Visible="False" ForeColor="Red" CausesValidation="False" onclick="lkTime_Click">*点击插入时间</asp:linkbutton><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="TextBox1_ListID"
											ErrorMessage="RequiredFieldValidator">单号不为空</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="RegularExpressionValidator2" runat="server" Display="Dynamic" ValidationExpression="^[0-9][0-9]{3}-[0-9]{2}-[0-9]{2}$"
											ControlToValidate="txDate" ErrorMessage="RegularExpressionValidator">正确格式：2005-07-01</asp:regularexpressionvalidator><asp:requiredfieldvalidator id="rfvDate" runat="server" Display="Dynamic" ControlToValidate="txDate" ErrorMessage="RequiredFieldValidator">日期不能为空</asp:requiredfieldvalidator></P>
								</td>
								<TD style="HEIGHT: 40px" width="3%">&nbsp;</TD>
							</tr>
						</table>
					</td>
					<td width="25%">
						<div align="center"><asp:button id="btQuery" runat="server" Width="66px" BorderStyle="Groove" Text="查询" Height="23px" onclick="btQuery_Click"></asp:button>&nbsp;</div>
					</td>
				</tr>
			</table>
			<div align="center"><asp:calendar id="Calendar1" runat="server" Visible="False" onselectionchanged="Calendar1_SelectionChanged"></asp:calendar><br>
				<TABLE height="362" cellSpacing="0" cellPadding="0" width="95%" align="center" border="0">
					<TR>
						<TD bgColor="#666666">
							<TABLE height="391" cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
								<TR bgColor="#e4e5f7">
									<TD style="HEIGHT: 15px" colSpan="5" height="18"><STRONG></STRONG>
										<table cellSpacing="0" cellPadding="1" width="100%" border="0">
											<tr>
												<td background="../IMAGES/Page/bg_bl.gif" height="20"><strong><font color="#ff0000"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
														</font></strong><font color="#ff0000">交易单资料</font>
												</td>
												<td width="10%" background="../IMAGES/Page/bg_bl.gif"><!--<div align="center"><A href="../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=<%=LB_Flistid.Text.Trim()%>" ><font color="red">分析</font></A>|</div> --><FONT face="宋体">
														<asp:HyperLink id="hlOrder" runat="server">订单详细信息</asp:HyperLink></FONT></td>
												<td width="5%" background="../IMAGES/Page/bg_bl.gif">
													<div align="left"><A href="tradeLogQuery.aspx?id=<%=LB_Flistid.Text.Trim()%>" target=_blank ><font color="red"><font color="red">全屏</font></font></A></div>
												</td>
											</tr>
										</table>
										<DIV align="center"><FONT face="宋体"></FONT></DIV>
									</TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px"  bgColor="#eeeeee">&nbsp; 交易单号:</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px"  bgColor="#ffffff">&nbsp;<span class="style4">
											<asp:label id="LB_Flistid" runat="server"></asp:label></span></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px"  bgColor="#eeeeee">&nbsp;交易状态:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colspan="2">&nbsp;
										<asp:Label ID="lblTradeState" Runat="server"></asp:Label></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="宋体">&nbsp;是否调帐标志:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff"><FONT face="宋体">&nbsp; </FONT>
										<asp:label id="lbAdjustFlag" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">
										<P><FONT face="宋体">&nbsp;交易类型:</FONT></P>
									</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colspan="2"><FONT face="宋体">&nbsp;
											<asp:label id="lbTradeType" runat="server"></asp:label></FONT></TD>
								</TR>
                                <TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="宋体">&nbsp;支付绑定序列号:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff"><FONT face="宋体">&nbsp; </FONT>
										<asp:label id="lbPayBindSeqId" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">
										<P><FONT face="宋体">&nbsp;关闭原因:</FONT></P>
									</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colspan="2"><FONT face="宋体">&nbsp;
											<asp:label id="lbCloseReason" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="宋体">&nbsp;给银行订单号:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fbank_listid" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="宋体">&nbsp;银行返回订单号:</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colspan="2"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fbank_backid" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="宋体">&nbsp;机构代码名称(发起):</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fspid" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="宋体">&nbsp;交易单的状态:</FONT></TD>
									<TD style="HEIGHT: 18px" width="62px" bgColor="#ffffff"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Flstate" runat="server" ForeColor="Red"></asp:label></FONT></TD>
									<TD style="HEIGHT: 18px" width="124px" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7">&nbsp;
										<asp:linkbutton id="LinkButton3_action" runat="server" onclick="LinkButton3_action_Click">冻结</asp:linkbutton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:linkbutton id="LinkButton_synchro" runat="server" onclick="LinkButton_synchro_Click">同步订单状态</asp:linkbutton></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="宋体">&nbsp;买家内部帐号:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fbuy_uid" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="宋体">&nbsp;卖家内部帐号:</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fsale_uid" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;买家账户号码:</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff">&nbsp;
										<asp:label id="LB_Fbuyid" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;卖家账户账号:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2">&nbsp;
										<asp:label id="LB_Fsaleid" runat="server"></asp:label></TD>
								</TR>
                                <TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff">&nbsp;
										<asp:label id="Label1" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;卖家财付通账号:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2">&nbsp;
										<asp:label id="LB_FsaleidCFT" runat="server"></asp:label></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;买家真实名称:</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">&nbsp;
										<asp:label id="LB_Fbuy_name" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;卖家真实名称:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" >&nbsp;
										<asp:label id="LB_Fsale_name" runat="server"></asp:label></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">
										<P>&nbsp;买家开户银行类型:</P>
									</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff">&nbsp;
										<asp:label id="LB_Fbuy_bank_type" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;卖家开户银行类型:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fsale_bank_type" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;
									</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">&nbsp;
										<asp:label id="LB_Fbuy_bankid" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18">&nbsp;代金券ID:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;
										<asp:label id="LB_Fsale_bankid" runat="server"></asp:label></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18">&nbsp;币种代码:</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">&nbsp;
										<asp:label id="LB_Fcurtype" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18">&nbsp;支付类型:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="宋体">
											<asp:label id="LB_Fpay_type" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;产品的价格:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fprice" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;物流费用:</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fcarriage" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;实际支付费用:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fpaynum" runat="server"></asp:label></FONT></TD>
									<TD  style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
										<P><FONT face="宋体">&nbsp;总支付费用:</FONT></P>
									</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Ffact" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;交易(服务)手续费:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fprocedure" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;服务费率:</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fservice" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;现金支付金额:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fcash" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;订单编码:</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fcoding" runat="server"></asp:label>
											<asp:HyperLink id="HyperLink1" runat="server" Target="_blank"></asp:HyperLink></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;订单创建时间(C2C):</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
											<asp:label id="LB_FCreate_time_c2c" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
										<P><FONT face="宋体">&nbsp;订单创建时间(本地):</FONT></P>
									</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fcreate_time" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;买家付款时间(Bank):</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fbargain_time" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18">
										<P><FONT face="宋体">&nbsp;买家付款时间(本地):</FONT></P>
									</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fpay_time" runat="server"></asp:label>&nbsp;</FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;可打款给卖家时间(C2C):</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Freceive_time_c2c" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;可打款卖家时间(本地):</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Freceive_time" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;最后修改交易单的IP:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fip" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;最后修改时间(本地)</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fmodify_time" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD sstyle="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="宋体">&nbsp;备注:</FONT></TD>
									<TD bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
											<asp:label id="LB_Fexplain" runat="server"></asp:label></FONT></TD>
                                    <TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
                                            <FONT face="宋体">&nbsp;</FONT>
										</TD>
										<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">
											<FONT face="宋体">&nbsp;</FONT>
										</TD>
								</TR>
                                <TR>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
                                            <FONT face="宋体">&nbsp;渠道编号：</FONT>
										</TD>
										<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">
											<FONT face="宋体">&nbsp;<asp:label id="Fchannel_idName" runat="server"></asp:label></FONT>
										</TD>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
                                            <FONT face="宋体">&nbsp;交易说明：</FONT>
										</TD>
										<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">
											<FONT face="宋体">&nbsp;<asp:label id="Fmemo" runat="server"></asp:label></FONT>
										</TD>
									</TR>
                                <TR>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
                                            <FONT face="宋体">&nbsp;退款类型：</FONT>
										</TD>
										<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">
											<FONT face="宋体">&nbsp;<asp:label id="Frefund_typeName" runat="server"></asp:label></FONT>
										</TD>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
                                            <FONT face="宋体">&nbsp;退买家金额：</FONT>
										</TD>
										<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">
											<FONT face="宋体">&nbsp;<asp:label id="FpaybuyName" runat="server"></asp:label></FONT>
										</TD>
									</TR>
									<TR>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="宋体">&nbsp;退卖家金额：</FONT>
										</TD>
										<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">
											<FONT face="宋体">&nbsp;<asp:label id="FpaysaleName" runat="server"></asp:label></FONT>
										</TD>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="宋体">&nbsp;请求退款时间：</FONT>
										</TD>
										<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">
											<FONT face="宋体">&nbsp;<asp:label id="Freq_refund_time" runat="server"></asp:label></FONT>
										</TD>
									</TR>
                                    <TR>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="宋体">&nbsp;退款时间：</FONT>
										</TD>
										<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">
											<FONT face="宋体">&nbsp;<asp:label id="Fok_time" runat="server"></asp:label></FONT>
										</TD>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="宋体">&nbsp;退款时间(帐务)：</FONT>
										</TD>
										<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">
											<FONT face="宋体">&nbsp;<asp:label id="Fok_time_acc" runat="server"></asp:label></FONT>
										</TD>
									</TR>
                                    <TR>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="宋体">&nbsp;申诉标志：</FONT>
										</TD>
										<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">
											<FONT face="宋体">&nbsp;<asp:label id="Fappeal_signName" runat="server"></asp:label></FONT>
										</TD>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="宋体">&nbsp;中介标志：</FONT>
										</TD>
										<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">
											<FONT face="宋体">&nbsp;<asp:label id="Fmedi_signName" runat="server"></asp:label></FONT>
										</TD>
									</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
				<br>
				<TABLE height="35" cellSpacing="0" cellPadding="0" width="95%" align="center" border="0">
					<TR>
						<TD bgColor="#666666">
							<TABLE height="35" cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
								<TR bgColor="#e4e5f7">
									<TD bgColor="#e4e5f7" height="20">
										<div align="left">
											<table height="20" cellSpacing="0" cellPadding="1" width="100%" background="../IMAGES/Page/bg_bl.gif"
												border="0">
												<tr>
													<td style="WIDTH: 216px" width="216"><FONT color="#ff0000">&nbsp;<IMG height="16" src="../IMAGES/Page/post.gif" width="20">
															&nbsp;充值记录 </FONT><span class="style2">
															<asp:linkbutton id="LKBT_GatheringLog" runat="server" Visible="False" ForeColor="Black">
																<span class="style4">收款记录</span></asp:linkbutton>&nbsp;&nbsp;&nbsp; | </span>
													</td>
													<td width="55%">&nbsp;
														<asp:label id="Label1_listID" runat="server" Width="246px" Visible="False">Label</asp:label></td>
													<td width="5%">
														<div align="center"><FONT face="宋体"></FONT>&nbsp;</div>
													</td>
													<td width="5%">
														<div class="style2" align="center"><A href="tradeLogQuery.aspx?id=<%=LB_Flistid.Text.Trim()%>" target=_blank ><font color="red">全屏</font></A>|</div>
													</td>
													<td width="4%">
														<div class="style2" align="center"><A href="javascript:window.self.iframe1.location.reload()">刷新</A></div>
													</td>
													<td width="4%">
														<div align="center"><IMG height="18" src="../IMAGES/Page/down.gif"></div>
													</td>
												</tr>
											</table>
										</div>
									</TD>
								</TR>
								<TR>
									<TD bgColor="#ffffff" height="12"><iframe 

            name=iframe1 marginWidth=0 marginHeight=0 

            src="<%=iFramePath_Gathering%>" frameBorder=0 width="100%" 

            ></iframe>
									</TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
				<br>
				<TABLE height="35" cellSpacing="0" cellPadding="0" width="95%" align="center" border="0">
					<TR>
						<TD bgColor="#666666">
							<TABLE height="35" cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
								<TR bgColor="#e4e5f7">
									<TD height="20">
										<DIV align="center"></DIV>
										<table height="20" cellSpacing="0" cellPadding="1" width="100%" background="../IMAGES/Page/bg_bl.gif"
											border="0">
											<tr>
												<td><FONT color="#ff0000">&nbsp;<IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp; 
														资金流水 </FONT><span class="style2">
														<asp:linkbutton id="LKBT_bankrollLog" runat="server" Visible="False" ForeColor="Black">
															<span class="style4">资金流水</span></asp:linkbutton></span>&nbsp;<span class="style2">&nbsp; 
														| </span>
												</td>
												<td>&nbsp;</td>
												<td width="5%">
													<div align="center"><FONT face="宋体"></FONT>&nbsp;</div>
												</td>
												<td width="5%">
													<div class="style2" align="center"><A href="tradeLogQuery.aspx?id=<%=LB_Flistid.Text.Trim()%>" target=_blank ><font color="red"><font color="red">全屏</font></font></A>|</div>
												</td>
												<td style="WIDTH: 25px" width="26">
													<div class="style2" align="center"><A href="javascript:window.self.iframe3.location.reload()">刷新</A></div>
												</td>
												<td width="4%">
													<div align="center"><IMG height="18" src="../IMAGES/Page/down.gif"></div>
												</td>
											</tr>
										</table>
									</TD>
								</TR>
								<TR>
									<TD bgColor="#ffffff" height="12"><iframe 

            name=iframe3 marginWidth=0 marginHeight=0 

            src="<%=iFramePath_bankrollLog%>" frameBorder=0 width="100%" 

            ></iframe>
									</TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
				<br>
				<TABLE height="35" cellSpacing="0" cellPadding="0" width="95%" align="center" border="0">
					<TR>
						<TD bgColor="#666666">
							<TABLE height="35" cellSpacing="1" cellPadding="0" width="100%" align="right" border="0">
								<TR bgColor="#e4e5f7">
									<TD height="20">
										<table height="20" cellSpacing="0" cellPadding="1" width="100%" background="../IMAGES/Page/bg_bl.gif"
											border="0">
											<tr>
												<td><FONT color="#ff0000">&nbsp;<IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp; 
														交易流水
														<asp:linkbutton id="LKBT_TradeLog" runat="server" Visible="False" ForeColor="Red">交易记录流水</asp:linkbutton></FONT><FONT color="#000000">&nbsp;|
													</FONT>
												</td>
												<td>&nbsp;</td>
												<td style="WIDTH: 42px" width="42">
													<div align="center"><FONT face="宋体"></FONT>&nbsp;</div>
												</td>
												<td width="5%">
													<div class="style2" align="center"><A href="tradeLogQuery.aspx?id=<%=LB_Flistid.Text.Trim()%>" target=_blank ><font color="red">全屏</font></A>|</div>
												</td>
												<td width="4%">
													<div class="style2" align="center"><A href="javascript:window.self.iframe4.location.reload()">刷新</A></div>
												</td>
												<td width="4%">
													<div align="center"><IMG height="18" src="../IMAGES/Page/down.gif"></div>
												</td>
											</tr>
										</table>
									</TD>
								</TR>
								<TR>
									<TD bgColor="#ffffff" height="12"><iframe 

            name=iframe4 marginWidth=0 marginHeight=0 

            src="<%=iFramePath_TradeLog%>" frameBorder=0 width="100%" 

            ></iframe>
									</TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
				<br>
				<br>
				<!-- 不需要的功能暂时隐藏--></div>
		</form>
	</body>
</HTML>
