<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="RareNameQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.RareNameQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RareNameQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT>
			<br>
			<TABLE id="Table4" style="Z-INDEX: 101; LEFT: 16px; WIDTH: 1040px" cellSpacing="1" cellPadding="1"
				width="1040" align="center" border="1">
				<TR bgColor="#eeeeee">
					<TD><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;<asp:label id="lbTitle" runat="server" ForeColor="Red">姓名生僻字</asp:label>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN>
					</TD>
				</TR>
				<TR>
                    <TD>&nbsp;&nbsp;<asp:label id="Label2" runat="server">银行卡号</asp:label>
					<asp:textbox id="txtCardNo" runat="server"></asp:textbox>
                    &nbsp;&nbsp;<FONT face="宋体"><asp:button id="Button1" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button>
                    <asp:button id="Button2" runat="server" Width="80px" Text="新增" onclick="btnAdd_Click"></asp:button>
                    </FONT></TD>
				</TR>
                </TABLE>
			<TABLE id="Table1" style="Z-INDEX: 102; LEFT: 16px; WIDTH: 1040px" cellSpacing="1" cellPadding="1"
				width="1040" align="center" border="1" runat="server">
				<TR>
					<TD><asp:datagrid id="DataGrid1" runat="server" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" AllowPaging="false" width="100%">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                                <asp:BoundColumn DataField="Fuser_type" Visible=false></asp:BoundColumn>
                                 <asp:BoundColumn DataField="Fcard_state" Visible=false></asp:BoundColumn>
                                 <asp:BoundColumn DataField="modify_type" Visible=false></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmodify_time" HeaderText="日期"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcard_no" HeaderText="卡号" DataFormatString="{0:D}"></asp:BoundColumn>
								<asp:BoundColumn DataField="Faccount_name" HeaderText="姓名"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fuser_type_str" HeaderText="用户类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="updateuser" HeaderText="修改人"></asp:BoundColumn>
                                <asp:BoundColumn DataField="record_type_str" HeaderText="登记类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="card_state_str" HeaderText="卡状态"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="编辑">
									<ItemTemplate>
										<asp:LinkButton id="lbChange" runat="server" CommandName="CHANGE">编辑</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="删除">
									<ItemTemplate>
										<asp:LinkButton id="lbDel" runat="server" CommandName="DEL">删除</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
               <TABLE visible="false" id="tableDetail" runat="server" cellSpacing="1" cellPadding="0" style="Z-INDEX: 103; position:absolute;LEFT:140px; WIDTH: 300px;" border="1">
				<TR>
					<TD align="right" height="30px">&nbsp;卡号:</FONT></TD>
					<TD><asp:textbox id="card_no" runat="server"></asp:textbox></TD>
				</TR>
                   <TR>
					<TD align="right" width="30%" height="30px"><FONT face="宋体">姓名: </FONT>
					</TD>
					<TD><asp:textbox id="account_name" runat="server"></asp:textbox></TD>
				</TR>
                 <TR>
                    <TD align="right" height="30px"><asp:label id="Label8" runat="server">用户类型</asp:label></TD>
					<TD><asp:dropdownlist id="ddlUserType" runat="server" Width="152px">
							<asp:ListItem Value="0" Selected="True">公司</asp:ListItem>
							<asp:ListItem Value="1">个人</asp:ListItem>
						</asp:dropdownlist>
                    </TD>
				</TR>
                 <TR>
                    <TD align="right" height="30px"><asp:label id="Label3" runat="server">登记类型</asp:label></TD>
					<TD><asp:dropdownlist id="ddlModifyType" runat="server" Width="152px">
							<asp:ListItem Value="a" Selected="True">改名</asp:ListItem>
							<asp:ListItem Value="b">生僻字</asp:ListItem>
                            <asp:ListItem Value="c">中小银行信息补填</asp:ListItem>
						</asp:dropdownlist>
                    </TD>
				</TR>
                <TR>
                    <TD align="right" height="30px"><asp:label id="Label4" runat="server">卡状态</asp:label></TD>
					<TD><asp:dropdownlist id="ddlCardState" runat="server" Width="152px">
							<asp:ListItem Value="1" Selected="True">付款</asp:ListItem>
							<asp:ListItem Value="2">成功付款</asp:ListItem>
                            <asp:ListItem Value="3">无效</asp:ListItem>
						</asp:dropdownlist>
                    </TD>
				</TR>
                <TR>
                    <TD colspan="2" align=center height="30px">
                    <FONT face="宋体"><asp:button id="Button4" runat="server" Width="80px" Text="新增" onclick="operation_Click"></asp:button>
                    </FONT></TD>
				</TR>
            </TABLE>
		</form>
	</body>
</HTML>

