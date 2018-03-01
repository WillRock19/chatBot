using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace FirstBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var resposta = ((Activity)context.Activity).CreateReply();

            if (activity.Text.Equals("Estou com o Carioca", StringComparison.InvariantCultureIgnoreCase))
            {
                resposta.Attachments.Add(CreateAudioCard().ToAttachment());
                await context.PostAsync(resposta);
            }
            else if (activity.Text.Equals("19", StringComparison.InvariantCultureIgnoreCase))
            {
                resposta.Attachments.Add(CreateAnimationCard().ToAttachment());
                await context.PostAsync(resposta);
            }
            else
                await CalculateLengthOfUserText(context, activity);

            context.Wait(MessageReceivedAsync);
        }

        private AudioCard CreateAudioCard() =>
            new AudioCard()
            {
                Title = "The truth about him",
                Subtitle = "The unspoken truth",
                Autostart = true,
                Autoloop = false,
                Media = new List<MediaUrl>()
                {
                    new MediaUrl() { Url = "http://wavlist.com/movies/300/sthprk-kidfdup.wav" }
                }
            };

        private AnimationCard CreateAnimationCard() =>
            new AnimationCard()
            {
                Title = "19-99",
                Subtitle = "Kan Ka No rey",
                Media = new List<MediaUrl>()
                {
                    new MediaUrl() { Url = "https://3.bp.blogspot.com/-ULVtURDREV8/V8vq5WPSNfI/AAAAAAAAcHw/cGUKAZWIRzgzgTSSaRk0dAQbH6twybY1ACLcB/s1600/_the%2Bdark%2Btower.gif" }
                }
            };

        private async Task CalculateLengthOfUserText(IDialogContext context, Activity activity)
        {
            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {length} characters");
        }
    }
}