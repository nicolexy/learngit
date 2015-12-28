<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FindHandQRedPacket.aspx.cs"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.HandQBusiness.FindHandQRedPacket" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>手Q红包查询</title>
    <style type="text/css">@import url( ../STYLES/ossstyle.css ); 
        UNKNOWN { COLOR: #000000 }
	    .style3 { COLOR: #ff0000 }
	    BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		.auto-style1
        {
            width: 84px;
        }
		</style>
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="formMain" runat="server">
    <div>
        <table border="1" cellspacing="1" cellpadding="1" width="1100">
            <tr>
                <td bgcolor="#e4e5f7" colspan="5">
                    <font color="red">
                        <img src="../images/page/post.gif" width="20" height="16">手Q红包查询</font>
                </td>
            </tr>
            <tr>
                <td>
                    财付通帐号:<asp:TextBox ID="txtUin" runat="server" Width="150px"></asp:TextBox>                  
                   
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;
                    开始日期：                   
                    <asp:TextBox ID="textBoxBeginDate" runat="server" Width="130px" onClick="WdatePicker()"  CssClass="Wdate"></asp:TextBox>
                    结束日期：
                    <asp:TextBox ID="textBoxEndDate" runat="server" Width="130px" onClick="WdatePicker()"  CssClass="Wdate"></asp:TextBox>   
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click">
                    </asp:Button>             
                </td>
            </tr>

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
                    <asp:GridView ID="gvReceiveHQList" runat="server" Width="1100px" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                        GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                        BorderColor="#E7E7FF" OnRowCommand="gvReceiveHQList_RowCommand" >
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                        </HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="create_time" HeaderText="日期">
                                <HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Title" HeaderText="主题">
                                <HeaderStyle Width="200px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="amount_text" HeaderText="金额">
                                <HeaderStyle Width="80px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField> 
                           <asp:BoundField DataField="recv_listid" HeaderText="订单号">
                                <HeaderStyle Width="110px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="channel_text" HeaderText="红包类型">
                                <HeaderStyle Width="80px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <HeaderStyle Width="200px"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnView1" runat="server" CausesValidation="False" CommandArgument='<%#string.Format("{0},{1}",((System.Data.DataRowView)Container.DataItem)["send_listid"].ToString(), ((System.Data.DataRowView)Container.DataItem)["create_time"].ToString())%>'
                                        CommandName="ViewDetail" Text="详情"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <webdiyer:aspnetpager id="receivePager" runat="server" horizontalalign="right"
                        numericbuttoncount="5" pagingbuttonspacing="0" showinputbox="always" cssclass="mypager"
                        submitbuttontext="转到" numericbuttontextformatstring="[{0}]" alwaysshow="True"
                        onpagechanged="receiveHQPager_PageChanged"></webdiyer:aspnetpager>
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
                    <asp:GridView ID="gvHQRedPacketDetail" runat="server" Width="1100px" itemstyle-horizontalalign="center"
                        HeaderStyle-HorizontalAlign="center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                        GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                        BorderColor="#E7E7FF">
                        <Columns>
                            <asp:BoundField DataField="recv_listid" HeaderText="转账单号">
                            <HeaderStyle Width="200px" /> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="create_time" HeaderText="日期">
                            <HeaderStyle HorizontalAlign="Center" Width="100px" /> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Title" HeaderText="红包主题">
                            <HeaderStyle Width="100px" /> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="send_uin" HeaderText="发送方财付通帐号">
                            <HeaderStyle Width="100px" /> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="recv_uin" HeaderText="接收方财付通帐号">
                            <HeaderStyle Width="100px" /> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="recv_name" HeaderText="接收方昵称">
                            <HeaderStyle Width="80px" /> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                             <asp:BoundField DataField="amount_text" HeaderText="领取金额">
                            <HeaderStyle Width="80px" /> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="channel_text" HeaderText="红包类型">
                                <HeaderStyle Width="80px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>

                        </Columns>
                        <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" 
                            BackColor="#4A3C8C">
                        </HeaderStyle>
                    </asp:GridView>
                    <webdiyer:aspnetpager id="redPacketHBDetailPager" runat="server" horizontalalign="right"
                        numericbuttoncount="5" pagingbuttonspacing="0" showinputbox="always" cssclass="mypager"
                        submitbuttontext="转到" numericbuttontextformatstring="[{0}]" alwaysshow="true"
                        onpagechanged="redPacketHBDetailPager_PageChanged"></webdiyer:aspnetpager>
  
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
                    <asp:GridView ID="gvSendHQList" runat="server" Width="1100px" itemstyle-horizontalalign="center"
                        HeaderStyle-HorizontalAlign="center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
                        GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
                        BorderColor="#E7E7FF" onrowcommand="gvSendHQList_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="create_time" HeaderText="日期">
                            <HeaderStyle HorizontalAlign="Center" Width="200px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Title" HeaderText="红包主题" >
                            <HeaderStyle Width="200px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="send_listidex" HeaderText="订单号">
                            <HeaderStyle Width="200px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="amount_text" HeaderText="金额">
                            <HeaderStyle Width="80px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="state_text" HeaderText="状态">
                            <HeaderStyle Width="200px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="summary" HeaderText="红包数量" >
                            <HeaderStyle Width="200px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="refund" HeaderText="退款金额" >
                            <HeaderStyle Width="200px" /><ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="channel_text" HeaderText="红包类型">
                                <HeaderStyle Width="80px"></HeaderStyle> <ItemStyle  HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="详情" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnView2" runat="server" CausesValidation="False" CommandArgument='<%#string.Format("{0},{1}",((System.Data.DataRowView)Container.DataItem)["send_listid"].ToString(), ((System.Data.DataRowView)Container.DataItem)["create_time"].ToString())%>'
                                        CommandName="ViewDetail" Text="详情"></asp:LinkButton>
                                </ItemTemplate>
                                 <HeaderStyle Width="200px" />
                                 <ItemStyle HorizontalAlign="Center" />
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
                        onpagechanged="sendHQListPager_PageChanged"></webdiyer:aspnetpager>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
