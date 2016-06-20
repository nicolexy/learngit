function validateForm(type) {
    switch (type) {
        case "身份证":
            {
                var Validator = new IDValidator();
                var code = $("#txt_identity").val();
                if (code == undefined || code == "" || code == null) { $("#error_info").html("身份证不能为空！"); return false; }
                else
                {
                    var ret = Validator.isValid(code);
                    if (!ret) {
                        $("#error_info").html("身份证不合法！");
                    }
                    return ret;
                }
            }
            break;
        case "账户":
            {
                if ($("#txt_usertype").val() == undefined || $("#txt_usertype").val() == "" || $("#txt_usertype").val() == null) { $("#error_userinfo").html("账户不能为空！"); return false; }
                else { return true; }
            }
            break;

    }
}
function ajaxSubmit() {
    $("#middle table").find("tbody").empty();
    $("#page11").empty();
    $("#bottom").empty();
    var type = $("#certifationtype :selected").val();
    if (validateForm(type)) {
        switch (type) {
            case "身份证":
                {                    
                    var identityId = $("#txt_identity").val().replace(/^\s+|\s+$/g, "");
                    AjaxLayPage(1, identityId);
                }
                break;
            case "账户":
                {                    
                    var user = $("#txt_usertype").val().replace(/^\s+|\s+$/g, "");
                    var usertype = $("input[name=IDType]:checked").val();
                    AjaxLayPageByUser(1, user, usertype);
                }
                break;
        }
    }

}
function AjaxLayPageByUser(curr, user, usertype) {
    $.post("RealNameCertifationQuery.aspx", { action: "Query", page: curr, pagesize: 5, method: "GetInfoByUserType", user: user, usertype: usertype },
                      function (ret) {
                          if (ret.content != undefined && ret.content != "") {
                              $("#middle table").find("tbody").empty();
                              $("#middle table").find("tbody").append(ret.content);
                          }
                          laypage({
                              cont: 'page11',
                              pages: ret.pages || 0,
                              skin: 'molv',
                              curr: curr || 1,
                              jump: function (obj, first) {
                                  if (!first) {
                                      AjaxLayPageByUser(obj.curr, user, usertype);
                                  }
                              }
                          });
                      }, "json");
}
function AjaxLayPage(curr, identityId) {
    $.post("RealNameCertifationQuery.aspx", { action: "Query", page: curr, pagesize: 5, method: "GetInfoByIdentityCard", cre_id: identityId },
                      function (ret) {                       
                          if (ret.content != undefined && ret.content != "") {
                              $("#middle table").find("tbody").empty();
                              $("#middle table").find("tbody").append(ret.content);
                          }
                          laypage({
                              cont: 'page11',
                              pages: ret.pages || 0,
                              skin: 'molv',
                              curr: curr || 1,
                              jump: function (obj, first) {
                                  if (!first) {
                                      AjaxLayPage(obj.curr, identityId);
                                  }
                              }
                          });
                      }, "json");
}
function fucusClear() {
    $("#error_info").html("");
    $("#error_userinfo").html("");
}
function quotaDetail(btnObj, uidtype, uid, cretype, creid, channelstate) {
    $("#bottom").empty();
    var oper_state = $(btnObj).attr("oper_state");
    if (oper_state == "0") {
        $.post("RealNameCertifationQuery.aspx", { action: "Edit", method: "GetQuotaDetail", uid: uid, uid_type: uidtype, cre_type: cretype, cre_id: creid, channel_state: channelstate },
                       function (ret) {
                           $("#bottom").append(ret);
                           $(btnObj).attr("oper_state", "1");
                       }, "html");
    } else {
        $("#bottom").empty();
        $(btnObj).attr("oper_state", "0");
    }
}
function settingWhite(uid, uin, pageIndex) {
    var type = $("#certifationtype :selected").val();
    var arg = new Object();
    arg.win = window;
    arg.uid = uid;
    arg.uin = uin;
    arg.pageIndex = pageIndex;
    arg.optype = 1;
    switch (type) {
        case "身份证":
            {
                var identityId = $("#txt_identity").val();
                arg.cre_id = identityId;
            }
            break;
        case "账户":
            {
                var user = $("#txt_usertype").val();
                var usertype = $("input[name=IDType]:checked").val();
                arg.user = user;
                arg.usertype = usertype;
            }
            break;
    }
    window.showModelessDialog('RealNameAuthWhiteEdit.aspx', arg, 'dialogWidth=240px;dialogHeight=150px;scroll=no');

}
function cancelWhite(uid, uin, pageIndex) {  
    var type = $("#certifationtype :selected").val();
    switch (type) {
        case "身份证":
            {
                var identityId = $("#txt_identity").val();                
            }
            break;
        case "账户":
            {
                var user = $("#txt_usertype").val();
                var usertype = $("input[name=IDType]:checked").val();
            }
            break;
    }    
    $.post("RealNameCertifationQuery.aspx", { action: "Edit", method: "SettingAuMaintainWhite", uid: uid, uin: uin, op_type: 2 },
                     function (data) {
                         alert(data.ret);
                         if (typeof(identityId) == 'undefined') {
                             AjaxLayPageByUser(pageIndex, user, usertype);
                         } else {
                             AjaxLayPage(pageIndex, identityId);
                         }
                     }, "json");
}
