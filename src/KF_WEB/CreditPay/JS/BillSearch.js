$(function () {

    $("#div_BillDetailInfo").dialog("close");
    //时间:起
    $("#txt_BeginDate").datebox({
        value: getCurrentMonthFirst(),
        ShowSeconds: false
    });
    //时间:止
    $("#txt_EndDate").datebox({
        value: SetDate(0),
        ShowSeconds: false
    });

    LoadCommonCombobox("txt_BillStatus", "id", "name", 100, 100, 150, true, "全部", false, false, "CreditPay_BillSearch_BillStatus", 0);
    //$("#txt_BillStatus").combobox({
    //    delay: 1000,
    //    mode: 'remote',
    //    valueField: "id",
    //    textField: "text",
    //    width: 150,
    //    panelWidth: 150,
    //    panelHeight: 150,
    //    multiple: false,
    //    editable: false,
    //    method: "get",
    //    url: 'JS/BillStatus.json',
    //    //queryParams: postDatas,        
    //    onLoadSuccess: function () {
    //        var datas = $("#txt_BillStatus").combobox("getData");
    //        $("#txt_BillStatus").combobox("setValue", datas[0].id);
    //    }
    //});
    $("#txt_Account").change(function () {
        $("#hid_BillListnextpage_flg").val("True");
        $("#hid_BillDetailInfonextpage_flg").val("True");
    });
    $("#btn_Search").click(function () {        
        var accountNo = $("#txt_Account").val();
        var accountType = $("input[name=AccountType]:checked").val();
        var billStatus = $("#txt_BillStatus").combobox("getValue");
        var beginDate = $("#txt_BeginDate").datebox("getValue");
        var endDate = $("#txt_EndDate").datebox("getValue");
        if (accountNo.length < 1 || accountNo == null) {
            $.messager.alert("提示", "请输入用户帐号", "info", null);
            return;
        }
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
        var queryData = {
            accountNo: accountNo,
            accountType: accountType,
            billStatus: billStatus,
            beginDate: beginDate,
            endDate: endDate,
            //next_row_key: $("#hid_BillListnext_row_key").val(),
            nextpage_flg: $("#hid_BillListnextpage_flg").val(),
            pageNumber: $("#hid_BillListPageNumber").val(),
            pageSize: $("#hid_BillListPageSize").val()
        };
        $.ajax({
            type: "POST",
            async: false,  // 设置同步方式
            cache: false,
            dataType: "text",
            url: "/CreditPay/BillSearch.aspx?getAction=LoadBillList&Rand=" + Math.random() + "",
            data: queryData,
            success: function (returnData) {
                var data = eval('(' + returnData + ')');
                
                if (data.result != 0) {
                    if (data.result == "false" || data.result == "False") {
                        var message = data.message;
                        if (message != null && message.length > 0) {
                            if (message == "NoRight") {
                                $.messager.confirm("操作提示", "页面超时,是否刷新？", function (checkResult) {
                                    if (checkResult) {
                                        var loginPath = data.loginPath;
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

                        var res_info = data.res_info;
                        if (res_info != null && res_info.length > 0) {
                            $.messager.alert("提示", res_info, "info", null);
                            return;
                        }
                    }
                                            
                }
                else {
                    var layout = "";
                    if (data.nextpage_flg == "False") {
                        layout = ['prev', 'manual'];
                    }
                    else {
                        layout = ['prev', 'manual', 'next'];
                    }
                    $("#hid_BillListnextpage_flg").val(data.nextpage_flg);
                    LoadBillList();
                    $("#tb_BillList").datagrid('loadData', data); //将数据绑定到datagrid   
                    var pg = $("#tb_BillList").datagrid("getPager");
                    if (pg) {
                        $(pg).pagination({
                            total: Number(data.total),
                            pageNumber: Number(data.page),
                            pageSize: $('#hid_BillListPageSize').val(),//每页显示的记录条数，默认为10  
                            pageList: [20],//可以设置每页记录条数的列表  
                            beforePageText: "", //'第',//页数文本框前显示的汉字  
                            afterPageText: "", //'页    共 {pages} 页',
                            displayMsg: "", // '当前显示 {from} - {to} 条记录   共 {total} 条记录',
                            showPageList: false,
                            showRefresh: false,
                            layout: layout,
                            onSelectPage: function (pageNumber, pageSize) {
                                $(this).pagination('loading');
                                $('#hid_BillListPageNumber').val(Number(pageNumber));
                                $('#hid_BillListPageSize').val(Number(pageSize));
                                $(this).pagination('loaded');
                                $("#btn_Search").click();
                            }
                        });
                    }
                }
            }
            , error: function () {
                $.messager.alert("错误", "出错了!", "info", null);
            }
        });

       
    });
});

function LoadBillList()
{
    //var accountNo = $("#txt_Account").val();
    //var accountType = $("input[name=AccountType]:checked").val();
    //var billStatus = $("#txt_BillStatus").combobox("getValue");
    //var beginDate = $("#txt_BeginDate").datebox("getValue");
    //var endDate = $("#txt_EndDate").datebox("getValue");
    //if (accountNo.length < 1 || accountNo == null) {
    //    $.messager.alert("提示", "请输入用户帐号", "info", null);
    //    return;
    //}
    //if (beginDate.length < 1 || beginDate == null) {
    //    $.messager.alert("提示", "请选择开始日期", "info", null);
    //    return;
    //}
    //else if (endDate.length < 1 || endDate == null) {
    //    $.messager.alert("提示", "请选择结束日期", "info", null);
    //    return;
    //}
    //if (beginDate > endDate) {
    //    $.messager.alert("提示", "开始日期不能大于结束日期", "info", null);
    //    return;
    //}

    //var queryData = {
    //    accountNo: accountNo,
    //    accountType: accountType,
    //    billStatus: billStatus,
    //    beginDate: beginDate,
    //    endDate: endDate
    //};
    var divWidth = $("#div_BillList").width();
    var divHeight = $("#div_BillList").height();

    $('#tb_BillList').datagrid({
        width: divWidth * 99 / 100,
        height: $(document).height() * 92 / 100,
        toolbar: "toolbar",
        delay: 1000,
        mode: 'remote',
        idField: 'bill_id',
        textField: 'bill_id',
        loadMsg: "数据加载中，请稍后...",
        pagination: true,
        //pageSize: '30',
        //pageList: [10, 20, 30, 50],
        sortName: 'start_time',
        sortOrder: ' asc ',
        showFooter: true,
        rownumbers: true,
        singleSelect: true,
        fitColumns: true,
        //url: "/CreditPay/BillSearch.aspx?getAction=LoadBillList&Rand=" + Math.random() + "",
        //queryParams: queryData,  //异步查询的参数

        columns: [[
                { field: 'bill_id', title: '账单ID', halign: 'center', align: 'left', width: $(this).width() },
                {
                    field: 'bill_status', title: '账单状态', halign: 'center', align: 'left', width: $(this).width(),
                    formatter: function (value, row, index) {
                        var span = "<span></span>";
                        if (value == "10") {
                            span = "<span>未出</span>";
                        }
                        else if (value == "20") {
                            span = "<span>已出</span>";
                        }
                        else if (value == "30") {
                            span = "<span>逾期</span>";
                        }
                        else if (value == "40") {
                            span = "<span>结清</span>";
                        }
                        return span;
                    }
                },
                { field: 'start_time', title: '账期开始时间', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'end_time', title: '账期结束时间', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'bill_date', title: '账单日', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'repay_date', title: '还款日', halign: 'center', align: 'left', width: $(this).width() },
                 {
                     field: 'repay_status', title: '还款状态', halign: 'center', align: 'left', width: $(this).width(),
                     formatter: function (value, row, index) {
                         var span = "<span></span>";
                         if (value == "0") {
                             span = "<span>未还款</span>";
                         }
                         else if (value == "1") {
                             span = "<span>已还清</span>";
                         }
                         else if (value == "3") {
                             span = "<span>部分还</span>";
                         }
                         return span;
                     }
                 },
                 { field: 'total_amount', title: '账单总金额', halign: 'center', align: 'right', width: $(this).width() },
                 { field: 'balance', title: '应还金额', halign: 'center', align: 'right', width: $(this).width() },
                 { field: 'interest', title: '利息', halign: 'center', align: 'right', width: $(this).width() },
                 { field: 'overdue_balance', title: '逾期应还款', halign: 'center', align: 'right', width: $(this).width() },
                 {
                     field: '详情', title: '详情', halign: 'center', align: 'center', width: $(this).width(),
                     formatter: function (value, row, index) {
                         var span = "<span><a href='#'  onclick='LoadBillDetailInfo(" + index + ")'>详情</a></span>";
                         return span;
                     }
                 }

        ]], onLoadSuccess: function (data) {

        },
        onLoadError: function () {

        }
    });
}
///加载账单详情
function LoadBillDetailInfo(index) {
    var rows = $("#tb_BillList").datagrid('getData').rows;
    var bill_id = rows[index]["bill_id"];
    var bill_status = rows[index]["bill_status"];
    var start_time = rows[index]["start_time"];
    var end_time = rows[index]["end_time"];
    var bill_date = rows[index]["bill_date"];
    var repay_date = rows[index]["repay_date"];
    var repay_status = rows[index]["repay_status"];
    var total_amount = rows[index]["total_amount"];
    var balance = rows[index]["balance"];
    var interest = rows[index]["interest"];
    var overdue_balance = rows[index]["overdue_balance"];
    var bill_statusValue = "";
    if (bill_status == "10") {
        bill_statusValue = "未出";
    }
    else if (bill_status == "20") {
        bill_statusValue = "已出";
    }
    else if (bill_status == "30") {
        bill_statusValue = "逾期";
    }
    else if (bill_status == "40") {
        bill_statusValue = "结清";
    }
    var repay_statusValue = "";
    if (repay_status == "0") {
        repay_statusValue = "未还款";
    }
    else if (repay_status == "1") {
        repay_statusValue = "已还清";
    }
    else if (repay_status == "3") {
        repay_statusValue = "部分还";
    }
    var tableWidth = $(document).width() * 100 / 100;
    var divHeight = $(document).height() * 100 / 100;
    $("#div_BillDetailInfo").dialog({
        title: 'ID：' + bill_id,
        width: tableWidth * 75 / 100,//90 / 100
        height: divHeight * 70 / 100,//95 / 100
        //resizable: true,
        maximizable: false,
        //minimizable:true,
        closed: false,
        cache: false,
        modal: true,
        //toolbar: '#tb_HaiKangPriceProtectionReport_toolbar',
        onBeforeOpen: function () {
            $("#lab_bill_id").text("");
            $("#lab_bill_status").text("");
            $("#lab_start_time").text("");
            $("#lab_end_time").text("");
            $("#lab_bill_date").text("");
            $("#lab_repay_date").text("");
            $("#lab_repay_status").text("");
            $("#lab_total_amount").text("");
            $("#lab_balance").text("");
            $("#lab_interest").text("");
            $("#lab_overdue_balance").text("");
        },
        onOpen: function () {
            $("#lab_bill_id").text(bill_id);
            $("#lab_bill_status").text(bill_statusValue);
            $("#lab_start_time").text(start_time);
            $("#lab_end_time").text(end_time);
            $("#lab_bill_date").text(bill_date);
            $("#lab_repay_date").text(repay_date);
            $("#lab_repay_status").text(repay_statusValue);
            $("#lab_total_amount").text(total_amount);
            $("#lab_balance").text(balance);
            $("#lab_interest").text(interest);
            $("#lab_overdue_balance").text(overdue_balance);
            LoadBillDetail(bill_id);
        }
    });
    $("#div_BillDetailInfo").window("center");

}

function LoadBillDetail(bill_id) {
    var accountNo = $("#txt_Account").val();
    if (accountNo.length < 1 || accountNo == null) {
        $.messager.alert("提示", "请输入用户帐号", "info", null);
        return;
    }
    var accountType = $("input[name=AccountType]:checked").val();
    var beginDate = $("#txt_BeginDate").datebox("getValue");
    var endDate = $("#txt_EndDate").datebox("getValue");
    var queryData = {
        accountNo: accountNo,
        accountType: accountType,
        beginDate: beginDate,
        endDate: endDate,
        bill_id: bill_id,
        nextpage_flg: $("#hid_BillDetailInfonextpage_flg").val(),        
        pageNumber: $("#hid_BillDetailInfoPageNumber").val(),
        pageSize: $("#hid_BillDetailInfoPageSize").val()
    };
   
    $.ajax({
        type: "POST",
        async: false,  // 设置同步方式
        cache: false,
        dataType: "text",
        url: "/CreditPay/BillSearch.aspx?getAction=LoadBillDetailInfo&Rand=" + Math.random() + "",
        data: queryData,
        success: function (returnData) {
            var data = eval('(' + returnData + ')');
            
            if (data.result != 0) {
                if (data.result == "false" || data.result == "False") {
                    var message = data.message;
                    
                    if (message != null && message.length > 0) {
                        if (message == "NoRight") {
                            $.messager.confirm("操作提示", "页面超时,是否刷新？", function (checkResult) {
                                if (checkResult) {
                                    var loginPath = data.loginPath;
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

                    var res_info = data.res_info;                    
                    if (res_info != null && res_info.length > 0) {
                        $.messager.alert("提示", res_info, "info", null);
                        return;
                    }
                }

            }
            else {
                var layout = "";
                if (data.nextpage_flg == "False") {
                    layout = ['prev', 'manual'];
                }
                else {
                    layout = ['prev', 'manual', 'next'];
                }
                $("#hid_BillDetailInfonextpage_flg").val(data.nextpage_flg);
                LoadBillDetailDatagrid();
                $("#tb_BillDetailInfo").datagrid('loadData', data); //将数据绑定到datagrid   
                var pg = $("#tb_BillDetailInfo").datagrid("getPager");
                if (pg) {
                    $(pg).pagination({
                        total: Number(data.total),
                        pageNumber: Number(data.page),
                        pageSize: $('#hid_BillDetailInfoPageSize').val(),//每页显示的记录条数，默认为10  
                        pageList: [20],//可以设置每页记录条数的列表  
                        beforePageText: "", //'第',//页数文本框前显示的汉字  
                        afterPageText: "", //'页    共 {pages} 页',
                        displayMsg: "", // '当前显示 {from} - {to} 条记录   共 {total} 条记录',
                        showPageList: false,
                        showRefresh: false,
                        layout: layout,
                        onSelectPage: function (pageNumber, pageSize) {
                            $(this).pagination('loading');
                            $('#hid_BillDetailInfoPageNumber').val(Number(pageNumber));
                            $('#hid_BillDetailInfoPageSize').val(Number(pageSize));
                            $(this).pagination('loaded');
                            LoadBillDetail(bill_id);
                        }
                    });
                }
            }
        }
           , error: function () {
               $.messager.alert("错误", "出错了!", "info", null);
           }
    });
}

function LoadBillDetailDatagrid()
{
    //var accountNo = $("#txt_Account").val();
    //if (accountNo.length < 1 || accountNo == null) {
    //    $.messager.alert("提示", "请输入用户帐号", "info", null);
    //    return;
    //}
    //var accountType = $("input[name=AccountType]:checked").val();
    //var beginDate = $("#txt_BeginDate").datebox("getValue");
    //var endDate = $("#txt_EndDate").datebox("getValue");
    //var queryData = {
    //    accountNo: accountNo,
    //    accountType: accountType,
    //    beginDate: beginDate,
    //    endDate: endDate,
    //    bill_id: bill_id,
    //};
    var divWidth = $("#div_BillDetailInfo").width();
    var divHeight = $("#div_BillDetailInfo").height();

    $('#tb_BillDetailInfo').datagrid({
        width: divWidth * 100 / 100,
        height: divHeight * 79 / 100,
        toolbar: "toolbar",
        delay: 1000,
        mode: 'remote',
        idField: 'bill_id',
        textField: 'bill_id',
        loadMsg: "数据加载中，请稍后...",
        pagination: true,
        //pageSize: '10',
        //pageList: [10, 20, 30, 50],
        sortName: 'start_time',
        sortOrder: ' asc ',
        showFooter: true,
        rownumbers: true,
        singleSelect: true,
        fitColumns: true,
        //url: "/CreditPay/BillSearch.aspx?getAction=LoadBillDetailInfo&Rand=" + Math.random() + "",
        //queryParams: queryData,  //异步查询的参数

        columns: [[
                { field: 'trans_time', title: '时间', halign: 'center', align: 'left', width: $(this).width() },
                {
                    field: 'trans_type', title: '类型', halign: 'center', align: 'left', width: $(this).width(),
                    formatter: function (value, row, index) {
                        var span = "<span></span>";
                        if (value == "1") {
                            span = "<span>消费</span>";
                        }
                        else if (value == "2") {
                            span = "<span>往期分期（使账单金额增加）</span>";
                        }
                        else if (value == "3") {
                            span = "<span>本期分期（使账单金额减少）</span>";
                        }
                        else if (value == "4") {
                            span = "<span>还款冲账总单</span>";
                        }
                        else if (value == "5") {
                            span = "<span>还款冲账分单（根据产品交互，前端可选择不显示）</span>";
                        }
                        else if (value == "6") {
                            span = "<span>退款冲账总单</span>";
                        }
                        else if (value == "7") {
                            span = "<span>退款冲账分单（根据产品交互，前端可选择不显示）</span>";
                        }
                        else if (value == "8") {
                            span = "<span>调账</span>";
                        }
                        else if (value == "9") {
                            span = "<span>利息</span>";
                        }
                        return span;
                    }
                },
                { field: 'trans_amount', title: '交易金额', halign: 'center', align: 'right', width: $(this).width() },
                 { field: 'trans_info', title: '商品名称', halign: 'center', align: 'left', width: $(this).width() },
                 {
                     field: 'trans_status', title: '交易状态', halign: 'center', align: 'left', width: $(this).width(),
                     formatter: function (value, row, index) {
                         var span = "<span></span>";
                         if (value == "0") {
                             span = "<span>交易成功</span>";
                         }
                         else if (value == "1") {
                             span = "<span>交易退货</span>";
                         }
                         return span;
                     }
                 },
                 { field: 'sp_name', title: '商户名称', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'trans_flow_id', title: '交易流水号', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'trans_id', title: '交易订单号', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'sp_bill_no', title: '商户订单号', halign: 'center', align: 'left', width: $(this).width() }

        ]], onLoadSuccess: function (data) {

        },
        onLoadError: function () {

        }
    });
}