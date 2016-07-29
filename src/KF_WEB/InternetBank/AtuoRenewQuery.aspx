<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="AtuoRenewQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.InternetBank.AtuoRenewQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AtuoRenewQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT>
			<br>
			<TABLE id="Table4" style="Z-INDEX: 101; LEFT: 16px; WIDTH: 1040px" cellSpacing="1" cellPadding="1"
				width="1040" align="center" border="1">
				<TR bgColor="#eeeeee">
					<TD><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;<asp:label id="lbTitle" runat="server" ForeColor="Red">自动续费查询</asp:label>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN>
					</TD>
				</TR>
				<TR>
                    <TD>&nbsp;&nbsp;<asp:label id="Label2" runat="server">财付通号码</asp:label>
					<asp:textbox id="txtqqid" runat="server"></asp:textbox>
                    &nbsp;&nbsp;<FONT face="宋体"><asp:button id="Button1" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
                </TABLE>
			<TABLE id="Table1" style="Z-INDEX: 102; LEFT: 16px; WIDTH: 1040px" cellSpacing="1" cellPadding="1"
				width="1040" align="center" border="1" runat="server">
				<TR>
					<TD>
                    <asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
							CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" OnItemDataBound="DataGrid1_ItemDataBound">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                                <asp:BoundColumn DataField="Fmemo_str" HeaderText="绑定业务"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbank_type_str" HeaderText="开通方式" DataFormatString="{0:D}"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fstandby4" HeaderText="卡尾号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbind_time_local" HeaderText="开通时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Funchain_time_local" HeaderText="关闭时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fqqid" HeaderText="QQ号码"></asp:BoundColumn>
                                <asp:BoundColumn DataField="state" HeaderText="状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fchannel_id" Visible="false"></asp:BoundColumn>
                                 <asp:BoundColumn DataField="Fbind_status" Visible="false"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="操作">
                                    <ItemTemplate>
                                        <asp:LinkButton id="lbChange" Visible="false" runat="server" CommandName="CHANGE">解绑</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
								<%--<asp:TemplateColumn HeaderText="编辑">
									<ItemTemplate>
										<asp:LinkButton id="lbChange" runat="server" CommandName="CHANGE">编辑</asp:LinkButton>&nbsp;
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="删除">
									<ItemTemplate>
										<asp:LinkButton id="lbDel" runat="server" CommandName="DEL">删除</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>--%>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>

