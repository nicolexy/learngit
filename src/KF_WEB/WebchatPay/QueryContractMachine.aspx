<%@ Page language="c#" Codebehind="QueryContractMachine.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.QueryContractMachine" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ComplainBussinessInput</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="2"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;合约机详情查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				
                <TR>
                    <TD align="right"><asp:label id="Label3" runat="server">订单号：</asp:label></TD>
                    <TD><asp:textbox id="txtCftNo" style="WIDTH: 280px;" runat="server"></asp:textbox></TD>
					
				</TR>
                <tr>
                    <td>
                        <label style="vertical-align: middle; width: 80px; height: 20px">请输入：</label>
                        <asp:TextBox ID="tbx_payAccount" runat="server" Width="140px"></asp:TextBox>
                    </td>
                    <td>
                        <input id="WeChatId" name="IDType" runat="server" type="radio" checked/><label for="WeChatId">微信帐号</label>
                        <input id="WeChatQQ" name="IDType" runat="server" type="radio" /><label for="WeChatQQ">微信绑定QQ</label>
                        <input id="WeChatMobile" name="IDType" runat="server" type="radio" /><label for="WeChatMobile">微信绑定手机</label>
                        <input id="WeChatEmail" name="IDType" runat="server" type="radio" /><label for="WeChatEmail">微信绑定邮箱</label>
                        <input id="WeChatUid" name="IDType" runat="server" type="radio" /><label for="WeChatUid">微信内部ID</label>
                        <input id="WeChatCft" name="IDType" runat="server" type="radio" /><label for="WeChatCft">微信财付通账号</label>
                    </td>
                </tr>
				<TR>
                    <TD align="center" colspan="4"><asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
            <TABLE id="ListTable" visible="false" cellSpacing="1" cellPadding="1" width="870px" border="1" runat="server">
                <TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Flistid" HeaderText="订单号"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="操作">
                                    <ItemTemplate>
                                        <asp:LinkButton id="lbDetail" runat="server" CommandName="DETAIL">详情</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
            </TABLE>
			<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="870px" border="1" runat="server">
				<TR bgcolor="#e4e5f7">
					<TD align="left" colspan="6">
                        <font>合约机详细信息</font>
					</TD>
				</TR>
                <TR>
					<TD align="left" colspan="6">
                        <font><b>交易购买信息</b></font>
					</TD>
				</TR>
                <TR>
					<TD>财付通订单：</TD>
                    <td Width="145px"><asp:label id="lb_CftOrderId" runat="server" ></asp:label></td>
                    <TD>南方基金订单号：</TD>
                    <td Width="145px"><asp:label id="lb_FundOrderId" runat="server" ></asp:label></td>
                    <TD>联通订单号：</TD>
                    <td Width="145px"><asp:label id="lb_LtOrderId" runat="server" ></asp:label></td>
				</TR>
                <TR>
					<TD>冻结金额交易单：</TD>
                    <td><asp:label id="lb_FreezeTradeId" runat="server"></asp:label></td>
                    <TD>交易状态：</TD>
                    <td><asp:label id="lb_TradeState" runat="server" ></asp:label></td>
                    <TD>冻结时间：</TD>
                    <td><asp:label id="lb_FreezeTime" runat="server"></asp:label></td>
				</TR>
                <TR>
					<TD>冻结金额：</TD>
                    <td><asp:label id="lb_FreezeAmt" runat="server" ></asp:label></td>
                    <TD>解冻时间：</TD>
                    <td><asp:label id="lb_UnFreezeTime" runat="server" ></asp:label></td>
                    <TD>解冻单号：</TD>
                    <td><asp:label id="lb_UnFreezeNo" runat="server" ></asp:label></td>
				</TR>
                <TR>
					<TD>违约扣款：</TD>
                    <td><asp:label id="lb_Default" runat="server" ></asp:label></td>
                    <TD>商户号：</TD>
                    <td><asp:label id="lb_Spid" runat="server" ></asp:label></td>
                    <TD>商户订单：</TD>
                    <td><asp:label id="lb_SpNo" runat="server" ></asp:label></td>
				</TR>
                <TR>
					<TD>赠送份额：</TD>
                    <td><asp:label id="lb_Present" runat="server" ></asp:label></td>
                    <TD>物理状态：</TD>
                    <td><asp:label id="lb_lstate" runat="server" ></asp:label></td>
                    <TD>购买渠道：</TD>
                    <td><asp:label id="lb_Channel" runat="server" ></asp:label></td>
				</TR>
                <TR>
					<TD>用户账号：</TD>
                    <td><asp:label id="lb_Uin" runat="server" ></asp:label></td>
                    <TD>下单时间：</TD>
                    <td><asp:label id="lb_OrderTime" runat="server" ></asp:label></td>
                    <TD>最后修改时间：</TD>
                    <td><asp:label id="lb_ModifyTime" runat="server" ></asp:label></td>
				</TR>
                <TR>
					<TD>买入方式：</TD>
                    <td><asp:label id="lb_BuyType" runat="server" ></asp:label></td>
                    <TD>用户冻结时间：</TD>
                    <td><asp:label id="lb_UinFreezeTime" runat="server" ></asp:label></td>
                    <TD>&nbsp</TD>
                    <td><asp:label id="Label20" runat="server" ></asp:label></td>
				</TR>
                <TR>
					<TD align="left" colspan="6">
                        <b>手机/卡信息</b>
					</TD>
				</TR>
                <TR>
					<TD>合约机机型：</TD>
                    <td><asp:label id="lb_hyjType" runat="server" ></asp:label></td>
                    <TD>合约机颜色：</TD>
                    <td><asp:label id="lb_hyjColor" runat="server" ></asp:label></td>
                    <TD>合约机内存大小：</TD>
                    <td><asp:label id="lb_hyjMemory" runat="server" ></asp:label></td>
				</TR>
                <TR>
					<TD>手机卡归属地：</TD>
                    <td><asp:label id="lb_Area" runat="server" ></asp:label></td>
                    <TD>手机号当前状态：</TD>
                    <td><asp:label id="lb_PhoneState" runat="server" ></asp:label></td>
                    <TD>首月资费：</TD>
                    <td><asp:label id="lb_FirstMonth" runat="server" ></asp:label></td>
				</TR>
                <TR>
					<TD>选择号码：</TD>
                    <td><asp:label id="lb_Phone" runat="server" ></asp:label></td>
                    <TD>选择套餐：</TD>
                    <td><asp:label id="lb_PlanType" runat="server" ></asp:label></td>
                    <TD>套餐内容：</TD>
                    <td><asp:label id="lb_PlanCont" runat="server" ></asp:label></td>
				</TR>
                <TR>
					<TD>入网证件：</TD>
                    <td><asp:label id="lb_creType" runat="server" ></asp:label></td>
                    <TD>入网证件id：</TD>
                    <td><asp:label id="lb_creId" runat="server" ></asp:label></td>
                    <TD>联系电话：</TD>
                    <td><asp:label id="lb_BindPhone" runat="server" ></asp:label></td>
				</TR>
                <TR>
					<TD>收件地址：</TD>
                    <td><asp:label id="lb_addr" runat="server" ></asp:label></td>
                    <TD>发货快递：</TD>
                    <td><asp:label id="lb_Express" runat="server" ></asp:label></td>
                    <TD>快递单号：</TD>
                    <td><asp:label id="lb_expTicket" runat="server" ></asp:label></td>
				</TR>
                
			</TABLE>
            
		</form>
	</body>
</HTML>
