$(function () {
    fucusClear();
});
function validateForm() {
    if ($("#txt_uin").val() == undefined || $("#txt_uin").val() == "" || $("#txt_uin").val() == null) { $("#tip_message").html("卖家账户不能为空！"); return false; }
    else if ($("#txt_transaction_id").val() == undefined || $("#txt_transaction_id").val() == "" || $("#txt_transaction_id").val() == null) { $("#tip_message").html("交易单号不能为空！"); return false; }
    else { return true;}
}
function ajaxSubmit() {
    var transaction_id = $("#txt_transaction_id").val().replace(/^\s+|\s+$/g, "");
    var uin = $("#txt_uin").val().replace(/^\s+|\s+$/g, "");
    if (validateForm()) {
        $.post("IceOutPPSecurityMoney.aspx", { action: "IceOut", uin: uin, transaction_id: transaction_id },
                    function (data) {
                        alert(data.ret);                     
                    }, "json");
    }
}
function fucusClear() {    
    $("#tip_message").html("");
}