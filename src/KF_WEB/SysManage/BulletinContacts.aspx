<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="BulletinContacts.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.SysManage.BulletinContacts" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>通讯簿;联系人</title>
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
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 20px" cellSpacing="1" cellPadding="1" width="600"
				border="0">
                <TR>
                    <TD align="right"><asp:label id="lb" runat="server">组名：</asp:label></TD>
                     <TD><asp:textbox id="groupName" runat="server"></asp:textbox><Font color="red">*</Font></TD>
                     <TD align="left"><asp:button id="ButtonAddGroup" runat="server" Width="80px" Text="新增" onclick="btnAddGroup_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<TABLE id="TableGroup" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 50%; POSITION: relative; TOP: 50px; "
				cellSpacing="1" cellPadding="1" width="300" border="1" runat="server">
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
                                 <asp:TemplateColumn HeaderText="组名">
									<ItemTemplate>
										<asp:LinkButton id="lbQur" runat="server" CommandName="QueryOneGroup" Text='<%# DataBinder.Eval(Container, "DataItem.FgroupName") %>'></asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="删除">
									<ItemTemplate>
										<asp:LinkButton id="lbDelGroup" runat="server" CommandName="DeleteGroup">删除</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
                                </Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" PageSize="5"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
            <TABLE id="TableContacts" visible="false" style="Z-INDEX: 103; LEFT: 5.02%; WIDTH: 50%; POSITION:relative; top:70px;"
				cellSpacing="1" cellPadding="1" width="300" border="1" runat="server">
				<tr>
                <td>
               <Font style="font-weight:bold"> <asp:label id="contb1" runat="server">组名：</asp:label>
                <asp:label id="conGroupName" runat="server"></asp:label></Font>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:button id="ButtonAddContacts" runat="server" Width="80px" Text="添加新成员" onclick="btnAddContacts_Click"></asp:button>
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
                                  <asp:TemplateColumn HeaderText="删除">
									<ItemTemplate>
										<asp:LinkButton id="lbDelContacts" runat="server" CommandName="DeleteContacts">删除</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
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
            	<TABLE id="TableAddOne" style="Z-INDEX: 104; LEFT: 5.02%; WIDTH: 50%; POSITION:relative; top:100px;"
				cellSpacing="1" cellPadding="1" width="300" border="1" runat="server">
               <TR>
               <td colspan="2"><Font style="font-weight:bold"> 添加新成员</Font></td>
                </TR>
				<TR>
                    <TD align="left">&nbsp;&nbsp;<asp:label id="Label2" runat="server">显示名称：</asp:label>
                   </TD>
                    <TD><asp:textbox id="txtName" runat="server"></asp:textbox>
                   </TD>
                </TR>
                <TR>
                    <TD align="left"><asp:label id="Label1" runat="server">电子邮件地址：</asp:label>
                   </TD>
                   <TD>
                    <asp:textbox id="txtEmail" runat="server"></asp:textbox>
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  <asp:button id="ButtonAddOne" runat="server" Width="80px" Text="确定" onclick="btnAddOne_Click"></asp:button>
                   </TD>
                </TR>
               <TR>
                    <TD align="left"><asp:label id="Label10" runat="server">批量导入文件：</asp:label></TD>
                 <TD>&nbsp;<asp:FileUpload id="File1" runat="server" />
                 &nbsp;&nbsp;&nbsp;&nbsp;<asp:button id="ButtonBatch" runat="server" Width="80px" Text="批量导入" onclick="btnBatch_Click"></asp:button></TD>
               </tr>
                </TABLE>
		</form>
	</body>
</HTML>
