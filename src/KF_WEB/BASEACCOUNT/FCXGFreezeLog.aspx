<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCXGFreezeLog.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.FCXGFreezeLog" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>订单查询</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http：//schemas.microsoft.com/intellisense/ie5">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );

        .style2 {
            COLOR: #000000;
        }

        .style3 {
            COLOR: #ff0000;
        }

        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
        }

        .container {
            margin: 20px;
            width: 1200px;
        }

            .container table {
                width: 100%;
                background-color: #808080;
            }

            .container caption {
                text-align: left;
                background: #b0c3d1;
                padding: 4px;
            }

            .container td, .container th {
                background: #fff;
                height: 20px;
                padding: 2px 4px;
            }

            .container .tab_info th {
                font-weight: lighter;
                width: 120px;
                text-align: right;
            }

        .tab_dg tr:first-child td {
            background-color: #4A3C8C;
            font-weight: bold;
            line-height: 20px;
            color: white;
            text-align: center;
        }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <div class="container">
            <table class="tab_input" cellpadding="1" cellspacing="1">
                <tr>
                    <td width="50%">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" />
                        <span class="style3">香港钱包冻结记录查询</span>
                    </td>
                    <td>
                        <label class="style3">操作员ID：</label>
                        <asp:Label ID="lb_operatorID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>开始日期：
                     <input type="text" runat="server" id="txt_start_date" onclick="WdatePicker()" />
                        <img onclick="txt_start_date.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" alt="选择日期" /></td>
                    <td>结束日期：
                     <input type="text" runat="server" id="txt_end_date" onclick="WdatePicker()" />
                        <img onclick="txt_end_date.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" alt="选择日期" /></td>
                </tr>
                <tr>
                    <td>钱包账号：
                    <asp:TextBox ID="txt_uin" runat="server"/></td>
                    <td>内部Id：
                    <asp:TextBox ID="txt_uid" runat="server" /></td>
                </tr>
                <tr>
                    <td>操作员：
                    <asp:TextBox ID="txt_op_name" runat="server" /></td>
                    <td>冻结渠道：
                        <asp:DropDownList ID="list_channel" runat="server">
                            <asp:ListItem Value="" Selected="True">所有</asp:ListItem>
                            <asp:ListItem Value="1">风控冻结</asp:ListItem>
                            <asp:ListItem Value="2">拍拍冻结</asp:ListItem>
                            <asp:ListItem Value="3">用户冻结</asp:ListItem>
                            <asp:ListItem Value="4">商户冻结</asp:ListItem>
                            <asp:ListItem Value="5">BG接口冻结</asp:ListItem>
                            <asp:ListItem Value="6">涉嫌可疑交易冻结</asp:ListItem>
                            <asp:ListItem Value="7">ivr自助冻结</asp:ListItem>
                            <asp:ListItem Value="8">公众号自助冻结</asp:ListItem>
                            <asp:ListItem Value="9">微信安全</asp:ListItem>
                        </asp:DropDownList>
                        <span style="margin-left: 20px;">
                            <asp:Button ID="btn_query" runat="server" Text="查询" OnClick="btn_query_Click" /></span></span>
                    </td>
                </tr>
            </table>
            <br />
            <asp:DataGrid ID="dg_freezeLog" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="冻结记录查询">
                <HeaderStyle Font-Bold="True" Height="25px" />
                <Columns>
                    <asp:BoundColumn HeaderText="用户姓名" DataField="ftrue_name" />
                    <asp:BoundColumn HeaderText="联系方式" DataField="fphone" />
                    <asp:BoundColumn HeaderText="用户帐号" DataField="fuin" />
                    <asp:BoundColumn HeaderText="冻结渠道" DataField="channel_str" />
                    <asp:BoundColumn HeaderText="操作类型" DataField="lock_status_str" />
                    <asp:BoundColumn HeaderText="操作员" DataField="fop_name" />
                    <asp:BoundColumn HeaderText="操作时间" DataField="fcreate_time" />
                    <asp:BoundColumn HeaderText="原因" DataField="freason" ItemStyle-Width="300" />
                    <asp:BoundColumn HeaderText="备注" DataField="fmemo" />
                    <%--<asp:ButtonColumn CommandName="Select" HeaderText="详细" Text="查看详细"></asp:ButtonColumn>--%>
                </Columns>
            </asp:DataGrid>
            <webdiyer:AspNetPager ID="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" PageSize="10" ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" OnPageChanged="pager_PageChanged"></webdiyer:AspNetPager>
            <br />
        </div>
    </form>
</body>
</html>
