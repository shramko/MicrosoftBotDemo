using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.Interfaces.Managers
{
    public interface ITripManager
    {
        void SaveTrip(ITripRequestListConfig listConfig, ITripRequest request, IBotConversationInfo botConversationInfo, out int requestId);
    }
}
