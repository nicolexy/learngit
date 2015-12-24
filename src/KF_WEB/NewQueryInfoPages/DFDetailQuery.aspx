<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DFDetailQuery.aspx.cs"
    Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.DFBatchQuery" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>QueryDKInfoPage</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="C#" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/ >
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );
        UNKNOWN
        {
            color: #000000;
        }
        .style3
        {
            color: #ff0000;
        }
        BODY
        {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
    </style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
    <script language="javascript">
        function enterPress(e) {
            if (window.event) {
                if (e.keyCode == 13)
                    document.getElementById('btn_serach').click();
            }
        }

        function onLoadFun() {
            document.getElementById('btn_serach').focus();
        }

        function select_deselectAll(chkVal, idVal) {
            var frm = document.forms[0];
            for (i = 0; i < frm.length; i++) {
                if (idVal.indexOf('CheckAll') != -1) {
                    if (frm.elements[i].id.indexOf('CheckBox') != -1) {
                        if (chkVal == true) {
                            frm.elements[i].checked = true;
                        }
                        else {
                            frm.elements[i].checked = false;
                        }
                    }
                }
                else if (idVal.indexOf('DeleteThis') != -1) {
                    if (frm.elements[i].checked == false) {
                        frm.elements[1].checked = false;
                    }
                }
            }
        }
    </script>
</head>
<body onload="onLoadFun()">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="1" cellpadding="1" width="1300" border="1">
        <tr>
            <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                <font face="宋体"><font color="red">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;代付信息查询</font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </font>操作员代码: <span class="style3">
                    <asp:Label ID="lb_operatorID" runat="server" Width="73px" ForeColor="Red"></asp:Label></span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <font face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font> <span>银行卡号：</span>
                <asp:TextBox ID="tbx_bankID" runat="server"></asp:TextBox><span>用户名：</span>
                <asp:TextBox ID="tbx_userName" runat="server"></asp:TextBox><span>查询时间段：</span>
                <input type="text" runat="server" id="tbx_beginDate" onclick="WdatePicker()" />
                <img onclick="tbx_beginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                <span>到：</span>
                <input type="text" runat="server" id="tbx_endDate" onclick="WdatePicker()" />
                <img onclick="tbx_endDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <font face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font> <span>商户号：</span>
                <asp:TextBox ID="tbx_spid" runat="server"></asp:TextBox><span>单笔序列号：</span>
                <asp:TextBox ID="tbx_spListID" runat="server"></asp:TextBox><span>商户批次号：</span>
                <asp:TextBox ID="tbx_spBatchID" runat="server"></asp:TextBox><span>交易状态：</span>
                <asp:DropDownList ID="ddl_state" runat="server">
                    <asp:ListItem Selected="True" Value="0">全部</asp:ListItem>
                    <asp:ListItem Value="1">成功</asp:ListItem>
                    <asp:ListItem Value="2">失败</asp:ListItem>
                    <asp:ListItem Value="3">处理中</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <font face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font> <span>付款单号：</span>
                <asp:TextBox ID="txb_transaction_id" runat="server"></asp:TextBox>
                <span>银行渠道：</span>
               <%-- <asp:TextBox ID="txb_bank_type" runat="server"></asp:TextBox>--%>
                <asp:DropDownList id="ddlBankType" runat="server"></asp:DropDownList>

                <span>账户类型：</span>
                <asp:DropDownList ID="ddl_service_code" runat="server">
                    <asp:ListItem Selected="True" Value="0">全部</asp:ListItem>
                    <asp:ListItem Value="1">个人</asp:ListItem>
                    <asp:ListItem Value="2">公司</asp:ListItem>
                </asp:DropDownList>
            </td>
            </tr>
            <tr>
            <td align="center" colspan="5">
                <asp:Button ID="btn_serach" Width="80px" runat="server" Text="查询" OnClick="btn_serach_Click">
                </asp:Button>
            </td>
        </tr>
    </table>
    <table cellspacing="0" cellpadding="0" width="1300" border="0">
        <tr>
            <td valign="top">
                <asp:DataGrid ID="DataGrid_QueryResult" runat="server" Width="1300px" BorderColor="#E7E7FF"
                    BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal"
                    AutoGenerateColumns="False" PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" >
                    <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C">
                    </HeaderStyle>
                    <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="Fauto_id" HeaderText="唯一ID">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:ButtonColumn Text="查询详情" HeaderText="操作" CommandName="detail"></asp:ButtonColumn>
                        <asp:BoundColumn DataField="Fubatch_id" HeaderText="批次号">
                            <HeaderStyle Width='100px'></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Ftransaction_id" HeaderText="付款单号">
                            <HeaderStyle Width='100px'></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Fcreate_time" HeaderText="提交时间"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FamountName" HeaderText="订单金额"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fuser_typeName" HeaderText="账户类型"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FstatusName" HeaderText="交易状态"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fbank_typeName" HeaderText="银行渠道"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fbank_id" HeaderText="银行卡号"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Frecv_true_name" HeaderText="用户名"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Fresult_info" HeaderText="失败原因"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid><webdiyer:AspNetPager ID="pager" runat="server" HorizontalAlign="right"
                    AlwaysShow="True" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到"
                    CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5">
                </webdiyer:AspNetPager>
            </td>
        </tr>
    </table>
    <div>
        <p>
            <label>
                交易成功笔数：</label>
            <asp:Label ID="lb_successNum" runat="server">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
            <label>
                交易成功总额：</label>
            <asp:Label ID="lb_successAllMoney" runat="server">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
            <label>
                交易失败笔数：</label>
            <asp:Label ID="lb_failNum" runat="server">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
            <label>
                交易失败总额：</label>
            <asp:Label ID="lb_failAllMoney" runat="server">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
            <label>
                处理中笔数：</label>
            <asp:Label ID="lb_handlingNum" runat="server">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
            <label>处理中总额：</label>
            <asp:Label ID="lb_handlingAllMoney" runat="server">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_outExcel" Width="80px" runat="server" Text="导出结果" OnClick="btn_outExcel_Click" Visible="false"></asp:Button>
        </p>
    </div>
    <table cellspacing="1" cellpadding="0" width="1300" bgcolor="black" border="0">
        <tr>
            <td bgcolor="#eeeeee" colspan="4" height="18">
                <span>&nbsp;详细信息列表：</span>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 19px" width="229" bgcolor="#eeeeee" height="19">
                <font face="宋体">&nbsp;<font style="background-color: #eeeeee" face="宋体">商户号：</font></font>
            </td>
            <td style="height: 19px" width="225" bgcolor="#ffffff" height="19">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c1" runat="server"></asp:Label></font>
            </td>
            <td style="width: 225px; height: 20px" bgcolor="#eeeeee" height="20">
                <font face="宋体">&nbsp;订单金额:</font>
            </td>
            <td style="width: 136px; height: 20px" bgcolor="#ffffff" height="20">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c2" runat="server"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 20px" bgcolor="#eeeeee" height="20">
                <font face="宋体">&nbsp;批次号:</font>
            </td>
            <td style="height: 20px" bgcolor="#ffffff" height="20">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c3" runat="server"></asp:Label></font>
            </td>
            <td style="width: 225px; height: 16px" bgcolor="#eeeeee" height="16">
                &nbsp;&nbsp;付款说明:
            </td>
            <td style="width: 136px; height: 16px" bgcolor="#ffffff" height="16">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c4" runat="server"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 16px" bgcolor="#eeeeee" height="16">
                <font face="宋体">&nbsp;付款单号:</font>
            </td>
            <td style="height: 16px" bgcolor="#ffffff" height="16">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c5" runat="server"></asp:Label></font>
            </td>
            <td style="width: 225px; height: 19px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;最后修改时间:</font>
            </td>
            <td style="width: 136px; height: 19px" bgcolor="#ffffff" height="19">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c6" runat="server"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 19px" bgcolor="#eeeeee" height="19">
                <font face="宋体">&nbsp;交易单号:</font>
            </td>
            <td style="height: 19px" bgcolor="#ffffff" height="19">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c7" runat="server"></asp:Label></font>
            </td>
            <td style="width: 225px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;收款方手机号:</font>
            </td>
            <td style="width: 136px; height: 18px" bgcolor="#ffffff" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c8" runat="server"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;创建时间:</font>
            </td>
            <td style="height: 18px" bgcolor="#ffffff" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c9" runat="server"></asp:Label></font>
            </td>
            <td style="width: 229px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;流水号</font>
            </td>
            <td style="height: 18px" bgcolor="#ffffff" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c10" runat="server"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 16px" bgcolor="#eeeeee" height="16">
                <font face="宋体">&nbsp;付款状态:</font>
            </td>
            <td style="height: 16px" bgcolor="#ffffff" height="16">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c11" runat="server"></asp:Label></font>
            </td>
            <td style="width: 225px; height: 19px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;单笔序列号:</font>
            </td>
            <td style="width: 136px; height: 19px" bgcolor="#ffffff" height="19">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c12" runat="server"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 19px" bgcolor="#eeeeee" height="19">
                <font face="宋体">&nbsp;收款方姓名:</font>
            </td>
            <td style="height: 19px" bgcolor="#ffffff" height="19">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c13" runat="server"></asp:Label></font>
            </td>
            <td style="width: 225px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;最后错误信息:</font>
            </td>
            <td style="width: 136px; height: 18px" bgcolor="#ffffff" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c14" runat="server"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;发起方式（企业版/接口）:</font>
            </td>
            <td style="height: 18px" bgcolor="#ffffff" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c15" runat="server"></asp:Label></font>
            </td>
            <td style="width: 229px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;开户银行名称</font>
            </td>
            <td style="height: 18px" bgcolor="#ffffff" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c16" runat="server"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;开户行所在省份:</font>
            </td>
            <td style="height: 18px" bgcolor="#ffffff" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c17" runat="server"></asp:Label></font>
            </td>
            <td style="width: 229px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;操作员：</font>
            </td>
            <td style="height: 18px" bgcolor="#ffffff" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c18" runat="server"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;开户行所在城市:</font>
            </td>
            <td style="height: 18px" bgcolor="#ffffff" colspan="3" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c19" runat="server"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;退票时间：</font>
            </td>
            <td style="height: 18px" bgcolor="#ffffff" colspan="3" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c20" runat="server"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;支行名称:</font>
            </td>
            <td style="height: 18px" bgcolor="#ffffff" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c21" runat="server"></asp:Label></font>
            </td>
            <td style="width: 229px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;退票原因</font>
            </td>
            <td style="height: 18px" bgcolor="#ffffff" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c22" runat="server"></asp:Label></font>
            </td>
        </tr>
        <tr>
            <td style="width: 229px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;银行账号:</font>
            </td>
            <td style="height: 18px" bgcolor="#ffffff" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c23" runat="server"></asp:Label></font>
            </td>
            <td style="width: 229px; height: 18px" bgcolor="#eeeeee" height="18">
                <font face="宋体">&nbsp;</font>
            </td>
            <td style="height: 18px" bgcolor="#ffffff" height="18">
                <font face="宋体">&nbsp;
                    <asp:Label ID="lb_c24" runat="server"></asp:Label></font>
            </td>
        </tr>
        
    </table>
    </form>
</body>
</html>
