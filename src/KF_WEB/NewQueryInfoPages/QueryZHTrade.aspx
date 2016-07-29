<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryZHTrade.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryZHTrade" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryZHTrade</title>
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
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�����˺Ž��ײ�ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="right"><asp:label id="Label2" runat="server">�Ƹ�ͨ�˺ţ�</asp:label></TD>
                    <TD><asp:textbox id="cftNo" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label5" runat="server">��ѯʱ�䣺</asp:label></TD>
                    <TD> 
                    <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                    <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                        ��
                    <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                    <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                    </TD>
				</TR>
                <TR>
                    <TD align="right"><asp:label id="Label4" runat="server">���׵��ţ�</asp:label></TD>
                    <TD><asp:textbox id="tradeNo" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
                    <TD align="right"><asp:label id="Label3" runat="server">�����˺ţ�</asp:label></TD>
                    <TD><asp:textbox id="ytNo" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<%--<TD align="right"><asp:label id="Label6" runat="server">ϵͳ�ο��ţ�</asp:label></TD>
                    <TD><asp:textbox id="bussOrderNo" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>--%>
                    <TD align="center" colspan="4"><asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ" onclick="btnQuery_Click"></asp:button>
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
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="Flistid" HeaderText="���׵���"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FFreeOrderId" HeaderText="���ᵥ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fca_name" HeaderText="�̼�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftid" HeaderText="ϵͳ�ο���"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Ftransaction_amt_str" HeaderText="�������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcharge_amt_rmb_str" HeaderText="֧�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fstatus_str" HeaderText="����״̬"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcft_uin_str" HeaderText="�Ƹ�ͨ�˺�"></asp:BoundColumn>
								<asp:ButtonColumn Text="�鿴����" HeaderText="��ϸ" CommandName="detail"></asp:ButtonColumn>
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
            <TABLE cellSpacing="1" cellPadding="0" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 650px;"  bgColor="black" border="0">
                <TR>
					<TD bgColor="#eeeeee" colSpan="4" height="18"><SPAN>&nbsp;��ϸ��Ϣ�б�</SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;��ʵ����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;���й����˺�:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c2" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�Ƹ�ͨ�˺�:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;ϵͳ�ο���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;���׵���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;����ʱ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�̼�����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;��Ʒ����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�������:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;ʵ��֧�����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;����״̬:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c11" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;���ױ�ע:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c12" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;����ʱ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c13" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;��Ȩ��appcode:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c14" runat="server"></asp:label></FONT></TD>
				</TR>
                 <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;���ᵥ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c15" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;���л���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c16" runat="server"></asp:label></FONT></TD>
				</TR>
                
            </TABLE>
		</form>
	</body>
</HTML>
