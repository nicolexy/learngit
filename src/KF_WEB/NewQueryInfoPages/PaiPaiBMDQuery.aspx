<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="PaiPaiBMDQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.PaiPaiBMDQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PaiPaiBMDQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script>
		    function CheckEmail() {
		        var txtEmail = document.getElementById("txtEmail");

		        if (txtEmail.value.replace(/^\s*/, "").replace(/\s*$/, "").length == 0) {
		            txtEmail.focus();
		            txtEmail.select();
		            alert("邮箱不允许为空!");
		            return false;
		        }
		    }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="820" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="3"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;拍拍店铺白名单</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">卖家QQ</asp:label></TD>
					<TD><asp:textbox id="txtSalerQQ" runat="server"></asp:textbox></TD>
					<TD align="left"><asp:button id="Button1" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH: 820px; POSITION: absolute; TOP: 140px; HEIGHT: 150px">
				<table cellSpacing="0" cellPadding="0" width="820" border="0">
					<tr>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="false" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" AllowPaging="True" Width="820px">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="SalerQQ" HeaderText="卖家QQ">
										<HeaderStyle Width="200px"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="UsePlace" HeaderText="应用场景">
									    <HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="UseOrNot" HeaderText="是否启用">
										<HeaderStyle Width="50px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Describe" HeaderText="描述">
										<HeaderStyle Width="170px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
				</table>
			</div>
			
		</form>
	</body>
</HTML>
