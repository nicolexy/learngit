using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commLib.Entity
{
    /// <summary>
    /// 文件上传返回值类型
    /// </summary>
    public class UploadFileModel
    {
        /// <summary>
        ///    状态码： 0表示响应成功。其它查看文档
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 详细消息
        /// </summary>
        public string msg { get; set; }
        public string fileid { get; set; }
        public string fileurl { get; set; }
        public string filemd5 { get; set; }
        public string filepathname { get; set; }

        [System.Web.Script.Serialization.ScriptIgnore]
        public string url { get; set; }
    }
}
