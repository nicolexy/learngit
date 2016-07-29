<%@ Page language="c#" Codebehind="AppealDSettings.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.AppealDSettings" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundMain</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="900" border="1" align="center">
				<TR bgColor="#eeeeee">
					<TD colSpan="5"><FONT color="#ff0000"><IMG src="../IMAGES/Page/post.gif" width="15"><asp:label id="lbTitle" runat="server">���ֹ����ѯ</asp:label></FONT>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:label id="Label1" runat="server" ForeColor="ControlText">����Ա���룺</asp:label><FONT color="#ff0000"><asp:label id="Label_uid" runat="server">Label</asp:label></FONT>
					</TD>
				</TR>
				<tr>
					<TD><FONT face="����">�̻��� </FONT>
						<asp:textbox id="tb_spid" runat="server" Width="96px"></asp:textbox></TD>
					<TD><FONT face="����">�û��ʺ�
							<asp:textbox id="tb_user" runat="server" Width="96px"></asp:textbox></FONT></TD>
					<TD><FONT face="����">��������</FONT>
						<asp:dropdownlist id="ddl_pritype" runat="server" Width="179px">
							<asp:ListItem Value="-1" Selected="True">����</asp:ListItem>
							<asp:ListItem Value="1">������ȫ��תָ���Է��˻�</asp:ListItem>
							<asp:ListItem Value="2">������ȫ��ת�����ʻ�</asp:ListItem>
							<asp:ListItem Value="3">������ȫ�����汾�ʻ�</asp:ListItem>
							<asp:ListItem Value="4">�ʻ����ȫ��ת�����ʻ�</asp:ListItem>
							<asp:ListItem Value="5">�ʻ����ȫ��תָ���Է��ʻ�</asp:ListItem>
							<asp:ListItem Value="6">�ʻ��������ָ��������ת�Է��ʻ�</asp:ListItem>
							<asp:ListItem Value="7">�ʻ��������ָ��������ת�����ʻ�</asp:ListItem>
							<asp:ListItem Value="8">����ת�Է��ʻ���ʹ�Է���ָ��������ת�����ʻ�</asp:ListItem>
							<asp:ListItem Value="9">ת�ƶ�������Է��ʻ�</asp:ListItem>
							<asp:ListItem Value="10">T+0����</asp:ListItem>
						</asp:dropdownlist></FONT></TD>
					<TD>
						<P><FONT face="����">״̬
								<asp:dropdownlist id="ddl_state" runat="server" Width="96px">
									<asp:ListItem Value="-1">����</asp:ListItem>
									<asp:ListItem Value="1">�̻�����</asp:ListItem>
									<asp:ListItem Value="2">�̻�����</asp:ListItem>
									<asp:ListItem Value="3">�Ƹ�ͨ����ͨ��</asp:ListItem>
									<asp:ListItem Value="4">�ܾ�</asp:ListItem>
									<asp:ListItem Value="5">����</asp:ListItem>
								</asp:dropdownlist></FONT></P>
					</TD>
					<TD><FONT face="����">&nbsp;<asp:button id="btn_query" runat="server" Width="58px" BackColor="InactiveCaptionText" Text="��ѯ" onclick="btn_query_Click"></asp:button></FONT></TD>
				</tr>
				<TR>
					<TD colSpan="5"><FONT face="����"><asp:label id="lb_msg" runat="server" Width="720px" ForeColor="Red"></asp:label>&nbsp;&nbsp;</FONT></TD>
				</TR>
				<tr>
					<td colSpan="5"><asp:datagrid id="DataGrid1" runat="server" Width="100%" GridLines="Horizontal" AutoGenerateColumns="False"
							CellPadding="3" BackColor="White" BorderStyle="None" BorderColor="#E7E7FF" BorderWidth="1px" Height="122px">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fno" HeaderText="���"></asp:BoundColumn>
								<asp:BoundColumn DataField="FSpid" HeaderText="�̻���"></asp:BoundColumn>
								<asp:BoundColumn DataField="FSpidName" HeaderText="�̻���"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fuser" HeaderText="�û��ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="FuserName" HeaderText="�û���"></asp:BoundColumn>
								<asp:BoundColumn DataField="FPriTypeS" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="FAmountSetS" HeaderText="ָ�����(��)"></asp:BoundColumn>
								<asp:BoundColumn DataField="FUserIn" HeaderText="ת���ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="FUserNameIn" HeaderText="ת���ʺ���"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStateS" HeaderText="״̬"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td colSpan="5"><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��"
							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" PageSize="15"></webdiyer:aspnetpager></td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
