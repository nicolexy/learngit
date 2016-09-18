///加载人工历史组合名称
///controlId 控件ID
///isShowSelect 是否显示请选择
function LoadBrandAndProductTypeGroupForWorkingDaiesCombogrid(controlId, isMultiple, isEditable, type, value,removeSelfe) {
    var postData = {
        isShowSelect: false,
        showSelectText: "",
        type: type,
        removeSelfe:removeSelfe,
        controlType: "combogrid"
    };
    var columns = "";
    if (isMultiple == true) {
        columns = [[
            { field: 'id', hidden: true },
            { checkbox: isMultiple, width: 50, align: 'center' },
            { field: 'name', title: '名称', width: 150 },
        { field: 'remark', title: '备注', width: 150 },
        ]];
    }
    else {
        columns = [[
            { field: 'id', hidden: true },
                        { field: 'name', title: '名称', width: 150 },
        { field: 'remark', title: '备注', width: 150 },
        ]];
    }
    $("#" + controlId + "").combogrid({
        width: 100,
        panelWidth: 400,
        delay: 1000,
        mode: 'remote',
        idField: 'id',
        textField: 'name',
        rownumbers: true,
        multiple: isMultiple,
        singleSelect: isMultiple == true ? false : true,
        fitColumns: true,
        editable: isEditable,
        method: "POST",
        url: '/Ajax/CommonAjax/LoadControlsDataSource.ashx?getAction=LoadBrandAndProductTypeGroupForWorkingDaiesCombogrid',
        queryParams: postData,
        columns: columns,
        onSelect: function (rowIndex, rowData) {

        },
        onLoadSuccess: function () {
            if ((value != null && value != "undefined") && (value.toString().length > 0 || value > 0)) {
                if (value * 1 >= 1) {

                    $(this).combogrid("setValue", value);
                }
                else {
                    var array = [];
                    var aa = value.split(',');
                    $.each(aa, function (i, val) {
                        array.push(val);
                    });

                    $(this).combogrid("setValues", array);
                }
            }

        }
    });
}


///加载公共下拉框(需要自定义的下拉框可统一调用该方法，自定义的数据源在后台自己添加)
///controlId 控件ID
///valueField 
///textField ID
///width 
///panelWidth 
///isShowSelect 
///showSelectText 
///isMultiple 
///isEditable 
///type 
///value 
function LoadCommonCombobox(controlId, valueField, textField, width, panelWidth, isShowSelect, showSelectText, isMultiple, isEditable, type, value) {    
    //var postDatas = {
    //    isShowSelect: isShowSelect,
    //    showSelectText: showSelectText,
    //    type: type,        
    //};
    $("#" + controlId + "").combobox({
        delay: 1000,
        mode: 'remote',
        valueField: valueField,
        textField: textField,
        width: width,
        panelWidth: panelWidth,
        multiple: isMultiple,
        editable: isEditable,
        method: "get",        
        url: '/Ajax/CommonAjax/LoadControlsDataSource.ashx?getAction=LoadCommonCombobox&isShowSelect=' + isShowSelect + '&showSelectText=' + escape(showSelectText) + '&type=' + type + '',
        //queryParams: postDatas,        
        onLoadSuccess: function () {
            if ((value != null && value != "undefined") && (value.toString().length > 0 && value > 0)) {
                $(this).combobox("setValue", value);
            }
            else {
                var datas = $("#" + controlId + "").combobox("getData");
                $("#" + controlId + "").combobox("setValue", datas[0].id);
            }
        }
    });
}

///加载海康价保信息
///controlId 控件ID
function LoadPriceProtectionCombogrid(controlId, isMultiple, isEditable, value) {
    var postDatas = {
        isShowSelect: false,
        showSelectText: '',
        controlType: "combogrid",
        guid: Math.random()
    };
    var columns = "";
    if (isMultiple == true) {
        columns = [[
             { field: 'id', hidden: true },
            { checkbox: true, width: 50 },
            { field: 'name', title: '价保标签', width: 100 }
        ]];
    }
    else {
        columns = [[
             { field: 'id', hidden: true },
            { field: 'name', title: '价保标签', width: 100 }
        ]];
    }
    $("#" + controlId + "").combogrid({
        width: 100,
        panelWidth: 200,
        delay: 1000,
        mode: 'remote',
        idField: 'id',
        textField: 'name',
        rownumbers: true,
        multiple: isMultiple,
        singleSelect: isMultiple == true ? false : true,
        fitColumns: true,
        editable: isEditable,
        url: '/Ajax/CommonAjax/LoadControlsDataSource.ashx?getAction=LoadPriceProtection',
        queryParams: postDatas,
        columns: columns,
        onSelect: function (rowIndex, rowData) {

        },
        onLoadSuccess: function () {
            if ((value != null && value != "undefined") && (value.toString().length > 0 || value > 0)) {
                if (value * 1 >= 1) {

                    $(this).combogrid("setValue", value);
                }
                else {
                    var array = [];
                    var aa = value.split(',');
                    $.each(aa, function (i, val) {
                        array.push(val);
                    });

                    $(this).combogrid("setValues", array);
                }
            }

        }
    });
}
///加载EasyUI控件选择的值
///controlId 控件ID
///controlType 控件显示类型 
function LoadEasyUIControlsSelectedValues(controlId, controlType)
{
    var selectedValues = "";
    if (controlType == "combogrid")
    {
        var selectedRows = $("#" + controlId + "").combogrid("grid").datagrid('getSelections');
        $.each(selectedRows, function (i, val) {
            if (selectedValues != "") {
                selectedValues = selectedValues + "," + selectedRows[i].id;
                //selectedSales = selectedCustomerIndustry + "," + productBrandSelectedRows[i].name;
            } else {
                selectedValues = selectedRows[i].id;
                //selectedSales = productBrandSelectedRows[i].name;
            }
        })
    }
    else if (controlType == "combotree")
    {
       var selectedRows = $("#" + controlId + "").combotree('tree').tree('getChecked');
       $.each(selectedRows, function (i, val) {
           if (selectedValues != "") {
               selectedValues = selectedValues + "," + selectedRows[i].id;
               //selectedSales = selectedCustomerIndustry + "," + productBrandSelectedRows[i].name;
           } else {
               selectedValues = selectedRows[i].id;
               //selectedSales = productBrandSelectedRows[i].name;
           }
       })   
    }
    else if (controlType == "datagrid") {
        var selectedRows = $("#" + controlId + "").datagrid('getSelections');
        $.each(selectedRows, function (i, val) {
            if (selectedValues != "") {
                selectedValues = selectedValues + "," + selectedRows[i].id;
                //selectedSales = selectedCustomerIndustry + "," + productBrandSelectedRows[i].name;
            } else {
                selectedValues = selectedRows[i].id;
                //selectedSales = productBrandSelectedRows[i].name;
            }
        })
    }
    return selectedValues;
}

///EasyUI控件赋值
///controlId 控件ID
///controlType 控件显示类型 
function SetEasyUIControlsValues(controlId, controlType,values) {
    
    if (controlType == "combogrid") {
        if (values.toString().length > 0 || values * 1 > 0) {
            if (values * 1 >= 1) {
                $("#" + controlId + "").combogrid("setValue", values);
            }
            else {
                var array = [];
                var aa = values.split(',');
                $.each(aa, function (i, val) {
                    array.push(val);
                });

                $("#" + controlId + "").combogrid("setValues", array);
            }
        }
        else if (values * 1 == 0) {
            $("#" + controlId + "").combogrid("setText", ' ');
        }
    }
    else if (controlType == "combobox") {
        if (values.toString().length > 0 || values * 1 > 0) {
            if (values * 1 >= 1) {
                $("#" + controlId + "").combobox("setValue", values);
            }           
        }
        else if (values * 1 == 0) {
            $("#" + controlId + "").combobox("setText", ' ');
        }
        
    }
    else if (controlType == "combotree") {
       
    }
}

//-------------------------------------其他----------------------------------------
