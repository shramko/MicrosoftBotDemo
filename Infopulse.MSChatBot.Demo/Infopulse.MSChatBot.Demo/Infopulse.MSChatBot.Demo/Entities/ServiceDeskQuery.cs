using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace Infopulse.MSChatBot.Demo.Entities
{
    [Serializable]
    public class ServiceDeskQuery
    {
        [Prompt("Please enter {&} for Service Desk ticket:")]
        public string Title { get; set; }

        [Prompt("Please enter the name of the application or device with which you have a problem:")]
        public string Entity { get; set; }

        [Prompt("Please enter {&}:")]
        public string Description { get; set; }

        public static IForm<ServiceDeskQuery> BuildForm()
        {
            return new FormBuilder<ServiceDeskQuery>()
                .AddRemainingFields()
                .Build();
        }
    }
}