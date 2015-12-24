<%@ Page language="c#" Codebehind="SettQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.SettQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SettQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body id="bodyid" runat="server">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="820" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�����ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">��ͬ</asp:label></TD>
					<TD><asp:textbox id="TextBoxId" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label3" runat="server">�̻�</asp:label></TD>
					<TD><asp:textbox id="TextBoxSpid" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label4" runat="server">�շ�ʵ����</asp:label></TD>
					<TD><asp:textbox id="TextBoxFeeNo" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:checkbox id="CheckBoxDate" runat="server" Text="�ϴν�������" Font-Bold="True"></asp:checkbox></TD>
					<TD>
                        <input type="text" runat="server" id="txtSettDate" onclick="WdatePicker()" />
                        <img onclick="txtSettDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
					</TD>
				</TR>
				<TR>
					<td></td>
					<td><asp:dropdownlist id="ListUseTag" runat="server" Width="155px"></asp:dropdownlist></td>
					<td align="center" colSpan="2"><asp:button id="btnSearch" Text="��ѯ" Runat="server" onclick="btnSearch_Click"></asp:button></td>
				</TR>
				<tr>
					<td></td>
				</tr>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH: 820px; POSITION: absolute; TOP: 150px; HEIGHT: 350px">
				<table cellSpacing="0" cellPadding="0" width="820" border="0">
					<tr>
						<TD vAlign="top" align="center"><asp:datagrid id="DataGrid1" runat="server" Width="1900px" ItemStyle-HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="15" AutoGenerateColumns="False" GridLines="Horizontal"
								CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" AllowPaging="True">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="ʵ����">
										<HeaderStyle Width="100px"></HeaderStyle>
										<ItemTemplate>
											<asp:HyperLink runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FCalculateNo") %>' NavigateUrl='#' ID="LinkId">
											</asp:HyperLink>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="FFeeContract" HeaderText="��ͬ���">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="�̻�">
										<HeaderStyle Width="150px"></HeaderStyle>
										<ItemTemplate>
											<asp:HyperLink runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FSpid") %>' NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.FSpid") %>' ID="LinkSpid">
											</asp:HyperLink>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="FChannelNo" HeaderText="����">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FProductType" HeaderText="��Ʒ">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FTransactionCount" HeaderText="�Ʒѱ���">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FTransactionAmount" HeaderText="�Ʒѽ��">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FAddCountA" HeaderText="Q����">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FAddCountB" HeaderText="��Ա��">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FCalculateAmount" HeaderText="������">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FDueAmount" HeaderText="Ӧ�ս��">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FFeeNo" HeaderText="�շ�ʵ����">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FPreDate" HeaderText="�ϴν�������">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FNextDate" HeaderText="��ǰ��������">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="���׻���">
										<HeaderStyle Width="80px"></HeaderStyle>
										<ItemTemplate>
											<asp:HyperLink runat="server" Text="�鿴" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.FSpid", "TranLogIncomeSum.aspx?spid={0}")+DataBinder.Eval(Container, "DataItem.FChannelNo", "&channel={0}")+DataBinder.Eval(Container, "DataItem.FPreDate", "&predate={0:yyyy-MM-dd}")+DataBinder.Eval(Container, "DataItem.FNextDate", "&nextdate={0:yyyy-MM-dd}") %>' ID="Hyperlink1">
											</asp:HyperLink>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="FAgentSPID" HeaderText="ƽ̨��">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
						</TD>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
