<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="UserAppeal.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.UserAppeal" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>UserAppeal</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" id="Table1" border="1"
				cellSpacing="1" cellPadding="1" width="85%">
				<TR>
					<TD bgColor="#e4e5f7" colSpan="3"><FONT color="red" face="����"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">
							&nbsp;&nbsp;���ߺ�̨����</FONT> </FONT></TD>
					<TD bgColor="#e4e5f7" align="right"><FONT face="����">����Ա����: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">��ʼ����</asp:label></TD>
					<TD>
                        <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
					</TD>
					<TD align="right"><asp:label id="Label3" runat="server">��������</asp:label></TD>
					<TD>
                        <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 83px" align="right"><asp:label id="Label5" runat="server">�ʺ�</asp:label></TD>
					<TD><asp:textbox id="tbFuin" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label6" runat="server">��������</asp:label></TD>
					<TD><asp:dropdownlist id="ddlType" runat="server" Width="152px" AutoPostBack="True" onselectedindexchanged="ddlType_SelectedIndexChanged">
							<asp:ListItem Value="99" Selected="True">����������������</asp:ListItem>
							<asp:ListItem Value="0">�Ƹ��ܽ��</asp:ListItem>
							<asp:ListItem Value="1">�һ�����</asp:ListItem>
							<asp:ListItem Value="2">�޸�����</asp:ListItem>
							<asp:ListItem Value="3">�޸Ĺ�˾��</asp:ListItem>
							<asp:ListItem Value="4">ע���ʺ�</asp:ListItem>
							<asp:ListItem Value="5">����ע���û����������ֻ�</asp:ListItem>
							<asp:ListItem Value="6">��ע���û��������ֻ�</asp:ListItem>
							<asp:ListItem Value="7">����֤����</asp:ListItem>
							<asp:ListItem Value="9">�ֻ�����</asp:ListItem>
                            <asp:ListItem Value="10">����������</asp:ListItem>
							<asp:ListItem Value="20">ʵ����֤</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<%--<TD align="right"><asp:label id="Label8" runat="server">�ʺ�����</asp:label></TD>
					<TD><asp:dropdownlist id="ddlQQType" runat="server" Width="152px">
							<asp:ListItem Value="">��������</asp:ListItem>
							<asp:ListItem Value="0">�ǻ�Ա</asp:ListItem>
							<asp:ListItem Value="1">��ͨ��Ա</asp:ListItem>
							<asp:ListItem Value="2">VIP��Ա</asp:ListItem>
						</asp:dropdownlist></TD>--%>
					<td align="right">
						<asp:label id="Label9" runat="server">�������</asp:label></td>
					<td>
						<asp:dropdownlist id="DDL_DoType" runat="server" Width="152px" AutoPostBack="True">
							<asp:ListItem Value="9">����</asp:ListItem>
							<asp:ListItem Value="0" Selected="True">�˹�����</asp:ListItem>
							<asp:ListItem Value="1">�߷ֵ��Զ�ͨ��</asp:ListItem>
						</asp:dropdownlist></td>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label4" runat="server">����״̬</asp:label></TD>
					<TD><!--û���������߼�״̬��ֻ�ܸ��������ֶ��ж�--><asp:dropdownlist id="ddlState" runat="server" Width="152px">
							<asp:ListItem Value="99">����״̬</asp:ListItem>
							<asp:ListItem Value="0" Selected="True">δ����</asp:ListItem>
							<asp:ListItem Value="1">���߳ɹ�</asp:ListItem>
							<asp:ListItem Value="2">����ʧ��</asp:ListItem>
							<asp:ListItem Value="3">��������</asp:ListItem>
							<asp:ListItem Value="4">ֱ��ת��̨</asp:ListItem>
							<asp:ListItem Value="5">�쳣ת��̨</asp:ListItem>
							<asp:ListItem Value="6">���ʼ�ʧ��</asp:ListItem>
							<asp:ListItem Value="7">��ɾ��</asp:ListItem>
							<asp:ListItem Value="8">������״̬</asp:ListItem>
							<asp:ListItem Value="9">���ų���״̬</asp:ListItem>
							<asp:ListItem Value="10">ֱ�����߳ɹ�</asp:ListItem>
						</asp:dropdownlist><asp:dropdownlist id="ddlStateUserClass" runat="server" Width="152px">
							<asp:ListItem Value="99" Selected="True">ȫ��</asp:ListItem>
							<asp:ListItem Value="0">δ����</asp:ListItem>
							<asp:ListItem Value="1">������״̬</asp:ListItem>
							<asp:ListItem Value="2">��֤�ɹ�</asp:ListItem>
							<asp:ListItem Value="3">��֤ʧ��</asp:ListItem>
						</asp:dropdownlist><asp:label id="lblTotal" runat="server"></asp:label></TD>
					</TD>
					<TD align="right"><asp:label id="Label7" runat="server">����������</asp:label></TD>
					<td><asp:textbox id="txtCount" runat="server">50</asp:textbox>(���ֵΪ:50)
						<asp:button id="btnGet" runat="server" Width="80px" Text="�����쵥" onclick="btnGet_Click"></asp:button></td>
				</TR>
				<tr>
                  <TD align="right"><asp:label id="Label10" runat="server">��������</asp:label></TD>
					<TD><asp:dropdownlist id="ddlSortType" runat="server" Width="152px">
							<asp:ListItem Value="99">������</asp:ListItem>
							<asp:ListItem Value="0">ʱ��С����</asp:ListItem>
							<asp:ListItem Value="1">ʱ���С</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD colSpan="2" align="center"><asp:button id="Button2" runat="server" Width="80px" Text="�� ѯ" onclick="Button2_Click"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
				</tr>
			</TABLE>
			<TABLE style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 205px; HEIGHT: 60%"
				id="Table2" border="1" cellSpacing="1" cellPadding="1" width="808" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
							CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" OnItemDataBound="DataGrid1_ItemDataBound">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="FUin" HeaderText="�û��ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Femail" HeaderText="Email"></asp:BoundColumn>
								<asp:BoundColumn DataField="FTypeName" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStateName" HeaderText="����״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="FSubmitTime" HeaderText="����ʱ��" DataFormatString="{0:F}"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fpicktime" HeaderText="���ʱ��" DataFormatString="{0:F}"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCheckInfo" HeaderText="�����Ϣ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="balance" Visible="false" HeaderText="���"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="��ϸ����">
									<ItemTemplate>
										<%--<a href = '<%# DataBinder.Eval(Container, "DataItem.URL")%>' target=_blank>��ϸ����</a>--%>
                                        <asp:LinkButton id="queryButton" Visible="false" runat="server" CommandName="query"
                                            href = '<%# DataBinder.Eval(Container, "DataItem.URL")%>' target=_blank 
                                             Text="��ϸ����"></asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��"
							 HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
