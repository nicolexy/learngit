<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetailSearch.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.CreditPay.DetailSearch" %>

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
    <%--<script src="../SCRIPTS/LoadControlsDataSource.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]%>"></script>--%>
    <script src="JS/DetailSearch.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]%>"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table style="height: 15px; width: 100%; background-color: #e4e5f7; border: 0; padding: 1px;">
        <tr>
            <td width="80%" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										账单查询</font>
                <div align="right"></div>
            </td>
            <td width="20%">操作员代码: <span style="color: #ff0000">
                <asp:Label ID="Label_uid" runat="server">Label</asp:Label></span></td>
        </tr>
    </table>
    <div id="toolbar" style="width: 100%">
        <table style="width: 100%">
            <tr>
               <td style="width: 5%; min-width: 100px; text-align: right">
                    <label id="lab_Account">用户帐号：</label>
                </td>
                 <td style="width: 15%; min-width: 300px;">
                   <input type="text" id="txt_Account" />
                    <input id="QQ" name="AccountType" type="radio" value="0" checked="checked" /><label>QQ号</label>
                    <input id="WebChatAccount" name="AccountType" type="radio" value="1" /><label>微信号</label> 
                </td>
                <td style="width: 5%; min-width: 100px; text-align: right">
                    <label id="lab_TransID">交易单号：</label></td>
                <td style="width: 10%; min-width: 150px;">
                    <input type="text" id="txt_TransID" />

                </td>
                <td style="width: 5%; min-width: 100px; text-align: right">
                    <label id="lab_BeginDate">开始日期：</label></td>
                <td style="width: 10%; min-width: 150px;">
                    <input type="text" id="txt_BeginDate" />
                </td>
                <td style="width: 5%; min-width: 100px; text-align: right">
                    <label id="lab_EndDate">结束日期：</label></td>
                <td style="width: 10%; min-width: 150px;">
                    <input type="text" id="txt_EndDate" />
                </td>      
                <td style="width: 5%; min-width: 100px; text-align: right">
                    <label id="lab_SearchType">查询类型：</label></td>
                <td style="width: 10%; min-width: 150px;">
                    <input type="text" id="ddl_SearchType" />
                </td>                
                <td  style="text-align: left">
                    <a href="javascript:void(0)" id="btn_Search" class="easyui-linkbutton" iconcls="icon-search" plain="true">查询</a>                    
                </td>
            </tr>
            <tr>
                
                
            </tr>
        </table>
    </div>
    <%--支付明细查询--%>
    <div id="div_PayList"  style="width: 100%">
        <table id="tb_PayList"></table>
        <input type="hidden" id="hid_PayListnextpage_flg" value="True"/>
        <input type="hidden" id="hid_PayListnext_row_key" value="0"/>
        <input type="hidden" id="hid_PayListPageSize" value="20"/>
        <input type="hidden" id="hid_PayListPageNumber" value="1" />
    </div>
    <%--还款明细查询--%>
    <div id="div_RepayList"   style="width: 100%">
        <table id="tb_RepayList"></table>
        <input type="hidden" id="hid_RepayListnextpage_flg" value="True"/>
        <input type="hidden" id="hid_RepayListnext_row_key" value="0"/>
         <input type="hidden" id="hid_RepayListPageSize" value="20"/>
        <input type="hidden" id="hid_RepayListPageNumber" value="1" />
    </div>
    <%--退款明细查询--%>
    <div id="div_RefundList"   style="width: 100%">
        <table id="tb_RefundList"></table>
        <input type="hidden" id="hid_RefundListnextpage_flg" value="True"/>        
        <input type="hidden" id="hid_RefundListnext_row_key" value="0"/>
        <input type="hidden" id="hid_RefundListPageSize" value="20" />
        <input type="hidden" id="hid_RefundListPageNumber" value="1" />
    </div>
    <%--退款详情--%><%--退款去向--%>
     <div id="div_RefundDetail"  class="easyui-dialog" style="width: 100%">         
        <table id="tab_RefundDetail"></table>
         <table id="tab_RefundQuXiang"></table>
    </div>
    </form>
</body>
</html>
