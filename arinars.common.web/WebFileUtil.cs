using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Hosting;

namespace arinars.common.web
{
    public class WebFileUtil : FileUtil
    {
        /// <summary>
        /// 해당 경로 내의 파일 목록을 가져온다.
        /// </summary>
        /// <param name="aPath">경로 상대경로, 절대 경로 모두 가능.</param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> GetFiles(string aPath)
        {
            string lPath = null;
            if (Path.IsPathRooted(aPath))
            {
                lPath = aPath;
            }
            else
            {
                lPath = HostingEnvironment.MapPath(aPath);
            }
            DirectoryInfo lDirectoryInfo = new DirectoryInfo(lPath);
            return lDirectoryInfo.GetFiles();
        }
    }
}
