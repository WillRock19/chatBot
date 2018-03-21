using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot.Dialogs
{
    [Serializable]
    public class QnaPizzaDialog : QnAMakerDialog
    {
        private static string QnaSubscription = ConfigurationManager.AppSettings["QnaSubscriptionKey"];
        private static string QnaKnowladgeBase = ConfigurationManager.AppSettings["QnaKnowledgeBaseId"];
        private static double precisionScore = 0.40;

        public QnaPizzaDialog() : base(new QnAMakerService(new QnAMakerAttribute(QnaSubscription, QnaKnowladgeBase, "Não encontrei sua resposta", precisionScore, 2))) { }

        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            var resposta = ((Activity)context.Activity).CreateReply();
            var primeiraResposta = result.Answers.First().Answer;
            var dadosResposta = primeiraResposta.Split(';');

            if (dadosResposta.Length == 1)
            {
                await context.PostAsync(primeiraResposta);
                //context.Call(new CotacaoDialog(), ExecuteAfterFoward);

                return;
            }

            resposta.Attachments.Add(CreateHeroCardWithData(dadosResposta).ToAttachment());
            await context.PostAsync(resposta);

            context.Done<string>(null);

            //await context.Forward(new CotacaoDialog(), ExecuteAfterFoward, message, CancellationToken.None);
        }

        private async Task ExecuteAfterFoward(IDialogContext context, IAwaitable<object> item)
        {
            await context.PostAsync("Executando método após foward");
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
                    new CardAction(ActionTypes.OpenUrl, "Compre agora", value:linkCompra),
                    new CardAction()
                    {
                        Type = ActionTypes.PostBack,
                        Title = "Eira",
                        Value = "Teste"
                    }  
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