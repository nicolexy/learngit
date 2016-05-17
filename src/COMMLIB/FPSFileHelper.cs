using CFT.Apollo.Common.Configuration;
using commLib.Entity;
using SunLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace commLib
{
    /// <summary>
    /// FPS文件服务帮助类
    /// </summary>
    public class FPSFileHelper
    {
        private readonly static string appId = AppSettings.Get<string>("FPSAppid", "csoms");
        public readonly static string Uploadurl = AppSettings.Get<string>("FPSUploadFileURL", "http://fps.zw.cf.com");
        /// <summary>
        /// 上传文件到fps文件服务器
        /// </summary>
        /// <param name="file">文件流</param>
        /// <param name="filepathname">发送给FPS的[文件路径]和文件名称</param>
        /// <param name="upOutTime">超时时间</param>
        /// <returns></returns>
        public static UploadFileModel UploadFile(Stream file, string filepathname, double upOutTime = 60)
        {
            UploadFileModel fileModel = null;

            using (var client = new HttpClient())
            {
                var content = new MultipartFormDataContent();
                try
                {
                    upOutTime = upOutTime <= 0 ? 60 : upOutTime;
                    //设置请求超时时长：1小时
                    client.Timeout = TimeSpan.FromMinutes(upOutTime);
                    client.BaseAddress = new Uri(Uploadurl);

                    content.Add(new StringContent(filepathname), "filepathname");
                    using (var fileContent1 = new StreamContent(file))
                    {
                        fileContent1.Headers.Add("Content-Disposition", "form-data; name=\"filedata\"; filename=\"" + Path.GetFileName(filepathname) + "\"");
                        content.Add(fileContent1);       //添加上传数据

                        //上传文件
                        var result = client.PostAsync("/file/" + appId, content).Result;
                        var retData = result.Content.ReadAsStringAsync();

                        LogHelper.LogInfo(string.Format("FPS文件上传返回信息:{0}", retData.Result));
                        //json 转为 UploadFileModel对象
                        System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                        fileModel = jss.Deserialize<UploadFileModel>(retData.Result);
                        if (fileModel.errcode == 0)
                        {
                            fileModel.url = Uploadurl + "/" + fileModel.fileurl;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(string.Format("FPS文件上传失败:{0} ,异常:{1}", filepathname, ex.ToString()), "FPSFileHelper");
                    throw;
                }
                finally
                {
                    content.Dispose();
                }
            }

            return fileModel;
        }


        /// <summary>
        /// 上传文件到fps文件服务器
        /// </summary>
        /// <param name="filePath">文件磁盘地址</param>
        /// <param name="fileName">发送给FPS的[文件路径]和文件名称, 默认文件使用原名称</param>
        /// <param name="upOutTime">超时时间</param>
        /// <returns></returns>
        public static UploadFileModel UploadFile(string filePath, string filepathname, double upOutTime = 60)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            using (var fileStream = File.OpenRead(filePath))
            {
                if (string.IsNullOrEmpty(filepathname))
                {
                    filepathname = Path.GetFileName(filePath);
                }

                LogHelper.LogInfo(string.Format("public static UploadFileModel UploadFile(string filePath, string filepathname = null, double upOutTime = 60)   filepathname:{0} ,filePath:{1}", filepathname, filePath), "FPSFileHelper");

                return UploadFile(fileStream, filepathname, upOutTime);
            }
        }
    }
}
