<%@ Page language="c#" Codebehind="BusinessLogout.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BusinessLogout" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BusinessLogout</title>
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
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="3"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;商户注销</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<td align="right" width="25%"><asp:label id="Label5" runat="server">商户号</asp:label></td>
					<TD width="25%"><asp:textbox id="txtFspid" runat="server"></asp:textbox></TD>
					<TD align="center" width="50%"><asp:Button ID="btnLogout" Runat="server" Text="注 销" onclick="btnLogout_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnQuery" Runat="server" Text="查 询" onclick="btnQuery_Click"></asp:Button></TD>
				</TR>
				<tr>
					<td align="right" width="25%"><asp:label id="Label2" runat="server">原因</asp:label></td>
					<td colspan="2"><asp:TextBox ID="txtReason" Runat="server" TextMode="MultiLine" Width="440px"></asp:TextBox>
					</td>
				</tr>
			</TABLE>
            <TABLE id="Table2" visible="false" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 20%;"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
					<TD colspan="6" vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="companyname" HeaderText="商户名称"></asp:BoundColumn>
								<asp:BoundColumn DataField="applyuser" HeaderText="申请人"></asp:BoundColumn>
                                <asp:BoundColumn DataField="checkuser" HeaderText="审核人"></asp:BoundColumn>
                                <asp:BoundColumn DataField="amendtype" HeaderText="类型" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="applytime" HeaderText="申请时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="checktime" HeaderText="审核时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="amendstate_str" HeaderText="当前状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="applyresult" HeaderText="原因" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="taskid" HeaderText="tid" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="spid" HeaderText="spid" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="disagreeresult" HeaderText="不通过原因" Visible="false"></asp:BoundColumn>
								<asp:ButtonColumn Text="详情" HeaderText="详情" CommandName="detail"></asp:ButtonColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
                
			</TABLE>
            <table id="Table3" visible="false" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 50%;" runat="server">
                <tr>
                  <td align="left">商户号：</td>
                  <td><asp:label ID="lbSpid" Runat="server"></asp:label></td>
                  <td align="left">商户名称：</td>
                  <td><asp:label ID="lbSpname" Runat="server"></asp:label></td>
                  <td align="left">所属BD：</td>
                  <td><asp:label ID="lbBd" Runat="server"></asp:label></td>
                </tr>
                <tr>
                  <td align="left">网站域名：</td>
                  <td><asp:label ID="lbNetDns" Runat="server"></asp:label></td>
                  <td align="left">行业类别：</td>
                  <td><asp:label ID="lbClss" Runat="server"></asp:label></td>
                  <td align="left">&nbsp;</td>
                  <td><asp:label ID="Label6" Runat="server">&nbsp;</asp:label></td>
                </tr>
                <tr>
                  <td colspan="6">申请注销原因</td>
                </tr>
                <tr>
                    <td colspan="6"><asp:TextBox ID="txtApplyResult" Runat="server" TextMode="MultiLine" Width="840px"></asp:TextBox></td>
                </tr>
                <tr>
                  <td colspan="6">审核不通过原因</td>
                </tr>
                <tr>
                    <td colspan="6"><asp:TextBox ID="txtCheckNoResult" Runat="server" TextMode="MultiLine" Width="840px"></asp:TextBox></td>
                </tr>
            </table>
		</form>
	</body>
</HTML>
