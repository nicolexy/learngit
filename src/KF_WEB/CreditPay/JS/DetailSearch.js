$(function () {
    $("#div_PayList").hide();
    $("#div_RepayList").hide();
    $("#div_RefundList").hide();
    $("#div_RefundDetail").dialog("close");
    //$("#div_RefundQuXiang").dialog("close");
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
    $("#ddl_SearchType").combobox({
        delay: 1000,
        mode: 'remote',
        valueField: "id",
        textField: "text",
        width: 150,
        panelWidth: 150,
        panelHeight: 150,
        multiple: false,
        editable: false,
        method: "get",
        url: 'JS/SearchType.json',
        //queryParams: postDatas,        
        onLoadSuccess: function () {
            var datas = $("#ddl_SearchType").combobox("getData");
            $("#ddl_SearchType").combobox("setValue", datas[0].id);
        }
    });

    $("#btn_Search").click(function () {
        //$("#hid_PayListnext_row_key").val("0");
        //$("#hid_RepayListnext_row_key").val("0");
        //$("#hid_RefundListnext_row_key").val("0");

        var accountNo = $("#txt_Account").val();
        var accountType = $("input[name=AccountType]:checked").val();
        var transID = $("#txt_TransID").val();
        var beginDate = $("#txt_BeginDate").datebox("getValue");
        var endDate = $("#txt_EndDate").datebox("getValue");
        var searchType = $("#ddl_SearchType").combobox("getValue");
        if (accountNo.length < 1 || accountNo == null) {
            $.messager.alert("提示", "请输入用户帐号", "info", null);
            return;
        }
        else if (beginDate.length < 1 || beginDate == null) {
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

        ///searchType 1=支付明细查询；2=还款明细查询；3=退款明细查询
        if (searchType == 1) {
            //支付查询
            LoadPayList();            
        }
        else if (searchType == 2) {
            //还款查询
            LoadRepayList();            
        }
        else if (searchType == 3) {
            //退款查询
            LoadRefundList();
        }
    });
});

//支付查询
function LoadPayList()
{
    var accountNo = $("#txt_Account").val();
    var accountType = $("input[name=AccountType]:checked").val();
    var transID = $("#txt_TransID").val();
    var beginDate = $("#txt_BeginDate").datebox("getValue");
    var endDate = $("#txt_EndDate").datebox("getValue");

    var queryData = {
        accountNo: accountNo,
        accountType: accountType,
        transID: transID,
        beginDate: beginDate,
        endDate: endDate,
        pageNumber: $("#hid_PayListPageNumber").val(),
        pageSize: $("#hid_PayListPageSize").val(),
        nextpage_flg: $("#hid_PayListnextpage_flg").val(),
        next_row_key: $("#hid_PayListnext_row_key").val()
    };
    $.ajax({
        type: "POST",
        async: false,  // 设置同步方式
        cache: false,
        dataType: "text",
        url: "/CreditPay/DetailSearch.aspx?getAction=LoadPayList&Rand=" + Math.random() + "",
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
                $("#hid_PayListnextpage_flg").val(data.nextpage_flg);
                $("#hid_PayListnext_row_key").val(data.next_row_key);
                LoadPayListDatagrid();
                $("#tb_PayList").datagrid('loadData', data); //将数据绑定到datagrid   
                var pg = $("#tb_PayList").datagrid("getPager");
                if (pg) {
                    $(pg).pagination({
                        total: Number(data.total),
                        pageNumber: Number(data.page),
                        pageSize: $('#hid_PayListPageSize').val(),//每页显示的记录条数，默认为10  
                        pageList: [20],//可以设置每页记录条数的列表  
                        beforePageText: "", //'第',//页数文本框前显示的汉字  
                        afterPageText: "", //'页    共 {pages} 页',
                        displayMsg: "", // '当前显示 {from} - {to} 条记录   共 {total} 条记录',
                        showPageList: false,
                        showRefresh: false,
                        layout: ['prev', 'manual', 'next'],
                        onSelectPage: function (pageNumber, pageSize) {
                            $(this).pagination('loading');
                            $('#hid_PayListPageNumber').val(Number(pageNumber));
                            $('#hid_PayListPageSize').val(Number(pageSize));
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
}

function LoadPayListDatagrid() {
    //var accountNo = $("#txt_Account").val();
    //var accountType = $("input[name=AccountType]:checked").val();
    //var transID = $("#txt_TransID").val();
    //var beginDate = $("#txt_BeginDate").datebox("getValue");
    //var endDate = $("#txt_EndDate").datebox("getValue");

    //var queryData = {
    //    accountNo: accountNo,
    //    accountType: accountType,
    //    transID: transID,
    //    beginDate: beginDate,
    //    endDate: endDate,
    //    next_row_key: $("#hid_PayListnext_row_key").val()
    //};

    $("#div_PayList").show();
    $("#div_RepayList").hide();
    $("#div_RefundList").hide();

    var divWidth = $("#div_PayList").width();
    var divHeight = $("#div_PayList").height();

    $('#tb_PayList').datagrid({
        title: "支付明细",
        width: divWidth * 99 / 100,
        height: $(document).height() * 92 / 100,
        toolbar: "toolbar",
        delay: 1000,
        mode: 'remote',
        idField: 'trans_id',
        textField: 'trans_id',
        loadMsg: "数据加载中，请稍后...",
        pagination: true,
        //pageSize: '30',
        //pageList: [10, 20, 30, 50],
        sortName: 'trans_time',
        sortOrder: ' asc ',
        showFooter: true,
        rownumbers: true,
        singleSelect: true,
        fitColumns: true,
        //url: "/CreditPay/DetailSearch.aspx?getAction=LoadPayList&Rand=" + Math.random() + "",
        //queryParams: queryData,  //异步查询的参数

        columns: [[
             //{
             //    field: 'next_row_key', hidden: true,
             //    formatter: function (value, row, index) {
             //        if (value != null && value != "undefined") {
             //            $("#hid_PayListnext_row_key").val(value);
             //        }
             //    }
             //},
                { field: 'trans_time', title: '时间', halign: 'center', align: 'left', width: $(this).width() },
                { field: 'trans_amount', title: '交易金额', halign: 'center', align: 'right', width: $(this).width() },
                { field: 'trans_info', title: '商品名称', halign: 'center', align: 'left', width: $(this).width() },
                 {
                     field: 'trans_status', title: '交易状态', halign: 'center', align: 'left', width: $(this).width(),
                     formatter: function (value, row, index) {
                         var span = "<span></span>";
                         //if (value == "0") {
                         //    span = "<span>支付成功</span>";
                         //}
                         //else if (value == "1") {
                         //    span = "<span>支付失败</span>";
                         //}
                         if (value == "10") {
                             span = "<span>未知结果</span>";
                         }
                         else if (value == "20") {
                             span = "<span>收到冲正请求，准备冲正</span>";
                         }
                         else if (value == "30") {
                             span = "<span>订单信用付支付成功</span>";
                         }
                         else if (value == "31") {
                             span = "<span>关单成功</span>";
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

//还款查询
function LoadRepayList()
{
    var accountNo = $("#txt_Account").val();
    var accountType = $("input[name=AccountType]:checked").val();
    var transID = $("#txt_TransID").val();
    var beginDate = $("#txt_BeginDate").datebox("getValue");
    var endDate = $("#txt_EndDate").datebox("getValue");

    var queryData = {
        accountNo: accountNo,
        accountType: accountType,
        transID: transID,
        beginDate: beginDate,
        endDate: endDate,
        pageNumber: $("#hid_RepayListPageNumber").val(),
        pageSize: $("#hid_RepayListPageSize").val(),
        nextpage_flg: $("#hid_RepayListnextpage_flg").val(),
        next_row_key: $("#hid_RepayListnext_row_key").val()
    };

    $.ajax({
        type: "POST",
        async: false,  // 设置同步方式
        cache: false,
        dataType: "text",
        url: "/CreditPay/DetailSearch.aspx?getAction=LoadRepayList&Rand=" + Math.random() + "",
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
                $("#hid_RepayListnextpage_flg").val(data.nextpage_flg);
                $("#hid_RepayListnext_row_key").val(data.next_row_key);
                LoadRepayListDatagrid();
                $("#tb_RepayList").datagrid('loadData', data); //将数据绑定到datagrid   
                var pg = $("#tb_RepayList").datagrid("getPager");
                if (pg) {
                    $(pg).pagination({
                        total: Number(data.total),
                        pageNumber: Number(data.page),
                        pageSize: $('#hid_RepayListPageSize').val(),//每页显示的记录条数，默认为10  
                        pageList: [20],//可以设置每页记录条数的列表  
                        beforePageText: "", //'第',//页数文本框前显示的汉字  
                        afterPageText: "", //'页    共 {pages} 页',
                        displayMsg: "", // '当前显示 {from} - {to} 条记录   共 {total} 条记录',
                        showPageList: false,
                        showRefresh: false,
                        layout: ['prev', 'manual', 'next'],
                        onSelectPage: function (pageNumber, pageSize) {
                            $(this).pagination('loading');
                            $('#hid_RepayListPageNumber').val(Number(pageNumber));
                            $('#hid_RepayListPageSize').val(Number(pageSize));
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
}

function LoadRepayListDatagrid() {
    //var accountNo = $("#txt_Account").val();
    //var accountType = $("input[name=AccountType]:checked").val();
    //var transID = $("#txt_TransID").val();
    //var beginDate = $("#txt_BeginDate").datebox("getValue");
    //var endDate = $("#txt_EndDate").datebox("getValue");

    //var queryData = {
    //    accountNo: accountNo,
    //    accountType: accountType,
    //    transID: transID,
    //    beginDate: beginDate,
    //    endDate: endDate,
    //    next_row_key: $("#hid_RepayListnext_row_key").val()
    //};

    $("#div_PayList").hide();
    $("#div_RepayList").show();
    $("#div_RefundList").hide();

    var divWidth = $("#div_RepayList").width();
    var divHeight = $("#div_RepayList").height();

    $('#tb_RepayList').datagrid({
        title: "还款明细",
        width: divWidth * 99 / 100,
        height: $(document).height() * 92 / 100,
        toolbar: "toolbar",
        delay: 1000,
        mode: 'remote',
        idField: 'trans_id',
        textField: 'trans_id',
        loadMsg: "数据加载中，请稍后...",
        pagination: true,
        //pageSize: '30',
        //pageList: [10, 20, 30, 50],
        sortName: 'trans_time',
        sortOrder: ' asc ',
        showFooter: true,
        rownumbers: true,
        singleSelect: true,
        fitColumns: true,
        //url: "/CreditPay/DetailSearch.aspx?getAction=LoadRepayList&Rand=" + Math.random() + "",
        //queryParams: queryData,  //异步查询的参数

        columns: [[
            //{
            //    field: 'next_row_key', hidden: true,
            //    formatter: function (value, row, index) {
            //        if (value != null && value != "undefined") {
            //            $("#hid_RepayListnext_row_key").val(value);
            //        }
            //    }
            //},
                { field: 'repay_time', title: '交易时间', halign: 'center', align: 'left', width: $(this).width() },
                { field: 'repay_amount', title: '交易金额', halign: 'center', align: 'right', width: $(this).width() },
                {
                    field: 'rapay_info', title: '还款类型', halign: 'center', align: 'left', width: $(this).width(),
                    formatter: function (value, row, index) {
                        var span = "<span></span>";
                        //if (value == "0") {
                        //    span = "<span>主动还</span>";
                        //}
                        //else if (value == "1") {
                        //    span = "<span>代扣</span>";
                        //}
                        //else if (value == "2") {
                        //    span = "<span>其他</span>";
                        //}
                        if (value == "1") {
                            span = "<span>逾期还款</span>";
                        }
                        else if (value == "2") {
                            span = "<span>当期还款</span>";
                        }
                        else if (value == "3") {
                            span = "<span>未出账单还款</span>";
                        }
                        else if (value == "4") {
                            span = "<span>全部结清</span>";
                        }
                        else if (value == "5") {
                            span = "<span>后台代扣（代扣还款单不区分逾期当期）</span>";
                        }
                        return span;
                    }
                },
                 { field: 'repay_channel', title: '还款方式', halign: 'center', align: 'left', width: $(this).width(),
                     formatter: function (value, row, index) {
                         var span = "<span></span>";                   
                         if (value == "1") {
                             span = "<span>主动还款</span>";
                         }
                         else if (value == "2") {
                             span = "<span>后台代扣</span>";
                         }                   
                         return span;
                     }
                 },
                 { field: 'balance_orig', title: '还款资金来源', halign: 'center', align: 'left', width: $(this).width() },
                 {
                     field: 'repay_status', title: '还款状态', halign: 'center', align: 'left', width: $(this).width(),
                     formatter: function (value, row, index) {
                         var span = "<span></span>";
                         //if (value == "0") {
                         //    span = "<span>成功</span>";
                         //}
                         //else if (value == "1") {
                         //    span = "<span>失败</span>";
                         //}
                         if (value == "10") {
                             span = "<span>初始化</span>";
                         }
                         else if (value == "20") {
                             span = "<span>待冲账</span>";
                         }
                         else if (value == "30") {
                             span = "<span>冲账成功</span>";
                         }
                         else if (value == "40") {
                             span = "<span>冲账失败</span>";
                         }
                         else if (value == "50") {
                             span = "<span>无需冲账</span>";
                         }
                         return span;
                     }
                 },
                 { field: 'trans_flow_id', title: '交易流水号', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'trans_id', title: '交易订单号', halign: 'center', align: 'left', width: $(this).width() }

        ]], onLoadSuccess: function (data) {

        },
        onLoadError: function () {

        }
    });
}

//退款查询
function LoadRefundList()
{
    var accountNo = $("#txt_Account").val();
    var accountType = $("input[name=AccountType]:checked").val();
    var transID = $("#txt_TransID").val();
    var beginDate = $("#txt_BeginDate").datebox("getValue");
    var endDate = $("#txt_EndDate").datebox("getValue");
    var queryData = {
        accountNo: accountNo,
        accountType: accountType,
        transID: transID,
        beginDate: beginDate,
        endDate: endDate,
        nextpage_flg: $("#hid_RefundListnextpage_flg").val(),
        next_row_key: $("#hid_RefundListnext_row_key").val(),
        pageNumber: $("#hid_RefundListPageNumber").val(),
        pageSize: $("#hid_RefundListPageSize").val()
    };
    $.ajax({
        type: "POST",
        async: false,  // 设置同步方式
        cache: false,
        dataType: "text",
        url: "/CreditPay/DetailSearch.aspx?getAction=LoadRefundList&Rand=" + Math.random() + "",
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
                $("#hid_RefundListnextpage_flg").val(data.nextpage_flg);
                $("#hid_RefundListnext_row_key").val(data.next_row_key);
                LoadRefundListDatagrid();
                $("#tb_RefundList").datagrid('loadData', data); //将数据绑定到datagrid   
                var pg = $("#tb_RefundList").datagrid("getPager");
                if (pg) {
                    $(pg).pagination({
                        total: Number(data.total),
                        pageNumber: Number(data.page),
                        pageSize: $('#hid_RefundListPageSize').val(),//每页显示的记录条数，默认为10  
                        pageList: [20],//可以设置每页记录条数的列表  
                        beforePageText: "", //'第',//页数文本框前显示的汉字  
                        afterPageText: "", //'页    共 {pages} 页',
                        displayMsg: "", // '当前显示 {from} - {to} 条记录   共 {total} 条记录',
                        showPageList: false,
                        showRefresh: false,
                        layout: ['prev', 'manual', 'next'],
                        onSelectPage: function (pageNumber, pageSize) {
                            $(this).pagination('loading');
                            $('#hid_RefundListPageNumber').val(Number(pageNumber));
                            $('#hid_RefundListPageSize').val(Number(pageSize));
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
}

function LoadRefundListDatagrid() {
    //var accountNo = $("#txt_Account").val();
    //var accountType = $("input[name=AccountType]:checked").val();
    //var transID = $("#txt_TransID").val();
    //var beginDate = $("#txt_BeginDate").datebox("getValue");
    //var endDate = $("#txt_EndDate").datebox("getValue");
    //var queryData = {
    //    accountNo: accountNo,
    //    accountType: accountType,
    //    transID: transID,
    //    beginDate: beginDate,
    //    endDate: endDate,
    //    //next_row_key: $("#hid_RefundListnext_row_key").val()
    //};

    $("#div_PayList").hide();
    $("#div_RepayList").hide();
    $("#div_RefundList").show();

    var divWidth = $("#div_RefundList").width();
    var divHeight = $("#div_RefundList").height();

    $('#tb_RefundList').datagrid({
        title: "退款明细",
        width: divWidth * 99 / 100,
        height: $(document).height() * 92 / 100,
        toolbar: "toolbar",
        delay: 1000,
        mode: 'remote',
        idField: 'trans_id',
        textField: 'trans_id',
        loadMsg: "数据加载中，请稍后...",
        pagination: true,
        //pageSize: '5',
        //pageList: [5, 10, 20, 30, 50],
        sortName: 'trans_time',
        sortOrder: ' asc ',
        showFooter: true,
        rownumbers: true,
        singleSelect: true,
        fitColumns: true,
        //url: "/CreditPay/DetailSearch.aspx?getAction=LoadRefundList&Rand=" + Math.random() + "",
        //queryParams: queryData,  //异步查询的参数

        columns: [[
              {
                  field: 'next_row_key', hidden: true,
                  formatter: function (value, row, index) {
                      if (value != null && value != "undefined") {
                          $("input[id = hid_RefundListnext_row_key]").val(value);
                      }
                  }
              },
                { field: 'c_rchg_id', hidden: true },
                { field: 'refund_time', title: '退款时间', halign: 'center', align: 'left', width: $(this).width() },
                { field: 'refund_amount', title: '金额', halign: 'center', align: 'right', width: $(this).width() },
                { field: 'refund_info', title: '商品名称', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'sp_name', title: '商户名称', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'refund_flow_id', title: '退款流水号', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'trans_id', title: '交易订单号', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'refund_trans_id', title: '退款交易订单号', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'sp_bill_no', title: '商户订单号', halign: 'center', align: 'left', width: $(this).width() },
                 {
                     field: '详细信息', title: '详细信息', halign: 'center', align: 'center', width: divWidth * 5 / 100,
                     formatter: function (value, row, index) {
                         var span = "";
                         span = "<span><a href='#'  onclick='LoadRefundDetailInfo(" + index + ")'>详细内容</a></span>";
                         return span;
                     }
                 }

        ]], onLoadSuccess: function (data) {
            
        },
        onLoadError: function () {

        }
    });
}


///加载退款详情
function LoadRefundDetailInfo(tb_RefundListRowIndex) {
    var rows = $("#tb_RefundList").datagrid('getData').rows;
    //var c_rchg_id = rows[tb_RefundListRowIndex]["c_rchg_id"];   
    var refund_flow_id = rows[tb_RefundListRowIndex]["refund_flow_id"];

    var tableWidth = $(document).width() * 100 / 100;
    var divHeight = $(document).height() * 100 / 100;
    $("#div_RefundDetail").dialog({
        title: '退款流水号：' + refund_flow_id,
        width: tableWidth * 80 / 100,//90 / 100
        height: divHeight * 60 / 100,//95 / 100
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
            LoadRefundDetail(tb_RefundListRowIndex);
            LoadRefundQuXiangInfo(tb_RefundListRowIndex);
        }
    });
    $("#div_RefundDetail").window("center");

}

function LoadRefundDetail(tb_RefundListRowIndex) {
    var accountNo = $("#txt_Account").val();
    var accountType = $("input[name=AccountType]:checked").val();
    //var transID = $("#txt_TransID").val();
    var beginDate = $("#txt_BeginDate").datebox("getValue");
    var endDate = $("#txt_EndDate").datebox("getValue");

    var rows = $("#tb_RefundList").datagrid('getData').rows;
    //var c_rchg_id = rows[tb_RefundListRowIndex]["c_rchg_id"];

    var refund_flow_id = rows[tb_RefundListRowIndex]["refund_flow_id"];

    var queryData = {
        accountNo: accountNo,
        accountType: accountType,
        beginDate: beginDate,
        endDate: endDate,
        refund_flow_id: refund_flow_id
        //c_rchg_id: c_rchg_id
    };
    var divWidth = $("#div_RefundDetail").width();
    var divHeight = $("#div_RefundDetail").height();

    $('#tab_RefundDetail').datagrid({
        title: "退款详情",
        width: divWidth * 100 / 100,
        height: divHeight * 20 / 100,
        toolbar: "toolbar",
        delay: 1000,
        mode: 'remote',
        idField: 'refund_flow_id',
        textField: 'refund_flow_id',
        loadMsg: "数据加载中，请稍后...",
        pagination: false,
        pageSize: '10',
        pageList: [10, 20, 30, 50],
        sortName: 'start_time',
        sortOrder: ' asc ',
        showFooter: true,
        rownumbers: true,
        singleSelect: true,
        fitColumns: true,
        url: "/CreditPay/DetailSearch.aspx?getAction=LoadRefundDetail&Rand=" + Math.random() + "",
        queryParams: queryData,  //异步查询的参数

        columns: [[
                //{ field: 'list_num', title: '退款去向记录条数', halign: 'center', align: 'left', width: $(this).width() },
                { field: 'trans_time', title: '原支付时间', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'trans_info', title: '商品名称', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'refund_time', title: '退款时间', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'sp_name', title: '商户名称', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'trans_id', title: '交易订单号', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'refund_flow_id', title: '退款流水号', halign: 'center', align: 'left', width: $(this).width() },
                 { field: 'refund_trans_id', title: '退款交易订单号', halign: 'center', align: 'left', width: $(this).width() },

                 { field: 'sp_bill_no', title: '商户订单号', halign: 'center', align: 'left', width: $(this).width() }
                 //{
                 //    field: 'row_n', title: '详细信息', halign: 'center', align: 'right', width: $(this).width(),
                 //    formatter: function (value, row, index) {
                 //        var span = "<span><a href='#'  onclick='LoadRefundQuXiangInfo(" + index + ")'>" + value + "</a></span>";
                 //        return span;
                 //    }
                 //},

        ]], onLoadSuccess: function (data) {

        },
        onLoadError: function () {

        }
    });
}

function LoadRefundQuXiangInfo(tb_RefundListRowIndex) {
    var accountNo = $("#txt_Account").val();
    var accountType = $("input[name=AccountType]:checked").val();
    //var transID = $("#txt_TransID").val();
    var beginDate = $("#txt_BeginDate").datebox("getValue");
    var endDate = $("#txt_EndDate").datebox("getValue");

    var rows = $("#tb_RefundList").datagrid('getData').rows;
    //var c_rchg_id = rows[tb_RefundListRowIndex]["c_rchg_id"];
    var refund_flow_id = rows[tb_RefundListRowIndex]["refund_flow_id"];

    var queryData = {
        accountNo: accountNo,
        accountType: accountType,
        beginDate: beginDate,
        endDate: endDate,
        refund_flow_id: refund_flow_id
        //c_rchg_id: c_rchg_id
    };
    var divWidth = $("#div_RefundDetail").width();
    var divHeight = $("#div_RefundDetail").height();

    $('#tab_RefundQuXiang').datagrid({
        title: "退款去向",
        width: divWidth * 100 / 100,
        height: divHeight * 80 / 100,
        toolbar: "toolbar",
        delay: 1000,
        mode: 'remote',
        idField: 'refund_flow_id',
        textField: 'refund_flow_id',
        loadMsg: "数据加载中，请稍后...",
        pagination: true,
        pageSize: '10',
        pageList: [10, 20, 30, 50],
        sortName: 'fr_trans_id',
        sortOrder: ' asc ',
        showFooter: true,
        rownumbers: true,
        singleSelect: true,
        fitColumns: true,
        url: "/CreditPay/DetailSearch.aspx?getAction=LoadRefundQuXiangInfo&Rand=" + Math.random() + "",
        queryParams: queryData,  //异步查询的参数

        columns: [[
                { field: 'refund_amount', title: '金额', halign: 'center', align: 'right', width: $(this).width() },
                { field: 'balance_go', title: '资金去向', halign: 'center', align: 'center', width: $(this).width() },
                 { field: 'rf_trans_id', title: '资金去向的交易订单', halign: 'center', align: 'center', width: $(this).width() }

        ]], onLoadSuccess: function (data) {

        },
        onLoadError: function () {

        }
    });
}

//处理返回结果，并显示数据表格  
function ShowRefundListGrid(data) {
    var options = {
        width: $("#div_RefundList").width(),
        height: $(document).height() * 98 / 100,
        rownumbers: true,
        //idField: '产品编号',
        //showFooter: true,
        pagination: true,
        //toolbar: '#toolbar',
        singleSelect: true
    };

    //options.frozenColumns = eval(data.frozenColumns);
    options.columns = eval(data.columns);//把返回的数组字符串转为对象，并赋于datagrid的column属性  

    var dataGrid = $("#tb_RefundList");
    dataGrid.datagrid(options);//根据配置选项，生成datagrid      
    if (options.columns != undefined) {
        dataGrid.datagrid("loadData", data.data[0].rows); //载入本地json格式的数据  
        var p = dataGrid.datagrid('getPager');
        $(p).pagination({
            total: data.data[0].total,
            pageNumber: data.data[0].page,
            pageSize: $('#hid_RefundListPageSize').val(),//每页显示的记录条数，默认为10  
            pageList: [20, 50, 100, 200, 500, 1000, 5000, 10000],//可以设置每页记录条数的列表  
            beforePageText: '第',//页数文本框前显示的汉字  
            afterPageText: '页    共 {pages} 页',
            displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录'
           , onSelectPage: function (pageNumber, pageSize) {
               $(this).pagination('loading');
               $('#hid_RefundListPageNumber').val(Number(pageNumber));
               $('#hid_RefundListPageSize').val(Number(pageSize));
               $(this).pagination('loaded');
               LoadRefundList();
           }
        });
        //dataGrid.datagrid('clearSelections');
        //dataGrid.datagrid('hideColumn', 'userid');
    }
}
