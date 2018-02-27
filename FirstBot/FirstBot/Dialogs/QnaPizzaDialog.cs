using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FirstBot.Dialogs
{
    [Serializable]
    public class QnaPizzaDialog : QnAMakerDialog
    {
        //private string QnaSubscription = ConfigurationManager.AppSettings["QnaSubscriptionKey"];
        //private string QnaKnowladgeBase = ConfigurationManager.AppSettings["QnaKnowledgeBaseId"];

        public QnaPizzaDialog() : base(new QnAMakerService(new QnAMakerAttribute(ConfigurationManager.AppSettings["QnaSubscriptionKey"], ConfigurationManager.AppSettings["QnaKnowledgeBaseId"], "Não encontrei sua resposta", 0.5)))
        {

        }
    }
}