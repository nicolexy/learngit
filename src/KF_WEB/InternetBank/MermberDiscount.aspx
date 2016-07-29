<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MermberDiscount.aspx.cs"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.InternetBank.MermberDiscount" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>会员优惠额度</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
        UNKNOWN
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
    <form id="form1" runat="server">
    <table style="z-index: 100; position: absolute; top: 5%; left: 5%" id="Table1" border="1"
        cellspacing="1" cellpadding="1" width="900px">
        <tr style="background-color: #e4e5f7">
            <td colspan="7">
                <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" /><label style="color: Red;
                    padding-left: 30px">当月会员优惠额度查询</label>
            </td>
        </tr>
        <tr>
            <td colspan="1">
                <label style="padding-left: 10px;">
                    请输入支付QQ号码:</label>
            </td>
            <td style="text-align: left" colspan="5">
                <asp:TextBox ID="txtQQ" Width="200px" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: center" colspan="1">
                <asp:Button Text="查询" runat="server" ID="btn_Query" Width="100px" OnClick="btn_Query_Click">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td>
                日期(月份)
            </td>
            <td>
                QQ号码
            </td>
            <td>
                会员级别
            </td>
            <td>
                优惠额度(Q币)
            </td>
            <td>
                本月剩余Q币优惠额度(自己)
            </td>
            <td>
                本月剩余Q币优惠额度(好友)
            </td>
            <td>
                有效期
            </td>
        </tr>
        <tr>
            <td style="width:130px;">
                <label runat="server" id="dateLbl"></label>
            </td>
            <td style="width:130px;">
                <label runat="server" id="qqLbl"></label>
            </td>
            <td style="width:130px;">
                <label runat="server" id="levelLbl"></label>
            </td>
            <td style="width:130px;">
                <label runat="server" id="discountLbl"></label>
            </td>
            <td style="width:130px;">
                <label runat="server" id="remainDiscountLbl"></label>
            </td>
            <td style="width:130px;">
                <label runat="server" id="lb_friendQB"></label>
            </td>
             <td style="width:130px;">
                <label runat="server" id="validDateLbl"></label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
