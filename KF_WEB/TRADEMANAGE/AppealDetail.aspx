<%@ Page language="c#" Codebehind="AppealDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.AppealDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>batPayReturn</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server" encType="multipart/form-data">
			&nbsp;
			<TABLE id="Table2" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"
				cellPadding="1" width="90%" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;<asp:label id="lbTitle" runat="server">Ͷ����ϸ��Ϣ</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:Label id="Label2" runat="server">����Ա���룺</asp:Label>
									<asp:Label id="labUid" runat="server" Width="64px"></asp:Label><SPAN class="style3"></SPAN></STRONG></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD colSpan="2" align="center">
						<TABLE style="WIDTH: 100%" cellSpacing="1" cellPadding="1" width="100%" border="0" runat="server"
							ID="Table1">
							<TBODY>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4">
									</td>
								</tr>
								<TR>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label10" runat="server">�������룺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="Fspid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label1" runat="server">�ʺţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="Fqqid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 17px">
										<P align="right"><asp:label id="Label14" runat="server">�����ַ��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 17px">
										<P align="left"><asp:label id="FEmail" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 17px">
										<P align="right"><asp:label id="Label12" runat="server">Ͷ����������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 17px">
										<P align="left"><asp:label id="FName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 18px">
										<P align="right"><asp:label id="Label5" runat="server">Ͷ���˵绰��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 18px">
										<P align="left"><asp:label id="FMobile" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 18px">
										<P align="right"><asp:label id="Label15" runat="server">�Է��ʺţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 18px">
										<P align="left"><asp:label id="Fvs_qqid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label7" runat="server">�Է����䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="FVs_Email" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center">
										<P align="right"><FONT face="����"></FONT><asp:label id="Label16" runat="server">�Է�������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="FVs_Name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label23" runat="server">�Է��绰��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="FVS_Mobile" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label25" runat="server">���׵���</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Flistid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label3" runat="server">���׵�״̬��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Flist_stateName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label6" runat="server">�˿��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Freturnid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label9" runat="server">�˿״̬��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Freturn_stateName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label13" runat="server">��������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Ftranid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label18" runat="server">������״̬��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Ftran_stateName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label20" runat="server">���׵�״̬(��)��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Flist_state_nextName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label22" runat="server">�˿״̬(��)��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Freturn_state_nextName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label26" runat="server">������״̬(��)��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Ftran_state_nextName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label28" runat="server">��Ʒ���ƣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="FCommTitle" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label30" runat="server">����Ա���룺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fuser" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label32" runat="server">Ͷ�ߵ�״̬��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="FstateName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label34" runat="server">���߱�־��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fresponse_flagName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label4" runat="server">������־��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fpunish_flagName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label11" runat="server">�÷����״̬��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fcheck_stateName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label19" runat="server">һ������ˣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fcheck_user1" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label24" runat="server">һ�����ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fcheck_time1" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label29" runat="server">��������ˣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fcheck_user2" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label33" runat="server">�������ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fcheck_time2" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label36" runat="server">��������ˣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fcheck_user3" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label38" runat="server">�������ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fcheck_time3" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label40" runat="server">�Ƿ��漰�˿</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Ffund_flagName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label42" runat="server">�����ܽ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Ftotal_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label44" runat="server">����ȯ��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Ftoken_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label46" runat="server">����ҽ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="FpaybuyName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label48" runat="server">�����ҽ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="FpaysaleName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label50" runat="server">Ͷ�����</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fappeal_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label52" runat="server">Ͷ�����ݣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px" colspan="3">
										<P align="left"><asp:label id="Fappeal_con" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label56" runat="server">����״̬��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="FlstateName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label58" runat="server">Ͷ��ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fappeal_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label60" runat="server">�������ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fend_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label62" runat="server">��ע��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fmemo" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label64" runat="server">����޸�ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fmodify_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left">&nbsp;</P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR style="HEIGHT: 46px">
									<TD vAlign="middle" align="center" colSpan="4"><FONT face="����"></FONT></TD>
								</TR>
							</TBODY>
						</TABLE>
						<INPUT style="WIDTH: 64px; HEIGHT: 22px" type="button" value="����" onclick="history.go(-1)">
					</TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
