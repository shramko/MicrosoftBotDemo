using System;

namespace Infopulse.MSChatBot.Interfaces.Managers
{
    public interface IBotLogger
    {
        void WriteMessage(string message, string category);
        void WriteExceprion(Exception ex);
    }
}
