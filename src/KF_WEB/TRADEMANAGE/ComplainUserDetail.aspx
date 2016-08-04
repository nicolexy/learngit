<%@ Page language="c#" Codebehind="ComplainUserDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.ComplainUserDetail" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SysBulletinManage_Detail</title>
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
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 15%; WIDTH: 70%; POSITION: absolute; TOP: 8%"
				cellSpacing="1" cellPadding="1" border="1">
				<TR>
					<TD colSpan="2" style="HEIGHT: 25px"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;
						<asp:Label id="labTitle" runat="server" ForeColor="Red">新增用户投诉</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></TD>
				</TR>
				
				<TR>
					<TD align="left" width="15%">
						<asp:Label id="Label2" runat="server">商户号码：</asp:Label></TD>
					<TD align="left">
						<asp:TextBox id="bussNumber" runat="server" Width="300px"></asp:TextBox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="请输入" ControlToValidate="bussNumber"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><asp:Label id="Label3" runat="server">财付通订单号：</asp:Label></TD>
					<TD align="left">
						<asp:TextBox id="cftOrderId" runat="server" Width="300px"></asp:TextBox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="请输入" ControlToValidate="cftOrderId"></asp:RequiredFieldValidator></TD>
				</TR>
                <TR>
					<TD align="left" width="15%"><asp:Label id="Label8" runat="server">商户订单编号：</asp:Label></TD>
					<TD align="left">
						<asp:TextBox id="bussOrderId" runat="server" Width="300px"></asp:TextBox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ErrorMessage="请输入" ControlToValidate="bussOrderId"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
                    <TD align="left"><asp:label id="Label4" runat="server">投诉类型：</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlComplainType" runat="server" Width="152px">
							<asp:ListItem Value="1" Selected="True">买家要求补发货</asp:ListItem>
							<asp:ListItem Value="2">买家申请退款</asp:ListItem>
							<asp:ListItem Value="3">买家对商品质量不满意</asp:ListItem>
							<asp:ListItem Value="4">交易纠纷</asp:ListItem>
						</asp:DropDownList>
                    </TD>
                </TR>
                <TR id="statevisible" runat="server">
                    <TD align="left"><asp:label id="Label5" runat="server">状态：</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlCompState" runat="server" Width="152px">
							<asp:ListItem Value="1" Selected="True">已通知商户</asp:ListItem>
							<asp:ListItem Value="2">已催办商户</asp:ListItem>
							<asp:ListItem Value="3">商户已答复结果</asp:ListItem>
							<asp:ListItem Value="4">结单</asp:ListItem>
						</asp:DropDownList>
                    </TD>
                </TR>
                <TR>
                    <TD align="left"><asp:label id="Label6" runat="server">回复方式：</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlReplyType" runat="server" Width="152px">
							<asp:ListItem Value="1" Selected="True">电话回复</asp:ListItem>
							<asp:ListItem Value="2">手机短信回复</asp:ListItem>
							<asp:ListItem Value="3">QQ回复</asp:ListItem>
							<asp:ListItem Value="4">邮箱回复</asp:ListItem>
						</asp:DropDownList>
                    </TD>
                </TR>
                <TR>
                    <TD align="left"><asp:label id="Label7" runat="server">用户联系方式：</asp:label></TD>
                    <TD><asp:TextBox id="userContact" runat="server" Width="300px"></asp:TextBox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="请输入" ControlToValidate="userContact"></asp:RequiredFieldValidator></TD>
                </TR>
                <TR>
                    <TD align="left"><asp:label id="Label9" runat="server">备注：</asp:label></TD>
                    <TD><asp:TextBox id="memo" runat="server" Height="75px" Width="300px"></asp:TextBox></TD>
                </TR>
				<TR>
					<TD align="center" colspan="2">
						<asp:Button id="btnSave" runat="server" Width="80px" Text="提交" onclick="btnSave_Click"></asp:Button>
						&nbsp;<input type="button" name="btn_back" value="返回" style="WIDTH: 80px; HEIGHT: 22px" onclick="history.go(-1)" />
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
