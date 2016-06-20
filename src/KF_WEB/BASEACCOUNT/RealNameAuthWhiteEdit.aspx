<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RealNameAuthWhiteEdit.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.RealNameAuthWhiteEdit" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>添加白名单有效期</title>
    <script type="text/javascript" src="../SCRIPTS/jquery-1.11.3.min.js"></script>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );

        BODY {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
        table {
            font-family: "Microsoft YaHei";
            font-size: 12px;
            background-color: #CCCCCC;
            width:100%;
            height:100%;
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
    <script type="text/javascript">      
        var arg = window.dialogArguments;
        var uid = arg.uid;
        var pageIndex = arg.pageIndex;
        var uin = arg.uin;
        var optype = arg.optype;
        function setting() {
            var days = $("#txt_valid_days").val();
            if (days == ""||days==undefined) {
                alert("白名单有效期不能为空！");
                return;
            }
            $.post("RealNameCertifationQuery.aspx", { action: "Edit", method: "SettingAuMaintainWhite", uid: uid, uin: uin, valid_days: days, op_type: optype },
                     function (data) {                         
                         alert(data.ret);
                         setTimeout(function () {
                             if (typeof (arg.cre_id) == 'undefined')
                             {                               
                                 arg.win.AjaxLayPageByUser(pageIndex, arg.user, arg.usertype);
                             }else{
                                 arg.win.AjaxLayPage(pageIndex, arg.cre_id);
                             }
                             window.close();
                         }, 1000);
                     }, "json");
        }
        function isNumberKey(evt)
        {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
            {
                return false;
            }
            return true;
        }  
    </script>
</head>
<body>
    <form id="form1" runat="server">
           <table cellspacing="1" cellpadding="0" align="center" bgcolor="#666666">
               <tr><td class="name">白名单有效期：</td><td class="value"><input type="text" id="txt_valid_days" onkeypress="return isNumberKey(this)"/></td></tr>
               <tr><td colspan="2" style="text-align:right"><input type="button" onclick="setting();" value="确 定"></td></tr>
            </table>
    </form>
</body>
</html>
