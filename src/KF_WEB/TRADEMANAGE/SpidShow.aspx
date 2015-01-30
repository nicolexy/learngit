<%@ Page language="c#" Codebehind="SettShow.aspx.cs" AutoEventWireup="false" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.SpidShow" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SpidShow</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
	</HEAD>
	<body id="bodyid" runat="server">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="5" width="100%" align="center" border="0">
				<tr>
					<td vAlign="top">
						<TABLE height="50" cellSpacing="0" cellPadding="5" width="98%" align="center" border="0">
							<TR>
								<TD vAlign="middle" align="center">&nbsp;&nbsp;
									<asp:label id="Label2" runat="server" CssClass="title_info">�鿴�̻�����</asp:label></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="5" cellPadding="2" width="600" border="0" align="center">
							<TR>
								<TD class="detailitem" align="right" width="100">�̻����</TD>
								<TD>&nbsp;
									<asp:Label id="LabelModId" runat="server"></asp:Label></TD>
								<TD class="detailitem" align="right" width="100">QQ��</TD>
								<TD>&nbsp;
									<asp:Label id="LabelModSpecial" runat="server"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="detailitem" align="right" width="100">�̻�����</TD>
								<TD colSpan="3">&nbsp;
									<asp:Label id="LabelModName" tabIndex="51" runat="server"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="detailitem" align="right" width="100">�������˺�</TD>
								<TD colSpan="3">&nbsp;
									<asp:Label id="LabelModBank" tabIndex="51" runat="server"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="detailitem" align="right">�Ƹ�ͨ�˺�</TD>
								<TD>&nbsp;
									<asp:Label id="LabelModUid" tabIndex="3" runat="server"></asp:Label></TD>
								<TD class="detailitem" align="right">�н��˻���</TD>
								<TD>&nbsp;
									<asp:Label id="LabelModUidMiddle" tabIndex="4" runat="server"></asp:Label></TD>
							</TR>
							<TR>
								<TD class="detailitem" align="right">��ͬ���</TD>
								<TD>&nbsp;
									<asp:Label id="LabelModContract" tabIndex="5" runat="server"></asp:Label></TD>
								<TD class="detailitem" align="right">���״̬</TD>
								<TD>&nbsp;
									<asp:Label id="LabelModStatus" runat="server" ForeColor="Red"></asp:Label></TD>
							</TR>
							<tr>
								<TD class="detailitem" align="right">��ͬ�ⲿ���</TD>
								<TD colspan="3">&nbsp;
									<asp:Label id="LabelModContract1" tabIndex="6" runat="server"></asp:Label></TD>
							</tr>
							<TR>
								<TD align="right" class="detailitem">��������</TD>
								<TD>&nbsp;
									<asp:Label id="LabelModTime" runat="server"></asp:Label></TD>
								<TD align="right" class="detailitem">����Ա</TD>
								<TD>&nbsp;
									<asp:Label id="LabelModUserId" runat="server"></asp:Label></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table2" height="50" cellSpacing="0" cellPadding="5" width="98%" align="center"
							border="0">
							<TR>
								<TD vAlign="middle" align="center">&nbsp;&nbsp;
									<asp:label id="Label5" runat="server" CssClass="title_info">���̻���ǩ�����º�ͬ</asp:label></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<tr>
					<td align="center">
						<asp:datagrid id="Datagrid2" runat="server" Width="820px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="50" EnableViewState="False" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
							ItemStyle-HorizontalAlign="Center">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="FNo" HeaderText="���"></asp:BoundColumn>
								<asp:BoundColumn DataField="FChannelNo" HeaderText="����"></asp:BoundColumn>
								<asp:BoundColumn DataField="FProductType" HeaderText="��Ʒ"></asp:BoundColumn>
								<asp:BoundColumn DataField="FFeeItem" HeaderText="����&lt;br&gt;��Ŀ"></asp:BoundColumn>
								<asp:BoundColumn DataField="FFeeStandard" HeaderText="����&lt;br&gt;��׼"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="ʵ��&lt;br&gt;����">
									<ItemTemplate>
										<asp:Label id=Label6 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FDisCount") %>'>
										</asp:Label>%
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox id=TextBox1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FDisCount") %>'>
										</asp:TextBox>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="FCyc" HeaderText="����&lt;br&gt;����"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCycNumber" HeaderText="��׼&lt;br&gt;����"></asp:BoundColumn>
								<asp:BoundColumn DataField="FAppendAmount" HeaderText="����&lt;br&gt;����"></asp:BoundColumn>
								<asp:BoundColumn DataField="FPriorityAmount" HeaderText="����&lt;br&gt;����"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStandardStatus" HeaderText="Э��&lt;br&gt;״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStartDate" HeaderText="��Ч&lt;br&gt;��ʼ"></asp:BoundColumn>
								<asp:BoundColumn DataField="FEndDate" HeaderText="��Ч&lt;br&gt;����"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
					</td>
				</tr>
				<TR height="50">
					<TD align="center" colSpan="6"><a href="#" onclick="javascript:window.close();">�رմ���</a>&nbsp;&nbsp;|&nbsp;&nbsp;
						<asp:HyperLink id="LinkRefresh" runat="server">ˢ��</asp:HyperLink></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
