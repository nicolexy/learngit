<%@ Page language="c#" Codebehind="FreezeDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.FreezeDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FreezeDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			&nbsp;
			<TABLE id="Table2" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"
				cellPadding="1" width="90%" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;<asp:label id="lbTitle" runat="server">冻结详细信息</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:label id="Label2" runat="server">操作员代码：</asp:label><asp:label id="labUid" runat="server" Width="64px"></asp:label><SPAN class="style3"></SPAN></STRONG></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">
						<TABLE id="Table1" style="WIDTH: 100%" cellSpacing="1" cellPadding="1" width="100%" border="0"
							runat="server">
							<TBODY>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label10" runat="server">用户姓名：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labFUserName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label1" runat="server">联系方式：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labFContact" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label14" runat="server">操作类型：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="labFFreezeTypeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label12" runat="server">冻结ＩＤ：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="labFFreezeID" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label3" runat="server">冻结操作人员：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="labFFreezeUserID" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label6" runat="server">冻结时间：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="labFFreezeTime" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="宋体"></FONT></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label4" runat="server">冻结说明：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:TextBox id="txtFFreezeReason" runat="server" ForeColor="Blue" TextMode="MultiLine" Width="900px"></asp:TextBox><FONT face="宋体"></FONT></P>
									</TD>
                                    <TD align="center">
										<P align="right"><asp:label id="Label5" runat="server">冻结渠道：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labFfreezeChannel" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label19" runat="server">处理人员：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labFHandleUserID" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center">
										<P align="right"><asp:label id="Label21" runat="server">处理时间：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labFHandleTime" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="宋体"></FONT></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label7" runat="server">处理结论：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" colSpan="3">
										<P align="left"><asp:label id="labFHandleResult" runat="server" ForeColor="Blue"></asp:label><FONT face="宋体"></FONT></P>
									</TD>
								</TR>
								<tr style="HEIGHT: 4px" borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><asp:Label ID="lblfid" Runat="server" Visible="False"></asp:Label></td>
								</tr>
								<TR style="HEIGHT: 46px">
									<TD vAlign="middle" align="center" colSpan="4"><FONT face="宋体"></FONT></TD>
								</TR>
							</TBODY>
						</TABLE>
						<asp:Button ID="btnSave" Runat="server" Text="保存" Width="64px" Height="22" onclick="btnSave_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<INPUT style="WIDTH: 64px;HEIGHT: 22px" onclick="history.go(-1)" type="button" value="返回">
					</TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
