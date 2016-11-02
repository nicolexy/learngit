$(function () {
    $("#btn_Search").click(function () {
        var account = $("#txt_Account").val();
        var accountType = $("input[name=AccountType]:checked").val();
        var datas = {
            accountNo: account,
            accountType: accountType
        };
        $.ajax({
            type: "POST",
            url: "/CreditPay/AccountSearch.aspx?getAction=LoadAccountInfo&Rand=" + Math.random() + "",
            data: datas,
            dataType: "text",
            async: false,
            success: function (returnData) {
                var dataObj = eval("(" + returnData + ")");
                var result = dataObj.result;
                if (result != "undefined" && result.length > 0) {
                    if (result == "false" || result == "False") {
                        var message = dataObj.message;
                        
                        if (message!=null&&message != "undefined" && message.length > 0) {
                            if (message == "NoRight") {
                                $.messager.confirm("操作提示", "页面超时,是否刷新？", function (checkResult) {
                                    if (checkResult) {
                                        var loginPath = dataObj.loginPath;
                                        window.location.href = (loginPath == null || loginPath == "" || loginPath == "undefined" ? "../login.aspx?wh=1" : loginPath);
                                        return;
                                    }
                                });
                            }
                            else {
                                $.messager.alert('提示', message, 'Info');
                                return;
                            }
                        }

                        var res_info = dataObj.res_info;
                        if (res_info != null && res_info.length > 0)
                        {
                            $.messager.alert('提示', res_info, 'Info');
                            return;
                        }
                    }
                    else {

                        var acct_no = dataObj.acct_no;//渠道账号                           
                        var acct_type = dataObj.acct_type;//渠道账号类型0：qq 1：微信
                        var name = dataObj.name;//姓名，明文
                        var mobile = dataObj.mobile;
                        var id_card_type = dataObj.id_card_type;
                        var id_card_no = dataObj.id_card_no;
                        var cur_type = dataObj.cur_type;
                        var credit_line = dataObj.credit_line;
                        var credit = dataObj.credit;
                        var credit_used = dataObj.credit_used;
                        var credit_freeze = dataObj.credit_freeze;
                        var status = dataObj.status;
                        var cft_status = dataObj.cft_status;
                        var bill_date = dataObj.bill_date;
                        var repay_date = dataObj.repay_date;
                        var is_overdue = dataObj.is_overdue;
                        var overdue_days = dataObj.overdue_days;
                        var overdue_balance = dataObj.overdue_balance;

                        $("#lab_Acct_no").text(acct_no);
                        $("#lab_Acct_Type").text(acct_type);
                        $("#lab_Name").text(name);
                        $("#lab_Mobile").text(mobile);
                        $("#lab_id_card_no").text(id_card_no);
                        $("#lab_cur_type").text(cur_type);
                        $("#lab_credit_line").text(credit_line);
                        $("#lab_credit").text(credit);
                        $("#lab_credit_frezz").text(credit_freeze);
                        $("#lab_credit_used").text(credit_used);
                        $("#lab_cft_staus").text(cft_status);
                        $("#lab_bill_date").text(repay_date);
                        $("#lab_repay_date").text(repay_date);
                        $("#lab_IsOverDue").text(is_overdue);
                        $("#lab_OverdueDays").text(overdue_days);
                        $("#lab_OverdueBalance").text(overdue_balance);
                        $("#lab_staus").text(status);
                    }
                }
            }
        });
    })
});