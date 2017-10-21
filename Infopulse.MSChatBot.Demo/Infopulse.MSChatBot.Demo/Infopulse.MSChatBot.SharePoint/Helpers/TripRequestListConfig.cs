using System;
using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.SharePoint.Helpers
{
    [Serializable]
    public class TripRequestListConfig : ITripRequestListConfig
    {
        public string ListTitle => "Trip Requests";
        public string IdFieldName => "ID";
        public string Requestor => "Title";
        public string Departure => "Departure";
        public string Destination => "Destination";
        public string Transport => "Transport";
        public string BotConversationInfo => "BotConversationInfo";
    }
}