<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCXGBindCardQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCXGBindCardQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>绑卡查询</title>
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
            margin: 20px;
            width: 1500px;
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
                        <span class="style3">绑卡查询</span>
                    </td>
                    <td>
                        <label class="style3">操作员ID：</label>
                        <asp:Label ID="lb_operatorID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">输入：
                            <asp:TextBox ID="txt_input" runat="server" Width="250" />
                        <asp:RadioButton ID="checkWeChatId" runat="server" GroupName="chekedType" Text="WeChat ID" />
                        <asp:RadioButton ID="checkUin" runat="server" GroupName="chekedType" Text="钱包账户" Checked="true" />
                        <asp:RadioButton ID="checkBankCard" runat="server" GroupName="chekedType" Text="银行卡号" Checked="true" />
                        <span style="margin-left:20px;"></span>
                        <asp:Button ID="Button1" runat="server" Text="绑卡查询" OnClick="Button1_Click" />
                    </td>
                </tr>
            </table>
            <asp:DataGrid ID="dg_passwordLog" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="绑卡详情" OnItemDataBound="dg_passwordLog_ItemDataBound">
                <HeaderStyle Font-Bold="True" Height="25px" />
                <Columns>
                    <asp:BoundColumn HeaderText="绑定时间" DataField="bind_time" />
                    <asp:BoundColumn HeaderText="绑定状态" DataField="bind_status_str" />
                    <asp:BoundColumn HeaderText="绑定账号" DataField="uin" />
                    <%--<asp:BoundColumn HeaderText="绑卡序列号" DataField="bind_serialno" />--%>
                    <asp:BoundColumn HeaderText="卡标" DataField="card_type_str" />
                    <asp:BoundColumn HeaderText="卡号后四位" DataField="card_tail" />
                    <asp:BoundColumn HeaderText="持卡人姓名" DataField="bill_user_name" />
                    <asp:BoundColumn HeaderText="电话" DataField="mobile" />
                    <asp:BoundColumn HeaderText="邮箱" DataField="bill_email" />
                    <asp:BoundColumn HeaderText="国家/区域" DataField="bill_area" />
                    <asp:BoundColumn HeaderText="城市" DataField="bill_address" />
                    <asp:BoundColumn HeaderText="街道" DataField="bill_address" />
                    <asp:HyperLinkColumn HeaderText="操作" Target="_blank" Text="解绑" DataNavigateUrlField="UnbundlingUrl" />
                    <asp:HyperLinkColumn HeaderText="解绑详细" Target="_blank" Text="查看" DataNavigateUrlField="UnbundInfoUrl" />
                </Columns>
            </asp:DataGrid>
        </div>
    </form>
</body>
</html>
