<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillSearch.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.CreditPay.BillSearch" %>

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
    <script src="JS/BillSearch.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]%>"></script>


</head>
<body>
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
                 <td style="width: 20%; min-width: 300px;">
                   <input type="text" id="txt_Account" />
                    <input id="QQ" name="AccountType" type="radio" value="0" checked="checked" /><label>QQ号</label>
                    <input id="WebChatAccount" name="AccountType" type="radio" value="1" /><label>微信号</label> 
                </td>
                <td style="width: 10%; min-width: 100px; text-align: right">
                    <label id="lab_BillStatus">账单状态：</label></td>
                <td style="width: 10%; min-width: 150px;">
                    <input type="text" id="txt_BillStatus" />

                </td>
                <td style="width: 10%; min-width: 100px; text-align: right">
                    <label id="lab_BeginDate">开始日期：</label></td>
                <td style="width: 10%; min-width: 150px;">
                    <input type="text" id="txt_BeginDate" />
                </td>
                <td style="width: 10%; min-width: 100px; text-align: right">
                    <label id="lab_EndDate">结束日期：</label></td>
                <td style="width: 10%; min-width: 150px;">
                    <input type="text" id="txt_EndDate" />
                </td>                
                <td  style="text-align: left">
                    <a href="javascript:void(0)" id="btn_Search" class="easyui-linkbutton" iconcls="icon-search" plain="true">查询</a>                    
                </td>
            </tr>
            <tr>
                
                
            </tr>
        </table>
    </div>
    <div id="div_BillList" style="width: 100%">
        <table id="tb_BillList"></table>
        <%--<input type="hidden" id="hid_BillListnext_row_key" value="0"/>--%>
        <input type="hidden" id="hid_BillListnextpage_flg" value="True"/>        
        <input type="hidden" id="hid_BillListPageSize" value="20" />
        <input type="hidden" id="hid_BillListPageNumber" value="1" />
    </div>
     <div id="div_BillDetailInfo" class="easyui-dialog" style="width: 100%">
         <table>
            <tr>
                <td style="width: 10%; min-width: 100px; text-align: right;font-weight:bold;background-color:#f3f3f3;border-color:#95b8e7"><span>账单ID:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_bill_id"></label>
                </td>
                <td style="width: 10%; min-width: 100px; text-align: right;font-weight:bold;background-color:#f3f3f3"><span>账单状态:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_bill_status"></label>
                </td>
            </tr>
            <tr>
                <td style="width: 10%; min-width: 100px; text-align: right;font-weight:bold;background-color:#f3f3f3"><span>账期开始时间:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_start_time"></label>
                </td>
                <td style="width: 10%; min-width: 100px; text-align: right;font-weight:bold;background-color:#f3f3f3"><span>账期结束时间:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_end_time"></label>
                </td>

            </tr>
            <tr>
                <td style="width: 10%; min-width: 100px; text-align: right;font-weight:bold;background-color:#f3f3f3"><span>账单日:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_bill_date"></label>
                </td>
                <td style="width: 10%; min-width: 100px; text-align: right;font-weight:bold;background-color:#f3f3f3"><span>还款日:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_repay_date"></label>
                </td>                
            </tr>
            <tr>
                <td style="width: 10%; min-width: 100px; text-align: right;font-weight:bold;background-color:#f3f3f3"><span>还款状态:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_repay_status"></label>
                </td>
                <td style="width: 10%; min-width: 100px; text-align: right;font-weight:bold;background-color:#f3f3f3"><span>账单总金额:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_total_amount"></label>
                </td>
               
            </tr>
            <tr>
                 <td style="width: 10%; min-width: 100px; text-align: right;font-weight:bold;background-color:#f3f3f3"><span>应还金额:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_balance"></label>
                </td>
                <td style="width: 10%; min-width: 100px; text-align: right;font-weight:bold;background-color:#f3f3f3"><span>利息:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_interest"></label>
                </td>
               
            </tr>
            <tr>
                 <td style="width: 10%; min-width: 100px; text-align: right;font-weight:bold;background-color:#f3f3f3"><span>逾期应还款:</span></td>
                <td style="width: 40%; min-width: 100px; text-align: left">
                    <label id="lab_overdue_balance"></label>
                </td>
                
            </tr>           
        </table>
        <table id="tb_BillDetailInfo"></table>
          <input type="hidden" id="hid_BillDetailInfonextpage_flg" value="True"/>        
        <input type="hidden" id="hid_BillDetailInfoPageSize" value="20" />
        <input type="hidden" id="hid_BillDetailInfoPageNumber" value="1" />
    </div>
</body>
</html>
