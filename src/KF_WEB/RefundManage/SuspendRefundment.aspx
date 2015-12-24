<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="SuspendRefundment.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.SuspendRefundment" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SuspendRefundment</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
		<script language="javascript">					
			function select_deselectAll (chkVal, idVal) 
			{
				var frm = document.forms[0];
				for (i=0; i<frm.length; i++) 
				{
					if (idVal.indexOf ('CheckAll') != -1)
					{
						if(frm.elements[i].id.indexOf('CheckBox') != -1)
						{
							if(chkVal == true) 
							{
								frm.elements[i].checked = true;
							} 
							else 
							{
								frm.elements[i].checked = false;
							}
						}
					} 
					else if (idVal.indexOf('DeleteThis') != -1) 
					{
						if(frm.elements[i].checked == false) 
						{
							frm.elements[1].checked = false;
						}
					}
				}
			}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; POSITION: absolute; TOP: 5%; LEFT: 5%" cellSpacing="1"
				cellPadding="1" width="85%" border="1">
				<TR>
					<TD bgColor="#e4e5f7" colSpan="3"><FONT face="����" color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;�̻����ٽ����˿������(������������ݾͲ�����ܵ��˿�������)</FONT> </FONT></TD>
					<TD align="right" bgColor="#e4e5f7"><FONT face="����">����Ա����: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label4" runat="server">���׵�ID</asp:label></TD>
					<TD style="WIDTH: 325px"><asp:textbox id="tbTransID" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right">
						<asp:Label id="Label11" runat="server">�����̻�</asp:Label></TD>
					<TD><asp:textbox id="tbSPID" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">��ʼ����</asp:label></TD>
					<TD style="WIDTH: 325px"><FONT face="����">
                        <asp:textbox id="TextBoxBeginDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
                        </FONT></TD>
					<TD align="right"><asp:label id="Label3" runat="server">��������</asp:label></TD>
					<TD>
                        <asp:textbox id="TextBoxEndDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
					</TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label8" runat="server">��������</asp:label></TD>
					<TD style="WIDTH: 325px">
						<asp:DropDownList id="ddlBankType" runat="server" Width="152px"></asp:DropDownList></TD>
					<TD align="right"><asp:label id="Label6" runat="server">ҵ��״̬</asp:label></TD>
					<TD><asp:dropdownlist id="ddlStatus" runat="server" Width="152px">
							<asp:ListItem Value="99">����״̬</asp:ListItem>
							<asp:ListItem Value="1">������</asp:ListItem>
							<asp:ListItem Value="2">������</asp:ListItem>
							<asp:ListItem Value="3">����ʧ��</asp:ListItem>
							<asp:ListItem Value="4">�˿�ɹ�</asp:ListItem>
							<asp:ListItem Value="5">�˿�ʧ��</asp:ListItem>
							<asp:ListItem Value="6">��������</asp:ListItem>
							<asp:ListItem Value="7">ת�����</asp:ListItem>
							<asp:ListItem Value="8">�ݲ�����</asp:ListItem>
							<asp:ListItem Value="9">�˿�������</asp:ListItem>
							<asp:ListItem Value="10">ת������ɹ�</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label10" runat="server">���ֵ�ID</asp:label></TD>
					<TD style="WIDTH: 325px"><asp:textbox id="tbDrawID" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label7" runat="server">����ʺ�</asp:label></TD>
					<TD><asp:textbox id="tbBuyerID" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label5" runat="server">����״̬</asp:label></TD>
					<TD style="WIDTH: 325px">
						<asp:DropDownList id="ddlSumType" runat="server" Width="152px">
							<asp:ListItem Value="99" Selected="True">����״̬</asp:ListItem>
							<asp:ListItem Value="0">��δ����</asp:ListItem>
							<asp:ListItem Value="1">�Ѿ�����</asp:ListItem>
							<asp:ListItem Value="2">��ͣ�˿����</asp:ListItem>
						</asp:DropDownList></TD>
					<td align="right">
						<asp:label style="Z-INDEX: 0" id="Label9" runat="server">�˿�����</asp:label></td>
					<TD>
						<asp:dropdownlist style="Z-INDEX: 0" id="ddlrefund_type" runat="server" Width="152px">
							<asp:ListItem Value="3">����ֱ���˿�</asp:ListItem>
							<asp:ListItem Value="9">��ֵ���˿�</asp:ListItem>
						</asp:dropdownlist>
						<asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ" onclick="btnQuery_Click"></asp:button>
						<asp:Button id="btnSuspend" runat="server" Text="���볷��" onclick="btnSuspend_Click"></asp:Button></TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; POSITION: absolute; WIDTH: 85%; TOP: 40%; LEFT: 5.02%"
				cellSpacing="1" cellPadding="1" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
							CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" PageSize="50">
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<Columns>
								<asp:HyperLinkColumn DataNavigateUrlField="FSpID" DataNavigateUrlFormatString="../BaseAccount/ModifyMedi.aspx?fqqid={0}"
									DataTextField="FSpID" HeaderText="SPID"></asp:HyperLinkColumn>
								<asp:TemplateColumn HeaderText="���׵�">
									<ItemTemplate>
										<a href='<%#  GetTransUrl(DataBinder.Eval(Container, "DataItem.Ftransaction_id").ToString()) %>' >
											<%# DataBinder.Eval(Container, "DataItem.Ftransaction_id") %>
										</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Fbuyid" HeaderText="����ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Frp_feeName" HeaderText="����ҽ��" DataFormatString="{0:N}"></asp:BoundColumn>
								<asp:BoundColumn DataField="Frb_feeName" HeaderText="�����ҷ���"></asp:BoundColumn>
								<asp:BoundColumn DataField="Frefund_typeName" HeaderText="�˿�����"></asp:BoundColumn>
								<asp:BoundColumn DataField="FstatusName" HeaderText="ҵ��״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ftrue_name" HeaderText="�ʻ�����"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Ftransaction_id"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Fstatus"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="ѡ��">
									<HeaderTemplate>
										<INPUT id="CheckAll" onclick="return select_deselectAll(this.checked,this.id)" type="checkbox"><LABEL>ѡ��</LABEL>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:CheckBox id="CheckBox1" runat="server" Text="ѡ��" Visible="False"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="��ϸ">
									<ItemTemplate>
										<a href='<%# String.Format("../TradeManage/B2cReturnQuery_Detail.aspx?tranid={0}&drawid={1}", DataBinder.Eval(Container, "DataItem.Ftransaction_id").ToString()
										, DataBinder.Eval(Container, "DataItem.Fdraw_id").ToString()) %>' >��ϸ </a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="Frefund_type"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Fdraw_id" HeaderText="Fdraw_id"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="fstandby1" HeaderText="fstandby1"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��"
							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager></TD>
				</TR>
				<TR>
					<TD align="center">
						<asp:Label id="Label12" runat="server"></asp:Label>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
