<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="GwqShow.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TokenCoin.GwqShow" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>GwqShow</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	TD { FONT-SIZE: 9pt }
	.style4 { COLOR: #ff0000 }
		</style>
	</HEAD>
	<BODY id="bodyId" runat="server">
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			&nbsp;
			<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="90%" align="center" border="1">
				<TBODY>
					<TR bgColor="#eeeeee" height="24">
						<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;<asp:label id="lbTitle" runat="server">�Ƹ�ȯ�鿴</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="Label2" runat="server">����Ա���룺</asp:label><asp:label id="labUid" runat="server" Width="64px"></asp:label><SPAN class="style3"></SPAN></STRONG></SPAN></FONT></TD>
					</TR>
					<TR>
						<TD align="right" colSpan="2">
							<TABLE id="Table1" style="WIDTH: 100%" cellSpacing="0" cellPadding="1" width="100%" border="0"
								runat="server">
								<TBODY>
									<tr borderColor="#999999" bgColor="#999999">
										<td colSpan="4"></td>
									</tr>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label7" runat="server">ʹ�����ʺţ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxUserId" runat="server"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label33" runat="server">�Ƹ�ȯID�ţ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxId" runat="server" Width="160px"></asp:textbox><asp:button id="Button1" runat="server" Text="��ѯ"></asp:button></TD>
									</TR>
									<tr borderColor="#999999" bgColor="#999999">
										<td colSpan="4"></td>
									</tr>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label10" runat="server">�������κţ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxTdeId" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label19" runat="server">�����κţ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxSonId" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label24" runat="server">�Ƹ�ȯ���ƣ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxAttName" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label25" runat="server">��Ʒ���룺</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxMerId" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label21" runat="server">�������ʺţ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubId" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label14" runat="server">�������ڲ��ʺţ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubUId" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label12" runat="server">���������ƣ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubName" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label39" runat="server">����IP��</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubIp" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label28" runat="server">���ͣ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxType" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label29" runat="server">�������ͣ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubType" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR borderColor="#999999" bgColor="#999999">
										<TD colSpan="4"><FONT face="����"></FONT></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label22" runat="server">�������ڣ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubTime" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label23" runat="server">���в���Ա��</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubUser" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label11" runat="server">��ֵ��Ԫ/��������</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxFee" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label36" runat="server">ʵ��ʹ�ý�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxFactFee" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label15" runat="server">���ʹ�ñ�����</asp:label></TD>
										<TD>&nbsp; ���֮
											<asp:textbox id="TextBoxUsePro" runat="server" Width="75px" BorderStyle="None" ReadOnly="True"></asp:textbox>
											<asp:label id="Label32" runat="server">(10000��ʾ������)</asp:label></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label16" runat="server">���ʹ���޶</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxMinFee" runat="server" Width="75px" BorderStyle="None" ReadOnly="True"></asp:textbox>Ԫ
											<asp:label id="Label35" runat="server">(0��ʾ������)</asp:label></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label17" runat="server">ÿ������ʹ��������</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxMaxNum" runat="server" Width="75px" BorderStyle="None" ReadOnly="True"></asp:textbox>��
											<asp:label id="Label34" runat="server">(0��ʾ������)</asp:label></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label13" runat="server">�Ƿ��������ͣ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxDonateType" runat="server" Width="75px" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label8" runat="server">��Ч���ڣ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxSTime" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label9" runat="server">�������ڣ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxETime" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<tr borderColor="#999999" bgColor="#999999">
										<td colSpan="4"></td>
									</tr>
									<TR>
										<TD style="HEIGHT: 17px" align="right"><asp:label id="Label44" runat="server">���������ʺţ�</asp:label></TD>
										<TD style="HEIGHT: 17px"><FONT face="����">&nbsp;<asp:textbox id="TextBoxAcUin" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></FONT></TD>
										<TD style="HEIGHT: 17px" align="right"><asp:label id="Label45" runat="server">���������ڲ��ʺţ�</asp:label></TD>
										<TD style="HEIGHT: 17px"><FONT face="����">&nbsp;<asp:textbox id="TextBoxAcUid" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></FONT></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px; HEIGHT: 17px" align="right"><asp:label id="Label46" runat="server">�Ƿ�����漴���ã�</asp:label></TD>
										<TD style="HEIGHT: 17px">&nbsp;
											<asp:textbox id="TextBoxAcFlag" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></TD>
										<TD style="HEIGHT: 17px" align="right"><asp:label id="Label47" runat="server">ͬһ�û����ô�����</asp:label></TD>
										<TD style="HEIGHT: 17px">&nbsp;
											<asp:textbox id="TextBoxAcNum" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px; HEIGHT: 17px" align="right"><asp:label id="Label48" runat="server">��ʼ�������ڣ�</asp:label></TD>
										<TD style="HEIGHT: 17px">&nbsp;
											<asp:textbox id="TextBoxAcSTime" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></TD>
										<TD style="HEIGHT: 17px" align="right"><asp:label id="Label49" runat="server">�����������ڣ�</asp:label></TD>
										<TD style="HEIGHT: 17px">&nbsp;
											<asp:textbox id="TextBoxAcETime" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></TD>
									</TR>
									<TR borderColor="#999999" bgColor="#999999">
										<TD colSpan="4"><FONT face="����"></FONT></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label20" runat="server">ʹ�����ڲ��ʺţ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxUserUid" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label26" runat="server">ʹ�����ڣ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxUseTime" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px; HEIGHT: 10px" align="right"><asp:label id="Label1" runat="server">�������ʺţ�</asp:label></TD>
										<TD style="HEIGHT: 10px">&nbsp;
											<asp:textbox id="TextBoxDonateId" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="HEIGHT: 10px" align="right"><asp:label id="Label5" runat="server">�������ڲ��ʺţ�</asp:label></TD>
										<TD style="HEIGHT: 10px">&nbsp;
											<asp:textbox id="TextBoxDonateUid" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px; HEIGHT: 22px" align="right"><asp:label id="Label3" runat="server">���ʹ�����</asp:label></TD>
										<TD style="HEIGHT: 22px">&nbsp;
											<asp:textbox id="TextBoxDonateNum" runat="server" Width="75px" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="HEIGHT: 22px" align="right"><asp:label id="Label27" runat="server">�������ڣ�</asp:label></TD>
										<TD style="HEIGHT: 22px">&nbsp;
											<asp:textbox id="TextBoxDonateTime" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px; HEIGHT: 22px" align="right"><asp:label id="Label30" runat="server">ҵ��״̬��</asp:label></TD>
										<TD style="HEIGHT: 22px">&nbsp;
											<asp:textbox id="TextBoxState" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="HEIGHT: 22px" align="right"><asp:label id="Label31" runat="server">����޸�ʱ�䣺</asp:label></TD>
										<TD style="HEIGHT: 22px">&nbsp;
											<asp:textbox id="TextBoxModifyTime" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<tr borderColor="#999999" bgColor="#999999">
										<td colSpan="4"><FONT face="����"></FONT></td>
									</tr>
									<TR>
										<TD style="WIDTH: 133px" align="right"><asp:label id="Label40" runat="server">�û�IP��</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxUserIp" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD align="right"><asp:label id="Label18" runat="server">�������룺</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxSpid" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right"><asp:label id="Label37" runat="server">���н��׵���</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxListid" runat="server" BorderStyle="None" ReadOnly="True" Width="220px"></asp:textbox></TD>
										<TD align="right"><asp:label id="Label38" runat="server">ʹ�ý��׵���</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxUseListid" runat="server" BorderStyle="None" ReadOnly="True" Width="220px"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right"><asp:label id="Label41" runat="server">����״̬��</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxListState" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD align="right"><asp:label id="Label42" runat="server">���˱�ǣ�</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxAdjustFlag" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR borderColor="#999999" bgColor="#999999">
										<TD colSpan="4"><FONT style="BACKGROUND-COLOR: #ffffff" face="����"></FONT><FONT face="����"></FONT></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" vAlign="top" align="right"><asp:label id="Label6" runat="server">ʹ�ó���URL��</asp:label><BR>
										</TD>
										<TD colSpan="3">&nbsp;
											<asp:textbox id="TextBoxUrl" runat="server" Width="550px" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" vAlign="top" align="right"><asp:label id="Label4" runat="server">��ע��</asp:label><BR>
										</TD>
										<TD colSpan="3">&nbsp;
											<asp:textbox id="TextBoxMemo" runat="server" Width="550px" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR borderColor="#999999" bgColor="#999999">
										<TD colSpan="4"><FONT face="����"></FONT></TD>
									</TR>
								</TBODY>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD align="left" colSpan="2"><asp:Label id="Label43" runat="server" Font-Bold="True">��ǰʹ���߲�����ˮ��</asp:Label>
							<asp:datagrid id="DataGrid1" runat="server" Width="100%" BorderStyle="None" ForeColor="Black"
								PageSize="20" AutoGenerateColumns="False" HorizontalAlign="Center" CellPadding="1" BackColor="White"
								BorderColor="#DEDFDE" BorderWidth="1px">
								<FooterStyle BackColor="#CCCC99"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" Wrap="False" ForeColor="White" BackColor="#CE5D5A"></SelectedItemStyle>
								<EditItemStyle Wrap="False"></EditItemStyle>
								<AlternatingItemStyle Wrap="False" BackColor="White"></AlternatingItemStyle>
								<ItemStyle Wrap="False" BackColor="#F7F7DE"></ItemStyle>
								<HeaderStyle Font-Bold="True" Wrap="False" HorizontalAlign="Center" ForeColor="White" BackColor="#6B696B"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="FCreate_time" HeaderText="����ʱ��">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:BoundColumn>
									<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="Fuser_id" DataNavigateUrlFormatString="../BaseAccount/InfoCenter.aspx?id={0}"
										DataTextField="Fuser_id" HeaderText="ʹ�����ʺ�">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:HyperLinkColumn>
									<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="Facce_id" DataNavigateUrlFormatString="../BaseAccount/InfoCenter.aspx?id={0}"
										DataTextField="Facce_id" HeaderText="�������ʺ�">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:HyperLinkColumn>
									<asp:BoundColumn DataField="Frequest_type" HeaderText="��������">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:BoundColumn>
									<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="Flistid" DataNavigateUrlFormatString="../TradeManage/TradeLogQuery.aspx?id={0}"
										DataTextField="Flistid" HeaderText="���׵���">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:HyperLinkColumn>
									<asp:BoundColumn DataField="Ffact_fee" HeaderText="ʹ��&lt;br&gt;���">
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Flist_state" HeaderText="��ˮ&lt;br&gt;���">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fold_state" HeaderText="ԭ״̬">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fnew_state" HeaderText="��״̬">
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fuser_ip" HeaderText="�û�IP"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fmemo" HeaderText="��ע"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Left" ForeColor="Black" BackColor="White" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
				</TBODY>
			</TABLE>
			</TR></TBODY></TABLE></TR></TBODY></TABLE></form>
	</BODY>
</HTML>
