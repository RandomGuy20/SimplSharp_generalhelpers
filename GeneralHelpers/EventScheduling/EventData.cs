using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralHelpers.EventScheduling
{
    [Serializable]
    public class EventData
    {
        [JsonProperty("EventName")]
        public string EventName
        {
            get; set;
        }

        [JsonProperty("EventID")]
        public int EventID
        {
            get; set;
        }

        [JsonProperty("Days")]
        public bool[] Days
        {
            get; set;
        }

        [JsonProperty("DaysString")]
        public string[] DaysString
        {
            get; set;
        }

        [JsonProperty("Hour")]
        public int Hour
        {
            get; set;
        }

        [JsonProperty("Minute")]
        public int Minute
        {
            get; set;
        }

        [JsonProperty("Activate")]
        public bool Active
        {
            get; set;
        }
    }
}
