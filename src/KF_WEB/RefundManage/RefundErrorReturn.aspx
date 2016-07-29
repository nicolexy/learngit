<%@ Page language="c#" Codebehind="RefundErrorReturn.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.RefundErrorReturn" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundErrorReturn</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #ff0000; FONT-WEIGHT: bold }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<SCRIPT language="javascript">		
			function checkvlid()
			{
				with(Form1)
				{
					return confirm("请你确认是否进行此操作？");					
				}
			}
		</SCRIPT>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server" encType="multipart/form-data">
			&nbsp;
			<TABLE id="Table1" style="Z-INDEX: 101; POSITION: absolute; TOP: 5%; LEFT: 5%" cellSpacing="1"
				cellPadding="1" width="90%" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;
									<asp:label id="Label2" runat="server" ForeColor="Blue"></asp:label>
									<asp:label id="Label6" runat="server" ForeColor="Blue"></asp:label><asp:label id="lbTitle" runat="server">lbTitle</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:Label id="ajax_lb_batchid" runat="server" ForeColor="#EEEEEE" Width="80px">Label</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:Label id="ajax_lb_user" runat="server" Width="64px" ForeColor="#EEEEEE">Label</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:hyperlink id="hlRefresh" runat="server" ForeColor="Blue">刷新</asp:hyperlink>&nbsp;&nbsp;
									<asp:hyperlink id="hlBack" runat="server" ForeColor="Blue">返回</asp:hyperlink><SPAN class="style3"></SPAN></STRONG></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD colSpan="2" align="center">
						<TABLE style="WIDTH: 100%" cellSpacing="1" cellPadding="1" width="100%" border="0" runat="server"
							ID="Table1">
							<TBODY>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4">
										<asp:Label id="labErrmsg" runat="server" ForeColor="Red"></asp:Label>
									</td>
								</tr>
								<TR>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label10" runat="server">日期</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labDate" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label1" runat="server">银行</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labBank" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label3" runat="server">状态</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" colSpan="3">
										<P align="left"><asp:label id="labStatusName" runat="server" ForeColor="Red"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 17px">
										<P align="right"><asp:label id="Label14" runat="server">审批人</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 17px">
										<P align="left"><asp:label id="labApprover" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 17px">
										<P align="right"><asp:label id="Label12" runat="server">审批时间</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 17px">
										<P align="left"><asp:label id="labAproveDate" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="宋体"></FONT></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label4" runat="server">审批意见</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" colSpan="3">
										<P align="left"><asp:label id="labAproveMsg" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label19" runat="server">退款发起人</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labExecutor" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center">
										<P align="right"><asp:label id="Label21" runat="server">退款发起时间</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labExecuteDate" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="宋体"></FONT></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 18px">
										<P align="right"><asp:label id="Label5" runat="server">业务更新人</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 18px">
										<P align="left"><asp:label id="labUpdate" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 18px">
										<P align="right"><asp:label id="Label15" runat="server">业务更新时间</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 18px">
										<P align="left"><asp:label id="labUpdateTime" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="宋体"></FONT></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label7" runat="server">退款总笔数</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labPayCount" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center">
										<P align="right"><FONT face="宋体"></FONT><asp:label id="Label16" runat="server">退款总金额</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labPaySum" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label23" runat="server">退款成功笔数</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labSuccessCount" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label25" runat="server">实际退款金额</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labSuccessSum" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label8" runat="server">退款失败笔数</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labErrorCount" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label11" runat="server">退款失败金额</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labErrorSum" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label17" runat="server">退款处理中笔数</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labPayingCount" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label20" runat="server">退款处理中金额</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labPayingSum" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR style="HEIGHT: 46px">
									<TD vAlign="middle" align="center" colSpan="4"><FONT face="宋体">
											<asp:DropDownList id="ddlBankType" runat="server" Visible="False">
												<asp:ListItem Value="1001" Selected="True">招行出款</asp:ListItem>
												<asp:ListItem Value="1010">商行出款</asp:ListItem>
											</asp:DropDownList>
											<asp:DropDownList id="ddlFileType" runat="server">
												<asp:ListItem Value="1">对私退款结果</asp:ListItem>
												<asp:ListItem Value="2">对公退款结果</asp:ListItem>
											</asp:DropDownList><input id="File1" type="file" runat="server" NAME="File1"><asp:button id="btnMain" runat="server" Text="Button"></asp:button><asp:label id="labMain" runat="server" Visible="False"></asp:label>
											<asp:Button id="btFinish" runat="server" Text="上传完成/任务单成功" Visible="False"></asp:Button>
											<asp:TextBox id="tbCancelReason" runat="server" Visible="False"></asp:TextBox>
											<asp:Button id="btCancel" runat="server" Text="上传作废/收回" Visible="False"></asp:Button></FONT></TD>
								</TR>
							</TBODY>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
