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
			<FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT>
			<br>
			<table cellSpacing="1" cellPadding="0" width="95%" align="center" bgColor="#666666" border="0">
				<tr bgColor="#e4e5f7">
					<td vAlign="middle" colSpan="2" height="20">
						<table height="90%" cellSpacing="0" cellPadding="1" width="100%" border="0">
							<tr>
								<td width="80%" background="../IMAGES/Page/bg_bl.gif" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										���ײ�ѯ</font>
									<div align="right"><FONT face="Tahoma" color="#ff0000"></FONT></div>
								</td>
								<td width="20%" background="../IMAGES/Page/bg_bl.gif">����Ա����: <span class="style3">
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
										���룺&nbsp;
										<asp:textbox id="TextBox1_ListID" runat="server" Width="200px" BorderWidth="1px" BorderStyle="Solid"></asp:textbox><asp:radiobutton id="rbList" runat="server" Text="���׵���" Checked="True" GroupName="id" AutoPostBack="True"
											ToolTip="���ݽ��׵��Ų�ѯ"></asp:radiobutton>&nbsp;<asp:radiobutton id="rbBank" runat="server" Text="���з��ض�����" GroupName="id" AutoPostBack="True" ToolTip="�������ж����Ų�ѡ"></asp:radiobutton>
										<asp:regularexpressionvalidator id="rfvNum" runat="server" Display="Dynamic" ValidationExpression="^[0-9 ][0-9 ][0-9 ][0-9 ]+"
											ControlToValidate="TextBox1_ListID" ErrorMessage="RegularExpressionValidator" Enabled="False">��Ϊ���ֻ򵥺Ź���</asp:regularexpressionvalidator><br>
										&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="lbEnter" runat="server" Width="38px" Visible="False">���ڣ�</asp:label><asp:textbox id="txDate" runat="server" Width="77px" BorderStyle="Groove" Visible="False"></asp:textbox><asp:linkbutton id="lkTime" runat="server" Visible="False" ForeColor="Red" CausesValidation="False" onclick="lkTime_Click">*�������ʱ��</asp:linkbutton><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="TextBox1_ListID"
											ErrorMessage="RequiredFieldValidator">���Ų�Ϊ��</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="RegularExpressionValidator2" runat="server" Display="Dynamic" ValidationExpression="^[0-9][0-9]{3}-[0-9]{2}-[0-9]{2}$"
											ControlToValidate="txDate" ErrorMessage="RegularExpressionValidator">��ȷ��ʽ��2005-07-01</asp:regularexpressionvalidator><asp:requiredfieldvalidator id="rfvDate" runat="server" Display="Dynamic" ControlToValidate="txDate" ErrorMessage="RequiredFieldValidator">���ڲ���Ϊ��</asp:requiredfieldvalidator></P>
								</td>
								<TD style="HEIGHT: 40px" width="3%">&nbsp;</TD>
							</tr>
						</table>
					</td>
					<td width="25%">
						<div align="center"><asp:button id="btQuery" runat="server" Width="66px" BorderStyle="Groove" Text="��ѯ" Height="23px" onclick="btQuery_Click"></asp:button>&nbsp;</div>
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
														</font></strong><font color="#ff0000">���׵�����</font>
												</td>
												<td width="10%" background="../IMAGES/Page/bg_bl.gif"><!--<div align="center"><A href="../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=<%=LB_Flistid.Text.Trim()%>" ><font color="red">����</font></A>|</div> --><FONT face="����">
														<asp:HyperLink id="hlOrder" runat="server">������ϸ��Ϣ</asp:HyperLink></FONT></td>
												<td width="5%" background="../IMAGES/Page/bg_bl.gif">
													<div align="left"><A href="tradeLogQuery.aspx?id=<%=LB_Flistid.Text.Trim()%>" target=_blank ><font color="red"><font color="red">ȫ��</font></font></A></div>
												</td>
											</tr>
										</table>
										<DIV align="center"><FONT face="����"></FONT></DIV>
									</TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px"  bgColor="#eeeeee">&nbsp; ���׵���:</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px"  bgColor="#ffffff">&nbsp;<span class="style4">
											<asp:label id="LB_Flistid" runat="server"></asp:label></span></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px"  bgColor="#eeeeee">&nbsp;����״̬:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colspan="2">&nbsp;
										<asp:Label ID="lblTradeState" Runat="server"></asp:Label></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="����">&nbsp;�Ƿ���ʱ�־:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff"><FONT face="����">&nbsp; </FONT>
										<asp:label id="lbAdjustFlag" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">
										<P><FONT face="����">&nbsp;��������:</FONT></P>
									</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colspan="2"><FONT face="����">&nbsp;
											<asp:label id="lbTradeType" runat="server"></asp:label></FONT></TD>
								</TR>
                                <TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="����">&nbsp;֧�������к�:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff"><FONT face="����">&nbsp; </FONT>
										<asp:label id="lbPayBindSeqId" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">
										<P><FONT face="����">&nbsp;�ر�ԭ��:</FONT></P>
									</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colspan="2"><FONT face="����">&nbsp;
											<asp:label id="lbCloseReason" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="����">&nbsp;�����ж�����:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff"><FONT face="����">&nbsp;
											<asp:label id="LB_Fbank_listid" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="����">&nbsp;���з��ض�����:</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colspan="2"><FONT face="����">&nbsp;
											<asp:label id="LB_Fbank_backid" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="����">&nbsp;������������(����):</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff"><FONT face="����">&nbsp;
											<asp:label id="LB_Fspid" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="����">&nbsp;���׵���״̬:</FONT></TD>
									<TD style="HEIGHT: 18px" width="62px" bgColor="#ffffff"><FONT face="����">&nbsp;
											<asp:label id="LB_Flstate" runat="server" ForeColor="Red"></asp:label></FONT></TD>
									<TD style="HEIGHT: 18px" width="124px" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7">&nbsp;
										<asp:linkbutton id="LinkButton3_action" runat="server" onclick="LinkButton3_action_Click">����</asp:linkbutton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:linkbutton id="LinkButton_synchro" runat="server" onclick="LinkButton_synchro_Click">ͬ������״̬</asp:linkbutton></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="����">&nbsp;����ڲ��ʺ�:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff"><FONT face="����">&nbsp;
											<asp:label id="LB_Fbuy_uid" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"><FONT face="����">&nbsp;�����ڲ��ʺ�:</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2"><FONT face="����">&nbsp;
											<asp:label id="LB_Fsale_uid" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;����˻�����:</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff">&nbsp;
										<asp:label id="LB_Fbuyid" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;�����˻��˺�:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2">&nbsp;
										<asp:label id="LB_Fsaleid" runat="server"></asp:label></TD>
								</TR>
                                <TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff">&nbsp;
										<asp:label id="Label1" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;���ҲƸ�ͨ�˺�:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2">&nbsp;
										<asp:label id="LB_FsaleidCFT" runat="server"></asp:label></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;�����ʵ����:</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">&nbsp;
										<asp:label id="LB_Fbuy_name" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;������ʵ����:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" >&nbsp;
										<asp:label id="LB_Fsale_name" runat="server"></asp:label></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">
										<P>&nbsp;��ҿ�����������:</P>
									</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff">&nbsp;
										<asp:label id="LB_Fbuy_bank_type" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;���ҿ�����������:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2"><FONT face="����">&nbsp;
											<asp:label id="LB_Fsale_bank_type" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;
									</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">&nbsp;
										<asp:label id="LB_Fbuy_bankid" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18">&nbsp;����ȯID:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;
										<asp:label id="LB_Fsale_bankid" runat="server"></asp:label></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18">&nbsp;���ִ���:</TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">&nbsp;
										<asp:label id="LB_Fcurtype" runat="server"></asp:label></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18">&nbsp;֧������:</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">&nbsp;<FONT face="����">
											<asp:label id="LB_Fpay_type" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;��Ʒ�ļ۸�:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
											<asp:label id="LB_Fprice" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��������:</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="����">&nbsp;
											<asp:label id="LB_Fcarriage" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;ʵ��֧������:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
											<asp:label id="LB_Fpaynum" runat="server"></asp:label></FONT></TD>
									<TD  style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
										<P><FONT face="����">&nbsp;��֧������:</FONT></P>
									</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="����">&nbsp;
											<asp:label id="LB_Ffact" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;����(����)������:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
											<asp:label id="LB_Fprocedure" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;�������:</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="����">&nbsp;
											<asp:label id="LB_Fservice" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;�ֽ�֧�����:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
											<asp:label id="LB_Fcash" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;��������:</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="����">&nbsp;
											<asp:label id="LB_Fcoding" runat="server"></asp:label>
											<asp:HyperLink id="HyperLink1" runat="server" Target="_blank"></asp:HyperLink></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;��������ʱ��(C2C):</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
											<asp:label id="LB_FCreate_time_c2c" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
										<P><FONT face="����">&nbsp;��������ʱ��(����):</FONT></P>
									</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="����">&nbsp;
											<asp:label id="LB_Fcreate_time" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;��Ҹ���ʱ��(Bank):</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
											<asp:label id="LB_Fbargain_time" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18">
										<P><FONT face="����">&nbsp;��Ҹ���ʱ��(����):</FONT></P>
									</TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="����">&nbsp;
											<asp:label id="LB_Fpay_time" runat="server"></asp:label>&nbsp;</FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;�ɴ�������ʱ��(C2C):</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
											<asp:label id="LB_Freceive_time_c2c" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;�ɴ������ʱ��(����):</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="����">&nbsp;
											<asp:label id="LB_Freceive_time" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;����޸Ľ��׵���IP:</FONT></TD>
									<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
											<asp:label id="LB_Fip" runat="server"></asp:label></FONT></TD>
									<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;����޸�ʱ��(����)</FONT></TD>
									<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17"><FONT face="����">&nbsp;
											<asp:label id="LB_Fmodify_time" runat="server"></asp:label></FONT></TD>
								</TR>
								<TR>
									<TD sstyle="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
										height="18"><FONT face="����">&nbsp;��ע:</FONT></TD>
									<TD bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
											<asp:label id="LB_Fexplain" runat="server"></asp:label></FONT></TD>
                                    <TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
                                            <FONT face="����">&nbsp;</FONT>
										</TD>
										<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">
											<FONT face="����">&nbsp;</FONT>
										</TD>
								</TR>
                                <TR>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
                                            <FONT face="����">&nbsp;������ţ�</FONT>
										</TD>
										<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">
											<FONT face="����">&nbsp;<asp:label id="Fchannel_idName" runat="server"></asp:label></FONT>
										</TD>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
                                            <FONT face="����">&nbsp;����˵����</FONT>
										</TD>
										<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">
											<FONT face="����">&nbsp;<asp:label id="Fmemo" runat="server"></asp:label></FONT>
										</TD>
									</TR>
                                <TR>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
                                            <FONT face="����">&nbsp;�˿����ͣ�</FONT>
										</TD>
										<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">
											<FONT face="����">&nbsp;<asp:label id="Frefund_typeName" runat="server"></asp:label></FONT>
										</TD>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
                                            <FONT face="����">&nbsp;����ҽ�</FONT>
										</TD>
										<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">
											<FONT face="����">&nbsp;<asp:label id="FpaybuyName" runat="server"></asp:label></FONT>
										</TD>
									</TR>
									<TR>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="����">&nbsp;�����ҽ�</FONT>
										</TD>
										<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">
											<FONT face="����">&nbsp;<asp:label id="FpaysaleName" runat="server"></asp:label></FONT>
										</TD>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="����">&nbsp;�����˿�ʱ�䣺</FONT>
										</TD>
										<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">
											<FONT face="����">&nbsp;<asp:label id="Freq_refund_time" runat="server"></asp:label></FONT>
										</TD>
									</TR>
                                    <TR>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="����">&nbsp;�˿�ʱ�䣺</FONT>
										</TD>
										<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">
											<FONT face="����">&nbsp;<asp:label id="Fok_time" runat="server"></asp:label></FONT>
										</TD>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="����">&nbsp;�˿�ʱ��(����)��</FONT>
										</TD>
										<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">
											<FONT face="����">&nbsp;<asp:label id="Fok_time_acc" runat="server"></asp:label></FONT>
										</TD>
									</TR>
                                    <TR>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="����">&nbsp;���߱�־��</FONT>
										</TD>
										<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18">
											<FONT face="����">&nbsp;<asp:label id="Fappeal_signName" runat="server"></asp:label></FONT>
										</TD>
										<TD style="WIDTH: 125px; HEIGHT: 18px" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" height="18">
											<FONT face="����">&nbsp;�н��־��</FONT>
										</TD>
										<TD style="WIDTH: 156px; HEIGHT: 18px" bgColor="#ffffff" colSpan="2" height="17">
											<FONT face="����">&nbsp;<asp:label id="Fmedi_signName" runat="server"></asp:label></FONT>
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
															&nbsp;��ֵ��¼ </FONT><span class="style2">
															<asp:linkbutton id="LKBT_GatheringLog" runat="server" Visible="False" ForeColor="Black">
																<span class="style4">�տ��¼</span></asp:linkbutton>&nbsp;&nbsp;&nbsp; | </span>
													</td>
													<td width="55%">&nbsp;
														<asp:label id="Label1_listID" runat="server" Width="246px" Visible="False">Label</asp:label></td>
													<td width="5%">
														<div align="center"><FONT face="����"></FONT>&nbsp;</div>
													</td>
													<td width="5%">
														<div class="style2" align="center"><A href="tradeLogQuery.aspx?id=<%=LB_Flistid.Text.Trim()%>" target=_blank ><font color="red">ȫ��</font></A>|</div>
													</td>
													<td width="4%">
														<div class="style2" align="center"><A href="javascript:window.self.iframe1.location.reload()">ˢ��</A></div>
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
														�ʽ���ˮ </FONT><span class="style2">
														<asp:linkbutton id="LKBT_bankrollLog" runat="server" Visible="False" ForeColor="Black">
															<span class="style4">�ʽ���ˮ</span></asp:linkbutton></span>&nbsp;<span class="style2">&nbsp; 
														| </span>
												</td>
												<td>&nbsp;</td>
												<td width="5%">
													<div align="center"><FONT face="����"></FONT>&nbsp;</div>
												</td>
												<td width="5%">
													<div class="style2" align="center"><A href="tradeLogQuery.aspx?id=<%=LB_Flistid.Text.Trim()%>" target=_blank ><font color="red"><font color="red">ȫ��</font></font></A>|</div>
												</td>
												<td style="WIDTH: 25px" width="26">
													<div class="style2" align="center"><A href="javascript:window.self.iframe3.location.reload()">ˢ��</A></div>
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
														������ˮ
														<asp:linkbutton id="LKBT_TradeLog" runat="server" Visible="False" ForeColor="Red">���׼�¼��ˮ</asp:linkbutton></FONT><FONT color="#000000">&nbsp;|
													</FONT>
												</td>
												<td>&nbsp;</td>
												<td style="WIDTH: 42px" width="42">
													<div align="center"><FONT face="����"></FONT>&nbsp;</div>
												</td>
												<td width="5%">
													<div class="style2" align="center"><A href="tradeLogQuery.aspx?id=<%=LB_Flistid.Text.Trim()%>" target=_blank ><font color="red">ȫ��</font></A>|</div>
												</td>
												<td width="4%">
													<div class="style2" align="center"><A href="javascript:window.self.iframe4.location.reload()">ˢ��</A></div>
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
				<!-- ����Ҫ�Ĺ�����ʱ����--></div>
		</form>
	</body>
</HTML>
