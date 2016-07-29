<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LCTReserveOrder.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.WebchatPay.LCTReserveOrder" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
    <title>理财通预约买入</title>
    <style type="text/css">
        @import url( ../STYLES/common.css );
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
        BODY {
            background-image: url(../IMAGES/Page/bg01.gif);
        }
        caption {
            text-align: left;
            background: #b0c3d1;
            padding: 4px;
        }
        tr {
            height: 25px;
        }
    </style>   
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="formMain" runat="server">
        <table cellspacing="0" rules="all" border="1" style="width: 1200px; border-collapse: collapse;">
            <tr>
                <td style="width: 100%" bgcolor="#e4e5f7" colspan="2"><font color="red">
                    <img src="../images/page/post.gif" width="20" height="16">理财通预约买入</font>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:RadioButton ID="WeChatId" GroupName="USERTYPE" runat="server" Text="微信帐号" Checked="true" />
                    <asp:RadioButton ID="WeChatQQ" GroupName="USERTYPE" runat="server" Text="微信绑定QQ" />
                    <asp:RadioButton ID="WeChatMobile" GroupName="USERTYPE" runat="server" Text="微信绑定手机" />
                    <asp:RadioButton ID="WeChatEmail" GroupName="USERTYPE" runat="server" Text="微信绑定邮箱" />
                    <asp:RadioButton ID="WeChatUid" GroupName="USERTYPE" runat="server" Text="微信内部ID" />
                    <asp:RadioButton ID="WeChatCft" GroupName="USERTYPE" runat="server" Text="微信财付通账号Or手Q账号" />
                    &nbsp; &nbsp;
                     <asp:TextBox ID="txt_user" Width="200px" runat="server"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td style="width: 50%">
                    <label>开始时间:</label>
                    <asp:TextBox ID="txtStime" runat="server" class="Wdate" onclick="WdatePicker()"></asp:TextBox>
                    &nbsp;&nbsp;
                         <label>结束时间:</label>
                    <asp:TextBox ID="txtEtime" runat="server" class="Wdate" onclick="WdatePicker()"></asp:TextBox>
                </td>
                <td align="center">
                    <asp:Button ID="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click"></asp:Button></td>
            </tr>
        </table>
        <br />
        <asp:DataGrid Width="1200" ID="DataGrid1" runat="server" AutoGenerateColumns="False" Caption="理财通预约买入" OnItemCommand="DataGrid1_ItemCommand">
            <HeaderStyle Font-Bold="True" Height="25px" />
            <Columns>
                <asp:BoundColumn DataField="Freserve_time" HeaderText="预约成功时间" />
                <asp:BoundColumn DataField="Fstate" HeaderText="预约状态" />
                <asp:BoundColumn DataField="Flistid" HeaderText="预约单号" />
                <asp:BoundColumn DataField="Freserve_fund_name" HeaderText="预约基金" />
                <asp:BoundColumn DataField="Ffrom_fund_name" HeaderText="转出基金" />
                <asp:BoundColumn DataField="Ftotal_fee" HeaderText="转出金额" />
                <asp:BoundColumn DataField="Facc_time" HeaderText="转换/解冻时间" />
                <asp:BoundColumn DataField="Fexpect_date" HeaderText="预计转换日期" />
                <asp:BoundColumn DataField="Fconfirm_time" HeaderText="二次确认时间" />
                <asp:BoundColumn DataField="Ftransfer_date" HeaderText="实际转入日期" />
                <asp:BoundColumn DataField="Fcancel_reason" HeaderText="取消原因" />
                <asp:ButtonColumn Text="详情" CommandName="detail" />

            </Columns>
        </asp:DataGrid>
        <webdiyer:AspNetPager ID="pager1" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left" Width="1200px"
            PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage1"
            SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]">
        </webdiyer:AspNetPager>
        <asp:Panel runat="server" ID="panelDetail" Width="1200" Visible="false">
            <table cellspacing="0" rules="all" border="1" style="width: 1200px; border-collapse: collapse;">
                <caption>
                    详情
                </caption>
                <tr>
                    <td style="text-align: right; width: 25%">预约单号：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Flistid"></asp:Label></td>
                    <td style="text-align: right; width: 25%">预约基金：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Freserve_fund_name"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right">转出金额：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Ftotal_fee"></asp:Label></td>
                    <td style="text-align: right">转出基金：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Ffrom_fund_name"></asp:Label></td>
                </tr>

                <tr>
                    <td style="text-align: right">货币基金申购单号：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Fbuy_id"></asp:Label></td>
                    <td style="text-align: right">货币基金冻结单号：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Ffreeze_id"></asp:Label></td>
                </tr>

                <tr>
                    <td style="text-align: right">转换单号：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Ftransfer_id"></asp:Label></td>
                    <td style="text-align: right">转换方式：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Freserve_type"></asp:Label></td>
                </tr>

                <tr>
                    <td style="text-align: right">转换赎回单号：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Ftransfer_redeem_id"></asp:Label></td>
                    <td style="text-align: right">转换申购单号：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Ftransfer_buy_id"></asp:Label></td>
                </tr>

                <tr>
                    <td style="text-align: right">转换/冻结时间：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Facc_time"></asp:Label></td>
                    <td style="text-align: right">份额扣减单号：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Fscope_listid"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right">预计转换日期：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Fexpect_date"></asp:Label></td>
                    <td style="text-align: right">预约成功时间：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Freserve_time"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right">实际转入日期：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Ftransfer_date"></asp:Label></td>
                    <td style="text-align: right">二次确认时间：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Fconfirm_time"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right">预约状态：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Fstate"></asp:Label></td>
                    <td style="text-align: right">取消原因：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Fcancel_reason"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right">创建时间：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Fcreate_time"></asp:Label></td>
                    <td style="text-align: right">最后修改时间：</td>
                    <td>
                        <asp:Label runat="server" ID="Label_Fmodify_time"></asp:Label></td>
                </tr>
            </table>
        </asp:Panel>

    </form>
</body>
</html>
