<%@ Page language="c#" Codebehind="RefundErrorQuery_Detail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.RefundErrorQuery_Detail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundErrorQuery_Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			&nbsp;
			<TABLE id="Table2" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"
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
										<P align="right">
											<asp:label id="Label10" runat="server">�˵�ID��</asp:label></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labFoldID" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label1" runat="server">��ֵ���ţ�</asp:label></P>
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
										<P align="right">
											<asp:label id="Label6" runat="server">SPID��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFspid" runat="server" ForeColor="Blue"></asp:label></P>
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
										<P align="right"><asp:label id="Label8" runat="server">����״̬��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFHandleType" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label17" runat="server">��ʷ�˿ע��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFRefundMemo" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label11" runat="server">�˿������</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFRefundCount" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label50" runat="server">��ʧ���쳣���ͣ�</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFerrorType" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label19" runat="server">�쳣�����ˣ�</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFHandleUser" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label24" runat="server">�쳣����ʱ�䣺</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFHandleTime" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label27" runat="server">��ǰ�˿����Σ�</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFHandleBatchId" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label36" runat="server">ԭʼ�˿����Σ�</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFOrigBatchId" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label21" runat="server">��ʷ�˿�����1��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFBeforeBatchId1" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label34" runat="server">��ʷ�˿�����2��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFBeforeBatchId2" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label29" runat="server">��ʷ�˿���ʾ3��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFBeforeBatchId3" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label32" runat="server">��ʷ�˿���ʾ4��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFBeforeBatchId4" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label262" runat="server">����˵����</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFHandleMemo" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="label222" runat="server">��Ȩ�˿����Ϣ��</asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFAuthorizeFlagName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
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
