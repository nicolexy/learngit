$(function () {
    IsHaveRightForReviewCount();
    IsHaveRightForSeeDetail();
    $("#div_ReveiwIdCard").dialog("close");
    //时间:起
    $("#tbx_beginDate").datebox({
        value: getCurrentMonthFirst(),
        ShowSeconds: false
    });
    //时间:止
    $("#txt_EndDate").datebox({
        value: SetDate(0),
        ShowSeconds: false
    });

    LoadCommonCombobox("ddl_ReviewStatus", "id", "name", 150, 200,150, true, "全部", false, false, "IDCardManualReview_LoadReveiwStatus", 0);
    LoadCommonCombobox("ddl_ReviewResult", "id", "name", 150, 200,100, true, "全部", false, false, "IDCardManualReview_LoadReveiwResult", 0);
    $("#txt_ReviewCount").numberspinner({
        min: 1,
        max: 200,
        editable: true
    });
    $("#txt_ReviewCount").numberspinner("setValue", 200);  // 设置值

    $("#btn_ReceiveReview").click(function () {
        var uid = $("[id$='Label_uid']").text();// $("#Label_uid").val();
        var uin = $("#txt_uin").val();
        var reviewCount = $("#txt_ReviewCount").numberspinner("getValue");
        var beginDate = $("#tbx_beginDate").datebox("getValue");
        var endDate = $("#txt_EndDate").datebox("getValue");
        var datas = {
            uid: uid,
            uin: uin,
            reviewCount: reviewCount,
            beginDate: beginDate,
            endDate: endDate
        };

        $.ajax({
            type: "POST",
            async: false,  // 设置同步方式
            cache: false,
            dataType: "text",
            data: datas,
            url: "IDCardManualReview.aspx?getAction=ReceiveReview",
            success: function (returnData) {
                var dataObj = eval("(" + returnData + ")");
                $.each(dataObj, function (idx, item) {
                    var result = item.result;
                    var message = item.message;
                    if (message.length > 0) {                        
                        if (message == "NoRight") {
                            var loginPath = item.loginPath;
                            $.messager.confirm("操作提示", "页面超时,是否刷新？", function (data) {
                                if (data) {
                                    window.location.href = loginPath;
                                }
                            });
                        }
                        else {
                            //$.messager.alert('提示', message, 'Info');
                            $("#ddl_ReviewStatus").combobox("setValue", "2");
                            //$("#tb_IDCardManualReviewList").datagrid("load");
                            $("#btn_Search").click();
                        }
                    }
                });
                
               
            },
            error: function () {
                $.messager.alert("错误", "出错了!", "info", null);
            }
        });
    });

    //
    $("#btn_Search").click(function () {
        var uid = $("[id$='Label_uid']").text();//$("#Label_uid").text();
        var uin = $("#txt_uin").val();
        var isHaveRightForSeeDetail = $("#hid_IsHaveRightForSeeDetail").val();
        var reviewCount = $("#txt_ReviewCount").numberspinner("getValue");
        var beginDate = $("#tbx_beginDate").datebox("getValue");
        var endDate = $("#txt_EndDate").datebox("getValue");
        if (beginDate.length < 1 || beginDate == null) {
            $.messager.alert("提示", "请选择开始日期", "info", null);
            return;
        }
        else if (endDate.length < 1 || endDate == null) {
            $.messager.alert("提示", "请选择结束日期", "info", null);
            return;
        }
        if (beginDate > endDate) {
            $.messager.alert("提示", "开始日期不能大于结束日期", "info", null);
            return;
        }
        var datas = {
            beginDate: beginDate,
            endDate: endDate
        };
        $.ajax({
            type: "POST",
            url: "IDCardManualReview.aspx?getAction=CheckDate",
            data: datas,
            dataType: "text",
            success: function (data) {
                //if (data.length > 0) {
                //    $.messager.alert('提示', data, 'Info');
                //}

                var dataObj = eval("(" + data + ")");
                $.each(dataObj, function (idx, item) {
                    var result = item.result;
                    var message = item.message;
                    if ((result == "false" || result == "False") && message.length > 0) {
                        
                        if (message == "NoRight") {
                            var loginPath = item.loginPath;
                            $.messager.confirm("操作提示", "页面超时,是否刷新？", function (data) {
                                if (data) {
                                    window.location.href = loginPath;
                                }
                            });
                        }
                        else {
                            $.messager.alert('提示', message, 'Info');
                            return;
                        }
                        
                    }
                });
            }
        });
        var queryData = {
            uid: $("[id$='Label_uid']").text(),
            uin: $("#txt_uin").val(),
            reviewStatus: $("#ddl_ReviewStatus").combobox("getValue"),
            reviewResult: $("#ddl_ReviewResult").combobox("getValue"),
            beginDate: $("#tbx_beginDate").datebox("getValue"),
            endDate: $("#txt_EndDate").datebox("getValue")
        };
        var divWidth = $("#div_IDCardManualReviewList").width();
        var divHeight = $("#div_IDCardManualReviewList").height();

        $('#tb_IDCardManualReviewList').datagrid({
            width: divWidth * 99 / 100,
            height: $(document).height() * 73 / 100,
            toolbar: "toolbar",
            delay: 1000,
            mode: 'remote',
            idField: 'Fidentitycard',
            textField: 'Fserial_number',
            loadMsg: "数据加载中，请稍后...",
            pagination: true,
            pageSize: '30',
            pageList: [10, 20, 30, 50],
            sortName: 'Fcreate_time',
            sortOrder: ' desc ',
            showFooter: true,
            rownumbers: true,
            singleSelect: true,
            fitColumns: false,
            url: 'IDCardManualReview.aspx?getAction=LoadReview',
            queryParams: queryData,  //异步查询的参数

            columns: [[
                { field: 'Fid', hidden: true },
                { field: 'Fname', hidden: true },
                { field: 'Fidentitycard', hidden: true },
                { field: 'Fimage_path1', hidden: true },
                { field: 'Fimage_path2', hidden: true },
                { field: 'Fimage_file1', hidden: true },
                { field: 'Fimage_file2', hidden: true },
                 //{ title: '', field: 'productpkid', width: $(this).width(), checkbox: true },
                    { field: 'Fserial_number', title: '流水号', halign: 'center', align: 'left', width: divWidth * 12 / 100 },
                    { field: 'Fspid', title: '商户号', halign: 'center', align: 'left', width: divWidth * 7.5 / 100 },
                    { field: 'Fcreate_time', title: '申请时间', halign: 'center', align: 'left', width: divWidth * 10 / 100, sortable: true },
                     { field: 'Fuin', title: '用户帐号', halign: 'center', align: 'left', width: divWidth * 17 / 100 },
                     { field: 'Fmodify_time', title: '审核时间', halign: 'center', align: 'left', width: divWidth * 10 / 100, sortable: true },
                     {
                         field: 'Fstate', title: '审核状态', halign: 'center', align: 'left', width: divWidth * 10 / 100, sortable: true,
                         formatter: function (value, row, index) {
                             var span = "<span></span>";
                             if (value == "1") {
                                 span = "<span>未领单</span>";
                             }
                             else if (value == "2") {
                                 span = "<span>已领单</span>";
                             }
                             else if (value == "3") {
                                 span = "<span style='color:red'>推送到实名系统失败</span>";
                             }
                             else if (value == "4") {
                                 span = "<span style='color:green'>推送成功</span>";
                             }
                             return span;
                         }
                     },
                     {
                         field: 'Fresult', title: '审核结果', halign: 'center', align: 'left', width: divWidth * 5 / 100, sortable: true,
                         formatter: function (value, row, index) {
                             var span = "<span></span>";
                             if (value == "0") {
                                 span = "<span>未处理</span>";
                             }
                             else if (value == "1") {
                                 span = "<span style='color:green'>通过</span>";
                             }
                             else if (value == "2") {
                                 span = "<span style='color:red'>驳回</span>";
                             }
                             return span;
                         }
                     },
                     { field: 'Foperator', title: '处理人', halign: 'center', align: 'left', width: divWidth * 7 / 100, sortable: true },
                     {
                         field: 'Fmemo', title: '审核信息', halign: 'center', align: 'left', width: divWidth * 13 / 100,
                         formatter: function (value, row, index) {
                             var Fmemo = "";
                             if (value == "1") {
                                 Fmemo = "未显示图片";
                             }
                             else if (value == "2") {
                                 Fmemo = "上传非身份证照片";
                             }
                             else if (value == "3") {
                                 Fmemo = "身份证不清晰不完整";
                             }
                             else if (value == "4") {
                                 Fmemo = "身份证证件号不一致";
                             }
                             else if (value == "5") {
                                 Fmemo = "其他原因";
                             }
                             else if (value == "6") {
                                 Fmemo = "身份证姓名和提供姓名不符";
                             }
                             else if (value == "7") {
                                 Fmemo = "身份证签发机关和地址不一致";
                             }
                             else if (value == "8") {
                                 Fmemo = "两张均为正面或者反面";
                             }
                             else if (value == "9") {
                                 Fmemo = "身份证证件虚假";
                             }
                             else if (value == "10") {
                                 Fmemo = "身份证已超过有效期";
                             }
                             else if (value == "11") {
                                 Fmemo = "身份证照片非原件";
                             }
                             var span = "<span>" + Fmemo + "</span>";
                             return span;
                         }
                     },
                      {
                          field: '详细内容', title: '详细内容', halign: 'center', align: 'center', width: divWidth * 5 / 100,
                          formatter: function (value, row, index) {
                              var span = "";
                              //当前登录人员是该审核数据的领单人员或者当前登录人员有权限查看详细则可以点击详细内容进入详细查看
                              if (row.Foperator == uid || isHaveRightForSeeDetail == "True" || isHaveRightForSeeDetail == "true") {
                                  span = "<span><a href='#'  onclick='ReviewIdCard(" + index + ")'>详细内容</a></span>";
                              }
                              else {//if (row.Foperator == "" || row.Foperator.length < 1 || row.Foperator == "undefined")                                  
                                  span = "<span></span>";
                              }
                              return span;
                          }
                      }

            ]], onLoadSuccess: function () {
            }
        });
    });

    $("#a_Yes").click(function () {
        var postDatas = {
            Fuin: $("#lab_Fuin").text(),
            Fmemo: $("#txt_Fmemo").combobox("getValue"),
            reviewResult: 1,//通过
            TableName: $("#hid_TableName").val(),
            Fid: $("#hid_Fid").val(),
            Fserial_number: $("#hid_Fserial_number").val(),
        };

        $.ajax({
            type: "POST",
            url: "IDCardManualReview.aspx?getAction=SaveReview",
            data: postDatas,
            dataType: "text",
            success: function (data) {
                var dataObj = eval("(" + data + ")");
                $.each(dataObj, function (idx, item) {
                    var result = item.result;
                    var message = item.message;
     
                    if (result == "true" || result == "True") {
                        $("#div_ReveiwIdCard").dialog("close");
                        $("#tb_IDCardManualReviewList").datagrid("load");
                    }
                    else {
                        if (message.length > 0) {                          
                            
                            if (message == "NoRight") {
                                var loginPath = item.loginPath;
                                $.messager.confirm("操作提示", "页面超时,是否刷新？", function (data) {
                                    if (data) {
                                        window.location.href = loginPath;
                                    }
                                });
                            }
                            else {
                                $.messager.alert('提示', message, 'Info');
                            }
                        }
                    }
                });
            }
        });
    });

    $("#a_No").click(function () {
        var selectedFmemo = $("#txt_Fmemo").combobox("getValue");
        if (selectedFmemo == 0) {
            $.messager.alert("提示", "请选择失败原因", "info", null);
            return;
        }
        var postDatas = {
            Fuin: $("#lab_Fuin").text(),
            Fmemo: $("#txt_Fmemo").combobox("getValue"),
            reviewResult: 2,//驳回
            TableName: $("#hid_TableName").val(),
            Fid: $("#hid_Fid").val(),
            Fserial_number: $("#hid_Fserial_number").val(),
        };
        $.ajax({
            type: "POST",
            url: "IDCardManualReview.aspx?getAction=SaveReview",
            data: postDatas,
            dataType: "text",
            success: function (data) {
                var dataObj = eval("(" + data + ")");
                $.each(dataObj, function (idx, item) {
                    var result = item.result;
                    var message = item.message;

                    if (result == "true" || result == "True") {
                        $("#div_ReveiwIdCard").dialog("close");
                        $("#tb_IDCardManualReviewList").datagrid("load");
                    }
                    else {
                        if (message.length > 0) {

                            if (message == "NoRight") {
                                var loginPath = item.loginPath;
                                $.messager.confirm("操作提示", "页面超时,是否刷新？", function (data) {
                                    if (data) {
                                        window.location.href = loginPath;
                                    }
                                });
                            }
                            else {
                                $.messager.alert('提示', message, 'Info');
                            }
                        }
                    }
                });
            }
        });
    });

    $("#a_ReSend").click(function () {
        var postDatas = {
            Fuin: $("#lab_Fuin").text(),
            reviewResult: 3,//重新推送
            TableName: $("#hid_TableName").val(),
            Fid: $("#hid_Fid").val(),
            Fserial_number: $("#hid_Fserial_number").val(),
        };
        $.ajax({
            type: "POST",
            url: "IDCardManualReview.aspx?getAction=ReSend",
            data: postDatas,
            dataType: "text",
            success: function (data) {
                var dataObj = eval("(" + data + ")");
                $.each(dataObj, function (idx, item) {
                    var result = item.result;
                    var message = item.message;
                    
                    if (result == "true" || result == "True") {
                        $("#div_ReveiwIdCard").dialog("close");
                        $("#tb_IDCardManualReviewList").datagrid("load");
                    }
                    else {
                        if (message.length > 0) {
                            if (message == "NoRight") {
                                var loginPath = item.loginPath;
                                $.messager.confirm("操作提示", "页面超时,是否刷新？", function (data) {
                                    if (data) {
                                        window.location.href = loginPath;
                                    }
                                });
                            }
                            else {
                                $.messager.alert('提示', message, 'Info');
                            }
                        }                       
                    }
                });
            }
        });
    });

    $("#a_DownloadReviewData").click(function () {
        var uid = $("[id$='Label_uid']").text();//$("#Label_uid").text();
        var uin = $("#txt_uin").val();
        var reviewCount = $("#txt_ReviewCount").numberspinner("getValue");
        var beginDate = $("#tbx_beginDate").datebox("getValue");
        var endDate = $("#txt_EndDate").datebox("getValue");
        if (beginDate.length < 1 || beginDate == null) {
            $.messager.alert("提示", "请选择开始日期", "info", null);
            return;
        }
        else if (endDate.length < 1 || endDate == null) {
            $.messager.alert("提示", "请选择结束日期", "info", null);
            return;
        }
        if (beginDate > endDate) {
            $.messager.alert("提示", "开始日期不能大于结束日期", "info", null);
            return;
        }

        var url = "/Ajax/CommonAjax/NPOIExportToExcels.ashx?getAction=ExportIDCardManualReviewDataToExcel&uid=" + uid + "&uin=" + uin + "&reviewCount=" + reviewCount + "&reviewStatus=" + $("#ddl_ReviewStatus").combobox("getValue") + "&reviewResult=" + $("#ddl_ReviewResult").combobox("getValue") + "&beginDate=" + beginDate + "&endDate=" + endDate + ""
        $("#Form1").attr("action", url);//设置表单提交的对象
        $("#Form1").submit();//提交表单

        //var datas = {
        //    uid: uid,
        //    uin: uin,
        //    reviewCount: reviewCount,
        //    reviewStatus: $("#ddl_ReviewStatus").combobox("getValue"),
        //    reviewResult: $("#ddl_ReviewResult").combobox("getValue"),
        //    beginDate: beginDate,
        //    endDate: endDate
        //};
        //$.ajax({
        //    type: "POST",
        //    url: "/Ajax/CommonAjax/NPOIExportToExcels.ashx?getAction=ExportIDCardManualReviewDataToExcel",
        //    data: datas,
        //    dataType: "text",
        //    success: function (data) {
        //        var dataObj = eval("(" + data + ")");
        //        $.each(dataObj, function (idx, item) {
        //            var result = item.result;
        //            var message = item.message;
        //            if ((result == "false" || result == "False") && message.length > 0) {
        //                $.messager.alert('提示', message, 'Info');
        //                return;
        //            }
        //        });
        //    }
        //});
    })

})

function IsHaveRightForReviewCount() {
    $.ajax({
        type: "POST",
        url: "IDCardManualReview.aspx?getAction=IsHaveRightForReviewCount",
        //data: postDatas,
        dataType: "text",
        success: function (data) {
            var dataObj = eval("(" + data + ")");
            $.each(dataObj, function (idx, item) {
                var result = item.result;
                var message = item.message;
                if (result == "true" || result == "True") {
                    //$("#btn_ReceiveReview").show();
                    $("#td_ReviewCountName").show();
                    $("#td_ReviewCount").show();
                }
                else {
                    //$("#btn_ReceiveReview").hide();
                    $("#td_ReviewCountName").hide();
                    $("#td_ReviewCount").hide();
                    if (message == "NoRight") {
                        var loginPath = item.loginPath;
                        window.location.href = loginPath;
                    }

                }
            });
        }
    });
}


function IsHaveRightForSeeDetail() {
    $.ajax({
        type: "POST",
        url: "IDCardManualReview.aspx?getAction=IsHaveRightForSeeDetail",
        //data: postDatas,
        dataType: "text",
        success: function (data) {
            var dataObj = eval("(" + data + ")");
            $.each(dataObj, function (idx, item) {
                var result = item.result;
                var message = item.message;
                $("#hid_IsHaveRightForSeeDetail").val(result);

                if (result == "false" || result == "False") {
                    if (message == "NoRight") {
                        var loginPath = item.loginPath;
                        window.location.href = loginPath;
                    }
                }
            });
        }
    });
}

function ReviewIdCard(index) {
    var rows = $("#tb_IDCardManualReviewList").datagrid('getData').rows;
    var Fid = rows[index]["Fid"];
    var Fserial_number = rows[index]["Fserial_number"];
    var Fspid = rows[index]["Fspid"];
    var Fuin = rows[index]["Fuin"];
    var Fname = rows[index]["Fname"];
    var Fidentitycard = rows[index]["Fidentitycard"];
    var Fimage_path1 = rows[index]["Fimage_path1"];
    var Fimage_path2 = rows[index]["Fimage_path2"];
    var Fimage_file1 = rows[index]["Fimage_file1"];
    var Fimage_file2 = rows[index]["Fimage_file2"];
    var Fstate = rows[index]["Fstate"];
    var Fresult = rows[index]["Fresult"];
    var TableName = rows[index]["TableName"];
    var Fmemo = rows[index]["Fmemo"];



    var tableWidth = $(document).width() * 100 / 100;
    var divHeight = $(document).height() * 100 / 100;
    $("#div_ReveiwIdCard").dialog({
        title: '【身份证影印件审核】,流水号：' + Fserial_number,
        width: tableWidth * 90 / 100,
        height: divHeight * 95 / 100,
        //resizable: true,
        maximizable: false,
        //minimizable:true,
        closed: false,
        cache: false,
        modal: true,
        //toolbar: '#tb_HaiKangPriceProtectionReport_toolbar',
        onBeforeOpen: function () {
            $("#lab_Fname").text("");
            $("#lab_Fidentitycard").text("");
        },
        onOpen: function () {
            //解密姓名
            //var deFname = "";
            var postFname = {
                DecryptorStr: Fname
            };
            $.ajax({
                type: "POST",
                url: "IDCardManualReview.aspx?getAction=Decryptor",
                data: postFname,
                dataType: "text",
                success: function (retrunData) {
                    var dataObj = eval("(" + retrunData + ")");
                    $.each(dataObj, function (idx, item) {
                        var result = item.result;
                        var message = item.message;
                        if (result == "true" || result == "True") {
                            $("#lab_Fname").text(message);
                        }
                        else {
                            $("#lab_Fname").text("");
                            if (message == "NoRight") {
                                var loginPath = item.loginPath;
                                $.messager.confirm("操作提示", "页面超时,是否刷新？", function (data) {
                                    if (data) {
                                       window.location.href = loginPath;
                                    }
                                });
                            }
                        }
                    });
                }
            });

            //解密身份证
            //var deFidentitycard = "";
            var postFidentitycard = {
                DecryptorStr: Fidentitycard
            };
            $.ajax({
                type: "POST",
                url: "IDCardManualReview.aspx?getAction=Decryptor",
                data: postFidentitycard,
                dataType: "text",
                success: function (retrunData) {
                    var dataObj = eval("(" + retrunData + ")");
                    $.each(dataObj, function (idx, item) {
                        var result = item.result;
                        var message = item.message;
                        if (result == "true" || result == "True") {
                            $("#lab_Fidentitycard").text(message);
                        }
                        else {
                            $("#lab_Fidentitycard").text("");
                            if (message == "NoRight") {
                                var loginPath = item.loginPath;
                                $.messager.confirm("操作提示", "页面超时,是否刷新？", function (data) {
                                    if (data) {
                                        window.location.href = loginPath;
                                    }
                                });
                            }
                        }
                    });
                }
            });
            $("#lab_Fuin").text(Fuin);
            //$("#lab_Fname").text(deFname);
            //$("#lab_Fidentitycard").text(deFidentitycard);            
            $("#hid_TableName").val(TableName);
            $("#hid_Fid").val(Fid);
            $("#hid_Fserial_number").val(Fserial_number);
            LoadCommonCombobox("txt_Fmemo", "id", "name", 200, 250,300, true, "请选择", false, false, "IDCardManualReview_LoadFmemo", Fmemo);
            var aaa = $("#hid_IdCaredServerPath").val() + "?" + Fimage_path1;
            var bbb = $("#hid_IdCaredServerPath").val() + "?" + Fimage_path2;
            $("#ima_IDCardZ").attr("src", aaa);
            $("#ima_IDCardF").attr("src", bbb);

            $("#ima_IDCardZ,#ima_IDCardF").bind("contextmenu", function () {
                return false;
            });
            $("#div_ima_IDCardZ").empty();
            var index = 0;
            for (var i = 20; i < $("#div_ima_IDCardZ").width() ; i += 200) {
                index = index + 50;
                for (var j = 20; j < $("#div_ima_IDCardZ").height() ; j += 300) {
                    var span = "<div style='color:rgba(255, 255, 255, 0.3) ;width:50px;margin-top:" + index + "px;margin-left:" + (index + 10) + "px;transform:rotate(-45deg)'><span>" + $("[id$='Label_uid']").text() + "</span></div>";
                    $("#div_ima_IDCardZ").append(span);
                }
            }

            $("#div_ima_IDCardF").empty();
            var index2 = 0;
            for (var i = 20; i < $("#div_ima_IDCardF").width() ; i += 200) {
                index2 = index2 + 50;
                for (var j = 20; j < $("#div_ima_IDCardF").height() ; j += 300) {
                    var span = "<div style='color:rgba(255, 255, 255, 0.3) ;width:50px;margin-top:" + index2 + "px;margin-left:" + (index2 + 10) + "px;transform:rotate(-45deg)'><span>" + $("[id$='Label_uid']").text() + "</span></div>";
                    $("#div_ima_IDCardF").append(span);
                }
            }
            //$("#div_ima_IDCardZ").text($("[id$='Label_uid']").text());
            //$("#div_ima_IDCardF").text($("[id$='Label_uid']").text());
            //$("#ima_IDCardZ").watermark({
            //    text: 'AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA',
            //    textWidth: 3000,
            //    gravity: 'w',
            //    opacity: 1,
            //    margin: 3
            //});

            if (Fstate <= 1) {
                //未领单，不能做任何操作
                $("#a_Yes").hide();
                $("#a_No").hide();
                $("#a_ReSend").hide();
            }
            else if (Fstate == 2) {//已领单
                if (Fresult == 1 || Fresult == 2) {
                    $("#a_Yes").hide();
                    $("#a_No").hide();
                    $("#a_ReSend").hide();
                }
                else if (Fresult == 0) {
                    $("#a_Yes").show();
                    $("#a_No").show();
                    $("#a_ReSend").hide();
                }
            }
            else if (Fstate == 3) {//推送到实名系统失败
                $("#a_Yes").hide();
                $("#a_No").hide();
                $("#a_ReSend").show();
            }
            else if (Fstate == 4) {//推送成功
                $("#a_Yes").hide();
                $("#a_No").hide();
                $("#a_ReSend").hide();
            }
        }
    });
    $("#div_ReveiwIdCard").window("center");

}
