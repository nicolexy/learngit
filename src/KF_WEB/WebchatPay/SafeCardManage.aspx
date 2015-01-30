<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SafeCardManage.aspx.cs"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.SafeCardManage" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>理财通安全卡管理</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );
        .style2
        {
            color: #000000;
        }
        .style3
        {
            color: #ff0000;
        }
        BODY
        {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
    </style>
</head>
<body>
    <form id="formSafeCard" runat="server">
    <table cellspacing="1" cellpadding="0" width="95%" align="center" bgcolor="#666666"
        border="0">
        <tr bgcolor="#e4e5f7" style="background-image: ../IMAGES/Page/bg_bl.gif">
            <td valign="middle" height="20" colspan="2">
                <font color="#ff0000"><strong><font color="#ff0000">&nbsp;</font></strong><img height="16"
                                src="../IMAGES/Page/post.gif" width="20">
                                理财通安全卡管理</font>
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td>
               账号：&nbsp;
                            <asp:TextBox ID="txtQQId" runat="server" Width="300px"></asp:TextBox>
                            &nbsp;
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Width="150px"
                                Display="Dynamic" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtQQId">请输入用户帐号</asp:RequiredFieldValidator>
                                <input id="WeChatId" name="IDType" runat="server" type="radio" /><label for="WeChatId">微信帐号</label>
                                <input id="WeChatQQ" name="IDType" runat="server" type="radio" /><label for="WeChatQQ">微信绑定QQ</label>
                                <input id="WeChatMobile" name="IDType" runat="server" type="radio" /><label for="WeChatMobile">微信绑定手机</label>
                                <input id="WeChatEmail" name="IDType" runat="server" type="radio" /><label for="WeChatEmail">微信绑定邮箱</label>
                                <input id="WeChatUid" name="IDType" runat="server" type="radio" /><label for="WeChatUid">微信内部ID</label>
                                <input id="WeChatCft" name="IDType" runat="server" type="radio" checked/><label for="WeChatCft">微信财付通账号Or手Q账号</label>
                                
            </td>
            <td width="25%">
            理财通余额：
               <asp:Label ID="lblBalance" runat="server"></asp:Label>
            </td>
        </tr>
          <tr bgcolor="#ffffff">
          <td>
                <label>基金公司名称：</label><asp:DropDownList ID="ddl_companyName" runat="server" Width="350px"></asp:DropDownList>
          </td>
          <td width="10%">
                <div align="center">
                    <asp:Button ID="btnQuery" runat="server" Text="查 询" OnClick="btnQuery_Click"></asp:Button></div>
            </td>
          </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" width="95%" align="center" bgcolor="#666666"
        border="0">
        <tr bgcolor="#e4e5f7" style="background-image: ../IMAGES/Page/bg_bl.gif">
            <td valign="middle" colspan="2" height="20">
                <font color="#ff0000"><strong><font color="#ff0000">&nbsp;</font></strong><img height="16"
                                src="../IMAGES/Page/post.gif" width="20">
                                当前理财通安全卡</font>
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td>
                <table style="height: 100%" cellspacing="0" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td>
                            银行类型：
                            <asp:Label ID="lblBankType" runat="server" ></asp:Label>
                            
                        </td>
                        <td>
                            银行卡尾号：
                            <asp:Label ID="lblBankCardTail" runat="server"></asp:Label>
                            
                        </td>
                        <td>
                            绑定手机：
                            <asp:Label ID="lblCell" runat="server"></asp:Label>
                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <table border="1" cellspacing="0" cellpadding="0" width="95%" align="center">
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font color="red">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16">所有银行卡</font>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:GridView ID="gvBindBankCard" runat="server" Width="100%" PageSize="5" AutoGenerateColumns="False"
                    GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                    BorderColor="#E7E7FF" OnRowCommand="gvBindBankCard_RowCommand" DataKeyNames="bind_serialno">
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="uin" HeaderText="账号">
                            <HeaderStyle Width="250px" HorizontalAlign="Left"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="true_name" HeaderText="姓名">
                            <HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="bankTypeName" HeaderText="银行类型">
                            <HeaderStyle Width="200px" HorizontalAlign="Left"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="card_tail" HeaderText="银行卡尾号">
                            <HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="mobilephone" HeaderText="绑定手机">
                            <HeaderStyle Width="150px" HorizontalAlign="Left"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="supportFund" HeaderText="支持安全卡">
                            <HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="safeCard" HeaderText="安全卡状态">
                            <HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="bankid" HeaderText="银行卡号" ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden">
                            <FooterStyle CssClass="hidden"></FooterStyle>
                            <HeaderStyle Width="80px"></HeaderStyle>
                            <ItemStyle CssClass="hidden"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="uid" HeaderText="uid" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden">
                            <FooterStyle CssClass="hidden"></FooterStyle>
                            <HeaderStyle Width="80px" CssClass="hidden"></HeaderStyle>
                            <ItemStyle CssClass="hidden"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="bind_serialno" HeaderText="绑定序列号" ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden">
                            <FooterStyle CssClass="hidden"></FooterStyle>
                            <HeaderStyle Width="80px"></HeaderStyle>
                            <ItemStyle CssClass="hidden"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="bank_type" HeaderText="bank_type" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden">
                            <FooterStyle CssClass="hidden"></FooterStyle>
                            <HeaderStyle Width="80px"></HeaderStyle>
                            <ItemStyle CssClass="hidden"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="bind_time_local" HeaderText="银行卡绑定时间">
                            <HeaderStyle Width="200px" HorizontalAlign="Left"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="编辑">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" Visible='<%#((System.Data.DataRowView)Container.DataItem)["supportFund"].ToString()=="支持"?true:false %>'
                                    CommandName="ModifyFundPayCard" CommandArgument="<%#((GridViewRow)Container).RowIndex %>"  Text="绑定"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <br />
    <table border="1" cellSpacing="0" cellPadding="0" width="95%" align="center">
                <TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">理财通交易流水</FONT>
						</TD>
				</TR>
				<TR>
					<TD vAlign="top"><asp:datagrid id="dgFundTradeList" runat="server" Width="100%" ItemStyle-HorizontalAlign="Center"
							HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
							GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="交易时间">
									<HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="Flistid" HeaderText="订单号">
									<HeaderStyle Width="200px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="Ffund_name" HeaderText="基金名称">
									<HeaderStyle Width="200px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="PayAmountText" HeaderText="金额">
									<HeaderStyle Width="80px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="TradeType" HeaderText="类型">
									<HeaderStyle Width="200px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="ChannelName" HeaderText="渠道">
									<HeaderStyle Width="200px"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
                        <webdiyer:aspnetpager id="tradeListPager" runat="server" 
                            HorizontalAlign="right" NumericButtonCount="5" PagingButtonSpacing="0"
							ShowInputBox="always" CssClass="mypager" SubmitButtonText="转到" 
                            NumericButtonTextFormatString="[{0}]" AlwaysShow="True" PageSize="5"
                            onpagechanged="tradeListPager_PageChanged"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
            <br />
    <table border="1" cellSpacing="0" cellPadding="0" width="95%" align="center">
                <TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">理财通安全卡更换日志</FONT>
						</TD>
				</TR>
				<TR>
					<TD vAlign="top"><asp:datagrid id="dgFundTradeLog" runat="server" Width="100%" ItemStyle-HorizontalAlign="Center"
							HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
							GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fqqid" HeaderText="账号">
									<HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmodify_time" HeaderText="操作时间">
									<HeaderStyle Width="200px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmemo" HeaderText="操作人员">
									<HeaderStyle Width="200px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_type_str" HeaderText="旧卡银行类型">
									<HeaderStyle Width="80px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcard_tail" HeaderText="旧卡尾号">
									<HeaderStyle Width="200px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmobile" HeaderText="旧绑定手机号码">
									<HeaderStyle Width="200px"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
                        <webdiyer:aspnetpager id="tradeLogPager" runat="server" 
                            HorizontalAlign="right" NumericButtonCount="5" PagingButtonSpacing="0"
							ShowInputBox="always" CssClass="mypager" SubmitButtonText="转到" 
                            NumericButtonTextFormatString="[{0}]" AlwaysShow="True" PageSize="5"
                            onpagechanged="tradeLogPager_PageChanged"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
    </form>
</body>
</html>
