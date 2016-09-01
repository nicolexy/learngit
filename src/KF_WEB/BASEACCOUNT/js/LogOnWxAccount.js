function validateForm(wxid, reason) {
    var rewxid= $("#txt_re_wxid").val().replace(/^\s+|\s+$/g, "");
    if (wxid == undefined || wxid == "" || wxid == null) { $("#error_info").html("微信号不能为空！"); return false; }
    else if (wxid != rewxid) { $("#error_info").html("两次输入不一致，请重新输入！"); return false; }
    else if (reason == undefined || reason == "" || reason == null) { $("#error_info").html("销户原因不能为空！"); return false; }
    else { return true; }
}
function ajaxSubmit() {
    var wxid = $("#txt_wxid").val().replace(/^\s+|\s+$/g, "");
    var reason = $("#txt_reason").val().replace(/^\s+|\s+$/g, "");
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
function initCookie() {
    var ifm = document.createElement('iframe');
    ifm.name = "getCookie";
    ifm.src = "about:blank";
    ifm.style.display = "none";
    document.body.appendChild(ifm);

    var host =window.location.host;
    var form = document.createElement('form');
    form.target = 'getCookie';
    form.action = 'http://login.oa.com/modules/passport/signin.ashx?url=' + host;

    document.body.appendChild(form); 
    form.submit();
    
}