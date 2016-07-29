<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="FCAInfoQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCAInfoQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FCAInfoQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
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
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="3"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;商户信息查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="right"><asp:label id="Label2" runat="server">商户编号：</asp:label></TD>
                    <TD><asp:textbox id="txtspid" style="WIDTH: 180px;" runat="server"></asp:textbox><Font color="red">*</Font></TD>
                    <TD align="center"><asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
            <TABLE cellSpacing="1" cellPadding="0" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 150px;"  bgColor="black" border="0">
                <TR>
					<TD bgColor="#eeeeee" colSpan="4" height="18"><SPAN>&nbsp;基本信息：</SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;商户名称:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;商户地址:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c2" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;网站域名:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;邮政编号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;所属区域:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;联系电话:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;Email:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;联系手机:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;商户类型:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;法人姓名:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;法人代表证件类型:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c11" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;法人证件号码:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c12" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;商户营业执照编号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c13" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;商户内部id:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c14" runat="server"></asp:label></FONT></TD>
				</TR>
                 <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;是否是内部商户:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c15" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;公司名称:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c16" runat="server"></asp:label></FONT></TD>
				</TR>
                  <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;国别:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c17" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;城市:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c18" runat="server"></asp:label></FONT></TD>
				</TR>
                  <TR>
                  <TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;说明:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c19" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;内部修改备注:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c20" runat="server"></asp:label></FONT></TD>
				</TR>
                  <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;操作员:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c21" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;商户拓展bd:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c22" runat="server"></asp:label></FONT></TD>
				</TR>
                  <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;IP地址:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c23" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;创建时间:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c24" runat="server"></asp:label></FONT></TD>
				</TR>
                 <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;修改时间:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c25" runat="server"></asp:label></FONT></TD>
                    <TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;商户状态:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c26" runat="server"></asp:label></FONT></TD>
				</TR>
                 <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;物理状态:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c27" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c28" runat="server"></asp:label></FONT></TD>
				</TR>
                 <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c29" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c30" runat="server"></asp:label></FONT></TD>
				</TR>
                
            </TABLE>
		</form>
	</body>
</HTML>
