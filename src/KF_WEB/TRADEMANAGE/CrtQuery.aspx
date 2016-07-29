<%@ Page language="c#" Codebehind="CrtQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.CrtQuery" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FundQuery</title>
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
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 2.51%; POSITION: absolute; TOP: 80px; HEIGHT: 0.93%"
				cellSpacing="0" cellPadding="0" width="95%" border="1" runat="server" visible=false>
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="LightGray" BorderWidth="1px" BackColor="White"
							CellPadding="3" AutoGenerateColumns="False" PageSize="50" EnableViewState="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="Black" BackColor="Silver"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fscene" HeaderText="ʹ�õص�"></asp:BoundColumn>
								<asp:BoundColumn DataField="FstateName" HeaderText="֤��״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="FlstateName" HeaderText="����״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fvalidity_begin" HeaderText="��Ч��ʼʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fvalidity_end" HeaderText="��Ч����ʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fip" HeaderText="�ͻ���IP"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fmodify_time" HeaderText="����޸�ʱ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmodify_time2" HeaderText="ɾ��֤��ʱ��"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
            <TABLE id="TableDelete" style="Z-INDEX: 102; LEFT: 2.51%; POSITION: absolute; TOP: 90px; HEIGHT: 0.93%"
				cellSpacing="0" cellPadding="0" width="50%" border="1" runat="server" visible="false" frame="box">
                 <TR>
						<TD width="150" align="center" >
							<asp:label id="Label29" runat="server">�ر�֤�����ʱ��</asp:label></TD>
						<TD width="250">
							<asp:label id="lbDeletetime" runat="server"></asp:label></TD>
                </TR>
                <TR>
						<TD width="150" align="center">
							<asp:label id="Label33" runat="server">�ر�֤�����IP</asp:label></TD>
						<TD width="250">
							<asp:label id="lbDeleteIP" runat="server"></asp:label></TD>
					</TR>
			</TABLE>
			<TABLE id="Table1" style="Z-INDEX: 103; LEFT: 2.51%; POSITION: absolute; TOP: 1.52%" cellSpacing="0"
				cellPadding="0" width="95%" border="1">
				<TR>
					<TD style="WIDTH: 100%" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7" colSpan="3">
						<DIV align="center">
							<TABLE id="Table3" height="100%" cellSpacing="0" cellPadding="1" width="100%" border="0">
								<TR>
									<TD width="79%"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
												&nbsp;�û�֤���ѯ</FONT></FONT></TD>
									<TD width="21%"><FONT face="����">&nbsp;</FONT>����Ա����: <SPAN class="style3">
											<asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
								</TR>
							</TABLE>
							<SPAN class="style3"></SPAN>
						</DIV>
					</TD>
				</TR>
				<TR>
					<TD align="center" width="20%"><asp:label id="Label2" runat="server">�û��ʺ�</asp:label></TD>
					<TD><asp:textbox id="tbQQID" runat="server" BorderStyle="Groove"></asp:textbox></TD>
					<TD align="left" width="50%"><asp:button id="btnQuery" runat="server" Text=" ��  ѯ " onclick="btnQuery_Click"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</asp:button><asp:button id="btnDeleteQuery" runat="server" Text="��ѯ�ر�֤�������Ϣ" onclick="btnDeleteQuery_Click"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:button id="btnDelete" runat="server" Text="ɾ��֤��" onclick="btnDelete_Click"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:button id="btnDeleteService" runat="server" Text="�ر�֤�����" onclick="btnDeleteService_Click"></asp:button></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
