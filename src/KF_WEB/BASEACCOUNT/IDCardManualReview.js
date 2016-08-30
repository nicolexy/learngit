$(function () {

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

    LoadCommonCombobox("ddl_ReviewStatus", "id", "name", 150, 200, true, "全部", false, false, "IDCardManualReview_LoadReveiwStatus", 0);
    LoadCommonCombobox("ddl_ReviewResult", "id", "name", 150, 200, true, "全部", false, false, "IDCardManualReview_LoadReveiwResult", 0);
    $("#txt_ReviewCount").numberspinner({
        min: 1,
        max: 50,
        editable: true
    });
    $("#txt_ReviewCount").numberspinner("setValue", 50);  // 设置值

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
            success: function (result) {
                if (result.length > 0) {
                    $.messager.alert("提示", result, "info", null);
                }

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
        $("#ddl_ReviewStatus").combobox("getValue");
        var reviewCount = $("#txt_ReviewCount").numberspinner("getValue");
        var beginDate = $("#tbx_beginDate").datebox("getValue");
        var endDate = $("#txt_EndDate").datebox("getValue");
        if (beginDate.length < 1 || beginDate==null)
        {
            $.messager.alert("提示", "请选择开始日期", "info", null);
            return;
        }
        else if (endDate.length < 1 || endDate==null)
        {
            $.messager.alert("提示", "请选择结束日期", "info", null);
            return;
        }
        if (beginDate > endDate)
        {
            $.messager.alert("提示", "开始日期不能大于结束日期", "info", null);
            return;
        }
        var datas = {
            uid: uid,
            uin: uin,
            reviewCount: reviewCount,
            beginDate: beginDate,
            endDate: endDate
        };
        $.ajax({
            type: "POST",
            url: "IDCardManualReview.aspx?getAction=CheckDate",
            data: datas,
            dataType: "text",
            success: function (data) {
                if (data.length > 0)
                {
                    $.messager.alert('提示', data, 'Info');
                }
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
            height: $(document).height() * 80 / 100,
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
            singleSelect: false,
            fitColumns: true,
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
                    { field: 'Fserial_number', title: '流水号', halign: 'center', align: 'left', width: $(this).width() },
                    { field: 'Fspid', title: '商户号', halign: 'center', align: 'left', width: $(this).width() },
                    { field: 'Fcreate_time', title: '申请时间', halign: 'center', align: 'left', width: $(this).width() },
                     { field: 'Fuin', title: '用户帐号', halign: 'center', align: 'left', width: $(this).width() },
                     { field: 'Fmodify_time1', title: '审核时间', halign: 'center', align: 'left', width: $(this).width() },
                     {
                         field: 'Fstate', title: '审核状态', halign: 'center', align: 'left', width: $(this).width(),
                         formatter: function (value, row, index) {
                             var Fstate = "";
                             if (value == "1") {
                                 Fstate = "未领单";
                             }
                             else if (value == "2") {
                                 Fstate = "已领单";
                             }
                             else if (value == "3") {
                                 Fstate = "推送到实名系统失败";
                             }
                             else if (value == "4") {
                                 Fstate = "推送成功";
                             }
                             var span = "<span>" + Fstate + "</span>";
                             return span;
                         }
                     },
                     {
                         field: 'Fresult', title: '审核结果', halign: 'center', align: 'left', width: $(this).width(),
                         formatter: function (value, row, index) {
                             var Fresult = "";
                             if (value == "0") {
                                 Fresult = "未处理";
                             }
                             else if (value == "1") {
                                 Fresult = "通过";
                             }
                             else if (value == "2") {
                                 Fresult = "驳回";
                             }
                             var span = "<span>" + Fresult + "</span>";
                             return span;
                         }
                     },
                     { field: 'Foperator', title: '处理人', halign: 'center', align: 'left', width: $(this).width() },
                     {
                         field: 'Fmemo', title: '审核信息', halign: 'center', align: 'left', width: $(this).width(),
                         formatter: function (value, row, index) {
                             var Fmemo = "";
                             if (value == "1") {
                                 Fmemo = "未显示图片";
                             }
                             else if (value == "2") {
                                 Fmemo = "未提供身份证扫描件";
                             }
                             else if (value == "3") {
                                 Fmemo = "上传的扫描件不够完整、清晰、有效";
                             }
                             else if (value == "4") {
                                 Fmemo = "证件号码与原注册证件号码不符";
                             }
                             else if (value == "5") {
                                 Fmemo = "其他原因";
                             }
                             var span = "<span>" + Fmemo + "</span>";
                             return span;
                         }
                     },
                      {
                          field: '详细类容', title: '详细类容', halign: 'center', align: 'center', width: $(this).width(),
                          formatter: function (value, row, index) {
                              var span = "";
                              if (row.Foperator == "" || row.Foperator.length < 1 || row.Foperator == "undefined") {
                                  span = "<span></span>";
                              }
                              else if (row.Foperator == uid) {
                                  span = "<span><a href='#'  onclick='ReviewIdCard(" + index + ")'>详细类容</a></span>";
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
            success: function (result) {
                if (result.length > 0) {
                    $.messager.alert("提示", result, "info", null);
                }
                else {
                    $.messager.alert('提示', '操作成功', 'Info');
                    $("#div_ReveiwIdCard").dialog("close");
                    $("#tb_IDCardManualReviewList").datagrid("load");
                }
            }
        });
    });
    $("#a_No").click(function () {
        var selectedFmemo = $("#txt_Fmemo").combobox("getValue");
        if (selectedFmemo == 0)
        {
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
            success: function (result) {
                if (result.length > 0) {
                    $.messager.alert("提示", result, "info", null);
                }
                else {
                    $.messager.alert('提示', '操作成功', 'Info');
                    $("#div_ReveiwIdCard").dialog("close");
                    $("#tb_IDCardManualReviewList").datagrid("load");
                }
                //if (data == "Success") {
                //    $.messager.alert('提示', '操作成功', 'Info');
                //    $("#div_ReveiwIdCard").dialog("close");
                //    //$("#tb_LoanList").datagrid("load");
                //}
                //else {
                //    $.messager.alert('提示', '操作失败', 'Info');
                //}
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
            success: function (result) {
                if (result.length > 0) {
                    $.messager.alert("提示", result, "info", null);
                }
                else {
                    $.messager.alert('提示', '操作成功', 'Info');
                    $("#div_ReveiwIdCard").dialog("close");
                    $("#tb_IDCardManualReviewList").datagrid("load");
                }
                //if (data == "Success") {
                //    $.messager.alert('提示', '操作成功', 'Info');
                //    $("#div_ReveiwIdCard").dialog("close");
                //    //$("#tb_LoanList").datagrid("load");
                //}
                //else {
                //    $.messager.alert('提示', '操作失败', 'Info');
                //}
            }
        });
    });

})

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
        title: '审核身份证',
        width: tableWidth * 60 / 100,
        height: divHeight * 80 / 100,
        //resizable: true,
        maximizable: false,
        //minimizable:true,
        closed: false,
        cache: false,
        modal: true,
        //toolbar: '#tb_HaiKangPriceProtectionReport_toolbar',
        onBeforeOpen: function () {


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
                success: function (data) {                    
                    $("#lab_Fname").text(data);
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
                success: function (data) {                    
                    $("#lab_Fidentitycard").text(data);
                }
            });
            $("#lab_Fuin").text(Fuin);
            //$("#lab_Fname").text(deFname);
            //$("#lab_Fidentitycard").text(deFidentitycard);            
            $("#hid_TableName").val(TableName);
            $("#hid_Fid").val(Fid);
            $("#hid_Fserial_number").val(Fserial_number);
            LoadCommonCombobox("txt_Fmemo", "id", "name", 200, 250, true, "请选择", false, false, "IDCardManualReview_LoadFmemo", Fmemo);
            var aaa = $("#hid_IdCaredServerPath").val() + "?" + Fimage_path1;
            var bbb = $("#hid_IdCaredServerPath").val() + "?" + Fimage_path2;
            $("#ima_IDCardZ").attr("src", aaa);
            $("#ima_IDCardF").attr("src", bbb);
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
