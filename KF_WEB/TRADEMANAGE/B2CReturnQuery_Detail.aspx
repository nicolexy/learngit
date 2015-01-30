<%@ Page language="c#" Codebehind="B2CReturnQuery_Detail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.B2CReturnQuery_Detail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>B2CReturnQuery_Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	TD { FONT-SIZE: 9pt }
	.style4 { COLOR: #ff0000 }
		</style>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			&nbsp;
			<TABLE id="Table2" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"
				cellPadding="1" width="90%" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;<asp:label id="lbTitle" runat="server">商户退款详细信息</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
										<P align="right"><asp:label id="Label10" runat="server">SPID：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labFspid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label1" runat="server">操作员：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labFop_name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label14" runat="server">交易单号：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="labFtransaction_id" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="right"><asp:label id="Label12" runat="server">买家帐号：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 17px" align="center">
										<P align="left"><asp:label id="labFbuyid" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="宋体"></FONT></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 18px" align="center">
										<P align="right"><asp:label id="Label5" runat="server">退买家费用 ：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="left"><asp:label id="labFrp_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="right"><asp:label id="Label15" runat="server">退卖家费用：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 18px" align="center">
										<P align="left"><asp:label id="labFrb_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="宋体"></FONT></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label7" runat="server">退款类型：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labFrefund_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center">
										<P align="right"><FONT face="宋体"></FONT><asp:label id="Label16" runat="server">业务状态：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labFstatusName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label23" runat="server">Email：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFemail" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label25" runat="server">物理状态：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFlist_stateName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label3" runat="server">退款单ＩＤ：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFdraw_id" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label6" runat="server">银行类型：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFbank_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label9" runat="server">银行名称：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFbank_name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label13" runat="server">开户地区：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFareaName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label18" runat="server">开户城市：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFcityName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label20" runat="server">财付通帐号：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFQQ" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label22" runat="server">银行帐号：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFbank_id" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label26" runat="server">最近ＩＰ：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFip" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label28" runat="server">生成时间：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFcreate_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label30" runat="server">修改时间：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFmodify_time" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label68" runat="server">备注：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labFmemo" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label4" runat="server">联系方式：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD style="HEIGHT: 16px" align="center">
										<P align="left"><asp:label id="labFaddress" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
<TR>
									<TD style="HEIGHT: 16px" align="center">
										<P align="right"><asp:label id="Label8" runat="server">处理说明：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD colspan="3" style="HEIGHT: 16px" align="center"><P align="left"><asp:label id="labHandleMemo" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>

								</TR>
							</TBODY>
						</TABLE>
						<INPUT style="WIDTH: 64px; HEIGHT: 22px" onclick="history.go(-1)" type="button" value="返回">
					</TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
