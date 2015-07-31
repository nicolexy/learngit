<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<%@ Page Language="c#" CodeBehind="AirTicketsOrderQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TravelPlatform.AirTicketsOrderQuery" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>AirTicketsOrderQuery</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <script src="/js/airportCity.js" type="text/ecmascript"></script>
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css );

        UNKNOWN {
            COLOR: #000000;
        }

        .style3 {
            COLOR: #ff0000;
        }

        BODY {
            BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
        }

        .tab_info thead th {
            background-color: #4A3C8C;
        }

        .tab_info {
            width: 100%;
            border-collapse: collapse;
            font-size: 14px;
            line-height: 1.5em;
        }

            .tab_info th {
                color: white;
            }

            .tab_info td {
                text-align: center;
            }

        .div_row {
            margin-top: 20px;
        }

        span.bright {
            color: #937010;
        }

        #HideDiv {
            width: 1200px;
            margin: 20px auto;
            text-align:left;
        }
    </style>
    <script src="../SCRIPTS/Local.js"></script>
    <script language="javascript">
        function openModeBegin() {
            var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.TextBoxBeginDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
            if (returnValue != null) Form1.TextBoxBeginDate.value = returnValue;
        }
    </script>
    <script language="javascript">
        function openModeEnd() {
            var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.TextBoxEndDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
            if (returnValue != null) Form1.TextBoxEndDate.value = returnValue;
        }
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellspacing="1" cellpadding="1"
            width="820" border="1">
            <tr>
                <td style="WIDTH: 100%" bgcolor="#e4e5f7" colspan="2"><font face="����"><font color="red">
                    <img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;��Ʊ������ѯ</font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </font>����Ա����: </FONT><span class="style3"><asp:Label ID="Label1" runat="server" Width="73px" ForeColor="Red"></asp:Label></span></td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label2" runat="server">�������š�Ʊ�Ų�ѯ��</asp:Label></td>
                <td align="left">&nbsp; &nbsp;
                    <asp:Label ID="Label3" runat="server">ƱԴ�����ţ�</asp:Label>
                    <asp:TextBox ID="TextSppreno" runat="server"></asp:TextBox>
                    <asp:Label ID="Label24" runat="server">Ʊ�ţ�</asp:Label>
                    <asp:TextBox ID="TextTicketno" runat="server"></asp:TextBox>
                    <asp:Label ID="Label25" runat="server">�Ƹ�ͨ���׵��ţ�</asp:Label>
                    <asp:TextBox ID="TextTransaction_id" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label4" runat="server">���˻�����Ϣ��ѯ��</asp:Label></td>
                <td align="left">&nbsp; &nbsp;
                    <asp:Label ID="Label21" runat="server">������</asp:Label>
                    <asp:TextBox ID="TextPassenger_name" runat="server"></asp:TextBox>
                    <asp:Label ID="Label26" runat="server">֤�����룺</asp:Label>
                    <asp:TextBox ID="TextCert_id" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label27" runat="server">����ϵ����Ϣ��ѯ��</asp:Label></td>
                <td align="left">&nbsp; &nbsp;<asp:Label ID="Label28" runat="server">������</asp:Label>
                    <asp:TextBox ID="TextName" runat="server"></asp:TextBox>
                    <asp:Label ID="Label29" runat="server">�ֻ����룺</asp:Label>
                    <asp:TextBox ID="TextMobile" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label30" runat="server">���Ƹ�ͨ�˺Ų�ѯ��</asp:Label></td>
                <td align="left">&nbsp; &nbsp;
                    <asp:TextBox ID="TextUin" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label31" runat="server">�����Ų�ѯ��</asp:Label></td>
                <td align="left">&nbsp; &nbsp;<asp:TextBox ID="TextInsur_no" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label32" runat="server">�������ڣ�</asp:Label></td>
                <td align="left">&nbsp; &nbsp;<asp:TextBox ID="TextBoxBeginDate" runat="server"></asp:TextBox><asp:ImageButton ID="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:ImageButton>
                    ��
                       <asp:TextBox ID="TextBoxEndDate" runat="server"></asp:TextBox><asp:ImageButton ID="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:ImageButton>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Label ID="Label33" runat="server">����״̬��</asp:Label>
                    <asp:DropDownList ID="ddlState" runat="server" Width="152px">
                        <asp:ListItem Value="all" Selected="True">ȫ������</asp:ListItem>
                        <asp:ListItem Value="payed">��֧��</asp:ListItem>
                        <asp:ListItem Value="unpay">��ռ��δ֧��</asp:ListItem>
                        <asp:ListItem Value="invalid">����״̬</asp:ListItem>
                        <asp:ListItem Value="pay_noticket">��֧��δ��Ʊ</asp:ListItem>
                        <asp:ListItem Value="pay_ticket">��֧���ѳ�Ʊ</asp:ListItem>
                        <asp:ListItem Value="refund_ticket">��Ʊ</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="Button2" runat="server" Width="80px" Text="�� ѯ" OnClick="btnSearch_Click"></asp:Button></td>
            </tr>
        </table>
        <div style="LEFT: 10px; OVERFLOW: auto; WIDTH: 95%; POSITION: absolute; TOP: 300px;">
            <table cellspacing="0" cellpadding="0" width="100%" border="1">
                <tr>
                    <td valign="top" align="center">
                        <asp:DataGrid ID="dgList" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
                            CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
                            <FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
                            <SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
                            <AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
                            <ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn DataField="listid" HeaderText="�������">
                                    <HeaderStyle Width="250px" Height="40px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="uin" HeaderText="�Ƹ�ͨ�˺�">
                                    <HeaderStyle Width="100px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="sppreno" HeaderText="ƱԴ������">
                                    <HeaderStyle Width="100px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="transaction_id" HeaderText="�Ƹ�ͨ���׵���">
                                    <HeaderStyle Width="170px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="book_time" HeaderText="����ʱ��">
                                    <HeaderStyle Width="140px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="pay_time" HeaderText="֧��ʱ��">
                                    <HeaderStyle Width="140px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="from" HeaderText="����">
                                    <HeaderStyle Width="50px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="flight_no" HeaderText="�����">
                                    <HeaderStyle Width="50px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="cabin" HeaderText="��λ">
                                    <HeaderStyle Width="50px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="dtime" HeaderText="���ʱ��">
                                    <HeaderStyle Width="140px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="total_money_str" HeaderText="���ϼ�">
                                    <HeaderStyle Width="150px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="strSp_code" HeaderText="��Ӧ��">
                                    <HeaderStyle Width="200px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="strTrade_state" HeaderText="����״̬">
                                    <HeaderStyle Width="120px"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:ButtonColumn CommandName="Select" HeaderText="����" Text="�鿴��ϸ">
                                    <HeaderStyle Width="120px"></HeaderStyle>
                                </asp:ButtonColumn>
                            </Columns>
                            <PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
                        </asp:DataGrid></td>
                </tr>
                <tr height="25">
                    <td>
                        <webdiyer:AspNetPager ID="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��"
                            HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
                            ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:center;">
                        <div id="HideDiv" style="<%=HideDivDisplay?"": "display:none" %>">
                            <div class="div_row">
                                <b>������ϸ</b>
                                <table class="tab_info">
                                    <thead>
                                        <tr>
                                            <th>ƱԴ������</th>
                                            <th>PNR</th>
                                            <th>���ս��׵���</th>
                                            <th>���չ�Ӧ��</th>
                                            <th>״̬</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="OrderInfo_Repeater" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%#Eval("sppreno") %></td>
                                                    <td><%#Eval("pnr") %></td>
                                                    <td><%#Eval("insurance_orderid") %></td>
                                                    <td><%#Eval("insur_com_str") %></td>
                                                    <td><%#Eval("strTrade_state") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <div class="div_row">
                                <b>������ϸ</b>
                                <table class="tab_info">
                                    <thead>
                                        <tr>
                                            <th>����</th>
                                            <th>���ʱ�� / ����</th>
                                            <th>����ʱ�� / ����</th>
                                            <th>�����</th>
                                            <th>��λ</th>
                                            <th>���� / ȼ�ͷ�</th>
                                            <th>��Ʊ�۸�</th>
                                            <th>�ϼ�</th>
                                        </tr>
                                    </thead>
                                    <tbody  id="weiyi">
                                        <asp:Repeater ID="Flights_Repeater" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%#Eval("trip_flag_str") %></td>
                                                    <td><%#DateTime.Parse((string)Eval("dtime")).ToString("hh:mm")%> <span class="airport"><%#Eval("from") %></span></td>
                                                    <td><%#DateTime.Parse((string)Eval("atime")).ToString("hh:mm")%> <span class="airport"><%#Eval("to") %></span></td>
                                                    <td><%#Eval("flight_no") %></td>
                                                    <td><%#Eval("cabin") %></td>
                                                    <td>
                                                        <span>����:��<%#Eval("adult_airport_tax_str") %> / ��<%#Eval("adult_fuel_tax_str") %> </span>
                                                        <br />
                                                        <span>��ͯ:��<%#Eval("child_airport_tax_str") %> / ��<%#Eval("child_fuel_tax_str") %></span>
                                                    </td>
                                                    <td>
                                                        <span>��<%#Eval("adult_price_str") %></span>
                                                        <br />
                                                        <span>��<%#Eval("child_price_str") %></span>
                                                    </td>
                                                    <td>
                                                        <span>��<%#Eval("adult_total_money") %></span>
                                                                     <br />
                                                        <span>��<%#Eval("child_total_money") %></span>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <script type="text/ecmascript">
                                var ss = document.getElementById("weiyi").getElementsByTagName("span");
                                for (var i = 0; i < ss.length; i++) {
                                    if (ss[i].attributes.getNamedItem("class").nodeValue.indexOf("airport") == -1) continue;
                                    var a= AirportCity[ss[i].innerText];
                                    ss[i].innerText = a ? a.airport : ss[i].innerText;
                                }
                            </script>
                            <div class="div_row">
                                <b>�˿���Ϣ</b>
                                <table class="tab_info">
                                    <thead>
                                        <tr>
                                            <th>�˿�����</th>
                                            <th>�˿�����</th>
                                            <th>֤������</th>
                                            <th>֤������</th>
                                            <th>���򺽿�������</th>
                                            <th>������</th>
                                            <th>���ӿ�Ʊ��</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="Passengers_Repeater" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%#Eval("type_str") %></td>
                                                    <td><%#Eval("name") %></td>
                                                    <td><%#Eval("cert_type_str") %></td>
                                                    <td><%#Eval("cert_id") %></td>
                                                    <td><%#((string)Eval("has_insur"))=="0"?"��":"��" %></td>
                                                    <td><%#Eval("insur_no") %></td>
                                                    <td><%#Eval("eticketno") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <div class="div_row">
                                <b>��ϵ����Ϣ</b>
                                <table class="tab_info" style="margin: 10px 0 0 30px;width:700px;">
                                    <tbody>
                                        <tr>
                                            <td><span>������</span><span runat="server" id="contact_name"></span></td>
                                            <td><span>�ֻ����룺</span><span runat="server" id="contact_mobile"></span></td>
                                            <td><span>Email��</span><span runat="server" id="contact_email"></span></td>
                                            <td><span>��ϵ�绰��</span><span runat="server" id="contact_telephone"></span></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="div_row">
                                <b>�г̵��ͱ��շ�Ʊ��ȡ</b>
                                <table style="margin-top: 10px;">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: right; width: 100px;">�г̵���</td>
                                            <td><span>��ѡ���ˣ�</span><span class="bright" id="Journey" runat="server"></span></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">���շ�Ʊ��</td>
                                            <td>
                                                <div runat="server" id="showHint"><span>�����տͷ��绰</span><span class='bright'>40001-558-558</span><span>�����ʼ�</span></div></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="div_row" style="text-align:center;">
                                <span runat="server" id="remark"></span>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
