$(function () {     
    //审核开始日期
    $("#txt_ModifyBeginDate").datebox({
        value: getCurrentMonthFirst(),
        ShowSeconds: false
    });
    //审核结束日期
    $("#txt_ModifyEndDate").datebox({
        value: SetDate(0),
        ShowSeconds: false
    });
    $("#txt_Foperator").val("");
    $("#div_HZReport").hide();
    $("#div_PersonalReviewReport").hide();
    $("#div_FailReasonReport").hide();
    //
    $("#btn_LoadHZReport").click(function () {
        $("#div_HZReport").show();
        $("#div_PersonalReviewReport").hide();
        $("#div_FailReasonReport").hide();
        var uid = $("[id$='Label_uid']").text();//$("#Label_uid").text();
        var uin = $("#txt_uin").val();
   
        var modifyBeginDate = $("#txt_ModifyBeginDate").datebox("getValue");
        var modifyEndDate = $("#txt_ModifyEndDate").datebox("getValue");

        if (modifyBeginDate.length < 1 || modifyBeginDate == null) {
            $.messager.alert("提示", "请选择开始日期", "info", null);
            return;
        }
        else if (modifyEndDate.length < 1 || modifyEndDate == null) {
            $.messager.alert("提示", "请选择结束日期", "info", null);
            return;
        }
        if (modifyBeginDate > modifyEndDate) {
            $.messager.alert("提示", "开始日期不能大于结束日期", "info", null);
            return;
        }


        var datas = {
            beginDate: modifyBeginDate,
            endDate: modifyEndDate
        };
        $.ajax({
            type: "POST",
            url: "/BaseAccount/IDCardManualReviewReport.aspx?getAction=CheckDate&Rand=" + Math.random() + "",
            data: datas,
            dataType: "text",
            async: false,
            success: function (data) {
                var dataObj = eval("(" + data + ")");
                $.each(dataObj, function (idx, item) {
                    var result = item.result;
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
                });
            }
        });

        
        var queryData = {
            uid: $("[id$='Label_uid']").text(),
            uin: $("#txt_uin").val(),                        
            modifyBeginDate: $("#txt_ModifyBeginDate").datebox("getValue"),
            modifyEndDate: $("#txt_ModifyEndDate").datebox("getValue"),
            foperator: $("#txt_Foperator").val()           
        };
        var divWidth = $("#div_Report").width();
        var divHeight = $("#div_Report").height();
        //$("#tb_Report").datagrid("loadData", { total: 0, rows: [] });
        $('#tb_HZReport').datagrid({
            title: "个人汇总报表",
            width: divWidth * 99 / 100,
            height: $(document).height() * 92 / 100,
            toolbar: "toolbar",
            delay: 1000,
            mode: 'remote',
            idField: '审核时间',
            textField: '审核时间',
            loadMsg: "数据加载中，请稍后...",
            pagination: true,
            pageSize: '31',
            pageList: [10, 20, 31, 50],
            sortName: '审核时间',
            sortOrder: ' desc ',
            showFooter: true,
            rownumbers: true,
            singleSelect: true,
            fitColumns: true,
            url: "/BaseAccount/IDCardManualReviewReport.aspx?getAction=LoadHZReport&Rand=" + Math.random() + "",
            queryParams: queryData,  //异步查询的参数

            columns: [[
                    { field: 'Date', title: '审核时间', halign: 'center', align: 'left', width: $(this).width() },
                    { field: 'ZongGongDanLiang', title: '总工单量', halign: 'center', align: 'right', width: $(this).width() },
                    { field: 'DaiShenHeZongLiang', title: '待审核总量', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'JinDanLiang', title: '进单量', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'YiChuLiLiang', title: '已处理量', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'ShenHeTongGuoLiang', title: '审核通过量', halign: 'center', align: 'right', width: $(this).width() },
                     {
                         field: 'ShenHeTongGuoLv', title: '审核通过率', halign: 'center', align: 'right', width: $(this).width(),
                         formatter: function (value, row, index) {                             
                             if (value != null && value != "" && value != "undefined") {
                                 return value.toString() + "%";
                             }
                             else  {
                                 return "-";
                             }
                         }
                     },
                     { field: 'ShenHeJuJueLiang', title: '审核拒绝量', halign: 'center', align: 'right', width: $(this).width() },
                     {
                         field: 'ShenHeJuJueLv', title: '审核拒绝率', halign: 'center', align: 'right', width: $(this).width(),
                         formatter: function (value, row, index) {
                             if (value != null && value != "" && value != "undefined") {
                                 return value.toString() + "%";
                             }
                             else {
                                 return "-";
                             }
                         }
                     }
            ]], onLoadSuccess: function (data) {

            },
            onLoadError: function () {

            }
        });
    });

    
    $("#btn_LoadPersonalReviewReport").click(function () {
        $("#div_HZReport").hide();
        $("#div_PersonalReviewReport").show();
        $("#div_FailReasonReport").hide();
        var uid = $("[id$='Label_uid']").text();//$("#Label_uid").text();
        var uin = $("#txt_uin").val();

        var modifyBeginDate = $("#txt_ModifyBeginDate").datebox("getValue");
        var modifyEndDate = $("#txt_ModifyEndDate").datebox("getValue");

        if (modifyBeginDate.length < 1 || modifyBeginDate == null) {
            $.messager.alert("提示", "请选择开始日期", "info", null);
            return;
        }
        else if (modifyEndDate.length < 1 || modifyEndDate == null) {
            $.messager.alert("提示", "请选择结束日期", "info", null);
            return;
        }
        if (modifyBeginDate > modifyEndDate) {
            $.messager.alert("提示", "开始日期不能大于结束日期", "info", null);
            return;
        }


        var datas = {
            beginDate: modifyBeginDate,
            endDate: modifyEndDate
        };
        $.ajax({
            type: "POST",
            url: "/BaseAccount/IDCardManualReviewReport.aspx?getAction=CheckDate&Rand=" + Math.random() + "",
            data: datas,
            dataType: "text",
            async: false,
            success: function (data) {
                var dataObj = eval("(" + data + ")");
                $.each(dataObj, function (idx, item) {
                    var result = item.result;
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
                });
            }
        });


        var queryData = {
            uid: $("[id$='Label_uid']").text(),
            uin: $("#txt_uin").val(),
            modifyBeginDate: $("#txt_ModifyBeginDate").datebox("getValue"),
            modifyEndDate: $("#txt_ModifyEndDate").datebox("getValue"),
            foperator: $("#txt_Foperator").val()
        };
        var divWidth = $("#div_Report").width();
        var divHeight = $("#div_Report").height();
        //$("#tb_Report").datagrid("loadData", { total: 0, rows: [] });
        $('#td_PersonalReviewReport').datagrid({
            title: "个人审核情况报表",
            width: divWidth * 99 / 100,
            height: $(document).height() * 92 / 100,
            toolbar: "toolbar",
            delay: 1000,
            mode: 'remote',
            idField: '审核时间',
            textField: '审核时间',
            loadMsg: "数据加载中，请稍后...",
            pagination: true,
            pageSize: '30',
            pageList: [10, 20, 30, 50],
            sortName: 'Fcreate_time',
            sortOrder: ' desc ',
            showFooter: true,
            rownumbers: true,
            singleSelect: true,
            fitColumns: true,
            url: "/BaseAccount/IDCardManualReviewReport.aspx?getAction=LoadPersonalReviewReport&Rand=" + Math.random() + "",
            queryParams: queryData,  //异步查询的参数

            columns: [[
                    { field: 'Foperator', title: '处理人(按首字母排序)', halign: 'center', align: 'left', width: $(this).width() },
                    { field: 'Fmodify_time', title: '审核时间', halign: 'center', align: 'left', width: $(this).width() },
                    //{ field: '未处理', title: '未处理', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'Agree', title: '通过', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'Refuse', title: '拒绝', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'CurrentDayFirstFmodify_time', title: '当天第一单处理时间', halign: 'center', align: 'left', width: $(this).width() },
                     { field: 'CurrentDayLastFmodify_time', title: '当天最后一单处理时间', halign: 'center', align: 'left', width: $(this).width() },
                     { field: 'Total', title: '汇总', halign: 'center', align: 'right', width: $(this).width() }
            ]], onLoadSuccess: function (data) {

            },
            onLoadError: function () {

            }
        });
    });
    
    $("#btn_LoadFailReasonReport").click(function () {
        $("#div_HZReport").hide();
        $("#div_PersonalReviewReport").hide();
        $("#div_FailReasonReport").show();
        var uid = $("[id$='Label_uid']").text();//$("#Label_uid").text();
        var uin = $("#txt_uin").val();

        var modifyBeginDate = $("#txt_ModifyBeginDate").datebox("getValue");
        var modifyEndDate = $("#txt_ModifyEndDate").datebox("getValue");

        if (modifyBeginDate.length < 1 || modifyBeginDate == null) {
            $.messager.alert("提示", "请选择开始日期", "info", null);
            return;
        }
        else if (modifyEndDate.length < 1 || modifyEndDate == null) {
            $.messager.alert("提示", "请选择结束日期", "info", null);
            return;
        }
        if (modifyBeginDate > modifyEndDate) {
            $.messager.alert("提示", "开始日期不能大于结束日期", "info", null);
            return;
        }


        var datas = {
            beginDate: modifyBeginDate,
            endDate: modifyEndDate
        };
        $.ajax({
            type: "POST",
            url: "/BaseAccount/IDCardManualReviewReport.aspx?getAction=CheckDate&Rand=" + Math.random() + "",
            data: datas,
            dataType: "text",
            async: false,
            success: function (data) {
                var dataObj = eval("(" + data + ")");
                $.each(dataObj, function (idx, item) {
                    var result = item.result;
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
                });
            }
        });


        var queryData = {
            uid: $("[id$='Label_uid']").text(),
            uin: $("#txt_uin").val(),
            modifyBeginDate: $("#txt_ModifyBeginDate").datebox("getValue"),
            modifyEndDate: $("#txt_ModifyEndDate").datebox("getValue"),
            foperator: $("#txt_Foperator").val()
        };
        var divWidth = $("#div_Report").width();
        var divHeight = $("#div_Report").height();
        //$("#tb_Report").datagrid("loadData", { total: 0, rows: [] });
        $('#td_FailReasonReport').datagrid({
            title: "失败原因报表",
            width: divWidth * 99 / 100,
            height: $(document).height() * 92 / 100,
            toolbar: "toolbar",
            delay: 1000,
            mode: 'remote',
            idField: '审核时间',
            textField: '审核时间',
            loadMsg: "数据加载中，请稍后...",
            pagination: true,
            pageSize: '31',
            pageList: [10, 20, 31, 50],
            sortName: 'Fcreate_time',
            sortOrder: ' desc ',
            showFooter: true,
            rownumbers: true,
            singleSelect: true,
            fitColumns: true,
            url: "/BaseAccount/IDCardManualReviewReport.aspx?getAction=LoadFailReasonReport&Rand=" + Math.random() + "",
            queryParams: queryData,  //异步查询的参数

            columns: [[
                    { field: 'Fmodify_time', title: '审核时间', halign: 'center', align: 'left', width: $(this).width() },
                    { field: 'Fmemo8', title: '两张均为正面或负面', halign: 'center', align: 'right', width: $(this).width() },
                    { field: 'Fmemo3', title: '身份证不清晰不完整', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'Fmemo6', title: '身份证姓名和提供姓名不符', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'Fmemo4', title: '身份证证件号不一致', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'Fmemo7', title: '身份证签发机关和地址不一致', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'Fmemo10', title: '身份证已超过有效期', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'Fmemo11', title: '身份证照片非原件', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'Fmemo9', title: '身份证证件虚假', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'Fmemo1', title: '未显示图片', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'Fmemo2', title: '上传非身份证照片', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'Fmemo5', title: '其他原因', halign: 'center', align: 'right', width: $(this).width() },
                     { field: 'Total', title: '汇总', halign: 'center', align: 'right', width: $(this).width() },
            ]], onLoadSuccess: function (data) {

            },
            onLoadError: function () {

            }
        });
    });

})

