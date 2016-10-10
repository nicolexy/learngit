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
            url: "/CreditPay/AccountSearch.aspx?getAction=SearchAccountInfo&Rand=" + Math.random() + "",
            data: datas,
            dataType: "text",
            async: false,
            success: function (data) {
                var dataObj = eval("(" + data + ")");
                $.each(dataObj, function (idx, item) {
                    var result = item.result;
                    if (result != "undefined" && result.length > 0) {
                        if (result == "false" || result == "False") {
                            var message = item.message;
                            if (message != "undefined" && message.length > 0) {
                                if (message == "NoRight") {                                    
                                    $.messager.confirm("操作提示", "页面超时,是否刷新？", function (data) {
                                        if (data) {
                                            var loginPath = item.loginPath;
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
                        }
                    }
                    else
                    {
                        var acct_no = item.acct_no;//渠道账号
                        var acct_type = item.acct_type;//渠道账号类型0：qq 1：微信
                        var name = item.name;//姓名，明文
                        var mobile = item.mobile;
                        var id_card_type = item.id_card_type;
                        var id_card_no = item.id_card_no;
                        var cur_type = item.cur_type;
                        var credit_line = item.credit_line;
                        var credit = item.credit;
                        var credit_used = item.credit_used;
                        var credit_freeze = item.credit_freeze;
                        var status = item.status;
                        var cft_status = item.cft_status;
                        var bill_date = item.bill_date;
                        var repay_date = item.repay_date;
                        var is_overdue = item.is_overdue;
                        var overdue_days = item.overdue_days;
                        var overdue_balance = item.overdue_balance;                        
                    }                   
                });
            }
        });
    })
});