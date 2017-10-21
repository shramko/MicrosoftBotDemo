namespace Infopulse.MSChatBot.Interfaces.Entities
{
    public interface IQuestionsCategoryListConfig:IBaseListConfig
    {
        string CategoryName { get; }
        string FirstQuestion { get; }
    }
}
