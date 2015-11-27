namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;

    /// <summary>
    ///		MenuControl ��ժҪ˵����
    /// </summary>
    public partial class MenuControl : System.Web.UI.UserControl
    {
        private System.Collections.Generic.List<MenuTag> MenuList = new System.Collections.Generic.List<MenuTag>();

        class MenuTag
        {
            public string Text { get; set; }
            public string Url { get; set; }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            if (!IsPostBack)
            {
                InitControls();
            }
        }
        /// <summary>
        /// �˵�����
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ����Ӳ˵�
        /// </summary>
        /// <param name="subTitle">�Ӳ˵�����</param>
        public void AddSubMenu(string subTitle)
        {
            AddSubMenu(subTitle, "javascript:alert('��ģ�����ڿ����С�����')");
        }

        /// <summary>
        /// ����Ӳ˵�
        /// </summary>
        /// <param name="subTitle">�Ӳ˵�����</param>
        /// <param name="subURL">�Ӳ˵�����</param>
        public void AddSubMenu(string subTitle, string subURL)
        {
            MenuList.Add(new MenuTag() { Text = subTitle, Url = subURL });
        }

        private void InitControls()
        {
            string expand = this.ViewState["Expand"] as string ?? "none";
            var buff = new System.Text.StringBuilder(MenuList.Count * 50 + 200);
            buff.AppendLine("<div style='cursor:hand;padding:2px 0 4px 4px;' onclick='expandObject(this)'> \r\n");
            buff.AppendFormat("<strong>{0}</strong> \r\n", Title);
            buff.AppendFormat("<div style='padding:4px 0 4px 8px;display:{0}'> \r\n", expand);

            foreach (var item in MenuList)
            {
                buff.AppendFormat("<div><a href='{0}' TARGET='WorkArea' style='display:block;padding:2px 0;'>{1}</a></div> \r\n", item.Url, item.Text);
            }

            buff.AppendLine("</div> \r\n");
            buff.AppendLine("</div> \r\n");

            this.lbMenu.Text = buff.ToString();
        }
    }
}

