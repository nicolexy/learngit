<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RealNameInformationQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.RealNameInformationQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
    <title>实名信息查询</title>
    <style type="text/css">
        @import url( ../STYLES/common.css );
        @import url( ../STYLES/ossstyle.css );
         BODY {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
    </style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>

</head>
<body>
    <form id="formMain" runat="server">
        <div class="container">
            <div class="style_title">
                实名信息查询
            </div>
            <table cellspacing="1" cellpadding="1">
                <caption>
                    查询条件
                </caption>
                <tr>
                    <td>
                        <span>输入：</span>
                        <asp:TextBox ID="txt_input_id" runat="server" Width="250"></asp:TextBox>
                        <asp:RadioButton ID="checkWeChatId" runat="server" GroupName="chekedType" Text="WeChat ID" Checked="true" />
                        <asp:RadioButton ID="checkUin" runat="server" GroupName="chekedType" Text="钱包账户" />
                        <asp:RadioButton ID="checkUid" runat="server" GroupName="chekedType" Text="内部 ID" />
                        <asp:Button ID="Button1" runat="server" Text="查询" Style="margin-left: 100px;" OnClick="btn_Query_Click" />
                    </td>

                </tr>
            </table>
            <br />
            <asp:Panel runat="server" ID="panDetail" Visible="false">
                <table cellspacing="1" cellpadding="1">
                    <caption>
                        详情
                    </caption>
                    <tr>
                        <td class="tb_query_title">姓名：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_name"></asp:Label>
                        </td>
                        <td class="tb_query_title">国籍：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_country"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tb_query_title">生日：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_birthday"></asp:Label>
                        </td>
                        <td class="tb_query_title">联系手机：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_mobile"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tb_query_title">证件类型：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_cre_type"></asp:Label>
                        </td>
                        <td class="tb_query_title">实名制时间：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_modify_time"> </asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tb_query_title">证件号码：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_cre_id"></asp:Label>
                        </td>
                        <td class="tb_query_title">认证方式：</td>
                        <td>
                            <asp:Label runat="server" ID="lbl_type"> </asp:Label></td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <br />
    </form>
</body>
</html>
