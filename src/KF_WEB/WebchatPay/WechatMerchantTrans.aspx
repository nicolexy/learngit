<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WechatMerchantTrans.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.WechatMerchantTrans" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>微信/财付通商户信息互查</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );

        BODY {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
		</style>
</head>
<body>
    <form id="formMain" runat="server">
        <div>
            <table border="1" cellspacing="1" cellpadding="1">
                <tr>
                    <td style="width: 100%" bgcolor="#e4e5f7" colspan="2"><font color="red">
                        <img src="../images/page/post.gif" width="20" height="16">微信/财付通商户信息互查</font>
                    </td>
                </tr>
                <tr>
                    <td>账号:<asp:TextBox ID="txtAccId" Width="400px" runat="server"></asp:TextBox></td>
                    <td align="center">
                        <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询"
                            OnClick="btnQuery_Click"></asp:Button></td>
                </tr>
                <tr>
                    <td align="left" colspan="2">财付通商户号：
                       <asp:Label ID="lbSpid" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">微信支付商户号：
                       <asp:Label ID="lbMchid" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
