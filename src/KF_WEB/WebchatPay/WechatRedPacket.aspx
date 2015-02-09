<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WechatRedPacket.aspx.cs"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.WechatRedPacket" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>微信红包</title>
    <style type="text/css">@import url( ../STYLES/ossstyle.css ); 
        UNKNOWN { COLOR: #000000 }
	    .style3 { COLOR: #ff0000 }
	    BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
</head>
<body>
    <form id="formMain" runat="server">
    <div>
        <table border="1" cellspacing="1" cellpadding="1" width="1100">
            <tr>
                <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                    <font color="red">
                        <img src="../images/page/post.gif" width="20" height="16">微信红包查询</font>
                </td>
            </tr>
            <tr>
                <td>
                    微信号:<asp:TextBox ID="txtWechatName" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hfHBUin" runat="server" />
                   <%-- 支付交易单号:<asp:TextBox ID="txtPayListId" runat="server"></asp:TextBox>--%>
                </td>
                <%--<td>
                    开始日期：
                    <asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
                    
                    结束日期：<asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
                </td>--%>
                <td align="center" colspan="2">
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click">
                    </asp:Button>
                </td>
            </tr>
            <tr>
                <td align="left">
                    微信红包财付通帐号：
                    <asp:Label ID="lblHongbaoUin" runat="server"></asp:Label>
                </td>
                <td>
                        账户余额：
                        <asp:Label ID="lblBanlance" runat="server"></asp:Label>
                </td>
                <td>
                        冻结金额：
                        <asp:Label ID="lblFreezen" runat="server"></asp:Label>
                </td>
        </table>
        <br />
        <table border="1" cellspacing="0" cellpadding="0" width="1100">
            <tr>
                <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                    <font color="red">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16">收到的红包</font>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:GridView ID="gvReceiveList" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                        GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                        BorderColor="#E7E7FF" OnRowCommand="gvReceiveList_RowCommand">
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                        </HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="Fcreate_time" HeaderText="日期">
                                <HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Title" HeaderText="主题">
                                <HeaderStyle Width="200px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Famount_text" HeaderText="金额">
                                <HeaderStyle Width="80px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField> 
                            <asp:BoundField DataField="Fwishing" HeaderText="祝福语">
                                <HeaderStyle Width="110px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <HeaderStyle Width="200px"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnView1" runat="server" CausesValidation="False" CommandArgument='<%#string.Format("{0},{1}",((System.Data.DataRowView)Container.DataItem)["Fsend_list_id"].ToString(), ((System.Data.DataRowView)Container.DataItem)["Fcreate_time"].ToString())%>'
                                        CommandName="ViewDetail" Text="详情"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <webdiyer:aspnetpager id="receivePager" runat="server" horizontalalign="right"
                        numericbuttoncount="5" pagingbuttonspacing="0" showinputbox="always" cssclass="mypager"
                        submitbuttontext="转到" numericbuttontextformatstring="[{0}]" alwaysshow="True"
                        onpagechanged="receivePager_PageChanged"></webdiyer:aspnetpager>
                </td>
            </tr>
        </table>
        <br />
        <table border="1" cellspacing="0" cellpadding="0" width="1100">
            <tr>
                <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                    <font color="red">
                        <img src="../images/page/post.gif" width="20" height="16">红包详情</font>
                        <asp:HiddenField ID="hfSendListId" runat="server" />
                        <asp:HiddenField ID="hfCreateTime" runat="server" />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:GridView ID="gvRedPacketDetail" runat="server" Width="1100px" itemstyle-horizontalalign="center"
                        HeaderStyle-HorizontalAlign="center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                        GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                        BorderColor="#E7E7FF">
                        <Columns>
                            <asp:BoundField DataField="Fcreate_time" HeaderText="日期">
                            <HeaderStyle HorizontalAlign="Center" Width="150px" /> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Freceive_name" HeaderText="好友昵称">
                            <HeaderStyle Width="200px" /> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                             <asp:BoundField DataField="Fsend_openid_text" HeaderText="发送方零钱账号">
                            <HeaderStyle Width="200px" /> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Freceive_openid_text" HeaderText="接收方零钱账号">
                            <HeaderStyle Width="200px" /> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Famount_text" HeaderText="领取金额">
                            <HeaderStyle Width="80px" /> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Fwishing" HeaderText="祝福语"> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" 
                            BackColor="#4A3C8C">
                        </HeaderStyle>
                    </asp:GridView>
                    <webdiyer:aspnetpager id="redPacketDetailPager" runat="server" horizontalalign="right"
                        numericbuttoncount="5" pagingbuttonspacing="0" showinputbox="always" cssclass="mypager"
                        submitbuttontext="转到" numericbuttontextformatstring="[{0}]" alwaysshow="true"
                        onpagechanged="redPacketDetailPager_PageChanged"></webdiyer:aspnetpager>
                </td>
            </tr>
        </table>
        <br />
        <table border="1" cellspacing="0" cellpadding="0" width="1100">
            <tr>
                <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                    <font color="red">
                        <img src="../images/page/post.gif" width="20" height="16">发送的红包</font>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:GridView ID="gvSendList" runat="server" Width="1100px" itemstyle-horizontalalign="center"
                        HeaderStyle-HorizontalAlign="center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                        GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                        BorderColor="#E7E7FF" onrowcommand="gvSendList_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Fcreate_time" HeaderText="日期">
                            <HeaderStyle HorizontalAlign="Center" Width="150px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Fpay_listid" HeaderText="订单号">
                            <HeaderStyle Width="200px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Ftotal_amount_text" HeaderText="金额">
                            <HeaderStyle Width="80px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Fstate_text" HeaderText="状态">
                            <HeaderStyle Width="200px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="summary" HeaderText="红包数量" >
                            <HeaderStyle Width="200px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="refund" HeaderText="退款金额" >
                            <HeaderStyle Width="200px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Fwishing" HeaderText="祝福语" >
                            <HeaderStyle Width="200px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="详情" ShowHeader="False">
                                 <HeaderStyle Width="200px" /><ItemStyle  HorizontalAlign="Center"/>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnView2" runat="server" CausesValidation="False" CommandArgument='<%#string.Format("{0},{1}",((System.Data.DataRowView)Container.DataItem)["Flistid"].ToString(), ((System.Data.DataRowView)Container.DataItem)["Fcreate_time"].ToString())%>'
                                        CommandName="ViewDetail" Text="详情"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" 
                            BackColor="#4A3C8C">
                        </HeaderStyle>
                    </asp:GridView>
                    <webdiyer:aspnetpager id="sendListPager" runat="server" horizontalalign="right"
                        numericbuttoncount="5" pagingbuttonspacing="0" showinputbox="always" cssclass="mypager"
                        submitbuttontext="转到" numericbuttontextformatstring="[{0}]" alwaysshow="true"
                        onpagechanged="sendListPager_PageChanged"></webdiyer:aspnetpager>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script language="javascript">
        function openModeBegin() {
            var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", formMain.TextBoxBeginDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
            if (returnValue != null) formMain.TextBoxBeginDate.value = returnValue;
        }
        function openModeEnd() {
            var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", formMain.TextBoxEndDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
            if (returnValue != null) formMain.TextBoxEndDate.value = returnValue;
        }
		</script>
</body>
</html>
