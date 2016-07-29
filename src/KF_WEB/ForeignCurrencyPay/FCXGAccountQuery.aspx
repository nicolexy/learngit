<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCXGAccountQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCXGAccountQuery" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>外币账户查询</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );

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
            width: 900px;
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
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <div class="container">
            <table class="tab_input" cellpadding="1" cellspacing="1">
                <tr>
                    <td width="50%">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" />
                        <span class="style3">外币账户查询</span>
                    </td>
                    <td>
                        <label class="style3">操作员ID：</label>
                        <asp:Label ID="lb_operatorID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span>输入：</span>
                        <asp:TextBox ID="txt_input_id" runat="server" Width="250"></asp:TextBox>
                        <asp:RadioButton ID="checkWeChatId" runat="server" GroupName="chekedType" Text="WeChat ID" Checked="true" />
                        <asp:RadioButton ID="checkUin" runat="server" GroupName="chekedType" Text="钱包账户" />
                        <asp:RadioButton ID="checkUid" runat="server" GroupName="chekedType" Text="内部 ID" />
                        <asp:Button ID="Button1" runat="server" Text="查询" Style="margin-left: 100px;" OnClick="btn_Query_Click" />
                    </td>
                </tr>
            </table>
            <div id="info_box" runat="server" visible="false">
                <table class="tab_info" cellpadding="1" cellspacing="1">
                    <caption>
                        账户基本信息
                    </caption>
                    <tr>
                        <th>WeChat ID：</th>
                        <td>
                            <asp:Label runat="server" ID="lb_WeChatID" /></td>
                        <th>钱包账户：</th>
                        <td>
                            <asp:Label runat="server" ID="lb_uin" /></td>
                    </tr>
                    <tr>
                        <th>内部ID：</th>
                        <td>
                            <asp:Label runat="server" ID="lb_uid" /></td>
                        <th>绑定手机：</th>
                        <td>
                            <asp:Label runat="server" ID="lb_mobile" /></td>
                    </tr>
                    <tr>
                        <th>总账户余额：</th>
                        <td>
                            <asp:Label runat="server" ID="lbl_total" /></td>
                        <th>冻结余额：</th>
                        <td>
                            <asp:Label runat="server" ID="lbl_freeze" /></td>
                    </tr>
                    <tr>
                        <th>可用余额：</th>
                        <td>
                            <asp:Label runat="server" ID="lbl_balance" /></td>
                        <th>注册时间：</th>
                        <td>
                            <asp:Label runat="server" ID="lb_create_time" /></td>
                    </tr>
                    <tr>
                        <th>用户类型：</th>
                        <td>
                            <asp:Label runat="server" ID="lb_user_type_str" /></td>
                        <th>注册状态：</th>
                        <td>
                            <asp:Label runat="server" ID="lb_state" /></td>
                    </tr>
                    <tr>
                        <th>账户状态：</th>
                        <td>
                            <asp:Label runat="server" ID="lb_lstate_str" />
                            <span style="margin: 0 10px 0 20px;">
                                <a id="btn_freeze" runat="server" target="_blank">冻结/解冻</a>
                            </span>
                        </td>
                        <th>密码操作：</th>
                        <td>
                            <asp:LinkButton ID="btn_pwd_reset" Visible="false" runat="server" OnClick="btn_pwd_reset_Click"></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="btn_reset_log" runat="server" OnClick="QueryLog_Click">重置记录查询</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:DataGrid ID="dg_passwordLog" runat="server" AutoGenerateColumns="False" CssClass="tab_dg" Caption="快速重置密码记录查询">
                <HeaderStyle Font-Bold="True" Height="25px" />
                <Columns>
                    <asp:BoundColumn HeaderText="用户姓名" DataField="Ftrue_name" />
                    <asp:BoundColumn HeaderText="联系方式" DataField="Fphone" />
                    <asp:BoundColumn HeaderText="重置原因" DataField="Freason" />
                    <asp:BoundColumn HeaderText="操作员" DataField="Fop_name" />
                    <asp:BoundColumn HeaderText="操作时间" DataField="Fcreate_time" />
                </Columns>
            </asp:DataGrid>
            <webdiyer:AspNetPager ID="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" PageSize="10" ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" OnPageChanged="pager_PageChanged"></webdiyer:AspNetPager>
        </div>
    </form>
</body>
</html>
