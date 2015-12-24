<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="UserClassTJ.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.UserClassTJ" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FundQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"
				cellPadding="1" width="85%" border="1">
				<TR>
					<TD bgColor="#e4e5f7" colSpan="3"><FONT face="宋体" color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
							&nbsp;&nbsp;实名认证统计</FONT> </FONT></TD>
					<TD align="right" bgColor="#e4e5f7" colSpan="2"><FONT face="宋体">操作员代码: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">处理起始日期</asp:label></TD>
					<TD>
                        <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
					</TD>
					<TD align="right"><asp:label id="Label3" runat="server">处理结束日期</asp:label></TD>
					<TD>
                        <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
					</TD>
					<TD><asp:button id="btnTJ" runat="server" Text="统计" onclick="btnTJ_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 100px; HEIGHT: 70%"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
					<TD vAlign="top" colSpan="6"><asp:datagrid id="DataGrid1" runat="server" Width="100%" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" EnableViewState="False"
							PageSize="50">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fpickuser" HeaderText="客服帐号"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="通过数">
									<ItemTemplate>
										<asp:HyperLink id="HL" runat="server" NavigateUrl='<%# GetUrl(2,DataBinder.Eval(Container, "DataItem.Fpickuser").ToString()) %>'>
											<%# DataBinder.Eval(Container, "DataItem.FCommitCount") %>
										</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="拒绝数">
									<ItemTemplate>
										<asp:HyperLink id="Hyperlink1" runat="server" NavigateUrl='<%# GetUrl(3,DataBinder.Eval(Container, "DataItem.Fpickuser").ToString()) %>'>
											<%# DataBinder.Eval(Container, "DataItem.FCancelCount") %>
										</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="合计">
									<ItemTemplate>
										<asp:HyperLink id="Hyperlink4" runat="server" NavigateUrl='<%# GetUrl(9,DataBinder.Eval(Container, "DataItem.Fpickuser").ToString()) %>'>
											<%# DataBinder.Eval(Container, "DataItem.FTotalCount") %>
										</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><asp:label id="Label4" runat="server">申诉数:</asp:label></TD>
					<TD><asp:label id="labSumCount" runat="server"></asp:label></TD>
					<TD><asp:label id="Label6" runat="server">已处理:</asp:label></TD>
					<TD><asp:label id="labHandleCount" runat="server"></asp:label></TD>
					<TD><asp:label id="Label8" runat="server">未处理:</asp:label></TD>
					<TD><asp:label id="labothercount" runat="server"></asp:label></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
