<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomsBatchRedeclare.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.CustomsBatchRedeclare" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>海关重推</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );

        UNKNOWN {
            COLOR: #000000;
        }

        .style3 {
            COLOR: #ff0000;
        }

        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
        }

        .auto-style1 {
            width: 100%;
            height: 19px;
        }
    </style>
</head>
<body>
    <form id="formMain" runat="server">
        <table border="1" cellspacing="1" cellpadding="1" width="1100">
            <tr>
                <td style="width: 100%" bgcolor="#e4e5f7" colspan="5"><font color="red">
                    <img src="../images/page/post.gif" width="20" height="16">海关重推</font>
                </td>
            </tr>
            <tr>
                <td>请上传重推文件：<asp:FileUpload runat="server" ID="fileUpload" />
                    <asp:Button ID="btnUpload" runat="server" Width="80px" Text="上传" OnClick="btnUpload_Click" ></asp:Button>
                    <a href="/Template/Excel/CustomsTemplate.xls">点击下载文件模板</a>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblmessage" Text="重推已提交，相关情况请查看邮件。" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </form>
</body>
</html>
