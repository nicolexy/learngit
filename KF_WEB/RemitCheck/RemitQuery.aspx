<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="RemitQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RemitCheck.RemitQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RemitQuery</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style4 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
						var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
						if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
					}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="����">
				<TABLE style="Z-INDEX: 101; POSITION: absolute; WIDTH: 94%; TOP: 5%; LEFT: 3%" id="Table1"
					border="1" cellSpacing="1" cellPadding="1" width="648">
					<TR height="24" bgColor="#eeeeee">
						<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG src="../IMAGES/Page/post.gif" width="15" height="16"><asp:label id="Label9" runat="server">��ѯ����</asp:label><FONT face="����"></FONT></SPAN></FONT></STRONG></TD>
					</TR>
					<TR>
						<TD colSpan="2">
							<TABLE id="Table2" border="1" cellSpacing="1" cellPadding="1" width="100%">
								<TR>
									<TD><asp:label id="Label8" runat="server" style="Z-INDEX: 0">�̻���</asp:label></TD>
									<TD><FONT face="����">
											<asp:DropDownList id="ddlSpid" runat="server" Width="104px" style="Z-INDEX: 0"></asp:DropDownList></FONT></TD>
									<TD><asp:label id="Label6" runat="server" style="Z-INDEX: 0">��������</asp:label></TD>
									<TD><asp:dropdownlist id="ddlDataType" runat="server" Width="104px" style="Z-INDEX: 0">
											<asp:ListItem Value="99" Selected="True">��������</asp:ListItem>
											<asp:ListItem Value="1">���ֱ�ӳɹ�</asp:ListItem>
											<asp:ListItem Value="2">������</asp:ListItem>
											<asp:ListItem Value="3">���ʧ��</asp:ListItem>
											<asp:ListItem Value="4">�������ɹ�</asp:ListItem>
											<asp:ListItem Value="5">�������ʧ��</asp:ListItem>
											<asp:ListItem Value="6">�˻�ɹ�</asp:ListItem>
											<asp:ListItem Value="7">�˻����</asp:ListItem>
											<asp:ListItem Value="8">�˻�ʧ��</asp:ListItem>
											<asp:ListItem Value="9">�˻�����ɹ�</asp:ListItem>
											<asp:ListItem Value="10">�˻�����ʧ��</asp:ListItem>
											<asp:ListItem Value="11">�Ļ�ɹ�</asp:ListItem>
											<asp:ListItem Value="12">�Ļ����</asp:ListItem>
											<asp:ListItem Value="13">�Ļ�ʧ��</asp:ListItem>
											<asp:ListItem Value="14">�Ļ����ɹ�</asp:ListItem>
											<asp:ListItem Value="15">�Ļ����ʧ��</asp:ListItem>
											<asp:ListItem Value="16">�����˻�</asp:ListItem>
											<asp:ListItem Value="17">�ʴ������˻�</asp:ListItem>
										</asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="Label5" runat="server">��������</asp:label></TD>
									<TD>
										<asp:dropdownlist id="ddlTrantype" runat="server" Width="104px">
											<asp:ListItem Value="99" Selected="True">��������</asp:ListItem>
											<asp:ListItem Value="1">���</asp:ListItem>
											<asp:ListItem Value="2">�˻�</asp:ListItem>
											<asp:ListItem Value="3">�Ļ�</asp:ListItem>
											<asp:ListItem Value="4">�����˻�</asp:ListItem>
										</asp:dropdownlist></TD>
									<TD><asp:label id="Label4" runat="server" style="Z-INDEX: 0">����״̬</asp:label></TD>
									<TD><asp:dropdownlist id="ddlTranState" runat="server" Width="104px">
											<asp:ListItem Value="99" Selected="True">����״̬</asp:ListItem>
											<asp:ListItem Value="1">�ɹ�</asp:ListItem>
											<asp:ListItem Value="2">ʧ��</asp:ListItem>
											<asp:ListItem Value="3">����</asp:ListItem>
											<asp:ListItem Value="4">�����ɹ�</asp:ListItem>
											<asp:ListItem Value="5">�����ʧ��</asp:ListItem>
										</asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="Label3" runat="server">�������</asp:label></TD>
									<TD>
										<asp:dropdownlist id="ddlRemitType" runat="server" Width="104px">
											<asp:ListItem Value="99" Selected="True">��������</asp:ListItem>
											<asp:ListItem Value="1">����ַ���</asp:ListItem>
											<asp:ListItem Value="2">��������</asp:ListItem>
											<asp:ListItem Value="3">���˻��</asp:ListItem>
										</asp:dropdownlist></TD>
									<TD></TD>
									<TD></TD>
								</TR>
								<TR>
									<TD><asp:label id="Label17" runat="server">��Ʊ��</asp:label></TD>
									<TD><asp:textbox id="tbRemitRec" runat="server" Width="200px"></asp:textbox></TD>
									<TD><asp:label id="Label1" runat="server">���׵���</asp:label></TD>
									<td><asp:textbox id="tblistID" runat="server" Width="200px"></asp:textbox>
										<asp:Button id="btnQuery" runat="server" Text="��ѯ" onclick="btnQuery_Click"></asp:Button></td>
								</TR>
								<tr>
									<TD></TD>
									<TD></TD>
									<td colSpan="2" align="center"></td>
								</tr>
							</TABLE>
						</TD>
					</TR>
					<TR height="24" bgColor="#eeeeee">
						<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG src="../IMAGES/Page/post.gif" width="15" height="16"><asp:label id="Label15" runat="server">��ϸ��Ϣ</asp:label><FONT face="����"></FONT></SPAN></FONT></STRONG></TD>
					</TR>
					<TR>
						<TD vAlign="top" colSpan="2"><asp:datagrid id="DataGrid1" runat="server" Width="100%" Height="70px" AutoGenerateColumns="False"
								BackColor="White" CellPadding="3" GridLines="Horizontal" BorderColor="#E7E7FF" BorderWidth="1px" BorderStyle="None">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:HyperLinkColumn DataNavigateUrlField="flistid" DataNavigateUrlFormatString="../TradeManage/TradeLogQuery.aspx?id={0}"
										DataTextField="flistid" HeaderText="���׵���"></asp:HyperLinkColumn>
									<asp:BoundColumn Visible="False" DataField="Fbatchid" HeaderText="���κ�"></asp:BoundColumn>
									<asp:BoundColumn DataField="Ford_date" HeaderText="���ƽ̨����"></asp:BoundColumn>
									<asp:BoundColumn DataField="Ford_ssn" HeaderText="���ƽ̨��ˮ"></asp:BoundColumn>
									<asp:BoundColumn DataField="FremitfeeName" HeaderText="�����"></asp:BoundColumn>
									<asp:BoundColumn DataField="FremitpayfeeName" HeaderText="���"></asp:BoundColumn>
									<asp:BoundColumn DataField="FprocedureName" HeaderText="���������"></asp:BoundColumn>
									<asp:BoundColumn DataField="FotherprocedureName" HeaderText="��/�Ļ�������"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fremit_rec" HeaderText="��Ʊ��"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fdest_name" HeaderText="�տ���"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fdest_card" HeaderText="���п�"></asp:BoundColumn>
									<asp:BoundColumn DataField="FtrantypeName" HeaderText="��������"></asp:BoundColumn>
									<asp:BoundColumn DataField="FtranstateName" HeaderText="����״̬"></asp:BoundColumn>
									<asp:BoundColumn DataField="FremittypeName" HeaderText="�������"></asp:BoundColumn>
									<asp:BoundColumn DataField="FdatatypeName" HeaderText="��������"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fstate" HeaderText="�һ�״̬"></asp:BoundColumn>
									<asp:TemplateColumn Visible="False" HeaderText="����">
										<ItemTemplate>
											<asp:CheckBox id="CheckBox1" runat="server" Text="ѡ��"></asp:CheckBox>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR>
						<TD colSpan="2"><webdiyer:aspnetpager id="pager" runat="server" PageSize="15" AlwaysShow="True" NumericButtonCount="5"
								ShowCustomInfoSection="left" PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"
								OnPageChanged="ChangePage" SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
					</TR>
					<TR>
						<TD colSpan="2">
							<TABLE id="Table3" border="0" cellSpacing="0" cellPadding="0" width="100%">
								<TR>
									<TD width="20%" align="center"></TD>
									<TD width="20%" align="center"></TD>
									<TD width="20%" align="center"></TD>
									<TD width="20%" align="center"></TD>
									<TD width="20%" align="center"></TD>
								</TR>
							</TABLE>
							<asp:label id="labErrMsg" runat="server" ForeColor="Red"></asp:label></TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
