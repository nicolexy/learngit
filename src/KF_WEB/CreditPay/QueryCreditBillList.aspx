<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryCreditBillList.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.CreidtPay.QueryCreditBillList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryCreditBillList</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="3"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�˵���ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="right"><asp:label id="Label2" runat="server">�Ƹ�ͨ�˺ţ�</asp:label></TD>
                    <TD><asp:textbox id="cftNo" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
                    <TD align="center"><asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 105px; HEIGHT: 40%"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="month" HeaderText="�·�"></asp:BoundColumn>
								<asp:BoundColumn DataField="start_date" HeaderText="��ʼ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="end_date" HeaderText="������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="consume_amount_str" HeaderText="�������ѽ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="return_date" HeaderText="������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="type_str" HeaderText="�˵���ʶ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="instal_flag_str" HeaderText="�Ƿ����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="instal_period" HeaderText="��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="instal_prin_str" HeaderText="���ڽ��"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="����">
                                    <ItemTemplate>
                                        <asp:LinkButton id="lbDetail" runat="server" CommandName="DETAIL">��������</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
							SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
            <TABLE cellSpacing="1" cellPadding="0" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 435px;" BackColor="White" border="0">
                <TR>
					<TD bgColor="#eeeeee" height="18"><SPAN>&nbsp;��ϸ��Ϣ�б�</SPAN></TD>
				</TR>
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid2" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="pay_date" HeaderText="Ӧ������"></asp:BoundColumn>
								<asp:BoundColumn DataField="prin_str" HeaderText="���ڱ���"></asp:BoundColumn>
                                <asp:BoundColumn DataField="fee_str" HeaderText="����������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="status_str" HeaderText="״̬"></asp:BoundColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pager2" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage2"
							SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
            </TABLE>
		</form>
	</body>
</HTML>
