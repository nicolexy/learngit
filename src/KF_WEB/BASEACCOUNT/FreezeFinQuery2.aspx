<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FreezeFinQuery2.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.FreezeFinQuery2" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>冻结资金记录查询</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
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
            width: 900px;
        }

            .container > table {
                border-collapse: collapse;
            }

            .container caption {
                text-align: left;
                background: #b0c3d1;
                padding: 4px;
            }

            .container td, .container th {
                border: 1px solid grey;
                padding: 2px 4px;
            }

            .container .tab_info th {
                background: none;
                font-weight: initial;
                width: 120px;
                text-align: right;
            }

            .container .tab_input {
                width: 100%;
            }

            .container .tab_info {
                width: 100%;
            }

            .container .tab_dg {
                width: 100%;
            }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <div class="container">
            <table class="tab_input">
                <tr>
                    <td width="50%">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" />
                        <span class="style3">冻结资金记录查询</span>
                    </td>
                    <td>
                        <label class="style3">操作员ID：</label>
                        <asp:Label ID="lb_operatorID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span>财付通账号：</span>
                        <asp:TextBox ID="txt_uin" runat="server" Width="200"></asp:TextBox>
                        <span>冻结金额(元)：</span>
                        <asp:TextBox ID="txt_moeny" runat="server" Width="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span>开始日期：</span>
                        <input type="text" runat="server" id="txt_sDate" onclick="WdatePicker()" />
                        <img onclick="txt_sDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" alt="选择日期" />
                        <span>结束日期：</span>
                        <input type="text" runat="server" id="txt_eDate" onclick="WdatePicker()" />
                        <img onclick="txt_eDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" alt="选择日期" />
                        <asp:Button ID="Button1" runat="server" Text="查询" Style="margin-left: 50px;" OnClick="Button1_Click" />
                    </td>
                </tr>
            </table>
            <asp:DataGrid ID="dg_FundFlow" runat="server" AutoGenerateColumns="False" GridLines="Horizontal" CssClass="tab_dg" Caption="冻结资金记录" Width="100%" OnDataBinding="dg_passwordLog_DataBinding" OnSelectedIndexChanged="dg_FundFlow_SelectedIndexChanged">
                <HeaderStyle Font-Bold="true" BorderColor="Black" />
                <Columns>
                    <asp:BoundColumn HeaderText="财付通账号" DataField="Fqqid" />
                    <asp:BoundColumn HeaderText="冻结原因" DataField="Fsubject_str" />
                    <asp:BoundColumn HeaderText="冻结金额(元)" DataField="Fcon_str" />
                    <asp:BoundColumn HeaderText="冻结时间" DataField="Fcreate_time" />
                    <asp:BoundColumn HeaderText="冻结单号" DataField="Flistid" />
                    <asp:BoundColumn HeaderText="订单号/交易单号" DataField="Fapplyid" />
                    <asp:ButtonColumn CommandName="Select" HeaderText="详情" Text="查看"></asp:ButtonColumn>
                </Columns>
            </asp:DataGrid>
            <webdiyer:AspNetPager ID="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" PageSize="10" ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" OnPageChanged="pager_PageChanged" Visible="false"></webdiyer:AspNetPager>

            <table class="tab_info">
                <caption>
                    详细信息列表
                </caption>
                <tr>
                    <th>订单号/交易单号：</th>
                    <td colspan="3">
                        <label runat="server" id="lb_Fapplyid" />
                    </td>
                   <%-- <th>流水号：</th>
                    <td>
                        <label runat="server" id="lb_Fbkid" />
                    </td>--%>
                </tr>
                <tr>
                    <th>冻结单号：</th>
                    <td>
                        <label runat="server" id="lb_Flistid" />
                    </td>

                    <th>财付通账号</th>
                    <td>
                        <label runat="server" id="lb_Fqqid" />
                    </td>
                </tr>
                <tr>
                    <th>处理状态：</th>
                    <td>
                        <label>冻结</label>
                    </td>
                    <th>冻结时间：</th>
                    <td>
                        <label runat="server" id="lb_Fcreate_time" />
                    </td>
                </tr>
                <tr>
                    <th>冻结金额(元)：</th>
                    <td>
                        <label runat="server" id="lb_Fcon_str" />
                    </td>
                    <th>交易金额(元)：</th>
                    <td>
                        <label runat="server" id="lb_Fpaynum_str" />
                    </td>
                </tr>
                <tr>
                    <th>用户银行类型：</th>
                    <td>
                        <label runat="server" id="lb_Fbank_type" />
                    </td>
                    <th>最后修改时间：</th>
                    <td>
                        <label runat="server" id="lb_Fmodify_time" />
                    </td>
                </tr>
                <tr>
                    <th>备注：</th>
                    <td>
                        <label runat="server" id="lb_Fmemo" />
                    </td>
                    <th>冻结原因：</th>
                    <td>
                        <label runat="server" id="lb_Fsubject" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
