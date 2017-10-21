using System;
using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.SharePoint.Entities
{
    [Serializable]
    public class CategoryEntity:ICategory
    {
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public int QuestionId { get; set; }
        public override string ToString() => Category;
    }

    [Serializable]
    public class NullCategoryEntity : ICategory
    {
        public int CategoryId
        {
            get { return -1; }
            set { }
        }

        public string Category
        {
            get { return string.Empty; }
            set { }
        }

        public int QuestionId
        {
            get { return -1; }
            set { }
        }

        public override string ToString() => Category;
    }
}
