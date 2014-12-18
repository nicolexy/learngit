<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryWebchatPayActivity.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryWebchatPayActivity" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryWebchatPayActivity</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
						var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
						if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
		            }
		            function openModeEnd() {
		                var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.TextBoxEndDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
		                if (returnValue != null) Form1.TextBoxEndDate.value = returnValue;
		            }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE  cellSpacing="1" cellPadding="1" width="1100"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;微信活动查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    
					<TD align="right"><asp:label id="Label5" runat="server">资格记录时间_开始：</asp:label></TD>
                    <TD><asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
                    </TD>
                    <TD align="right" id="txtTime1" runat="server"><asp:label id="Label4" runat="server">资格记录时间_结束：</asp:label></TD>
                    <TD id="txtTime2" runat="server"><asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
                    </TD>
				</TR>
                <TR>
                    <TD align="right"><asp:label id="Label3" runat="server">财付通订单号：</asp:label></TD>
                    <TD><asp:textbox id="txtCftNo" style="WIDTH: 220px;" runat="server"></asp:textbox></TD>
                    <TD align="right"><asp:label id="Label2" runat="server">活动名称：</asp:label></TD>
                    <TD><asp:DropDownList id="ddlActId" AutoPostBack="True" runat="server">
                        <asp:ListItem Value="wxzfact" Selected="True">微信支付活动</asp:ListItem>
                        <asp:ListItem Value="xyk">送心意</asp:ListItem>
                    </asp:DropDownList></TD>
					
				</TR>
				<TR>
                    <TD align="center" colspan="4"><asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
			<TABLE id="Table2" 
				cellSpacing="1" cellPadding="1" width="1100" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="FUin" HeaderText="用户账号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FActId" HeaderText="活动ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="FActName" HeaderText="活动名称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FTransId" HeaderText="订单号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FAccepter" HeaderText="接收奖品QQ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FState_str" HeaderText="资格状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FPrizeDesc_str" HeaderText="奖品描述"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FTicketOrder" HeaderText="奖品信息"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FPayFee_str" HeaderText="支付金额"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FPayTime" HeaderText="支付时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FCreateTime" HeaderText="参与时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FErrInfo" HeaderText="错误信息"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fstandby2_str" HeaderText="奖级类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fstandby2_str2" HeaderText="奖品类型"></asp:BoundColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"  
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
            <asp:Panel ID="PanelDetail" Runat="server" HorizontalAlign="left" Visible="False">
                <TABLE border="1" cellSpacing="1" cellPadding="1" width="1100">
                    <tr bgcolor="#e4e5f7" >
                        <td valign="middle" height="20"><font color="#ff0000">发送信息</font>&nbsp;&nbsp;
                            账号：<asp:Label ID="lblAccUin" runat="server" ></asp:Label>&nbsp;&nbsp;
                            
                        </td>
                    </tr>
                    <tr bgcolor="#ffffff">
                        <td>
                            <table style="height: 100%" cellspacing="0" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td>
                                        <asp:datagrid id="SendDG" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                                <asp:BoundColumn DataField="sendid" HeaderText="订单号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="name" HeaderText="物品名称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="totalgiftnum" HeaderText="数量"></asp:BoundColumn>
                                <asp:BoundColumn DataField="totalamountStr" HeaderText="支付金额"></asp:BoundColumn>
                                <asp:BoundColumn DataField="paytime" HeaderText="支付时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="wishing" HeaderText="祝福语"></asp:BoundColumn>
                                <asp:ButtonColumn Text="查看" HeaderText="操作" CommandName="detail"></asp:ButtonColumn>
							</Columns>
                                            </asp:datagrid>
                                        <webdiyer:aspnetpager id="sendPager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"  
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </TABLE>
                <TABLE border="1" cellSpacing="1" cellPadding="1" width="1100">
                    <tr bgcolor="#e4e5f7" >
                        <td valign="middle" height="20"><font color="#ff0000">接收信息</font>
                        </td>
                    </tr>
                    <tr bgcolor="#ffffff">
                        <td>
                            <table style="height: 100%" cellspacing="0" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td>
                                        <asp:datagrid id="ReceiveDG" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                                <asp:BoundColumn DataField="sendnickname" HeaderText="赠送方微信昵称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="name" HeaderText="物品名称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="suborderid" HeaderText="订单号"></asp:BoundColumn>
                                <asp:BoundColumn DataField="createtime" HeaderText="接收时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="expiretime" HeaderText="物品过期时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="usestateStr" HeaderText="使用状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="giftId" HeaderText="物品ID"></asp:BoundColumn>
							</Columns>
                                            </asp:datagrid>
                                        <webdiyer:aspnetpager id="receivePager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"  
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </TABLE>
                <TABLE border="1" cellSpacing="1" cellPadding="1" width="1100">
                    <tr bgcolor="#e4e5f7" >
                        <td valign="middle" height="20"><font color="#ff0000">发送详情</font>
                        </td>
                    </tr>
                    <tr bgcolor="#ffffff">
                        <td>
                            <table style="height: 100%" cellspacing="0" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td>
                                        <asp:datagrid id="SendDetailDG" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                                <asp:BoundColumn DataField="receivenickname" HeaderText="微信昵称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="createtime" HeaderText="赠送日期"></asp:BoundColumn>
                                <asp:BoundColumn DataField="recexpiretime" HeaderText="等待接收方响应截止日期"></asp:BoundColumn>
                                <asp:BoundColumn DataField="createtime" HeaderText="接收时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="expiretime" HeaderText="物品过期时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="statusStr" HeaderText="赠送状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="suborderid" HeaderText="物品ID"></asp:BoundColumn>
							</Columns>
                                            </asp:datagrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </TABLE>
            </asp:Panel>
		</form>
	</body>
</HTML>
