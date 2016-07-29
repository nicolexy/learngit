<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="PayLimitManage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.PayLimitManage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PayLimitManage</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
.style2 {
	FONT-WEIGHT: bold; COLOR: #ff0000
}
BODY {
	BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif)
}
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"
				cellPadding="1" width="85%" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="4"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�Զ��۷�Э��</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<tr>
					<TD align="right"><asp:label id="Label2" runat="server">�ʻ�����</asp:label></TD>
					<TD><asp:textbox id="tbQQID" runat="server"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="������" ControlToValidate="tbQQID"></asp:requiredfieldvalidator></TD>
					<TD align="right" colSpan="1" rowSpan="1"><asp:label id="Label4" runat="server">�Է��ʺ�</asp:label></TD>
					<TD><asp:textbox id="tbaqqID" runat="server"></asp:textbox></TD>
				</tr>
				<TR>
					<TD align="center" colSpan="2"><asp:radiobutton id="rbtKeap" GroupName="Grouprbt" Text="ί�пۿ�" Runat="server"></asp:radiobutton><asp:radiobutton id="rbtRefund" GroupName="Grouprbt" Text="ί���˿�" Runat="server"></asp:radiobutton></TD>
					<TD align="right"><asp:label id="Label3" runat="server">����</asp:label></TD>
					<TD><asp:dropdownlist id="ddlChannel" runat="server"></asp:dropdownlist></TD>
				</TR>
				<tr>
					<TD align="center" colSpan="4"><asp:button id="Button2" runat="server" Width="80px" Text="�� ѯ" onclick="Button2_Click"></asp:button></TD>
				</tr>
				<tr>
					<TD align="center" colSpan="4"><asp:label id="lblTrustLimit" Runat="server"></asp:label></TD>
				</tr>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 170px; HEIGHT: 70%"
				cellSpacing="1" cellPadding="1" width="805" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="805" AllowPaging="True" BorderColor="#E7E7FF"
							BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="15" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="fid" HeaderText="fid"></asp:BoundColumn>
								<asp:BoundColumn DataField="FQQID" HeaderText="�ʻ�����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fchannel_name" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="FdirectName" HeaderText="���׷���"></asp:BoundColumn>
								<asp:BoundColumn DataField="FAQQID" HeaderText="�Է��ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fsp_ip" HeaderText="����IP"></asp:BoundColumn>
								<asp:BoundColumn DataField="Flist_stateName" HeaderText="����״̬"></asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
