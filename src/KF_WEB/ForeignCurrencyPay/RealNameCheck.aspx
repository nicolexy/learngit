<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RealNameCheck.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.RealNameCheck" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>实名审核查询</title>
    <style type="text/css">
        @import url( ../STYLES/common.css );
            @import url( ../STYLES/ossstyle.css );
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
    <script language="javascript" type="text/javascript" src="../scripts/jquery-1.11.3.min.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#DropDownList2").change(function () {
                $("#txt_memo").val($("#DropDownList2").find("option:selected").text());
            });
        });
    </script>
</head>
<body>
    <form id="formMain" runat="server">
        <div class="container">
            <div class="style_title">
                实名审核查询
            </div>
            <table cellspacing="1" cellpadding="1">
                <caption>
                    查询条件
                </caption>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="txt_input_id" runat="server" Width="250"></asp:TextBox>
                        <asp:RadioButton ID="checkWeChatId" runat="server" GroupName="chekedType" Text="WeChat ID" Checked="true" />
                        <asp:RadioButton ID="checkUin" runat="server" GroupName="chekedType" Text="钱包账户" />
                        <asp:RadioButton ID="checkUid" runat="server" GroupName="chekedType" Text="内部 ID" />

                    </td>

                </tr>
                <tr>
                    <td class="tb_query_title">认证方式：</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddl_type" Width="172px">
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="1">客服验证</asp:ListItem>
                            <asp:ListItem Value="2">机器验证</asp:ListItem>
                            <asp:ListItem Value="3">银行验证</asp:ListItem>

                        </asp:DropDownList>
                    </td>
                    <td class="tb_query_title">处理状态：</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddl_state" Width="172px">
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="1">待审核</asp:ListItem>
                            <asp:ListItem Value="2">审核通过 </asp:ListItem>
                            <asp:ListItem Value="3">客服审核不通过 </asp:ListItem>
                            <asp:ListItem Value="4">机器审核不通过</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tb_query_title">开始时间：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtstime" onclick="WdatePicker()" class="Wdate"></asp:TextBox>
                    </td>
                    <td class="tb_query_title">结束时间：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtetime" onclick="WdatePicker()" class="Wdate"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center">
                        <asp:Button ID="Button1" runat="server" Text="查 询" OnClick="btn_Query_Click" />
                    </td>

                </tr>
            </table>
            <br />
            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" Caption="查询结果"
                CssClass="datagrid" GridLines="None" CellSpacing="1" CellPadding="1" OnItemCommand="DataGrid1_ItemCommand">
                <HeaderStyle CssClass="th" />
                <Columns>
                    <asp:BoundColumn DataField="approval_id" HeaderText="审批单号" />
                    <asp:BoundColumn DataField="uin" HeaderText="用户账户" />
                    <asp:BoundColumn DataField="create_time" HeaderText="申请时间" />
                    <asp:BoundColumn DataField="modify_time" HeaderText="审核时间" />
                    <asp:BoundColumn DataField="operator" HeaderText="处理人" />
                    <asp:BoundColumn DataField="state_str" HeaderText="状态" />
                    <asp:ButtonColumn Text="查看" CommandName="detail" />
                </Columns>
            </asp:DataGrid>
            <webdiyer:AspNetPager ID="pager1" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left" Width="1100px"
                PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
                SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]">
            </webdiyer:AspNetPager>
            <asp:Panel runat="server" ID="panDetail" Visible="false">
                <table cellspacing="1" cellpadding="1">
                    <caption>
                        详情
                    </caption>
                    <tr>
                        <td class="tb_query_title" style="width: 15%">用户账户：</td>
                        <td style="width: 35%">
                            <asp:Label runat="server" ID="lbl_uin"></asp:Label></td>
                        <td class="tb_query_title" style="width: 15%">姓名：</td>
                        <td style="width: 35%">
                            <asp:Label runat="server" ID="lbl_name"></asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <td class="tb_query_title">生日：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_birthday"></asp:Label>
                        </td>
                        <td class="tb_query_title">国籍：</td>
                        <td>
                            <asp:Label ID="lbl_country" runat="server"> </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tb_query_title">证件类型：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_cre_type"></asp:Label>
                        </td>
                        <td class="tb_query_title">证件号码：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_cre_id"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tb_query_title">审核单号：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_approval_id"></asp:Label>
                        </td>
                        <td class="tb_query_title">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="tb_query_title">身份证扫描件：</td>
                        <td>
                            <asp:Image runat="server" ID="img_photo_path1" Width="380" Height="380" /></td>
                        <td colspan="2">
                            <asp:Image runat="server" ID="img_photo_path2" Width="380" Height="380" /></td>
                    </tr>
                    <tr>
                        <td class="tb_query_title">拒绝原因：</td>
                        <td colspan="3">
                            <asp:DropDownList runat="server" ID="ddlmemo" Height="25px" Width="400px">
                                <asp:ListItem Selected="True" Value=""></asp:ListItem>
                                <asp:ListItem Value="01">登记姓名和证件照片中内容不符</asp:ListItem>
                                <asp:ListItem Value="02">登记出生日期和证件照片中内容不符</asp:ListItem>
                                <asp:ListItem Value="03">登记证件类型和证件照片中内容不符</asp:ListItem>
                                <asp:ListItem Value="04">登记证件号码和证件照片中内容不符</asp:ListItem>
                                <asp:ListItem Value="05">证件照片模糊不清</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center">
                            <asp:Button ID="btn_Pass" runat="server" Style="margin-right: 100px" Text="通 过" OnClick="btn_Pass_Click" />
                            <asp:Button ID="btn_Refuse" runat="server" Style="margin-right: 100px" Text="拒 绝" OnClick="btn_Pass_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <br />
    </form>
</body>
</html>
