using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace arinars.common
{
    public class FileUtil
    {
        /// <summary> Fast file copy with big buffers
        /// </summary>
        /// <param name="source">Source file path</param> 
        /// <param name="destination">Destination file path</param> 
        public static void FastCopy(string source, string destination)
        {
            int array_length = (int)Math.Pow(2, 19);
            byte[] dataArray = new byte[array_length];
            using (FileStream fsread = new FileStream
            (source, FileMode.Open, FileAccess.Read, FileShare.None, array_length))
            {
                using (BinaryReader bwread = new BinaryReader(fsread))
                {
                    using (FileStream fswrite = new FileStream
                    (destination, FileMode.Create, FileAccess.Write, FileShare.None, array_length))
                    {
                        using (BinaryWriter bwwrite = new BinaryWriter(fswrite))
                        {
                            for (; ; )
                            {
                                int read = bwread.Read(dataArray, 0, array_length);
                                if (0 == read)
                                    break;
                                bwwrite.Write(dataArray, 0, read);
                            }
                        }
                    }
                }
            }
        }
    }
}
