<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="NoticeBankByEmail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.NoticeBankByEmail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>公告发邮件</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <TABLE  style="Z-INDEX: 101; LEFT: 5.02%;POSITION: relative; TOP: 20px; " cellSpacing="1" cellPadding="1" width="500"
				border="1">
                <TR>
                     <TD align="center"><asp:button id="ButtonContacts" runat="server" Width="100px" Text="选择联系人分组" onclick="btnContacts_Click"></asp:button></TD>
                      <TD><asp:label id="tbGroup" runat="server" TextMode="MultiLine" Height="150px"></asp:label></TD>
                </TR>
                 <TR>
                      <TD colspan="2" align="center"><asp:button id="btnSendMail" Visible="false" runat="server" Width="80px" Text="发送邮件" onclick="btnSendMail_Click"></asp:button></TD>
                </TR>
			</TABLE>
			<TABLE id="TableGroup" style="Z-INDEX: 102; LEFT: 5.02%;POSITION: relative; TOP: 50px; "
				cellSpacing="1" cellPadding="1" width="400" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGridGroup" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                            	<asp:BoundColumn DataField="Fid" HeaderText="Fid" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FgroupName" HeaderText="FgroupName" Visible="false"></asp:BoundColumn>
                                <asp:templatecolumn HeaderText="选择">
                                     <itemtemplate>
                                          <input id="selectGroupName" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "FgroupName")%>' runat="server"/>
                                          <input id="selectGroupId" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "Fid")%>' runat="server"/>
                                          <asp:CheckBox  id="selGroup" runat="server"/>
                                     </itemtemplate>
                                 </asp:templatecolumn>
                                 <asp:TemplateColumn HeaderText="组名">
									<ItemTemplate>
										<asp:LinkButton id="lbQur" runat="server" CommandName="QueryOneGroup" Text='<%# DataBinder.Eval(Container, "DataItem.FgroupName") %>'></asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
                                </Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <tr>
                <td align="left">
                 <asp:button id="btnSubmitGroup" runat="server" Text="确定" Width="80px" onclick="btnSubmitGroup_Click"></asp:button>
                </td>
                </TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" PageSize="5"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
            <TABLE id="TableContacts" visible="false" style="Z-INDEX: 103; LEFT: 5.02%;POSITION:relative; TOP: 50px; "
				cellSpacing="1" cellPadding="1" width="500" border="1" runat="server">
				<tr>
                <td>
               <Font style="font-weight:bold"> <asp:label id="contb1" runat="server">组名：</asp:label>
                <asp:label id="conGroupName" runat="server"></asp:label></Font>
                </td>
                </tr>
                <TR>
					<TD vAlign="top"><asp:datagrid id="DataGridContacts" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                            	<asp:BoundColumn DataField="Fid" HeaderText="Fid" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fname" HeaderText="姓名"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Femail" HeaderText="电子邮件地址"></asp:BoundColumn>
                                </Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pagerContacts" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" PageSize="5"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
                </TABLE>
		</form>
	</body>
</HTML>
