function validateForm(wxid, reason) {
    var rewxid= $("#txt_re_wxid").val().replace(/^\s+|\s+$/g, "");
    if (wxid == undefined || wxid == "" || wxid == null) { $("#error_info").html("微信号不能为空！"); return false; }
    else if (wxid != rewxid) { $("#error_info").html("两次输入不一致，请重新输入！"); return false; }
    else if (reason == undefined || reason == "" || reason == null) { $("#error_info").html("理由不能为空！"); return false; }    
    else { return true; }
}
function ajaxSubmit() { 
    var wxid = $("#txt_wxid").val().replace(/^\s+|\s+$/g, "");
    var reason = $("#txt_reason").text().replace(/^\s+|\s+$/g, "");
    if (validateForm(wxid, reason)) {
        $.post("logOnWxAccount.aspx", { action: "CancelWx", wxid: wxid, reason: reason},
                function (data) {
                    alert(data.ret);
                }, "json");
    }
}
function fucusClear() {
    $("#error_info").html("");   
}