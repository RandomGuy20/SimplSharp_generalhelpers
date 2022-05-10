using Crestron.SimplSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralHelpers
{
    public static class CommandBuilder
    {

        /// <summary>
        /// Build a command where two pre built byte arrays need to be combined
        /// </summary>
        /// <param name="header">Header array</param>
        /// <param name="command">Command array</param>
        /// <returns></returns>
        public static byte[] BuildCommand(byte[] header, byte[] command)
        {
            try
            {
                return header.Concat(command).ToArray();
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("\nError in BuildCommand[] Byte[] Byte[] is: " + e);
                return null;
            }
        }
    }
}
