<%@ Page language="c#" Codebehind="RefundInfo.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.InternetBank.RefundInfo" %>
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
						<asp:Label id="labTitle" runat="server" ForeColor="Red">�����˿�Ǽ�</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></TD>
				</TR>
				
				<TR>
					<TD align="left" width="15%"><asp:Label id="Label3" runat="server">�Ƹ�ͨ�����ţ�</asp:Label></TD>
					<TD align="left">
						<asp:TextBox id="cftOrderId" runat="server" Width="300px" OnTextChanged="btnAmount_Click"></asp:TextBox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ErrorMessage="������" ControlToValidate="cftOrderId"></asp:RequiredFieldValidator></TD>
				    <asp:Button id="btnAmount" runat="server" Text="�ύ" onclick="btnAmount_Click" Style="display:none"></asp:Button>
                </TR>
                
				<TR>
                    <TD align="left"><asp:label id="Label4" runat="server">�˿����ͣ�</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlRefundType" runat="server" Width="152px">
							<asp:ListItem Value="10" Selected="True">Ͷ���˿�</asp:ListItem>
							<asp:ListItem Value="11">����ʧ��</asp:ListItem>
						</asp:DropDownList>
                    </TD>
                </TR>
                <TR id="statevisible" runat="server">
                    <TD align="left"><asp:label id="Label5" runat="server">�ύ�˿</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlRefundStatus" runat="server" Width="152px">
							<asp:ListItem Value="1">���ύ</asp:ListItem>
							<asp:ListItem Value="2" Selected="True">δ�ύ</asp:ListItem>
							<asp:ListItem Value="3">ʧЧ</asp:ListItem>
						</asp:DropDownList>
                    </TD>
                </TR>
                <TR>
					<TD align="left" width="15%"><asp:Label id="Label6" runat="server">������</asp:Label></TD>
					<TD align="left">
						<asp:TextBox id="tbAmount" runat="server" Width="300px" ReadOnly="true"></asp:TextBox>
                    </td>
				</TR>
                <TR>
					<TD align="left" width="15%"><asp:Label id="Label7" runat="server">�˿��</asp:Label></TD>
					<TD align="left">
						<asp:TextBox id="tbRefundAmount" runat="server" Width="300px"></asp:TextBox>
				    </td>
                </TR>
                <TR>
					<TD align="left" width="15%"><asp:Label id="Label2" runat="server">��Ʒ�����ˣ�</asp:Label></TD>
					<TD align="left">
						<asp:TextBox id="recycUser" runat="server" Width="200px"></asp:TextBox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="������" ControlToValidate="recycUser"></asp:RequiredFieldValidator></TD>
				</TR>
                <TR>
                    <TD align="left"><asp:label id="Label9" runat="server">SAM�����ţ�</asp:label></TD>
                    <TD><asp:TextBox id="memo" runat="server" Height="75px" Width="300px"></asp:TextBox></TD>
                </TR>
				<TR>
					<TD align="center" colspan="2">
						<asp:Button id="btnSave" runat="server" Width="80px" Text="�ύ" onclick="btnSave_Click"></asp:Button>
						&nbsp;<input type="button" name="btn_back" value="����" style="WIDTH: 80px;" onclick="history.go(-1)" />
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
