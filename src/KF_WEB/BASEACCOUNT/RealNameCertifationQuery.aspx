<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RealNameCertifationQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.RealNameCertifationQuery" %>

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
    <script type="text/javascript" src="../scripts/IdentityIDValidate.js"></script>
    <script type="text/javascript" src="../SCRIPTS/laypage/laypage.js"></script>
    <link href="../SCRIPTS/laypage/skin/laypage.css" rel="stylesheet" />
    <script type="text/javascript" src="js/RealNameCertifation.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#certifationtype").bind("change", function () {
                $("#middle table").find("tbody").empty();
                $("#page11").empty();
                $("#bottom").empty();
                $("#certicondtion div:eq(" + $(":selected", this).index() + ")").siblings().css("display", "none");
                $("#certicondtion div:eq(" + $(":selected", this).index() + ")").css("display", "block").find("input[type=text]").val("");
                $("#error_info").html("");
                $("#error_userinfo").html("");
            });
        });
    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
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
                                            实名认证查询</font>
                                    </td>
                                    <td width="20%" style="background-image: url(../IMAGES/Page/bg_bl.gif)">操作员代码: <span style="color: #ff0000">
                                        <asp:Label ID="Label_uid" runat="server" Width="200px"></asp:Label></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr bgcolor="#ffffff">
                        <td>
                            <table cellspacing="0" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td style="padding-left: 10px; width: 40px;">
                                        <select id="certifationtype">
                                            <option selected="selected">身份证</option>
                                            <option>账户</option>
                                        </select>
                                    </td>
                                    <td>
                                        <div id="certicondtion">
                                            <div style="padding: 1px;">
                                                <span style="display: block; float: left;">
                                                    <input type="text" id="txt_identity" style="width: 200px;" onfocus="fucusClear();" /></span><span id="error_info" style="display: block; color: red; float: left;"></span>
                                            </div>
                                            <div style="display: none">
                                                <table style="height: 100%" cellspacing="0" cellpadding="1" width="100%" border="0">
                                                    <tr>
                                                        <td>
                                                            <div style="padding: 1px;">
                                                                <span style="display: block; float: left;">
                                                                    <input type="text" id="txt_usertype" style="width: 200px;" onfocus="fucusClear();" /></span><span id="error_userinfo" style="display: block; color: red; float: left;"></span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <input id="WeChatId" name="IDType" runat="server" type="radio" checked /><label for="WeChatId">微信帐号</label>
                                                            <input id="WeChatQQ" name="IDType" runat="server" type="radio" /><label for="WeChatQQ">微信绑定QQ</label>
                                                            <input id="WeChatMobile" name="IDType" runat="server" type="radio" /><label for="WeChatMobile">微信绑定手机</label>
                                                            <input id="WeChatCft" name="IDType" runat="server" type="radio" /><label for="WeChatCft">财付通账户</label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <input type="button" style="width: 80px;" onclick="ajaxSubmit();" value="查询" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="middle" style="padding-top: 10px; width: 95%;">
                <table cellspacing="1" cellpadding="0" align="center" bgcolor="#666666" border="0" width="95%">
                    <thead class="th">
                        <tr>
                            <th>账户</th>
                            <th>姓名</th>
                            <th>证件类型</th>
                            <th>证件号码</th>
                            <th>公安部认证时间</th>
                            <th>影印件认证时间</th>
                            <th>运营商认证时间</th>
                            <th>学历认证时间</th>
                            <th>账户类型</th>
                            <th>认证结果</th>
                            <th>银行卡</th>
                            <th>银行名称</th>
                            <th>手机</th>
                            <th>银行卡认证时间</th>
                            <th>额度(单位：分)</th>
                            <th>白名单</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div style="padding-top: 10px; width: 90%;">
                <div id="page11" style="float: right;"></div>
            </div>
            <div id="bottom" style="padding-top: 10px; width: 95%;">
            </div>
        </div>
    </form>
</body>
</html>
