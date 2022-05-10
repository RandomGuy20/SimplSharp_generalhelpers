using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralHelpers
{
    public enum eDisplayChecksum
    {
        Samsung,
        Sony
    }
    /// <summary>
    /// Contains many Different helpers for when Math Operations are required
    /// </summary>
    public static class GeneralMaths
    {

        public static byte GetCheckSumAND(byte[] data, eDisplayChecksum display)
        {
            #region Samsung
            if (display == eDisplayChecksum.Samsung)
            {
                int sum = 0;
                int ctr = 0;
                foreach (var d in data)
                {
                    if (ctr > 0)
                        sum += (int)d;

                    ctr++;
                }
                return (byte)(sum & 0xFF);
            }
            #endregion
            #region Sony
            else if (display == eDisplayChecksum.Sony)
            {
                byte sum = 0x00;
                for (int i = 0; i < data.Length; i++)
                    sum += data[i];

                return sum;
            }
            #endregion
            else
            {
                return 0xff;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"> The byte array that conatins the pre-built command </param>
        /// <returns>Will Return the Xor'ed byte </returns>
        public static byte GetCheckSumXOR(byte[] data)
        {
            byte checksumByte = 0x00;
            for (int i = 0; i < data.Length; i++)
            {
                checksumByte ^= data[i];
            }
            return checksumByte;
        }

        /// <summary>
        /// Scale a value up
        /// </summary>
        /// <param name="newMax"></param>
        /// <param name="oldMax"></param>
        /// <param name="currentMin"></param>
        /// <param name="currvalue"></param>
        /// <returns></returns>
        public static int ScaleValueUp(int newMax, int oldMax, int currentMin, int currvalue)
        {
            return (currvalue - currentMin) * (newMax - currentMin) / (oldMax - currentMin);
        }

        /// <summary>
        /// Scale a value Down
        /// </summary>
        /// <param name="oldRange"></param>
        /// <param name="newRange"></param>
        /// <param name="currentlevel"></param>
        /// <returns></returns>
        public static int ScaleValueDown(int oldRange, int newRange, int currentlevel)
        {
            return (currentlevel * newRange) / oldRange;
        }

        /// <summary>
        /// Will convert a value from one range to another range
        /// </summary>
        public static double ConvertRange(int originalLow, int originalHigh, int newLow, int newHigh, int conversionValue)
        {
            double scale = (double)(newHigh - newLow) / (originalHigh - originalLow);
            return (newLow + ((conversionValue - originalLow) * scale));
        }
    }
}
