<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IDCardManualReview.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.IDCardManualReview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );

       <%-- BODY {
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
        }--%>
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="../SCRIPTS/jquery-3.0.0.min.js"></script>
    <script type="text/javascript" src="../SCRIPTS/jquery-easyui-1.5/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="../SCRIPTS/jquery-easyui-1.5/locale/easyui-lang-zh_CN.js" charset="utf-8"></script>
    <link href="../SCRIPTS/jquery-easyui-1.5/themes/default/easyui.css" rel="stylesheet" />
    <link href="../SCRIPTS/jquery-easyui-1.5/themes/icon.css" rel="stylesheet" />
    <script src="../SCRIPTS/KF.js"></script>
    <script src="../SCRIPTS/LoadControlsDataSource.js"></script>
    <script src="IDCardManualReview.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        
        <div id="toolbar" style="width: 100%">
            <table cellspacing="1" cellpadding="0" width="99%" align="center" bgcolor="#666666" border="0">
                <tr bgcolor="#e4e5f7">
                    <td valign="middle" colspan="2" height="20">
                        <table height="90%" cellspacing="0" cellpadding="1" width="97%" border="0">
                            <tr>
                                <td width="80%" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										身份证影印件客服人工审核</font>
                                    <div align="right"></div>
                                </td>
                                <td width="20%">操作员代码: <span style="color: #ff0000">
                                    <asp:Label ID="Label_uid" runat="server">Label</asp:Label></span></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr bgcolor="#ffffff">
                    <td>
<%--                        <div align="center"></div>
                        <div align="left">--%>
                            <table height="100%" cellspacing="0" cellpadding="1" width="100%" border="0">
                                <tr>
                                    <td style="width: 20%; text-align: right">
                                        <label id="lab_StartDate">开始日期：</label></td>
                                    <td style="width: 30%">
                                        <input type="text" runat="server" id="tbx_beginDate" />
                                        <%--onclick="WdatePicker()"--%>
                                    </td>
                                    <td style="width: 20%; text-align: right">
                                        <label id="lab_EndDate">结束日期：</label></td>
                                    <td style="width: 30%">
                                        <input type="text" runat="server" id="txt_EndDate" />
                                    </td>
                                </tr>
                                <tr>

                                    <td style="width: 20%; text-align: right">
                                        <label id="lab_status">审核状态：</label></td>
                                    <td style="width: 30%">
                                        <input id="ddl_ReviewStatus" type="text" />
                                    </td>
                                    <td style="width: 20%; text-align: right">
                                        <label id="Label2">审核结果：</label></td>
                                    <td style="width: 30%">
                                        <input id="ddl_ReviewResult" type="text" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: right">
                                        <label id="lab_uin">帐号：</label></td>
                                    <td style="width: 30%">
                                        <input type="text" runat="server" id="txt_uin" />
                                    </td>
                                    <td style="width: 20%; text-align: right">
                                        <label id="Label1">批处理数：</label></td>
                                    <td style="width: 30%">
                                        <input type="text" id="txt_ReviewCount" />
                                        <%--<input type="button" id="btn_ReceiveReview" value="批量领单" />--%>
                                        <a href="javascript:void(0)" id="btn_ReceiveReview" class="easyui-linkbutton" iconcls="icon-set" plain="true">批量领单</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: right"></td>
                                    <td style="width: 30%">
                                         <span style="color:red;">注:不能跨月查询</span>
                                    </td>
                                    <td style="width: 20%; text-align: left">
                                        <%--<input type="button" id="btn_Search" value="查 询" />--%>
                                        <a href="javascript:void(0)" id="btn_Search" class="easyui-linkbutton" iconcls="icon-search" plain="true">查 询</a>
                                       
                                        <%--<asp:Button ID="btn_Search" runat="server" Text="查 询" OnClick="btn_Search_Click"></asp:Button>--%>

                                    </td>
                                    <td style="width: 30%"></td>
                                </tr>
                            </table>
                        <%--</div>--%>

                    </td>

                </tr>
            </table>
        </div>
        <div id="div_IDCardManualReviewList" style="width: 99%">
            <table id="tb_IDCardManualReviewList"></table>
        </div>
        <div id="div_ReveiwIdCard" class="easyui-dialog">
            <table border="0" style="width: 100%; text-align: left; line-height: 20px; margin-top: 10px">
                <tr style="text-align: left">
                    <th colspan="2" style="width: 10%; color: red; text-align: center;" colspan="">提交资料(***流水号***)
                    </th>
                </tr>
                <tr style="text-align: left">
                    <th style="width: 30%; text-align: right">用户帐号：
                    </th>
                    <td style="width: 70%">
                        <label id="lab_Fuin"></label>
                    </td>
                </tr>

                <tr style="text-align: left">
                    <th style="width: 30%; text-align: right">用户姓名：
                    </th>
                    <td style="width: 70%">
                        <label id="lab_Fname"></label>
                    </td>
                </tr>
                <tr style="text-align: left">
                    <th style="width: 30%; text-align: right">证件号码：
                    </th>
                    <td style="width: 70%">
                        <label id="lab_Fidentitycard"></label>
                    </td>
                </tr>
                <tr>

                    <th style="width: 30%; text-align: right">失败原因：
                    </th>
                    <td style="width: 70%">
                        <input type="text" id="txt_Fmemo" />
                    </td>
                </tr>
                <tr>
                    <th colspan="2" style="width: 10%; text-align: center">身份证扫描件
                    </th>

                </tr>
                <tr>
                    <td style="width: 50%; text-align: center">
                        <span>正面</span><br />
                        <img id="ima_IDCardZ" src="" style="width: 300px; height: 300px" />
                    </td>
                    <td style="width: 50%; text-align: center">
                        <span>反面</span><br />
                        <img id="ima_IDCardF" src="" style="width: 300px; height: 300px" />
                    </td>
                </tr>
                <tr>
                    <th colspan="2" style="width: 10%; color: red; text-align: center;">
                        <input id="hid_TableName" type="hidden" />
                        <input id="hid_Fid" type="hidden" />
                        <input id="hid_Fserial_number" type="hidden" />

                        <a href="javascript:void(0)" id="a_Yes" class="easyui-linkbutton" iconcls="icon-save" plain="true">通过</a>&nbsp;&nbsp;&nbsp;
                        <a href="javascript:void(0)" id="a_No" class="easyui-linkbutton" iconcls="icon-save" plain="true">拒绝</a>&nbsp;&nbsp;&nbsp;
                        <a href="javascript:void(0)" id="a_ReSend" class="easyui-linkbutton" iconcls="icon-save" plain="true">重新提交</a>
                        <%--    <input id="btn_Yes" type="button" value="通过"/>
                        <input id="btn_No" type="button" value="拒绝"/>
                        <input id="btn_ReSend" type="button" value="重新提交"/>--%>
                    </th>
                </tr>
            </table>
        </div>
        <input id="hid_IdCaredServerPath" type="hidden" runat="server" />
    </form>
</body>
</html>
