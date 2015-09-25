<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankClassifyManage.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.SysManage.BankClassifyManage" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>银行分类信息管理</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );
        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
        }
        .tb_s{margin:10px 0;}

        .select_input {background:url(../Images/Public/icon-down.gif) right 8px no-repeat;cursor:default;}
        .on_changes{width:122px; position:absolute;list-style:none; background:#FFF; border:1px solid #000; display:none; padding:2px;overflow-y:auto;height:300px;left:65px;top:20px;margin:0;}
        .on_changes li{padding:4px;}
        .on_changes li.active{ background:#CEE7FF;}
    </style>
    <script language="javascript" type="text/javascript" src="../scripts/jquery-1.11.3.min.js"></script>
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var indexLi = -1;
            var tempBankName = "";
            var valChange = function (flag) {
                var bankname = $.trim($('#txt_bankname').val());
                if (bankname != '') {
                    var reg = new RegExp("[\\u4E00-\\u9FFF]+", "g");
                    if (!reg.test(bankname))
                        return;
                    if (!flag && bankname == tempBankName)
                        return;
                }
                var formData = {};
                formData['bankname'] = bankname;
                $.ajax({
                    type: "POST",
                    contentType: "application/x-www-form-urlencoded; charset=utf-8",
                    url: "../data/Sys_BankClassify.ashx?action=getbank",
                    cache: false,
                    data: formData,
                    success: function (obj) {
                        var json = eval(obj);
                        var thtml = '';
                        for (var i = 0; i < json.length; i++) {
                            thtml += '<li code="' + json[i].bank_code + '">' + json[i].bank_name + '</li>';
                        }
                        $('#ul_bank').html(thtml);
                        indexLi = -1;
                        $('#ul_bank').show();
                        $('#ul_bank li').hover(function () {
                            $(this).addClass("active").siblings().removeClass();
                        });
                        if ($('#ul_bank li').length > 1) {
                            $('#txt_bankcode').val('');
                        }
                    },
                    error: function (obj) {
                    }
                });
            };
            
            
            $('#txt_bankname').bind("input propertychange", function () {
                valChange();
            });

            $(document).keydown(function (event) {
                if (event.which == 13) {
                    return false;
                }
            });

            var liHeight = 23;
            $('#txt_bankname').keydown(function (event) {
                if (event.which == 38) {//向上
                    if (indexLi == 0) {
                        indexLi = $('#ul_bank li').length - 1;
                        $('#ul_bank').scrollTop(liHeight * indexLi);
                    } else {
                        indexLi--;
                        if ($('#ul_bank li').eq(indexLi).position().top < 2)
                            $('#ul_bank').scrollTop(liHeight * indexLi);
                    }
                    $('#ul_bank li').eq(indexLi).addClass("active").siblings().removeClass();
                } else if (event.which == 40) {//向下
                    if (indexLi == $('#ul_bank li').length - 1) {
                        indexLi = 0;
                        $('#ul_bank').scrollTop(liHeight * indexLi);
                    } else {
                        indexLi++;
                        if ($('#ul_bank li').eq(indexLi).position().top > 278)
                            $('#ul_bank').scrollTop(liHeight * indexLi);
                    }
                    $('#ul_bank li').eq(indexLi).addClass("active").siblings().removeClass();
                } else if (event.which == 13) { //回车
                    var liVal = $('#ul_bank li').eq(indexLi).text();
                    var code = $('#ul_bank li').eq(indexLi).attr('code');
                    tempBankName = liVal;
                    $('#txt_bankname').val(liVal);
                    $('#txt_bankcode').val(code);
                    $('#ul_bank').hide();
                }
            }).click(function () {
                valChange(true);
            });

            $(document).click(function (event) {
                if ($(event.target).attr("class") == 'active') {
                    var liVal = $(event.target).text();
                    var code = $(event.target).attr('code');
                    tempBankName = liVal;
                    $('#txt_bankname').val(liVal);
                    $('#txt_bankcode').val(code);
                    $('#ul_bank').hide();
                } else {
                    $('#ul_bank').hide();
                }
            });
        });
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="form1" method="post" runat="server">
        <div style="margin-left:5%;">
            <table class="tb_s" cellspacing="1" cellpadding="1" width="820" border="1">
                    <tr>
                        <td style="WIDTH: 100%" bgcolor="#e4e5f7" colspan="3">
                            <div style="float:left;">
                                <img height="16" src="../IMAGES/Page/post.gif" width="20">
                                <span style="color:red;margin-left:20px;">银行分类信息管理</span>
                            </div>
                            <div style="float:right;">
                                <span>操作员代码: </span>
                                <asp:Label ID="lab_operator" runat="server" ForeColor="Red" Width="73px"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:DropDownList ID="ddl_bankname" Visible="false" runat="server"></asp:DropDownList>
                            <div style="display: inline-block;position:relative;">
                                <span>开户行名：</span>
                                <input type="text" class="select_input" id="txt_bankname" autocomplete="off" runat="server" />
                                <input type="hidden" id="txt_bankcode" runat="server" />
                                <ul class="on_changes" id="ul_bank">
                                </ul>
                            </div>
                        </td>
                        <td align="left">银行业务类型：
                            <asp:DropDownList ID="ddl_busi_type" runat="server"></asp:DropDownList>
                        </td>
                        <td align="left">状态：
                            <asp:DropDownList ID="ddl_use_status" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>批量导入文件：<asp:FileUpload ID="File1" runat="server" Width="255px" /></td>
                        <td>
                            <a href="/Template/Excel/BankClassifyTemplate.xls" target="_blank">下载模版</a>
                            <asp:Button ID="btn_input_excel" runat="server" Width="80px" Text="导入Excel" OnClick="btn_input_excel_Click"></asp:Button>

                        </td>
                        <td colspan="2" style="text-align:center;">
                            <asp:Button ID="btn_query" runat="server" Width="80px" Text="查 询" OnClick="btn_query_Click"></asp:Button>
                            <span style="margin:0 10px;"></span>
                            <asp:Button ID="btn_add" runat="server" Width="80px" Text="新 增" OnClick="btn_add_Click"></asp:Button>
                        </td>
                    </tr>
                </table>

                <table class="tb_s"  id="Table2" cellspacing="1" cellpadding="1" border="1" width="850px" runat="server">
                    <tr>
                        <td valign="top">
                            <asp:DataGrid ID="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                                BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" OnItemCommand="DataGrid1_ItemCommand" OnItemDataBound="DataGrid1_ItemDataBound">
                                <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                                <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                                <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                                <ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                                <HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="bank_code" HeaderText="银行字符编码" Visible="false"></asp:BoundColumn>

                                    <asp:BoundColumn DataField="bank_type" HeaderText="银行类型"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="banktype_str" HeaderText="中文银行类型"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="bank_name" HeaderText="开户行名"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="business_type" HeaderText="银行业务类型"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="card_type" HeaderText="卡类型"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="use_status" HeaderText="状态"></asp:BoundColumn>

                                    <asp:BoundColumn DataField="business_type" HeaderText="银行业务类型" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="card_type" HeaderText="卡类型" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="use_status" HeaderText="状态" Visible="false"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="操作">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbChange" runat="server" CommandName="CHANGE">修改</asp:LinkButton>
                                            <asp:LinkButton ID="lbDel" runat="server" CommandName="DEL">删除</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid></td>
                    </tr>
                    <tr height="25">
                        <td>
                            <webdiyer:AspNetPager ID="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
                                PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"
                                SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]" OnPageChanged="ChangePage" PageSize="10">
                            </webdiyer:AspNetPager>
                        </td>
                    </tr>
                </table>

                <table class="tb_s"  id="table_action" visible="false" cellspacing="1" cellpadding="1" width="820" border="1" runat="server">
                <tr>
                    <td style="WIDTH: 100%" bgcolor="#e4e5f7" colspan="2">
                        <div style="float:left;">
                            <img height="16" src="../IMAGES/Page/post.gif" width="20">
                            <asp:Label ID="lab_action_title" runat="server" ForeColor="Red" Text="银行分类信息新增"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right">银行类型</td>
                    <td><asp:TextBox ID="tbx_au_bank_type" Width="300" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right">银行业务类型</td>
                    <td><asp:DropDownList ID="ddl_au_busi_type" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td align="right">开户行名</td>
                    <td><asp:DropDownList ID="ddl_au_bankname" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td align="right">卡类型</td>
                    <td><asp:DropDownList ID="ddl_au_card_type" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td align="right">状态</td>
                    <td><asp:DropDownList ID="ddl_au_use_status" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:right;">
                        <asp:Button ID="btn_submit" runat="server" Width="80px" Text="提 交" OnClick="btn_submit_Click"></asp:Button>
                        <span style="margin:0 10px;"></span>
                        <asp:Button ID="btn_back" runat="server" Width="80px" Text="返 回" OnClick="btn_back_Click"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
