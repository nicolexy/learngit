<%@ Page language="c#" Codebehind="SynRecordQuery_Detail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.SynRecordQuery_Detail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SynRecordQuery_Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server" encType="multipart/form-data">
			&nbsp;
			<TABLE id="Table2" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"
				cellPadding="1" width="90%" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;<asp:label id="lbTitle" runat="server">ͬ����¼��ϸ��Ϣ</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
										<P align="right"><asp:label id="Label10" runat="server">���׵�ID��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labFtransaction_id" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label1" runat="server">֧��״̬��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labFpay_statusName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 17px">
										<P align="right"><asp:label id="Label14" runat="server">ͬ��������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 17px">
										<P align="left"><asp:label id="labFsyn_count" runat="server" ForeColor="Blue"></asp:label>
											<asp:Button id="Button1" runat="server" Text="ͬ������" onclick="Button1_Click"></asp:Button>
											<asp:TextBox id="tbNewCount" runat="server" Width="48px">0</asp:TextBox></P>
									</TD>
									<TD align="center" style="HEIGHT: 17px">
										<P align="right"><asp:label id="Label12" runat="server">ͬ��״̬��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 17px">
										<P align="left"><asp:label id="labFsyn_statusName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 18px">
										<P align="right"><asp:label id="Label5" runat="server">��¼״̬��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 18px">
										<P align="left"><asp:label id="labFrec_statusName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 18px">
										<P align="right"><asp:label id="Label15" runat="server">ͬ�����ͣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 18px">
										<P align="left"><asp:label id="labFsyn_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="����"></FONT></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label7" runat="server">֧�����ͣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labFpay_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center">
										<P align="right"><FONT face="����"></FONT><asp:label id="Label16" runat="server">֧��������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labFpay_channelName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label23" runat="server">�̻��汾�ţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFverName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label25" runat="server">�̻���ţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFsp_id" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label3" runat="server">�̻������ţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFsp_billno" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label6" runat="server">�̻�����ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFsp_timeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label9" runat="server">�������ͣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFbank_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label13" runat="server">�����ж�����</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFbank_billno" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label18" runat="server">���з��ض�����</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFbank_ret_billno" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label20" runat="server">����ʺţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFpurchaser_uin" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label22" runat="server">���������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFpurchaser_true_name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label26" runat="server">�����ʺţ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFbargainor_uin" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label28" runat="server">����������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFbargainor_true_name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label30" runat="server">��Ʒ��ʶ��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFgoods_tag" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label32" runat="server">����������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFdesc" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label34" runat="server">֧���ܽ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFtotal_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label36" runat="server">��Ʒ���ã�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFpriceName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label38" runat="server">������ã�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFtransport_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label40" runat="server">���������ѣ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFprocedure_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label42" runat="server">�ֽ���֣�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFfee_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label44" runat="server">�ֽ�֧����</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFfee1Name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label46" runat="server">����ȯ֧����</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFfee2Name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label48" runat="server">�������ã�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFfee3Name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label50" runat="server">�Żݽ�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFvfeeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label52" runat="server">����ҷ��ã�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFrp_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label54" runat="server">�����ҷ��ã�</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFrb_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label56" runat="server">�û�IP��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFclient_ip" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label58" runat="server">�̻�IP��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFsp_ip" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label60" runat="server">����ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFcreat_timeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label62" runat="server">֧���ɹ�ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFpay_timeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label64" runat="server">ͬ��ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFlast_syn_timeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label66" runat="server">�޸�ʱ�䣺</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFlast_modify_timeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label68" runat="server">ͬ�������</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fsyn_resultName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label4" runat="server">�̻�MAC��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFsp_mac" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label8" runat="server">����URL��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px" colspan="3">
										<P align="left"><asp:label id="labFReturn_Url" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label11" runat="server">��ע��</asp:label><FONT face="����">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px" colspan="3">
										<P align="left"><asp:label id="labFMemo" runat="server" ForeColor="Blue"></asp:label></P>
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
