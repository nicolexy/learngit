<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="MediCertManage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MediCertManage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MediCertManage</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	.style5 { COLOR: #000000 }
	.style6 { COLOR: #ff0000 }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"
				cellPadding="1" width="85%" border="1">
				<TR>
					<TD bgColor="#e4e5f7" colSpan="3"><FONT face="����" color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
							&nbsp;&nbsp;&nbsp;�̻�֤�����</FONT> </FONT></TD>
					<TD align="right" bgColor="#e4e5f7"><FONT face="����">����Ա����: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="right">
						<asp:label id="lblSp" runat="server">�̻�</asp:label></TD>
					<TD><asp:textbox id="tbSp" runat="server"></asp:textbox></TD>
					<TD align="right" style="WIDTH: 83px; HEIGHT: 19px"><asp:label id="Label5" runat="server">����״̬</asp:label></TD>
					<TD style="HEIGHT: 19px">
						<asp:DropDownList id="ddlListStatus" runat="server" Width="152px">
							<asp:ListItem Value="0" Selected="True">����״̬</asp:ListItem>
							<asp:ListItem Value="1">��Ч</asp:ListItem>
							<asp:ListItem Value="2">����</asp:ListItem>
							<asp:ListItem Value="3">����</asp:ListItem>
						</asp:DropDownList></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 19px" align="right">
						<asp:label id="Label2" runat="server">ҵ��״̬</asp:label></TD>
					<TD style="HEIGHT: 19px">
						<asp:DropDownList id="ddlStatus" runat="server" Width="152px">
							<asp:ListItem Value="0" Selected="True">����״̬</asp:ListItem>
							<asp:ListItem Value="1">��������˽Կ</asp:ListItem>
							<asp:ListItem Value="2">��������֤������</asp:ListItem>
							<asp:ListItem Value="3">����ǩ֤</asp:ListItem>
							<asp:ListItem Value="4">�����ʽת��</asp:ListItem>
							<asp:ListItem Value="5">���</asp:ListItem>
						</asp:DropDownList></TD>
					<td align="center" colspan="2"><asp:button id="btnSearch" runat="server" Width="80px" Text="�� ѯ" onclick="btnSearch_Click"></asp:button></td>
				</TR>
			</TABLE>
			<TABLE style="Z-INDEX: 102; LEFT: 5%; WIDTH: 85%; POSITION: absolute; TOP: 136px; HEIGHT: 70%"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" EnableViewState="False" PageSize="15"
							AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderWidth="1px" BorderColor="LightGray">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="Black" BackColor="Silver"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fspid" HeaderText="SPID"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCrt_email" HeaderText="Email"></asp:BoundColumn>
								<asp:BoundColumn DataField="FstatusName" HeaderText="ҵ��״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="FList_StateName" HeaderText="����״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="fcrt_etime" HeaderText="��Ч��ֹ����"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="�̻�����ԱȨ��">
									<ItemTemplate>
										<asp:HyperLink runat="server" Text="�鿴" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.FSpid", "MediOperatorManage.aspx?spid={0}")%>' ID="Hyperlink1">
										</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" PageSize="15" CustomInfoTextAlign="Center" AlwaysShow="True"
							NumericButtonCount="5" ShowCustomInfoSection="left" PagingButtonSpacing="0" ShowInputBox="always"
							CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage" SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
