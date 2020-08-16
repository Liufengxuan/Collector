using System;
using System.Collections.Generic;
using System.IO;

using System.Runtime.InteropServices;
using System.Text;

namespace TestForm2
{
    public class IniOper
    {
        public string iniFilePath = "";

        #region API函数声明

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);


        #endregion

        public IniOper(string path)
        {
            iniFilePath = path;
        }

        #region 读Ini文件
        /// <summary>
        /// 读Ini文件
        /// </summary>
        /// <param name="Section">[]内的段落名</param>
        /// <param name="Key">key</param>
        /// <param name="NoText"></param>
        /// NoText对应API函数的def参数，它的值由用户指定，是当在配置文件中没有找到具体的Value时，就用NoText的值来代替。可以为空
        /// <param name="iniFilePath">ini配置文件的路径加ini文件名</param>
        /// <returns></returns>
        public string ReadIniData(string Section, string Key, string NoText)
        {
            if (File.Exists(iniFilePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, NoText, temp, 1024, iniFilePath);
                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

        #region 写Ini文件

        public bool WriteIniData(string Section, string Key, string Value)
        {
            if (File.Exists(iniFilePath))
            {
                long OpStation = WritePrivateProfileString(Section, Key, Value, iniFilePath);
                if (OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
