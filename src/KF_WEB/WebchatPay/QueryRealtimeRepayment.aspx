<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryRealtimeRepayment.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.QueryRealtimeRepayment" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>实时还款查询</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );

        UNKNOWN {
            COLOR: #000000;
        }

        .style3 {
            COLOR: #ff0000;
        }

        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
        }
    </style>
    <script type="text/javascript" src="../SCRIPTS/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="../js/contrastBankErrorCode.js"></script>
    <script type="text/javascript">
        $(function () {
            var bank_type = $("#lab_Fbank_type").text();
            var ret_code = $("#lab_Fret_code").text();
            if (bank_type.length > 0 || ret_code.length > 0) {
                $("#lab_Fbank_type").text(bankErrorCode.getBankName(bank_type));
                $("#lab_Fret_code_str").text(bankErrorCode.getBankErrorCode(bank_type, ret_code));
            }
        })
    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <table id="table1" border="1" cellspacing="1" cellpadding="1" width="900" runat="server" style="margin: 10px;">
            <tr>
                <td width="60%">
                    <img src="../IMAGES/Page/post.gif" width="20" height="16" alt="" /><label class="style3">实时还款查询</label></td>
                <td>
                    <label class="style3">操作员ID：</label><asp:Label ID="lb_operatorID" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="1">
                    <span>还款提现单号：</span>
                    <asp:TextBox ID="txt_listid" Width="200px" runat="server"></asp:TextBox>
                    <span style="margin: 0 10px;"></span>
                    <span>还款日：</span>
                    <input type="text" runat="server" id="txt_query_date" onclick="WdatePicker()" />
                    <img onclick="txt_query_date.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" alt="选择日期" />
                </td>
                <td>
                    <asp:Button ID="btn_Query" runat="server" Text="查询" OnClick="btn_Query_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <p style="line-height:2em">功能说明：支持微信、手Q、主站的发起的兴业、宁波、民生、平安、中信、光大、浦发、农业银行的实时还款结果查询</p>
                </td>
            </tr>
            <tr align="center">
                <td colspan="2">
                    <table border="1" cellspacing="1" cellpadding="1" width="100%">
                        <caption style="font-weight: bold; text-align: left; line-height: 2em;">还款详情</caption>
                        <tr>
                            <td style="width: 20%">还款提现单号：</td>
                            <td>
                                <asp:Label runat="server" ID="lab_Flistid" />
                            </td>
                            <td style="width: 20%">创建时间：</td>
                            <td>
                                <asp:Label runat="server" ID="lab_Fcreate_time" />
                            </td>
                        </tr>
                        <tr>
                            <td>银行交易单编号：</td>
                            <td>
                                <asp:Label runat="server" ID="lab_Fbank_billno" />
                            </td>
                            <td>银行类型：</td>
                            <td>
                                <asp:Label runat="server" ID="lab_Fbank_type" />
                            </td>
                        </tr>
                        <tr>
                            <td>业务状态：</td>
                            <td>
                                <asp:Label runat="server" ID="lab_Fstatus_str" />
                            </td>
                            <td>银行返回码：</td>
                            <td>
                                <asp:Label runat="server" ID="lab_Fret_code" />
                            </td>
                        </tr>
                        <tr>
                            <td>银行返回码含义：</td>
                            <td>
                                <asp:Label runat="server" ID="lab_Fret_code_str" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
