<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logOnUserBatch.aspx.cs" Inherits="TENCENT.OSS.C2C.KF.KF_Web.BaseAccount.logOnUserBatch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
    <form id="Form1" method="post" runat="server">
    <table cellspacing="1" cellpadding="0" width="95%" align="center" bgcolor="#666666"
        border="0">
        <tr bgcolor="#e4e5f7" style="background-image: ../IMAGES/Page/bg_bl.gif">
            <td valign="middle" colspan="2" height="20">
                <table style="height: 90%" cellspacing="0" cellpadding="1" width="97%" border="0">
                    <tr>
                        <td width="80%" style="background-image: ../IMAGES/Page/bg_bl.gif" height="18">
                            <font color="#ff0000">
                                <img height="16" src="../IMAGES/Page/post.gif" width="20">
                                批量注销</font>
                        </td>
                        <td width="20%" style="background-image: ../IMAGES/Page/bg_bl.gif">
                            操作员代码: <span class="style3">
                                <asp:Label ID="Label_uid" runat="server" Width="73px"></asp:Label></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td width="20%" align="right">
                销户类型
            </td>
            <td>
                <div align="left">
                    <%--<input id="CFT" name="IDType" runat="server" type="radio" checked /><label for="CFT">财付通、手Q</label>
                    &nbsp;&nbsp;--%>
                    <input id="InternalID" name="IDType" runat="server" type="radio" checked/><label for="InternalID">微信支付财付通帐号</label>
                </div>
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td width="20%" align="right">
                选择批量注销文件
            </td>
            <td align="left">
                <asp:FileUpload ID="File1" runat="server" Width="300px" />
                &nbsp;&nbsp;
                <a href="/Template/Excel/LogOffTemplate.xls" target="_blank">下载模版</a>
            </td>
        </tr>
        <tr bgcolor="#ffffff">
            <td colspan="2" align="center">
                <asp:Button ID="btn_batch_cancel" runat="server" Text="批量销户申请" OnClick="btn_batch_cancel_Click"/>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
