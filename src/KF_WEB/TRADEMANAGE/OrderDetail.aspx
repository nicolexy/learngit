<%@ Page language="c#" Codebehind="OrderDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.OrderDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>batPayReturn</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #ff0000; FONT-WEIGHT: bold }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			&nbsp;
			<TABLE id="Table2" style="Z-INDEX: 101; POSITION: absolute; TOP: 5%; LEFT: 5%" cellSpacing="1"
				cellPadding="1" width="90%" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;<asp:label id="lbTitle" runat="server">������ϸ��Ϣ</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:label id="Label2" runat="server">����Ա���룺</asp:label><asp:label id="labUid" runat="server" Width="64px"></asp:label><SPAN class="style3"></SPAN></STRONG></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">
						<TABLE id="Table1" style="WIDTH: 100%" cellSpacing="1" cellPadding="1" width="100%" border="0"
							runat="server">
							<TBODY>
								<TR>
									<TD align="center" colSpan="4"><asp:hyperlink id="hlTrade" runat="server">�鿴���׵���ϸ��Ϣ</asp:hyperlink></TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label10" runat="server">�տID��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="Flistid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label1" runat="server">�̻��������룺</asp:label><FONT face="����">:</FONT></P>
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
										<P align="right"><asp:label id="Label14" runat="server">�������룺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="Fspid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label12" runat="server">�����ж����ţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="Fbank_listid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label19" runat="server">���з��ض����ţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="Fbank_backid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center">
										<P align="right"><asp:label id="Label21" runat="server">֧�����ͣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="Fpay_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 18px" align="center">
										<P align="right"><asp:label id="Label5" runat="server">����ڲ�ID��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="left"><asp:label id="Fbuy_uid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="right"><asp:label id="Label15" runat="server">����ʺţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="left"><asp:label id="Fbuyid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label7" runat="server">�����ڲ�ID��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="Fsale_uid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center">
										<P align="right"><FONT face="����"></FONT><asp:label id="Label16" runat="server">�����ʺţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="Fsaleid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label23" runat="server">����״̬��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Ftrade_stateName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label25" runat="server">�˿�����״̬��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Frefund_stateName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label3" runat="server">����״̬��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="FlstateName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label6" runat="server">��Ʒ�۸�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="FpriceName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label9" runat="server">�������ã�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="FcarriageName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label13" runat="server">ʵ��֧�����ã�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="FpaynumName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label18" runat="server">��֧�����ã�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="FfactName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label22" runat="server">�����ѣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="FprocedureName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label26" runat="server">������ʣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fservice" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label28" runat="server">�ֽ�֧����</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="FcashName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label30" runat="server">����ȯ֧����</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="FtokenName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label32" runat="server">�������ã�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Ffee3Name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label34" runat="server">��������ʱ��(C2C)��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fcreate_time_c2c" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label36" runat="server">��������ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fcreate_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label38" runat="server">��Ҹ���ʱ��(����)��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fbargain_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label40" runat="server">��Ҹ���ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fpay_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label4" runat="server">��Ҹ���ʱ��(����)��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fpay_time_acc" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label11" runat="server">�������ʱ��(C2C)��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Freceive_time_c2c" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label20" runat="server">�������ʱ��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Freceive_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label27" runat="server">�������ʱ��(����)��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Freceive_time_acc" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label31" runat="server">���IP��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fip" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label35" runat="server">��̨��ע��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="LB_Fexplain" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label39" runat="server">��ע��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="LB_FMemo" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label42" runat="server">����޸�ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fmodify_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label44" runat="server">�������ͣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Ftrade_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label46" runat="server">���ʱ�־��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fadjust_flagName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label48" runat="server">������ţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fchannel_idName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label50" runat="server">ҵ����������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fcatch_desc" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label8" runat="server">�˿ʽ��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Frefund_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label24" runat="server">����ҽ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="FpaybuyName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label33" runat="server">�����ҽ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="FpaysaleName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label41" runat="server">�����˿�ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Freq_refund_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label45" runat="server">�˿�ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fok_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label49" runat="server">�˿�ʱ��(����)��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fok_time_acc" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label52" runat="server">���߱�־��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fappeal_signName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label54" runat="server">�н��־��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fmedi_signName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label17" runat="server">����ȯ���н��׵���</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="Fgwq_listid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><FONT face="����">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left">&nbsp;</P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
							</TBODY>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD><asp:label id="Label37" runat="server" ForeColor="Blue">������ˮ</asp:label></TD>
				</TR>
				<TR width="100%">
					<TD><asp:datagrid id="DataGrid2" runat="server" Width="100%" CellPadding="3" BackColor="White" BorderWidth="1px"
							BorderStyle="None" BorderColor="#CCCCCC" AutoGenerateColumns="False">
							<FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#669999"></SelectedItemStyle>
							<ItemStyle ForeColor="#000066"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#006699"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fqqid" HeaderText="�û��ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="FsubjectName" HeaderText="�������"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ffromid" HeaderText="�Է��ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="FpaynumName" HeaderText="���"></asp:BoundColumn>
								<asp:BoundColumn DataField="FpaybuyName" HeaderText="�����/�ֽ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="FpaysaleName" HeaderText="������/�ֽ�ȯ"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fmodify_time" HeaderText="����޸�ʱ��"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Left" ForeColor="#000066" BackColor="White" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<%--<TR>
					<TD><asp:label id="Label43" runat="server" ForeColor="Blue">������Ϣ</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="Datagrid3" runat="server" Width="100%" CellPadding="3" BackColor="White" BorderWidth="1px"
							BorderStyle="None" BorderColor="#CCCCCC" AutoGenerateColumns="False">
							<FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#669999"></SelectedItemStyle>
							<ItemStyle ForeColor="#000066"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#006699"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fqqid" HeaderText="�ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ftransport_typeName" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fgoods_typeName" HeaderText="��Ʒ����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ftran_typeName" HeaderText="������������"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ftransport_id" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="Frecv_truename" HeaderText="�ջ�����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fsend_truename" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="FstateName" HeaderText="������״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Left" ForeColor="#000066" BackColor="White" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>--%>
				<TR>
					<TD><asp:label id="Label29" runat="server" ForeColor="Blue">Ͷ�ߵ���Ϣ</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="DataGrid1" runat="server" Width="100%" CellPadding="3" BackColor="White" BorderWidth="1px"
							BorderStyle="None" BorderColor="#CCCCCC" AutoGenerateColumns="False">
							<FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#669999"></SelectedItemStyle>
							<ItemStyle ForeColor="#000066"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#006699"></HeaderStyle>
							<Columns>
								<asp:HyperLinkColumn DataNavigateUrlField="Fappealid" DataNavigateUrlFormatString="AppealDetail.aspx?appealid={0}"
									DataTextField="Fappealid" HeaderText="Ͷ�ߵ���"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="Fqqid" HeaderText="�ʻ�����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fvs_qqid" HeaderText="�Է��ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="FstateName" HeaderText="Ͷ�ߵ�״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fpunish_flagName" HeaderText="�������"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcheck_stateName" HeaderText="�÷���˱��"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fappeal_typeName" HeaderText="Ͷ�����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fappeal_time" HeaderText="Ͷ��ʱ��"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Left" ForeColor="#000066" BackColor="White" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
