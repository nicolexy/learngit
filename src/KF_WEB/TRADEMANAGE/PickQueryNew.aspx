<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="PickQueryNew.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.PickQueryNew" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PickQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 3%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;���ּ�¼��ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">��ʼ����</asp:label></TD>
					<TD><asp:textbox id="TextBoxBeginDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label3" runat="server">��������</asp:label></TD>
					<TD><asp:textbox id="TextBoxEndDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox></TD>
				</TR>
				<%--<TR>
					<TD align="right"><asp:label id="Label5" runat="server">��ѯ״̬</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlStateType" runat="server" Width="152px">
							<asp:ListItem Value="0" Selected="True">����״̬</asp:ListItem>
							<asp:ListItem Value="1">����ɹ�</asp:ListItem>
							<asp:ListItem Value="2">����ʧ��</asp:ListItem>
							<asp:ListItem Value="3">�ȴ�����</asp:ListItem>
							<asp:ListItem Value="4">������</asp:ListItem>
							<asp:ListItem Value="5">����</asp:ListItem>
						</asp:DropDownList></TD>
					<TD align="right"><asp:label id="Label6" runat="server">����޶�</asp:label></TD>
					<TD><asp:textbox id="tbFNum" runat="server">0</asp:textbox>
						<asp:regularexpressionvalidator id="Regularexpressionvalidator7" runat="server" ValidationExpression="^[0-9/.]+"
							ToolTip="**.**" ErrorMessage="RegularExpressionValidator" ControlToValidate="tbFNum" Display="Dynamic">��������ȷ���</asp:regularexpressionvalidator></TD>
				</TR>--%>
				<TR>
					<TD align="right" colSpan="1" rowSpan="1">
						<asp:DropDownList id="ddlIDType" runat="server">
							<asp:ListItem Value="0" Selected="True">�ʺ�</asp:ListItem>
							<asp:ListItem Value="1">�����ʺ�</asp:ListItem>
							<asp:ListItem Value="2">���ֵ���</asp:ListItem>
						</asp:DropDownList></TD>
					<TD><asp:textbox id="tbQQID" runat="server"></asp:textbox></TD>
					<TD align="right"><FONT face="����">
							<%--<asp:Label id="Label7" runat="server">��������</asp:Label></FONT></TD>--%>
					<TD align="left">
						<%--<asp:DropDownList id="ddlSortType" runat="server" Width="152px">
							<asp:ListItem Value="0" Selected="True">������</asp:ListItem>
							<asp:ListItem Value="1">ʱ��С����</asp:ListItem>
							<asp:ListItem Value="2">ʱ���С</asp:ListItem>
							<asp:ListItem Value="3">���С����</asp:ListItem>
							<asp:ListItem Value="4">����С</asp:ListItem>
						</asp:DropDownList>--%></TD>
				</TR>
				<%--<TR>
					<TD align="right" colSpan="1"><asp:label id="Label8" runat="server">��������</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlBankType" runat="server"></asp:DropDownList></TD>
					<TD align="right" colSpan="1"><asp:label id="Label9" runat="server">��������</asp:label></TD>
					<TD>
						<asp:DropDownList id="ddlCashType" runat="server"></asp:DropDownList></TD>
				</TR>--%>
                <TR>
					<TD align="center" colspan="4"><FONT face="����"><asp:button id="Button1" runat="server" Width="80px" Text="�� ѯ" onclick="Button2_Click"></asp:button></FONT></TD>
				</TR>
                  <TR>
					<TD align="center" colspan="4">
                        <FONT face="����" color="red">
                             1��ʱ����ֻ֧�ְ���Ȼ�²�ѯ����֧�ֿ��²�ѯ<br />
                            2��ͨ�����п������ּ�¼��ֻ֧�ֲ�2015��5��01��֮������ݣ�2015��5��1��֮ǰ�����������˺Ż����ֵ��Ų�ѯ�� 
                        </FONT>

					</TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 210px; HEIGHT: 70%"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" EnableViewState="False">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Faid" HeaderText="�ʺ�ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="Facc_name" HeaderText="����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_name" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fnum_str" HeaderText="���ֽ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcharge_str" HeaderText="������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fabank_type_str" HeaderText="��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_type_str" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="FaBankID" HeaderText="�����ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fpay_front_time" HeaderText="����ʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStateName" HeaderText="����״̬"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fsign_str" HeaderText="��Ʊ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmemo" HeaderText="��Ʊԭ��"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="��ϸ����">
									<ItemTemplate>
										<a href = 'PickQuery_Detail.aspx?listid=<%# DataBinder.Eval(Container, "DataItem.FlistID")%>&begintime=<%=begintime %>&endtime=<%=endtime %>'>
											��ϸ����</a>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
							SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
