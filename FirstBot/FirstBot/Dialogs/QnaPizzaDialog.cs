using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace FirstBot.Dialogs
{
    [Serializable]
    public class QnaPizzaDialog : QnAMakerDialog
    {
        private static string QnaSubscription = ConfigurationManager.AppSettings["QnaSubscriptionKey"];
        private static string QnaKnowladgeBase = ConfigurationManager.AppSettings["QnaKnowledgeBaseId"];
        private static double precisionScore = 0.40;

        public QnaPizzaDialog() : base(new QnAMakerService(new QnAMakerAttribute(QnaSubscription, QnaKnowladgeBase, "Não encontrei sua resposta", precisionScore, 2)))
        {

        }

        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            var resposta = ((Activity)context.Activity).CreateReply();
            var primeiraResposta = result.Answers.First().Answer;
            var dadosResposta = primeiraResposta.Split(';');

            if (dadosResposta.Length == 1)
            {
                await context.PostAsync(primeiraResposta);
                return;
            }

            resposta.Attachments.Add(CreateHeroCardWithData(dadosResposta).ToAttachment());
            await context.PostAsync(resposta);
        }

        private HeroCard CreateHeroCardWithData(string[] data)
        {
            var titulo = data[0];
            var descricao = data[1];
            var linkFoto = data[2];
            var linkCompra = data[3];

            var card = new HeroCard
            {
                Title = titulo,
                Subtitle = descricao,
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl, "Compre agora", value:linkCompra)
                }
            };

            card.Images = new List<CardImage>()
            {
                new CardImage { Url = linkFoto  }
            };

            return card;
        }
    }
}