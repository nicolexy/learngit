<%@ Page language="c#" Codebehind="SettleInfo.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.SettleInfo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>batPayReturn</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server">
			&nbsp;
			<TABLE id="Table2" style="Z-INDEX: 101; LEFT: 2%; POSITION: absolute; TOP: 2%" cellSpacing="1"
				cellPadding="1" width="90%" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;<asp:label id="lbTitle" runat="server">���˶�����Ϣ</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:label id="Label2" runat="server">����Ա���룺</asp:label><asp:label id="labUid" runat="server" Width="64px"></asp:label><SPAN class="style3"></SPAN></STRONG></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD>
						<TABLE style="WIDTH: 1080px; HEIGHT: 38px" cellSpacing="1" cellPadding="1" width="1080"
							border="1">
							<TR>
								<TD style="WIDTH: 489px">�����붩����:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:textbox id="txtListid" runat="server" Width="360px"></asp:textbox></TD>
								<TD><FONT face="����">&nbsp;<asp:button id="Button_qry" runat="server" Text="��ѯ" onclick="Button_qry_Click"></asp:button></FONT></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR bgColor="#b5c7de" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;<asp:label id="Label8" runat="server">���˶�����Ϣ</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								</SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">
						<TABLE id="Table1" style="WIDTH: 100%" cellSpacing="1" cellPadding="1" width="100%" border="0"
							runat="server">
							<TBODY>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label10" runat="server">�����ţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px" align="center" width="275"><P align="left"><asp:label id="Flistid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label1" runat="server">�̻������ţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="Fcoding" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label14" runat="server">PNR��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="Fpnr" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label12" runat="server">��ϵ�ˣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="Fcontact" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 18px" align="center">
										<P align="right"><asp:label id="Label5" runat="server">�����̣�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 18px" align="center">
										<P align="left"><asp:label id="Fpri_spid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="right"><asp:label id="Label15" runat="server">���̣�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="left"><asp:label id="Fflight_info" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label7" runat="server">��ϵ�绰��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px" align="center">
										<P align="left"><asp:label id="Fphone" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label23" runat="server">ҵ�����</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fbus_type" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label25" runat="server">ҵ�������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fbus_args" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label3" runat="server">����������Ϣ��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fbus_desc" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label6" runat="server">���˻ص�URL��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fsp_bankurl" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label9" runat="server">��Ʊ������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fticket_num" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label13" runat="server">�ۼ���Ʊ������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Frefund_num" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label18" runat="server">�ۼƶ���������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Ffreeze_num" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label22" runat="server">֧����</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Ftotal_fee" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label26" runat="server">���˽�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fsttl_fee" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label28" runat="server">���˻��˽�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Frefund_fee" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label30" runat="server">�˿��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Frf_fee" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 14px" align="center">
										<P align="right"><asp:label id="Label32" runat="server">�������˽�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 14px" align="center">
										<P align="left"><asp:label id="Fadjustin_fee" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 14px" align="center">
										<P align="right"><asp:label id="Label34" runat="server">���˳��˽�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 14px" align="center">
										<P align="left"><asp:label id="Fadjustout_fee" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label11" runat="server">�����</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Ffreeze_fee" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label4" runat="server">���������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 525px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Forder_fee" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label19" runat="server">�������ͣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fop_type" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label24" runat="server">�������ţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fop_listid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label29" runat="server">���˻��˲�����</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="Fbus_refund_args" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label36" runat="server">�˿������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fb2c_refund_args" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label40" runat="server">���������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fbus_freeze_args" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label44" runat="server">����ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fcreate_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label46" runat="server">����޸�ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fmodify_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
                                <tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
                                <TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label16" runat="server">�ӵ�֧����</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="WIDTH: 275px; HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fsp_pay_amount" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label20" runat="server">�ӵ��˿��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fsp_refund_amount" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
							</TBODY>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
