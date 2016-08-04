<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCXGSPQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCurrencyPay.FCXGSPQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>外币商户查询</title>
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
                        <span class="style3">外币商户查询</span>
                    </td>
                    <td>
                        <label class="style3">操作员ID：</label>
                        <asp:Label ID="lb_operatorID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span>商户编号：</span>
                        <asp:TextBox ID="txt_input_id" runat="server" Width="250"></asp:TextBox>
                        <%--             <asp:RadioButton ID="RadioButton1" runat="server" GroupName="chekedType" Text="商户编号" Checked="true" />
                        <asp:RadioButton ID="RadioButton2" runat="server" GroupName="chekedType" Text="公司名称" />--%>
                        <asp:Button ID="Button1" runat="server" Text="查询" Style="margin-left: 100px;" OnClick="Button1_Click" />
                    </td>
                </tr>
            </table>
            <table class="tab_info" cellpadding="1" cellspacing="1">
                <caption>
                    账户基本信息
                </caption>
                <tr>
                    <th>商户编号：</th>
                    <td>
                        <asp:Label runat="server" ID="lb_spid" /></td>
                    <th>公司名称：</th>
                    <td>
                        <asp:Label runat="server" ID="lb_company_name" /></td>
                </tr>
                <tr>
                    <th>行业类别：</th>
                    <td>
                        <asp:Label runat="server" ID="lb_mer_type" /></td>
                    <th>联系地址：</th>
                    <td>
                        <asp:Label runat="server" ID="lb_address" /></td>
                </tr>
                <tr>
                    <th>联系人姓名：</th>
                    <td>
                        <asp:Label runat="server" ID="lb_boss_name" /></td>
                    <th>联系人固话：</th>
                    <td>
                        <asp:Label runat="server" ID="lb_mobile" /></td>
                </tr>
                <tr>
                    <th>联系人电话：</th>
                    <td>
                        <asp:Label runat="server" ID="lb_phone" /></td>
                    <th>联系人邮箱：</th>
                    <td>
                        <asp:Label runat="server" ID="lb_email" /></td>
                </tr>
                <tr>
                    <th>国别：</th>
                    <td>
                        <asp:Label runat="server" ID="lb_country" /></td>
                    <th>地区/省级：</th>
                    <td>
                        <asp:Label runat="server" ID="lb_area" /></td>
                </tr>
                <tr>
                    <th>城市：</th>
                    <td>
                        <asp:Label runat="server" ID="lb_city" /></td>
                    <th>注册时间：</th>
                    <td>
                        <asp:Label runat="server" ID="lb_create_time" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
