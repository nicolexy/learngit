<%@ Page language="c#" Codebehind="AppealSSetting.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.AppealSSetting" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AppealSSetting</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		 p.font1 {
                font-weight:bold;
                font-size:14px;
                color:red;
		    }
    </style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="900"
				align="center" border="1">
				<TR bgColor="#eeeeee">
					<TD colSpan="5"><FONT color="#ff0000"><IMG src="../IMAGES/Page/post.gif" width="15"><asp:label id="lbTitle" runat="server">结算规则查询</asp:label></FONT>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:label id="Label2" runat="server" ForeColor="ControlText">操作员代码：</asp:label><FONT color="#ff0000"><asp:label id="Label_uid" runat="server">Label</asp:label></FONT></TD>
				</TR>
				<TR>
					<TD><FONT face="宋体">商户号： </FONT>
						<asp:textbox id="tb_user" runat="server"></asp:textbox></TD>
					<TD><FONT face="宋体">帐号类型： </FONT>
						<asp:dropdownlist id="ddl_usertype" runat="server" Width="80px">
							<asp:ListItem Value="-1">不限</asp:ListItem>
							<asp:ListItem Value="2">商户</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD><FONT face="宋体">状态： </FONT>
						<asp:dropdownlist id="ddl_state" runat="server" Width="96px">
							<asp:ListItem Value="-1">不限</asp:ListItem>
							<asp:ListItem Value="1">商户申请</asp:ListItem>
							<asp:ListItem Value="2">商户复核</asp:ListItem>
							<asp:ListItem Value="3">财付通审批通过</asp:ListItem>
							<asp:ListItem Value="4">拒绝</asp:ListItem>
							<asp:ListItem Value="5">作废</asp:ListItem>
							<asp:ListItem Value="6">过期</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD><asp:button id="btn_query" runat="server" Width="58px" BackColor="InactiveCaptionText" Text="查询" onclick="btn_query_Click"></asp:button></TD>
				</TR>
				<TR>
					<TD colSpan="5"><FONT face="宋体"><asp:label id="lb_msg" runat="server" ForeColor="Red" Width="720px"></asp:label>&nbsp;&nbsp;</FONT></TD>
				</TR>
				<tr>
					<td colSpan="5"><asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" BorderWidth="1px" BorderColor="#E7E7FF"
							BorderStyle="None" CellPadding="1" AutoGenerateColumns="False" GridLines="Horizontal"
                            OnItemDataBound="DataGrid1_ItemDataBound">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fno" HeaderText="序号"></asp:BoundColumn>
								<asp:BoundColumn DataField="FUser" HeaderText="外部帐号"></asp:BoundColumn>
								<asp:BoundColumn DataField="FUserId" HeaderText="内部帐号"></asp:BoundColumn>
								<asp:BoundColumn DataField="FUserTypeS" HeaderText="号码类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="FUserName" HeaderText="帐号名称"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCycTypeS" HeaderText="周期类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCycS" HeaderText="周期单位"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCycNumberS" HeaderText="周期跨度(T+)"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStateS" HeaderText="当前状态"></asp:BoundColumn>
                              <%--   <asp:TemplateColumn HeaderText="操作">
									    <ItemTemplate>
										    <asp:LinkButton id="queryButton" Visible="false" runat="server" CommandName="query" Text="编辑/查看"></asp:LinkButton>
									    </ItemTemplate>
								   </asp:TemplateColumn>--%>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#F7F7DE" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td colSpan="5"><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到"
							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" PageSize="15"></webdiyer:aspnetpager></td>
				</tr>
			</TABLE>
              <div id="RemaindDiv" runat="server" visible="true">
                <p class="font1">提醒：<br />请先在“结算规则查询”中核实是否是T+0，若是T+0，必须在“结算规则查询”中先关闭T+0，且次日再操作“暂停结算”。非T+0商户可直接操作“暂停结算”。
                </p>
            </div>
		</form>
	</body>
</HTML>
