<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountSearch.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.CreditPay.AccountSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
        .transp-block{background:#fff no-repeat right bottom;margin:0px auto;width:555px;height:320px;overflow:hidden}
        .transparent{filter:alpha(opacity=70);moz-opacity:.70;opacity:.70}
    </style>
    <%--<script src="../SCRIPTS/jquery-1.7.2/jquery.min.js"></script>--%>
    <script src="../SCRIPTS/jquery-1.9.1/jquery.min.js"></script>
    <link rel="stylesheet" type="text/css" href="css/normalize.css" />

    <script type="text/javascript" src="../SCRIPTS/jquery-easyui-1.5/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="../SCRIPTS/jquery-easyui-1.5/locale/easyui-lang-zh_CN.js" charset="utf-8"></script>
    <link href="../SCRIPTS/jquery-easyui-1.5/themes/default/easyui.css" rel="stylesheet" />
    <link href="../SCRIPTS/jquery-easyui-1.5/themes/color.css" rel="stylesheet" />
    <link href="../SCRIPTS/jquery-easyui-1.5/themes/icon.css" rel="stylesheet" />
    <script src="../SCRIPTS/KF.js"></script>
    <script src="../SCRIPTS/LoadControlsDataSource.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]%>"></script>
    <script src="JS/AccountSearch.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]%>"></script>
</head>
<body>
    <table style="height: 15px; width: 100%; background-color: #e4e5f7; border: 0; padding: 1px;">
        <tr>
            <td width="80%" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										账户信息</font>
                <div align="right"></div>
            </td>
            <td width="20%">操作人: <span style="color: #ff0000">
                <asp:Label ID="Label_uid" runat="server">Label</asp:Label></span></td>
        </tr>
    </table>
    <div id="toolbar" style="width: 100%">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%; min-width: 800px;">
                    <label id="lab_Foperator">用户：</label>
                    <input type="text" id="txt_Account" />
                    <input id="QQ" name="AccountType" type="radio" value="0" checked="checked" /><label>QQ号</label>
                    <input id="WebChatAccount" name="AccountType" type="radio" value="1" /><label>微信号</label>
                    <input id="IdCard" name="AccountType" type="radio" value="2" /><label>身份证</label>&nbsp;&nbsp;&nbsp; 
                    <a href="javascript:void(0)" id="btn_Search" class="easyui-linkbutton" iconcls="icon-search" plain="true">查询</a>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div style="width: 100%">
        <table style="width:80%;height:100%;">
            <tr>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>账户:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_Acct_no"></label>
                </td>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>账户类型:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_Acct_Type"></label>
                </td>
            </tr>
            <tr>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>姓名:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_Name"></label>
                </td>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>手机号码:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_Mobile"></label>
                </td>

            </tr>
            <tr>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>证件号码:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_id_card_no"></label>
                </td>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>币种类型:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_cur_type"></label>
                </td>
            </tr>
            <tr>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>总授信额度:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_credit_line"></label>
                </td>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>可用额度:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_credit"></label>
                </td>
            </tr>
            <tr>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>冻结额度:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_credit_frezz"></label>
                </td>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>已用额度:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_credit_used"></label>
                </td>
            </tr>
            <tr>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>财付通状态:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_cft_staus"></label>
                </td>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>账单日:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_bill_date"></label>
                </td>
            </tr>
            <tr>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>还款日:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_repay_date"></label>
                </td>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>是否逾期:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_IsOverDue"></label>
                </td>
            </tr>
            <tr>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>逾期天数:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_OverdueDays"></label>
                </td>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>逾期应还总额:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_OverdueBalance"></label>
                </td>
            </tr>
            <tr>
                <td style="width: 10%; min-width: 100px; padding:5px; margin:5px; text-align: right; font-size:larger;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>账户状态:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_staus"></label>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
