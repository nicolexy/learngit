<%@ Page language="c#" Codebehind="MediOperatorManage_Detail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MediOperatorManage_Detail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AccControl</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style4 { COLOR: #ff0000 }
	.style5 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<p><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><br>
			</p>
			<TABLE style="LEFT:5%; WIDTH:85%" cellSpacing="0" cellPadding="0" border="1"
				align="center" runat="server">
				<TR>
					<TD style="WIDTH: 146px">
						商户SP</TD>
					<TD style="WIDTH: 201px">
						<asp:Label id="labSP" runat="server">Label</asp:Label>
					</TD>
					<TD style="WIDTH: 94px;HEIGHT: 25px">用户帐户</TD>
					<TD>
						<asp:Label id="labQQ" runat="server">Label</asp:Label>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 146px"><FONT face="宋体">用户名称</FONT></TD>
					<TD style="WIDTH: 201px">
						<asp:Label id="labUserName" runat="server">Label</asp:Label>
					</TD>
					<td colspan="2">
					</td>
				</TR>
			</TABLE>
			<p align="center">
				<asp:label id="Label3" runat="server" Font-Bold="True">权限列表</asp:label></p>
			<%--<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="85%" border="1" align="center">
				<TR>
					<TD style="WIDTH: 49px; HEIGHT: 22px">
						<P align="left">1</P>
					</TD>
					<TD style="WIDTH: 139px">交易查询</TD>
					<TD style="WIDTH: 156px">
						<asp:checkbox id="CheckBoxX1" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px">2</TD>
					<TD>订单查询</TD>
					<TD>
						<asp:checkbox id="CheckBoxX2" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 49px;  HEIGHT: 22px">3</TD>
					<TD style="WIDTH: 139px">订单统计</TD>
					<TD style="WIDTH: 156px">
						<asp:checkbox id="CheckBoxX3" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px">4</TD>
					<TD style="WIDTH: 166px">结算查询</TD>
					<TD>
						<asp:checkbox id="CheckBoxX4" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 49px;  HEIGHT: 22px">5</TD>
					<TD style="WIDTH: 139px">对账单下载</TD>
					<TD style="WIDTH: 156px">
						<asp:checkbox id="CheckBoxX5" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px">6</TD>
					<TD style="WIDTH: 166px">商户代付</TD>
					<TD>
						<asp:checkbox id="CheckBoxX6" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 49px; HEIGHT: 24px">7</TD>
					<TD style="WIDTH: 139px; HEIGHT: 24px">商户退款_B2C</TD>
					<TD style="WIDTH: 156px; HEIGHT: 24px">
						<asp:checkbox id="CheckBoxX7" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px; HEIGHT: 24px">8</TD>
					<TD style="WIDTH: 166px; HEIGHT: 24px">商户退款_转账</TD>
					<TD style="HEIGHT: 24px">
						<asp:checkbox id="CheckBoxX8" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 49px">9</TD>
					<TD style="WIDTH: 139px">商户退款_银行退单</TD>
					<TD style="WIDTH: 156px">
						<asp:checkbox id="CheckBoxX9" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px">10</TD>
					<TD style="WIDTH: 166px">商户退款_银行提现</TD>
					<TD>
						<asp:checkbox id="CheckBoxX10" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 49px; HEIGHT: 23px">11</TD>
					<TD style="WIDTH: 139px; HEIGHT: 23px">操作员设置</TD>
					<TD style="WIDTH: 156px; HEIGHT: 23px">
						<asp:checkbox id="CheckBoxX11" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px; HEIGHT: 23px">12</TD>
					<TD style="WIDTH: 166px; HEIGHT: 23px">密码修改</TD>
					<TD style="HEIGHT: 23px">
						<asp:checkbox id="CheckBoxX12" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 49px; HEIGHT: 22px">13</TD>
					<TD style="WIDTH: 139px; HEIGHT: 22px">商户转账</TD>
					<TD style="WIDTH: 156px; HEIGHT: 22px">
						<asp:checkbox id="CheckBoxX13" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px; HEIGHT: 22px">14</TD>
					<TD style="WIDTH: 166px; HEIGHT: 22px">登录</TD>
					<TD style="HEIGHT: 22px">
						<asp:checkbox id="CheckBoxX14" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 49px; HEIGHT: 22px">15</TD>
					<TD style="WIDTH: 139px; HEIGHT: 22px">退款申请单查询</TD>
					<TD style="WIDTH: 156px; HEIGHT: 22px">
						<asp:checkbox id="CheckboxX15" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px; HEIGHT: 22px">16</TD>
					<TD style="WIDTH: 166px; HEIGHT: 22px">批量付款</TD>
					<TD style="HEIGHT: 22px">
						<asp:checkbox id="CheckboxX16" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 49px; HEIGHT: 22px">17</TD>
					<TD style="WIDTH: 139px; HEIGHT: 22px">批量转账</TD>
					<TD style="WIDTH: 156px; HEIGHT: 22px">
						<asp:checkbox id="CheckboxX17" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px; HEIGHT: 22px">18</TD>
					<TD style="WIDTH: 166px; HEIGHT: 22px">密钥修改</TD>
					<TD style="HEIGHT: 22px">
						<asp:checkbox id="CheckboxX18" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 49px; HEIGHT: 22px">19</TD>
					<TD style="WIDTH: 139px; HEIGHT: 22px">商户退款(信用卡退款)</TD>
					<TD style="WIDTH: 156px; HEIGHT: 22px">
						<asp:checkbox id="CheckboxX19" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px; HEIGHT: 22px">20</TD>
					<TD style="WIDTH: 166px; HEIGHT: 22px">实时批量转帐</TD>
					<TD style="HEIGHT: 22px">
						<asp:checkbox id="CheckboxX20" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 49px; HEIGHT: 22px">21</TD>
					<TD style="WIDTH: 139px; HEIGHT: 22px">实时分帐</TD>
					<TD style="WIDTH: 156px; HEIGHT: 22px">
						<asp:checkbox id="CheckboxX21" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px; HEIGHT: 22px">22</TD>
					<TD style="WIDTH: 166px; HEIGHT: 22px">分帐退款</TD>
					<TD style="HEIGHT: 22px">
						<asp:checkbox id="CheckboxX22" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 49px; HEIGHT: 22px">23</TD>
					<TD style="WIDTH: 139px; HEIGHT: 22px">商户充值</TD>
					<TD style="WIDTH: 156px; HEIGHT: 22px">
						<asp:checkbox id="CheckboxX23" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px; HEIGHT: 22px">24</TD>
					<TD style="WIDTH: 166px; HEIGHT: 22px">未定义</TD>
					<TD style="HEIGHT: 22px">
						<asp:checkbox id="CheckboxX24" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 49px; HEIGHT: 22px">25</TD>
					<TD style="WIDTH: 139px; HEIGHT: 22px">未定义</TD>
					<TD style="WIDTH: 156px; HEIGHT: 22px">
						<asp:checkbox id="CheckboxX25" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
					<TD style="WIDTH: 43px; HEIGHT: 22px">26</TD>
					<TD style="WIDTH: 166px; HEIGHT: 22px">未定义</TD>
					<TD style="HEIGHT: 22px">
						<asp:checkbox id="CheckboxX26" runat="server" Checked="True" Enabled="False"></asp:checkbox></TD>
				</TR>
			</TABLE>--%>

            <Table ID="OpTable" runat="server" align="center" CellPadding="0" CellSpacing="0" >
                 <tr>
        <td valign="top">
            <asp:Panel ID="P1" runat="server" Width="180px" >
                <asp:Label ID="Label1" runat="server" Font-Bold="True">基础功能</asp:Label>
                <asp:CheckBoxList ID="cblJCH" runat="server">
                </asp:CheckBoxList>
                <asp:Label ID="Label13" runat="server" Font-Bold="True">快捷支付</asp:Label>
                <asp:CheckBoxList ID="cblKJZF" runat="server">
                </asp:CheckBoxList>
                <asp:Label ID="Label15" runat="server" Font-Bold="True">外卡支付</asp:Label>
                <asp:CheckBoxList ID="cblWKZF" runat="server">
                </asp:CheckBoxList>
                <asp:Label ID="Label16" runat="server" Font-Bold="True">微收款</asp:Label>
                <asp:CheckBoxList ID="cblmicro" runat="server">
                </asp:CheckBoxList>
            </asp:Panel>
        </td>
            <td valign="top">
            <asp:Panel ID="P2" runat="server" Width="180px" >
                <asp:Label ID="Label2" runat="server" Font-Bold="True">退款</asp:Label>
                <asp:CheckBoxList ID="cblTK" runat="server">
                </asp:CheckBoxList>
                <asp:Label ID="Label4" runat="server" Font-Bold="True">充值</asp:Label>
                <asp:CheckBoxList ID="cblCHZH" runat="server">
                </asp:CheckBoxList>
                <asp:Label ID="Label5" runat="server" Font-Bold="True">转账</asp:Label>
                <asp:CheckBoxList ID="cblZHZH" runat="server">
                </asp:CheckBoxList>
                <asp:Label ID="Label6" runat="server" Font-Bold="True">其他</asp:Label>
                <asp:CheckBoxList ID="cblQT" runat="server">
                </asp:CheckBoxList>
                <asp:Label ID="Label7" runat="server" Font-Bold="True">代扣</asp:Label>
                <asp:CheckBoxList ID="cblDK" runat="server">
                </asp:CheckBoxList>
            </asp:Panel>
        </td>
           <td valign="top">
            <asp:Panel ID="P3" runat="server" Width="180px" >
                <asp:Label ID="Label8" runat="server" Font-Bold="True">分账</asp:Label>
                <asp:CheckBoxList ID="cblFZH" runat="server">
                </asp:CheckBoxList>
                <asp:Label ID="Label9" runat="server" Font-Bold="True">中介交易</asp:Label>
                <asp:CheckBoxList ID="cblZHJJY" runat="server">
                </asp:CheckBoxList>
                <asp:Label ID="Label10" runat="server" Font-Bold="True">代付</asp:Label>
                <asp:CheckBoxList ID="cblDF" runat="server">
                </asp:CheckBoxList>
                <asp:Label ID="Label14" runat="server" Font-Bold="True">预付卡</asp:Label>
                <asp:CheckBoxList ID="cblYFK" runat="server">
                </asp:CheckBoxList>
            </asp:Panel>
        </td>
          <td valign="top">
            <asp:Panel ID="P4" runat="server" Width="180px" >
                <asp:Label ID="Label11" runat="server" Font-Bold="True">跨境</asp:Label>
                <asp:CheckBoxList ID="cblKJ" runat="server">
                </asp:CheckBoxList>
                <asp:Label ID="Label12" runat="server" Font-Bold="True">POS</asp:Label>
                <asp:CheckBoxList ID="cblPOS" runat="server">
                </asp:CheckBoxList>
                <asp:Label ID="Label17" runat="server" Font-Bold="True">未启用</asp:Label>
                <asp:CheckBoxList ID="cblWQY" runat="server">
                </asp:CheckBoxList>
            </asp:Panel>
        </td>
    </tr>
            </Table>
		</form>
	</body>
</HTML>
