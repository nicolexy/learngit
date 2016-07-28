<%@ Page language="c#" Codebehind="detailCheck.aspx.cs" AutoEventWireup="false" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.detailCheck" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>detailCheck</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); 
	</style>
</HEAD>
	<body topMargin="0">
		<DIV align="left">
			<form id="Form1" runat="server">
				<TABLE height="117" cellSpacing="1" cellPadding="0" width="100%" align="center" bgColor="#000000"
					border="0">
					<TR>
						<TD height="20" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" style="HEIGHT: 20px">&nbsp;申请人：</TD>
						<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20">&nbsp;
							<asp:label id="lbStartUser" runat="server" Width="105px">Label</asp:label></TD>
						<TD height="20" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" style="HEIGHT: 20px">
							<P>&nbsp;申请时间：</P>
						</TD>
						<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20">&nbsp;
							<asp:label id="lbStartTime" runat="server" Width="136px">Label</asp:label></TD>
					</TR>
					<TR>
						<TD width="23%" height="19" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
							style="HEIGHT: 19px">&nbsp;审批主体ID号:</TD>
						<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19">
							<!--
							<% if (checkType == "batchpay") { %>
							&nbsp;<A href="../BatchPay/ShowDetail.Aspx?BatchID=<% = objID %>&amp;pos=check " target=_parent ><% = objID %></A>
							<% } else { %>
							&nbsp;<A href="../ACCOUNTMANAGE/AdjustDepositMoney.aspx?id=<% = objID %>&amp;pos=check " target=_parent ><% = objID %></A>
							<% } %>
							--><FONT face="宋体">&nbsp;
								<asp:HyperLink id="hylkObjID" runat="server" Target="_parent" Visible="True"></asp:HyperLink>
							</FONT>
						</TD>
						<TD width="26%" height="-1" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
							style="HEIGHT: 19px">&nbsp;申请类型：</TD>
						<TD style="HEIGHT: 19px" width="25%" bgColor="#ffffff" height="-1">&nbsp;
							<asp:label id="lbType" runat="server" Width="105px" ForeColor="Black">Label</asp:label></TD>
					</TR>
					<TR>
						<TD height="20" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;审批金额:</TD>
						<TD bgColor="#ffffff" height="4">&nbsp;
							<asp:label id="lbTotalAccount" runat="server" Width="152px">Label</asp:label>元</TD>
						<TD width="26%" height="4" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;当前状态:</TD>
						<TD width="25%" bgColor="#ffffff" height="4">&nbsp;
							<asp:label id="lbState" runat="server" Width="105px">Label</asp:label></TD>
					</TR>
					<TR>
						<TD height="36" bgColor="#eeeeee" style="HEIGHT: 36px">&nbsp;申请原因：</TD>
						<TD style="HEIGHT: 36px" bgColor="#ffffff" colSpan="3" height="36">&nbsp;
							<asp:textbox id="txtReason" runat="server" Width="500px" BorderStyle="Groove" ReadOnly="True"
								TextMode="MultiLine"></asp:textbox>&nbsp;&nbsp;&nbsp;
							</TD>
					</TR>
					<!--如果有权限，则显示当前审批人；如果没有权限，则不显示-->
					<% if (right == "true") {%>
					<TR>
						<TD height="20" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" style="HEIGHT: 18px">&nbsp;审批人：</TD>
						<TD style="HEIGHT: 18px" bgColor="#ffffff" height="8">&nbsp;
							<asp:label id="lbUid" runat="server" Width="166px">Label</asp:label></TD>
						<TD height="8" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee" style="HEIGHT: 18px">&nbsp;审批时间：
						</TD>
						<TD style="HEIGHT: 18px" bgColor="#ffffff" height="8">&nbsp;
							<asp:label id="lbTime" runat="server" Width="166px">Label</asp:label></TD>
					</TR>
					<% } %>
					<TR>
						<TD height="20" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">&nbsp;审批总级别:</TD>
						<TD bgColor="#ffffff" height="5">&nbsp;
							<asp:label id="lbCheckLevel" runat="server" Width="166px">Label</asp:label></TD>
						<TD height="5" background="../IMAGES/Page/bk_white.gif" bgColor="#eeeeee">
							<P>&nbsp;当前正在审批级别:</P>
						</TD>
						<TD bgColor="#ffffff" height="5">&nbsp;
							<asp:label id="lbCLevel" runat="server" Width="166px">Label</asp:label>
							<asp:Button id="btnReturn" runat="server" Visible="False" Text="回退" Width="50px"></asp:Button></TD>
					</TR>
					<%if (sign == "uncheck") {
					if (right == "true"){
				%>
					<TR>
						<TD height="33" bgColor="#eeeeee" style="HEIGHT: 33px">&nbsp;审批意见：</TD>
						<TD style="HEIGHT: 33px" bgColor="#ffffff" colSpan="3" height="-1">&nbsp;<FONT face="宋体">
							</FONT>
							<asp:textbox id="txSuguest" runat="server" Width="497px" BorderStyle="Groove" TextMode="MultiLine"
								Height="31px"></asp:textbox></TD>
					</TR>
					<TR>
						<TD bgColor="#ffffff" colSpan="4"><FONT face="宋体">
								<P align="center"><asp:button id="btPass" runat="server" Width="85px" BorderStyle="Groove" Height="24px" Text="通过审批"></asp:button>&nbsp;<asp:button id="btRefuse" runat="server" Width="85px" BorderStyle="Groove" Height="24px" Text="拒绝审批"></asp:button></P>
							</FONT>
						</TD>
						<%} } else if (sign == "checked"){  
									  if (right == "false") {
							%>
						<%
						} }%>
						</FONT></TD></TR>
				</TABLE>
			
			</form>
		</DIV>
	</body>
</HTML>
