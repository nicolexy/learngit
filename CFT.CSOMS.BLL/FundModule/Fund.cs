using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.FundModule
{
    /// <summary>
    /// 基金
    /// </summary>
    public class Fund
    {
        /// <summary>
        /// 基金名称
        /// </summary>
        public string Name { get; set; }
        public string SPId { get; set; }
        /// <summary>
        /// 基金公司名称
        /// </summary>
        public string SPName { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 币种类型代码
        /// </summary>
        public int CurrencyType { get; set; }
    }
}
