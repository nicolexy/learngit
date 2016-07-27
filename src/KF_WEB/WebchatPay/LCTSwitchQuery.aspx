<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LCTSwitchQuery.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.LCTSwitchQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title></title>
     <style type="text/css">
        @import url( ../STYLES/ossstyle.css );
         .style_h1 {
             font-family:"Microsoft YaHei";
             font-size: 18px;
             font-weight:bold;
             color:#646464;
         }
          .style_h2 {
             font-family:"Microsoft YaHei";
             font-size: 13px;
             font-weight:bold;
         }
           .style_h3 {
             font-family:"Microsoft YaHei";
             font-size: 13px;
             font-weight:bold;
             text-align:right;
             background-color:#e4e4e4;
         }
        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
             font-family:"Microsoft YaHei";
        }

        .container {
            margin: 10px;
            width: 1100px;
        }

            .container table {
                width: 100%;
                background-color: #CCCCCC;
                border-width:2px;
            }

            .container caption {
                padding-top:8px;
                padding-left:5px;
                vertical-align:middle;
                height:35px;
                text-align: left;
                background: #D7D7D7;
                color:#1462AD;
                font-size: 18px;
                font-weight:bold;
                font-family:"Microsoft YaHei";
                border:1px solid #CCCCCC;
                border-bottom-width:0px;
            }

            .container td, .container th {
                background: #fff;
                height: 30px;
                padding: 2px 4px;
            }

    </style>
</head>
<body>
    <form id="form1" runat="server">
         <div class="container">
    <table  class="tab_info" cellpadding="1" cellspacing="1">
         <caption>
                    理财通转投查询
                </caption>
        <tr>
            <td class="style_h1">
                  &nbsp;查询条件
            </td>
        </tr>
        <tr>
            <td class="style_h2">
                    <span>&nbsp;&nbsp;&nbsp;&nbsp;输 入：</span>
                    <asp:TextBox ID="txt_input_id" runat="server" Width="250" Height="18"></asp:TextBox>
                    <asp:RadioButton ID="WeChatId" runat="server" GroupName="UserType" Text="微信帐号" Checked="true" />
                    <asp:RadioButton ID="WeChatQQ" runat="server" GroupName="UserType" Text="微信绑定QQ" />
                    <asp:RadioButton ID="WeChatMobile" runat="server" GroupName="UserType" Text="微信绑定手机" />
                    <asp:RadioButton ID="WeChatEmail" runat="server" GroupName="UserType" Text="微信绑定邮箱" />
                    <asp:RadioButton ID="WeChatUid" runat="server" GroupName="UserType" Text="微信内部ID" />
                    <asp:RadioButton ID="WeChatCft" runat="server" GroupName="UserType" Text="微信财付通账号" />
            </td>
        </tr>
          <tr>
            <td class="style_h2">
                 <span>&nbsp;&nbsp;&nbsp;&nbsp;单 号：</span>
                        <asp:TextBox ID="txtListid" runat="server" Width="250" Height="18"></asp:TextBox>
                        <asp:RadioButton ID="Radio_redem_id" runat="server" GroupName="chekedType" Text="赎回单" Checked="true" />
                        <asp:RadioButton ID="Radio_buy_id" runat="server" GroupName="chekedType" Text="申购单" />
                        <asp:RadioButton ID="Radio_change_id" runat="server" GroupName="chekedType" Text="转换单" />
                     <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
            </td>
        </tr>
    </table>
            <br />
   <asp:Panel ID="tbdetail" runat="server" >
             <table   class="tab_info" cellpadding="1" cellspacing="1">
                 <caption style="color:#646464">
                    查询结果
                </caption>
                 <tr>
                     <td class="style_h3" style="width:40%; ">微信支付财付通账号：</td>
                     <td><asp:Label ID="lbluin"  runat="server"></asp:Label></td>
                 </tr>
                   <tr>
                     <td class="style_h3">份额转换单：</td>
                     <td><asp:Label ID="lblFchange_id"  runat="server"></asp:Label></td>
                 </tr>
                   <tr>
                     <td class="style_h3">申购单：</td>
                     <td><asp:Label ID="lblFbuy_id"  runat="server"></asp:Label></td>
                 </tr>
                   <tr>
                     <td class="style_h3">赎回单：</td>
                     <td><asp:Label ID="lblFredem_id"  runat="server"></asp:Label></td>
                 </tr>
                   <tr>
                     <td class="style_h3">转换金额：</td>
                     <td><asp:Label ID="lblFtotal_fee"  runat="server"></asp:Label></td>
                 </tr>
                   <tr>
                     <td class="style_h3">转出基金：</td>
                     <td><asp:Label ID="lblFori_fund_name"  runat="server"></asp:Label></td>
                 </tr>
                   <tr>
                     <td class="style_h3">转入基金：</td>
                     <td><asp:Label ID="lblFnew_fund_name"  runat="server"></asp:Label></td>
                 </tr>
                   <tr>
                     <td class="style_h3">状态：</td>
                     <td><asp:Label ID="lblFstateStr"  runat="server"></asp:Label></td>
                 </tr>
                   <tr>
                     <td class="style_h3">转换时间：</td>
                     <td><asp:Label ID="lblFacc_time"  runat="server"></asp:Label></td>
                 </tr>
             </table>
               </asp:Panel>  
            </div>
    </form>
</body>
</html>
