<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logOnWxAccount.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.logOnWxAccount" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>OverseasReturnQuery</title>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );

        BODY {
            background-image: url(../IMAGES/Page/bg01.gif);
        }

        table {
            font-family: "Microsoft YaHei";
            font-size: 12px;
            background-color: #CCCCCC;
        }

            table td {
                height: 20px;
                padding: 2px 4px;
                background-color: #FFFFFF;
            }
        .tbtr {
            text-align: left;
            background-color: #D7D7D7;
            color: #1462ad;
            font-weight: bold;
            height: 20px;
            padding: 2px 4px;
        }

        .th th {
            text-align: center;
            background-color: #D7D7D7;
            color: #1462ad;
            font-weight: bold;
            height: 20px;
            padding: 2px 4px;
        }

        table .name {
            width: 40%;
            font-weight: bold;
            text-align: right;
        }

        table .value {
            width: 50%;
            text-align: left;
        }
    </style>
    <script type="text/javascript" src="../scripts/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="js/LogOnWxAccount.js"></script>
    <script type="text/javascript">
        $(function () {
            $("input[type=text],#txt_reason").bind("focus", fucusClear);           
        });
       
    </script>
</head>
<body>
    <form id="Form2" method="post" runat="server">
        <div class="container">
            <div id="top" style="width: 95%;">
                <table cellspacing="1" cellpadding="0" align="center" bgcolor="#666666" border="0" width="95%">
                    <tr bgcolor="#e4e5f7" style="background-image: url(../IMAGES/Page/bg_bl.gif)">
                        <td valign="middle" colspan="2">
                            <table style="height: 90%; width: 100%" cellspacing="0" cellpadding="1" border="0" class="tdfull">
                                <tr>
                                    <td width="80%" style="background-image: url(../IMAGES/Page/bg_bl.gif)" height="18">
                                        <font color="#ff0000"><strong><font color="#ff0000">&nbsp;</font></strong><img height="16"
                                            src="../IMAGES/Page/post.gif" width="20">
                                            用户销户 </font>
                                    </td>
                                    <td width="20%" style="background-image: url(../IMAGES/Page/bg_bl.gif)">操作员代码: <span style="color: #ff0000">
                                        <asp:Label ID="Label_uid" runat="server" Width="200px"></asp:Label></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="middle" style="padding-top: 10px; width: 95%;">
                <table cellspacing="1" cellpadding="0" align="center" bgcolor="#666666" border="0" width="95%">
                    <tr>
                        <td class="name">输入销户账号（微信账号）:</td>
                        <td char="value">
                            <input type="text" id="txt_wxid" /></td>
                    </tr>
                    <tr>
                        <td class="name">再次确认账号:</td>
                        <td char="value">
                            <input type="text" id="txt_re_wxid" /></td>
                    </tr>
                    <tr>
                        <td class="name">销户原因:</td>
                        <td char="value">
                            <textarea id="txt_reason" rows="3" cols="50"></textarea></td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <input type="button" onclick="ajaxSubmit();" value="销  户"></td>
                    </tr>
                </table>
            </div>
            <div id="bottom" style="padding-top: 10px; width: 95%;">
                <table cellspacing="1" cellpadding="0" align="center" bgcolor="#666666" border="0" width="95%">
                    <tr>
                        <td>
                            <div id="error_info" style="display: block; color: red; float: left;"></div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
