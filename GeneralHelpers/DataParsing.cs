using Crestron.SimplSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralHelpers
{
    public static class DataParsing
    {
        /// <summary>
        /// Will return -1 If no match
        /// </summary>
        /// <param name="data">the string you are looking for</param>
        /// <param name="matchData">the array containing strings</param>
        /// <returns></returns>
        public static int GetMatchPosition(string data, string[] matchData)
        {
            try
            {
                for (int i = 0; i < matchData.Length; i++)
                {
                    if (matchData[i].Contains(data))
                        return i;
                }
                return -1;
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("\nError in Getmatch Position in DataParsing helper Methods is: " + e);
                return -1;
            }
        }
    }
}
