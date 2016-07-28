<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="FCASettlementQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCASettlementQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FCASettlementQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="5"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�̻������ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="right"><asp:label id="Label2" runat="server">�̻���ţ�</asp:label></TD>
                    <TD><asp:textbox id="txtspid" style="WIDTH: 180px;" runat="server"></asp:textbox><Font color="red">*</Font></TD>
                     <TD align="right"><asp:label id="Label5" runat="server">����ʱ�䣺</asp:label></TD>
                    <TD>
                       <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                       <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                    ��
                        <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                       <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                    <Font color="red">*</Font></TD>
                    <TD align="center"><asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 184px; HEIGHT: 35%"
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
								<asp:BoundColumn DataField="Fspid" HeaderText="�̻����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fincome_date" HeaderText="��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Faccount_date" HeaderText="��������"></asp:BoundColumn>
                                 <asp:BoundColumn DataField="Fcur_type_str" HeaderText="���ױ���"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fpay_amout_str" HeaderText="���׽��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fpay_count" HeaderText="���ױ���"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fref_amout_str" HeaderText="�˿���"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fref_amout" HeaderText="�˿����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcycins_freeze_amount_str" HeaderText="ѭ��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fdue_amount_str" HeaderText="Ӧ�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ffee_amount_str" HeaderText="������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fsettle_cur_type_str" HeaderText="�������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fsettle_amount_str" HeaderText="������"></asp:BoundColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"
							SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
