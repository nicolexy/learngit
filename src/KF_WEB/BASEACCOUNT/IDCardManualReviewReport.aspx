<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IDCardManualReviewReport.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.IDCardManualReviewReport" %>

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
    <script src="js/IDCardManualReviewReport.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]%>"></script>


</head>
<body>
    <table style="height: 15px; width: 100%; background-color: #e4e5f7; border: 0; padding: 1px;">
        <tr>
            <td width="80%" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										身份证影印件客服人工审核报表</font>
                <div align="right"></div>
            </td>
            <td width="20%">操作员代码: <span style="color: #ff0000">
                <asp:Label ID="Label_uid" runat="server">Label</asp:Label></span></td>
        </tr>
    </table>
    <div id="toolbar" style="width: 100%">
        <table style="width: 100%">

            <tr>
                <td style="width: 8%; min-width: 100px; text-align: right">
                    <label id="lab_ModifyBeginDate">审核开始日期：</label></td>
                <td style="width: 25%; min-width: 150px;">
                    <input type="text"  id="txt_ModifyBeginDate" />
                </td>
                <td style="width: 8%; min-width: 100px; text-align: right">
                    <label id="lab_ModifyEndDate">审核结束日期：</label></td>
                <td style="width: 25%; min-width: 150px;">
                    <input type="text"  id="txt_ModifyEndDate" />
                </td>
                <td style="width: 8%; min-width: 100px; text-align: right">
                    <label id="lab_Foperator">审核人：</label></td>
                <td style="width: 25%; min-width: 150px;">
                    <input type="text"  id="txt_Foperator" />

                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: center">

                    <a href="javascript:void(0)" id="btn_LoadHZReport" class="easyui-linkbutton" iconcls="icon-search" plain="true">个人汇总报表查询</a>&nbsp;&nbsp;&nbsp;
                        <a href="javascript:void(0)" id="btn_LoadPersonalReviewReport" class="easyui-linkbutton" iconcls="icon-search" plain="true">个人审核情况报表查询</a>&nbsp;&nbsp;&nbsp;
                        <a href="javascript:void(0)" id="btn_LoadFailReasonReport" class="easyui-linkbutton" iconcls="icon-search" plain="true">失败原因报表查询</a>&nbsp;&nbsp;&nbsp;
                        <span style="color: red;">注:不能跨月查询</span>
                    <%--<a href="javascript:void(0)" id="a_DownloadReviewData" class="easyui-linkbutton" iconcls="icon-print" plain="true">导出</a>--%>                        
                </td>
                <td style="width: 30%"></td>
            </tr>
        </table>
    </div>
    <br />
    <div id="div_HZReport" style="width: 100%">
        <table id="tb_HZReport"></table>
    </div>
    <div id="div_PersonalReviewReport" style="width: 100%">
        <table id="td_PersonalReviewReport"></table>
    </div>
    <div id="div_FailReasonReport" style="width: 100%">
        <table id="td_FailReasonReport"></table>
    </div>

</body>
</html>
