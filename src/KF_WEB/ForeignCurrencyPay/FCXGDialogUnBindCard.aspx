<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCXGDialogUnBindCard.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCXGDialogUnBindCard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title runat="server" id="pageTitle">解绑卡</title>
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
            width: 500px;
            margin: auto;
        }

            .container table {
                width: 100%;
                background-color: #808080;
            }

            .container caption {
                text-align: left;
                background: #b0c3d1;
                padding: 10px;
            }

            .container td, .container th {
                background: #fff;
                height: 20px;
                padding: 10px 4px;
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
            <table class="tab_input" cellpadding="1" cellspacing="1" id="tab_unbind" runat="server" visible="false">
                <tr>
                    <td width="30%">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" />
                        <span class="style3">解除绑定</span>
                    </td>
                    <td>
                        <label class="style3">操作员ID：</label>
                        <asp:Label ID="lb_operatorID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>用户账号：</td>
                    <td>
                        <asp:Label runat="server" ID="lb_userName" />
                    </td>
                </tr>
                <tr>
                    <td>卡号：</td>
                    <td>
                        <asp:Label runat="server" ID="lb_cardNo" />
                    </td>
                </tr>
                <tr>
                    <td>用户姓名：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_trueName" Height="26px" Width="223px" />
                    </td>
                </tr>
                <tr>
                    <td>联系方式：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_contact" Height="26px" Width="223px" />
                    </td>
                </tr>
                <tr>
                    <td><span runat="server" id="lb_reason">解除原因：</span></td>
                    <td>
                        <asp:TextBox ID="txt_reason" runat="server" Height="154px" Width="339px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="Button1" runat="server" Text="解除绑定" OnClick="Button1_Click" OnClientClick="return confirm('确定'+(this.value)+'?')" />
                    </td>
                </tr>
            </table>

            <table class="tab_input" cellpadding="1" cellspacing="1" id="tab_unbind_info" runat="server" visible="false">
                <tr>
                    <td width="30%">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" />
                        <span class="style3">解绑记录</span>
                    </td>
                    <td>
                        <label class="style3">操作员ID：</label>
                        <asp:Label ID="lab_info_uid" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>绑定账号：</td>
                    <td>
                        <asp:Label runat="server" ID="info_uin" />
                    </td>
                </tr>
                <tr>
                    <td>卡号：</td>
                    <td>
                        <asp:Label runat="server" ID="info_card" />
                    </td>
                </tr>
                <tr>
                    <td>解绑类型：</td>
                    <td>
                        <asp:Label runat="server" ID="info_unbind_type_str" />
                    </td>
                </tr>
                <tr>
                    <td>绑卡时间：</td>
                    <td>
                        <asp:Label runat="server" ID="info_bindTime" />
                    </td>
                </tr>
                <tr>
                    <td>解绑时间：</td>
                    <td>
                        <asp:Label runat="server" ID="info_unbindTime" />
                    </td>
                </tr>

                <tr>
                    <td>用户姓名：</td>
                    <td>
                        <asp:Label runat="server" ID="info_trueName" />
                    </td>
                </tr>
                <tr>
                    <td>联系方式：</td>
                    <td>
                        <asp:Label runat="server" ID="info_contact" />
                    </td>
                </tr>
                <tr>
                    <td><span runat="server" id="Span1">解除原因：</span></td>
                    <td>
                        <asp:Label runat="server" ID="info_reason" />
                    </td>
                </tr>
                <tr>
                    <td><span runat="server" id="Span2">操作员：</span></td>
                    <td>
                        <asp:Label runat="server" ID="info_opName" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="UnbindInfo_Close" runat="server" Text="关闭" OnClick="UnbindInfo_Close_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
