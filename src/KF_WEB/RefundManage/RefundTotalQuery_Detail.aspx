<%@ Page language="c#" Codebehind="RefundTotalQuery_Detail.aspx.cs" AutoEventWireup="false" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.RefundTotalQuery_Detail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundTotalQuery_Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #ff0000; FONT-WEIGHT: bold }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			&nbsp;
			<TABLE id="Table2" style="Z-INDEX: 101; margin-top:10px; margin-left:10px" cellSpacing="1"
				cellPadding="1" width="90%" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;<asp:label id="lbTitle" runat="server">�̻��˵���ϸ��Ϣ</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:label id="Label2" runat="server">����Ա���룺</asp:label><asp:label id="labUid" runat="server" Width="64px"></asp:label><SPAN class="style3"></SPAN></STRONG></SPAN></FONT></TD>
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
										<P align="right"><asp:label id="Label10" runat="server">SPID��</asp:label></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labFspid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label1" runat="server">���׵��ţ�</asp:label></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labFPaylistid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label14" runat="server">�����еĶ����ţ�</asp:label></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="labFbank_listid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label12" runat="server">���з��صĶ����ţ�</asp:label></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="labFbank_backid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 18px" align="center">
										<P align="right"><asp:label id="Label5" runat="server">�������� ��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="left"><asp:label id="labFbanktype" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="right"><asp:label id="Label15" runat="server">��ֵ��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="left"><asp:label id="labFamt" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label7" runat="server">�˿��</asp:label></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labFreturnamt" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center">
										<P align="right"><FONT face="����"></FONT><asp:label id="Label16" runat="server">����ʻ����룺</asp:label></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labFbuyid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label23" runat="server">������ƣ�</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFbuy_name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label25" runat="server">��ҵ������ʺţ�</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFbuy_bankid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label3" runat="server">����ʱ�䣺</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFPay_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label6" runat="server">���׵�ID��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFListid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label9" runat="server">����ʱ�䣺</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFCreateTime" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label13" runat="server">�˵�״̬��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFstate" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label18" runat="server">�˵�����״̬��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFlstate" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label20" runat="server">�������ͣ�</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFAdjustType" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label22" runat="server">�ص�״̬��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFreturnState" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label26" runat="server">������Դ��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFrefundType" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label28" runat="server">�˵�;����</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFrefundPath" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label30" runat="server">����޸�ʱ�䣺</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFmodify_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label68" runat="server">����˵����</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFmemo" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label4" runat="server">��ע��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFexplain" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label8" runat="server">�˿��(��ת��ѯ�˵��쳣)��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left">
											<asp:HyperLink id="labFoldid" Enabled="false" runat="server">labFoldid</asp:HyperLink></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label17" runat="server">������ע��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFCreateMemo" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center" colSpan="4"></TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
							</TBODY>
						</TABLE>
						<INPUT style="WIDTH: 64px; HEIGHT: 22px" onclick="history.go(-1)" type="button" value="����">
					</TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
