using System;
using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.Demo.Entities
{
    [Serializable]
    public class TripRequest: ITripRequest
    {
        public string Requestor { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public string Transport { get; set; }
        public int BotConversationInfo { get; set; }
    }
}