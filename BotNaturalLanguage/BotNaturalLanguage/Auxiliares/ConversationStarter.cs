﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using BotNaturalLanguage.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace BotNaturalLanguage.Auxiliares
{
    public class ConversationStarter
    {
        //Note: Of course you don't want this here. Eventually you will need to save this in some table
        //Having this here as static variable means we can only remember one user :)
        public static string conversationReference;
        public static string textReference;
        //This will interrupt the conversation and send the user to SurveyDialog, then wait until that's done 
        public static async Task Resume()
        {
            var message = JsonConvert.DeserializeObject<ConversationReference>(conversationReference).GetPostToBotMessage();
            message.Text = textReference;
            var client = new ConnectorClient(new Uri(message.ServiceUrl));

            using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message))
            {
                var botData = scope.Resolve<IBotData>();
                await botData.LoadAsync(CancellationToken.None);

                //This is our dialog stack
                var task = scope.Resolve<IDialogTask>();

                //interrupt the stack. This means that we're stopping whatever conversation that is currently happening with the user
                //Then adding this stack to run and once it's finished, we will be back to the original conversation
                var dialog = new LuisMainDialog();
                task.Forward(dialog, null, message, CancellationToken.None);

                await task.PollAsync(CancellationToken.None);
                textReference = string.Empty;

                //flush dialog stack
                await botData.FlushAsync(CancellationToken.None);

            }
        }

    }
}