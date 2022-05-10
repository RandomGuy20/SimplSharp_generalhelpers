using Crestron.SimplSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralHelpers
{
    public class ReadWriteFileSIMPL
    {
        #region Fields

        public bool debug;
        private string fileLocation;

        FileChangeMonitor monitor;
        List<string> data = new List<string>();
        #endregion

        #region properties

        #endregion

        #region Delegates

        public delegate void SerialOutputChange(ushort val, SimplSharpString data);

        #endregion

        #region Events

        public SerialOutputChange onSerialChange { get; set; }

        #endregion

        #region Constructors

        public void Initialize(string FileLocation)
        {
            try
            {
                fileLocation = FileLocation;
                monitor = new FileChangeMonitor(FileLocation, 10);
                monitor.onFileChanged += Monitor_onFileChanged;
                monitor.Start();
                ReadFromFile(fileLocation);
            }
            catch (Exception e)
            {
                SendDebug("\n ReadWriteSIMPl error in constructor is: " + e.Message);
            }

        }


        #endregion

        #region Internal Methods

        internal void SetSerialOutputs(string[] lines)
        {
            for (ushort i = 0; i < lines.Length; i++)
            {
                onSerialChange(i, (SimplSharpString)lines[i]);
            }

        }

        internal void SendDebug(string data)
        {
            if (debug)
            {
                CrestronConsole.PrintLine("\nError ReadnWriteSIMPL File is: " + data);
                ErrorLog.Error("\nError ReadnWriteSIMPL File is: " + data);
            }
        }

        private void Monitor_onFileChanged()
        {
            ReadFromFile(fileLocation);
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Write to gfile
        /// </summary>
        /// <param name="fileData">the data</param>
        /// <param name="filePath">where the file is</param>
        public void WriteToFile(SimplSharpString fileData,SimplSharpString filePath)
        {

            try
            {
                string _data = fileData.ToString();
                string path = filePath.ToString();
                if (File.Exists(filePath.ToString()))
                {
                    ReadFromFile(filePath);
                }
                else
                {
                    if (ReadWriteFile.WriteToFile(_data, path, true))
                        SendDebug("\nSIMPl Write to file - success!");
                    else
                        SendDebug("\nSIMPL Write To File - Could not write");
                }

            }
            catch (Exception e)
            {
                SendDebug("\nSIMPL Error Write To File: " + e.Message);
            }

        }

        /// <summary>
        /// Read Data from file
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadFromFile(SimplSharpString filePath)
        {
            try
            {

                string path = filePath.ToString();

                if (ReadWriteFile.ReadFromFile(out data,path))
                {
                    SetSerialOutputs(data.ToArray());
                    SendDebug("\nSIMPl Read From file - success!");
                }
                else
                {
                    SendDebug("\nSIMPL Read From File - Could not write");
                }
            }
            catch (Exception e)
            {
                SendDebug("\nSIMPL Error Read From File: " + e.Message);
            }
        }

        public void ChangeLineItem(ushort index, SimplSharpString newData)
        {
            try
            {
                string writeString = "";
                if (index - 1 < data.Count)
                {
                    data[index - 1] = newData.ToString();
                    foreach (var item in data)
                        writeString = item + "\n";


                    if (ReadWriteFile.WriteToFile(writeString, fileLocation, true))
                        SendDebug("\nSIMPl Write to file - success!");
                }
            }
            catch (Exception e)
            {
                SendDebug("ReadWriteSIMPL Error ChangeLineItem() is: " + e);
            }
        }

        /// <summary>
        /// Debug SIMPL
        /// </summary>
        /// <param name="val"></param>
        public void SetDebugSIMPL(ushort val)
        {
            debug = Convert.ToBoolean(val);
        }

        /// <summary>
        /// DebugReadWriteHelper
        /// </summary>
        /// <param name="val"></param>
        public void SetDebugReadWrite(ushort val)
        {
            ReadWriteFile.Debug = Convert.ToBoolean(val);
        }

        #endregion
    }
}
