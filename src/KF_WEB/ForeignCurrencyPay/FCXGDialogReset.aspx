<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCXGDialogReset.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCXGDialogReset" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title runat="server" id="pageTitle">重置密码</title>
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
            <table class="tab_input" cellpadding="1" cellspacing="1">
                <tr>
                    <td width="30%">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" />
                        <span class="style3" runat="server" id="lb_title">重置密码</span>
                    </td>
                    <td>
                        <label class="style3">操作员ID：</label>
                        <asp:Label ID="lb_operatorID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>用户账号：</td>
                    <td>
                        <asp:Label runat="server" ID="lb_uin" />
                    </td>
                </tr>
                <tr>
                    <td>用户姓名：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_userName" Height="26px" Width="223px" />
                    </td>
                </tr>
                <tr>
                    <td>联系方式：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_contact" Height="26px" Width="223px" />
                    </td>
                </tr>
                <tr>
                    <td><span runat="server" id="lb_reason">重置原因：</span></td>
                    <td>
                        <asp:TextBox ID="txt_reason" runat="server" Height="154px" Width="339px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="Button1" runat="server" Text="重置密码" OnClick="Button1_Click" OnClientClick="return confirm('确定'+(this.value)+'?')" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
