<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusCardPrepaidQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BusCardPrepaidQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>公交卡充值查询</title>
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
        <table border="0" cellspacing="0" cellpadding="10px" width="100%" style="margin:10px;">
            <tr>
                <td bgcolor="#e4e5f7" colspan="5">
                    <font color="red">
                        <img src="../images/page/post.gif" width="20" height="16">公交卡充值查询</font>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    开始日期：&nbsp;<b style="color:red">*</b>
                    <asp:TextBox ID="textBoxBeginDate" runat="server" Width="135px" onClick="WdatePicker()"  CssClass="Wdate"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;
                    结束日期：<b style="color:red">*</b>
                        <asp:TextBox ID="textBoxEndDate" runat="server" Width="131px" onClick="WdatePicker()"  CssClass="Wdate"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    财付通账号&nbsp;<b style="color:red">*</b>
                    <asp:TextBox ID="textBoxAccountID" runat="Server" Width="133px"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;
                    卡面号&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="textBoxCardNum" runat="Server" Width="130px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    财付通订单号
                    <asp:TextBox ID="textBoxOrderID" runat="Server" Width="130px"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="buttonQuery" runat="Server" Text="查询" OnClick="buttonQuery_Click"/>
                </td>
            </tr>
            <tr><td></td></tr>
            <tr><td></td></tr>
            
        </table>
        <table border="0" cellspacing="0" cellpadding="0" width="100%">
        	<tr>
                <td style="margin-left:30px;">
                    <asp:GridView ID="gridViewQueryResult" runat="Server" AllowPaging="True" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" Width="896px" OnPageIndexChanging="gridViewQueryResult_PageIndexChanging" PageSize="20">
                        <AlternatingRowStyle BackColor="#99FF33" />
                        <Columns>
                            <asp:BoundField HeaderText="时间" DataField="Fcreate_time"/>
                            <asp:BoundField HeaderText="财付通订单号" DataField="Ftransaction_id"/>
                            <asp:BoundField HeaderText="QQ" DataField="Fuin"/>
                            <asp:BoundField HeaderText="卡类型" DataField="Fcity_code"/>
                            <asp:BoundField HeaderText="卡面号" DataField="Fcard_face_no"/>
                            <asp:BoundField HeaderText="充值金额" DataField="Ftotal_fee"/>
                            <asp:BoundField HeaderText="支付状态" DataField="Fpay_status"/>
                            <asp:BoundField HeaderText="圈存状态" DataField="Fload_statu"/>
                            <asp:BoundField HeaderText="手机型号" DataField="Fdevice_type"/>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
