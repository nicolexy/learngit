<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomsBatchRedeclare.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.CustomsBatchRedeclare" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
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

        .container {
            width: 1200px;
            margin-top: 10px;
            margin-left: 10px;
        }

        .style_title {
            padding-top: 8px;
            vertical-align: middle;
            height: 35px;
            text-align: left;
            background: #D7D7D7;
            color: #1462AD;
            font-size: 18px;
            font-weight: bold;
            font-family: "Microsoft YaHei";
            border: 1px solid #CCCCCC;
        }

        .container caption {
            padding-top: 8px;
            padding-left: 5px;
            vertical-align: middle;
            height: 30px;
            text-align: left;
            color: #646464;
            font-size: 16px;
            font-weight: bold;
            font-family: "Microsoft YaHei";
            border: 1px solid #CCCCCC;
            border-bottom-width: 0px;
        }

        .container table {
            width: 100%;
            background-color:  #CCCCCC;
            border-width: 2px;
            font-family: "Microsoft YaHei";
            font-size: 13px;
            font-weight: bold;
        }

        .container td, .container th {
            background: #fff;
            height: 25px;
            padding: 2px 4px;
        }

        .tb_query_title {
            width: 40%;
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="formMain" runat="server">
        <div class="container">
            <div class="style_title">
                海关重推操作
            </div>
            <br />
            <table cellspacing="1" cellpadding="1" width="1100">
                <caption>
                    单笔重推
                </caption>
                <tr>
                    <td class="tb_query_title">商户号：
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_partner"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tb_query_title">海关备案号：
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_customs_company_code"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tb_query_title">海关编号：
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_customs"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tb_query_title">财付通支付单号：
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_transaction_id"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tb_query_title">商户订单号：
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_out_trade_no"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td class="tb_query_title">重推原因：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_redeclare_reason"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <asp:Button ID="btn_one" runat="server" Text="提 交" OnClick="btn_one_Click" />

                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblmessageOne" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <table cellspacing="1" cellpadding="1" width="1100">
                <caption>批量重推</caption>
                <tr>
                    <td>请上传重推文件：<asp:FileUpload runat="server" ID="fileUpload" />
                        <asp:Button ID="btnUpload" runat="server" Width="80px" Text="上传" OnClick="btnUpload_Click"></asp:Button>
                        <a href="/Template/Excel/CustomsTemplate.xls">点击下载文件模板</a>
                    </td>

                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblmessage" Text="重推已提交，相关情况请查看邮件。" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>

        </div>
        <br />
    </form>
</body>
</html>
