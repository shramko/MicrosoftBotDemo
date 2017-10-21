using System;
using Microsoft.Bot.Builder.FormFlow;

namespace Infopulse.MSChatBot.Demo.Entities
{
    [Serializable]
    public class HotelsQuery
    {
        [Prompt("Please enter your {&}")]
        [Optional]
        public string Destination { get; set; }

        [Prompt("Near which Airport")]
        [Optional]
        public string AirportCode { get; set; }
    }
}