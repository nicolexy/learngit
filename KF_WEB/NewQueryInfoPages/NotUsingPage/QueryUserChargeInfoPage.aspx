<%@ Page language="c#" Codebehind="QueryUserChargeInfoPage.aspx.cs" AutoEventWireup="false" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryUserChargeInfoPage" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryUserChargeInfoPage</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE border="1" cellSpacing="1" cellPadding="1" width="900">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;��ѯ<asp:label id="lb_pageTitle" Runat="server">Ͷ����ǩԼ��Ϣ</asp:label></FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD bgColor="#e4e5f7" align="right">��ѡ��Ҫ��������Ϣ��Ŀ��</TD>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:dropdownlist id="dd_querySubject" Runat="server" AutoPostBack="True">
							<asp:ListItem Value="Ͷ����ǩԼ��Ϣ" Selected="True" />
							<asp:ListItem Value="Ͷ���˽�Լ��Ϣ" Selected="False" />
							<asp:ListItem Value="Ͷ���˳�ֵ��Ϣ" Selected="False" />
							<asp:ListItem Value="Ͷ����������Ϣ" Selected="False" />
							<asp:ListItem Value="Ͷ���˳�����Ϣ" Selected="False" />
							<asp:ListItem Value="Ͷ���˽�����Ϣ" Selected="False" />
						</asp:dropdownlist></TD>
				<TR>
				<TR>
					<TD align="right"><asp:radiobutton id="rtnList" Runat="server" GroupName="rtnChoose" Text="���ݽ��׵��Ų�ѯ"></asp:radiobutton></TD>
					<td><asp:label id="lb_listID" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;���׵��ţ�</asp:label><asp:textbox id="tbx_tradeID" Width="350px" Runat="server"></asp:textbox></td>
					<td><label>��ѯ���ڣ�</label><asp:textbox id="tbx_findDate_byID" Runat="server"></asp:textbox><asp:imagebutton id="imgbtn_findDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></td>
				</TR>
				<tr>
					<TD rowSpan="3" align="right"><asp:radiobutton id="rtbSpid" Runat="server" GroupName="rtnChoose" Text="��ϸ��ѯ" Checked="True"></asp:radiobutton></TD>
					<TD><FONT face="����">&nbsp;&nbsp;&nbsp; </FONT>
						<asp:label id="Label3" runat="server">&nbsp;&nbsp;��ѯ����</asp:label><asp:textbox id="tbx_findDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
				</tr>
				<tr>
					<td><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT>
						<asp:label id="lb_1" runat="server">ǩԼ��&nbsp;</asp:label><asp:textbox id="tbx_1" Runat="server">0</asp:textbox></td>
					<td><asp:label id="lb_2" runat="server">֤����&nbsp;&nbsp;&nbsp;</asp:label><asp:textbox id="tbx_2" Runat="server">0</asp:textbox></td>
				</tr>
				<tr>
					<td><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT>
						<asp:label id="lb_3" runat="server">����&nbsp;&nbsp;&nbsp;&nbsp;</asp:label><asp:textbox id="tbx_3" Runat="server">0</asp:textbox></td>
					<td><asp:label id="lb_4" runat="server">״̬&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:label><asp:textbox id="tbx_4" Runat="server">0</asp:textbox></td>
				</tr>
				<tr>
					<TD colSpan="4" align="center"><asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ"></asp:button></TD>
				</tr>
			</TABLE>
			<table border="0" cellSpacing="0" cellPadding="0" width="900">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="900px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="listid" HeaderText="�Ƹ�ͨ������">
									<HeaderStyle Width="200px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="spListid" HeaderText="�̻�������">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="TimeStr" HeaderText="����ʱ��">
									<HeaderStyle Width="80px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="total" HeaderText="���">
									<HeaderStyle Width="80px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="type" HeaderText="����">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="status" HeaderText="״̬">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="ת��" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
