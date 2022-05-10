using Crestron.SimplSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralHelpers
{

    public static class FileOperations
    {
        #region Fields

        #endregion

        #region Properties


        public static bool Debug { get; set; }
        #endregion

        #region Delegates

        #endregion

        #region Events

        #endregion

        #region Constructors

        #endregion

        #region Internal Methods

        internal static void SendDebug(string data)
        {
            if (Debug)
            {
                CrestronConsole.PrintLine("\nError File Operations is: " + data);
                ErrorLog.Error("\nError File Operations is: " + data);
            }
        }

        #endregion

        #region Public Methods

        public static bool WriteToFile(string fileData, string fileLocation, bool overwriteFile)
        {
            try
            {
                if (File.Exists(fileLocation) && !overwriteFile)
                {
                    using (FileStream fs = new FileStream(fileLocation, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.Write(fileData);
                        }
                    }
                    return true;
                }
                else if (overwriteFile && File.Exists(fileLocation) || !File.Exists(fileLocation))
                {
                    using (FileStream fs = new FileStream(fileLocation, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                    {
                        StreamReader sr = new StreamReader(fs);
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            fs.SetLength(0);
                            sw.Write(fileData);
                        }
                    }
                    return true;
                }
                else
                {
                    SendDebug("WriteToFile: File did not exist, and was not set to create");
                    return false;
                }

            }
            catch (Exception e)
            {
                SendDebug("WriteToFile error is: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Basic text
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool ReadFromFile(out List<string> fileData, string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                fileData = lines.ToList();  
                return true;
            }
            catch (Exception e)
            {
                SendDebug("Error Read From File isL " + e.Message);
                fileData = new List<string>();
                return false;
            }
        }

        public static bool ReadFromFile(out string fileData, string filePath)
        {
            try
            {
                fileData = File.ReadAllText(filePath);
                return true;
            }
            catch (Exception e)
            {
                SendDebug("Error Read From File is " + e.Message);
                fileData = "";
                return false;
            }
        }



        /// <summary>
        /// If file was deleted will return a 1, if file does not exist, it will be a 0, if file was not deleted or error, will be -1 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int DeleteFile(string filePath)
        {
            try
            {
                int val;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    val = File.Exists(filePath) ? -1 : 1;
                }
                else
                    val = 0;

                return val;
            }
            catch (Exception e)
            {
                SendDebug("Error at Delet File is: " + e);
                return -1;
            }
        }

        #endregion
    }
}
