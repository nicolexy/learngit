<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IceOutPPSecurityMoney.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.IceOutPPSecurityMoney" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="js/IceOutPPSecurityMoney.js"></script>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );

        BODY {
            background-image: url(../IMAGES/Page/bg01.gif);
        }

        table {
            font-family: "Microsoft YaHei";
            font-size: 12px;
        }

            table td {
                height: 20px;
                padding: 2px 4px;
                background-color: #FFFFFF;
            }

            table .name {
                font-weight: bold;
                text-align: right;
            }

            table .value {
                text-align: left;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div id="top" style="width: 60%;">
                <table cellspacing="1" cellpadding="0" align="center" bgcolor="#666666" border="0" width="95%">
                    <tr bgcolor="#e4e5f7" style="background-image: url(../IMAGES/Page/bg_bl.gif)">
                        <td valign="middle" colspan="2">
                            <table style="height: 90%; width: 100%" cellspacing="0" cellpadding="1" border="0" class="tdfull">
                                <tr>
                                    <td width="80%" style="background-image: url(../IMAGES/Page/bg_bl.gif)" height="18">
                                        <font color="#ff0000"><strong><font color="#ff0000">&nbsp;</font></strong><img height="16"
                                            src="../IMAGES/Page/post.gif" width="20">
                                            拍拍保证金解冻</font>
                                    </td>
                                    <td width="20%" style="background-image: url(../IMAGES/Page/bg_bl.gif)">操作员代码: <span style="color: #ff0000">
                                        <asp:Label ID="Label_uid" runat="server" Width="200px"></asp:Label></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" colspan="2">
                            <table width="95%" cellspacing="0" cellpadding="1" border="0" class="tdfull">
                                <tr>
                                    <td class="name">卖家账户：</td>
                                    <td class="value">
                                        <input type="text" id="txt_uin" onfocus="fucusClear();"/></td>
                                </tr>
                                <tr>
                                    <td class="name">交易单号：</td>
                                    <td class="value">
                                        <input type="text" id="txt_transaction_id" onfocus="fucusClear();"/></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: right;">
                                        <button type="button" onclick="ajaxSubmit();">解冻</button></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><span id="tip_message" style="display:block;text-align: center; color: red; font-weight: bold;"></span></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
