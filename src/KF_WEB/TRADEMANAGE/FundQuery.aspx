<%@ Page language="c#" Codebehind="FundQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.FundQuery" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FundQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	.style5 { COLOR: #000000 }
	.style6 { COLOR: #ff0000 }
		</style>
		<script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 2.51%; POSITION: absolute; TOP: 168px; HEIGHT: 0.93%"
				cellSpacing="0" cellPadding="0" width="95%" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="LightGray" BorderWidth="1px" BackColor="White"
							CellPadding="3" AutoGenerateColumns="False" PageSize="50" EnableViewState="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#FFFFFF"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="Black" BackColor="Silver"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fbank_list" HeaderText="�����ж�����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Faid" HeaderText="�ʺ�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FListID" HeaderText="�տID����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Faname" HeaderText="����"></asp:BoundColumn>
								<asp:BoundColumn DataField="FbankName" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fpay_front_time" HeaderText="��ֵʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="FNewnum" HeaderText="��ֵ���"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStateName" HeaderText="��ֵ״̬"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="��ϸ����">
									<ItemTemplate>
										<a href = 'FUndQuery_Detail.aspx?tdeid=<%# DataBinder.Eval(Container, "DataItem.FTde_ID")%>&begintime=<%=begintime %>&endtime=<%=endtime %>&listid=<%# DataBinder.Eval(Container, "DataItem.Flistid")%>&fpay_front_time=<%# DataBinder.Eval(Container, "DataItem.Fpay_front_time")%>&Fbank_list=<%# DataBinder.Eval(Container, "DataItem.Fbank_list")%>&Fbank_type=<%# DataBinder.Eval(Container, "DataItem.Fbank_type")%>&FHistoryFlag=<%# DataBinder.Eval(Container, "DataItem.FHistoryFlag")%>'>
											��ϸ����</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="FlistId" HeaderText="FlistId"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<tr>
					<td><asp:label id="Label9" runat="server">��ѯ�ܼ�¼����</asp:label>
						<asp:label id="labCountNum" runat="server" Width="100"></asp:label>
						<asp:label id="Label10" runat="server">��ѯ�ܽ�</asp:label>
						<asp:label id="labAmount" runat="server" Width="100"></asp:label></td>
				</tr>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" PageSize="50" NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��"
							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" CustomInfoTextAlign="Center"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
			<TABLE id="Table1" style="Z-INDEX: 103; LEFT: 2.25%; POSITION: absolute; TOP: 1.52%; HEIGHT: 106px"
				cellSpacing="0" cellPadding="0" width="95%" border="1">
				<TR>
					<TD style="WIDTH: 100%" background="../IMAGES/Page/bg_bl.gif" bgColor="#e4e5f7" colSpan="5">
						<DIV align="center">
							<TABLE id="Table3" height="100%" cellSpacing="0" cellPadding="1" width="100%" border="0">
								<TR>
									<TD width="79%"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;
												<asp:Label id="Label8" runat="server" Width="192px">Label</asp:Label></FONT></FONT></TD>
									<TD width="21%"><FONT face="����">&nbsp;</FONT>����Ա����: <SPAN class="style3">
											<asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
								</TR>
							</TABLE>
							<SPAN class="style3"></SPAN>
						</DIV>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 91px" align="right">
						<asp:label id="Label2" runat="server">��ʼ����</asp:label></TD>
					<TD style="WIDTH: 290px">
						<asp:textbox id="TextBoxBeginDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',maxDate:'#F{$dp.$D(\'TextBoxEndDate\')}'})" Width="160px" CssClass="Wdate" BorderStyle="Groove"></asp:textbox>
					</TD>
					<TD align="right">
						<asp:label id="Label3" runat="server">��������</asp:label></TD>
					<TD>
						<asp:textbox id="TextBoxEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',minDate:'#F{$dp.$D(\'TextBoxBeginDate\')}'})" Width="160px" CssClass="Wdate" BorderStyle="Groove"></asp:textbox>
					</TD>
				</TR>
				<%--<TR>
					<TD style="WIDTH: 91px; HEIGHT: 25px" align="right">
						<asp:label id="Label5" runat="server">��ѯ״̬</asp:label></TD>
					<TD style="WIDTH: 290px; HEIGHT: 25px">
						<asp:dropdownlist id="ddlStateType" runat="server" Width="152px" AutoPostBack="True">
							<asp:ListItem Value="0" Selected="True">����״̬</asp:ListItem>
							<asp:ListItem Value="1">����ɹ�</asp:ListItem>
							<asp:ListItem Value="2">����ʧ��</asp:ListItem>
							<asp:ListItem Value="3">�ȴ�����</asp:ListItem>
							<asp:ListItem Value="4">������</asp:ListItem>
							<asp:ListItem Value="5">����</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD style="HEIGHT: 25px" align="right">
						<asp:label id="Label6" runat="server">����޶�</asp:label></TD>
					<TD style="HEIGHT: 25px">
						<asp:textbox id="tbFNum" runat="server" Width="88px" BorderStyle="Groove">0.00</asp:textbox><FONT face="����">-
							<asp:textbox id="txbNumMax" runat="server" Width="88px" BorderStyle="Groove">20000000.00</asp:textbox>Ԫ</FONT>
						<asp:regularexpressionvalidator id="Regularexpressionvalidator7" runat="server" ValidationExpression="^[0-9/.]+"
							ToolTip="**.**" ErrorMessage="RegularExpressionValidator" ControlToValidate="tbFNum" Display="Dynamic">��������ȷ���</asp:regularexpressionvalidator>
						<asp:regularexpressionvalidator id="Regularexpressionvalidator1" runat="server" ValidationExpression="^[0-9/.]+"
							ToolTip="**.**" ErrorMessage="RegularExpressionValidator" ControlToValidate="txbNumMax" Display="Dynamic">��������ȷ���</asp:regularexpressionvalidator></TD>
				</TR>--%>
				<TR>
					<TD style="WIDTH: 91px" align="right">
						<asp:dropdownlist id="dpLst" runat="server" AutoPostBack="True" onselectedindexchanged="dpLst_SelectedIndexChanged">
							<asp:ListItem Value="qq">���ʺ�</asp:ListItem>
							<asp:ListItem Value="czd">��ֵ����</asp:ListItem>
							<asp:ListItem Value="toBank" Selected="True">�����еĶ�����</asp:ListItem>
							<asp:ListItem Value="BankBack">���з��ض�����</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD style="WIDTH: 290px">
						<asp:textbox id="tbQQID" runat="server" Width="165px" BorderStyle="Groove"></asp:textbox>
						<asp:requiredfieldvalidator id="rfvNullCheck" runat="server" ErrorMessage="����Ϊ��" ControlToValidate="tbQQID"
							Display="Dynamic" Enabled="False"></asp:requiredfieldvalidator>
						<asp:regularexpressionvalidator id="revNumOnly" runat="server" ValidationExpression="^[0-9 ]{10,30}" ErrorMessage="�Ƿ���ֵ����"
							ControlToValidate="tbQQID" Display="Dynamic" Enabled="False"></asp:regularexpressionvalidator></TD>
					<%--<TD align="right"><FONT face="����">
							<asp:label id="Label4" runat="server">��������</asp:label></FONT></TD>
					<TD align="left">
						<asp:dropdownlist id="ddlSortType" runat="server" Width="152px" AutoPostBack="True">
							<asp:ListItem Value="1">ʱ��С����</asp:ListItem>
							<asp:ListItem Value="2">ʱ���С</asp:ListItem>
							<asp:ListItem Value="3">���С����</asp:ListItem>
							<asp:ListItem Value="4">����С</asp:ListItem>
						</asp:dropdownlist></TD>--%>
                    <TD align="center" colspan="2"><FONT face="����">
							<asp:CheckBox id="CheckBox1" runat="server" Text="��ʷ��¼"></asp:CheckBox>
							<asp:button id="Button2" runat="server" Width="80px" Text="�� ѯ" onclick="Button2_Click"></asp:button></FONT></TD>
				</TR>
				<TR>
				<%--	<TD style="WIDTH: 91px" align="right">
						<asp:Label id="Label7" runat="server">��ֵ����</asp:Label></TD>
					<TD style="WIDTH: 290px">
						<asp:DropDownList id="ddlBankType" runat="server"></asp:DropDownList></TD>--%>
					
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
