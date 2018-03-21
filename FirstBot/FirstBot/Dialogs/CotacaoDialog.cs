using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FirstBot.Dialogs
{
    [Serializable]
    [LuisModel("4e1a5b37-721e-4c0e-95c2-835ae9468f55", "bc1e37ee93a8408db7f30a120ff6e375")]
    public class CotacaoDialog : LuisDialog<object>
    {
        public CotacaoDialog() { }

        [LuisIntent("")]
        public async Task Nada(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Bom... tamos na dialog de cotação");
        }

        [LuisIntent("None")]
        public async Task SemIntencaoCadastrada(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Tu tá é doido, mermão?");
        }

        [LuisIntent("Comprimentos")]
        public async Task UsuarioEstaNosComprimentando(IDialogContext context, LuisResult result)
        {
            var horaAtual = DateTime.Now;

            if (horaAtual.Hour == 12)
                await context.PostAsync("VAI ALMOÇAR!");

            if (horaAtual.Hour < 12)
                await context.PostAsync("Bom dia, humano. Você tomou banho hoje?");

            if (horaAtual.Hour > 12 && horaAtual.Hour < 18)
                await context.PostAsync("Bom tarde, humano. Já almoçou?");

            if (horaAtual.Hour > 18)
                await context.PostAsync("Bom noite humano. Que você nunca veja outra.");

            context.Done<string>("eiiiiiiita");
        }

        [LuisIntent("Sobre")]
        public async Task Sobre(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Call me... The Judgement Day. :D ");
        }

        [LuisIntent("Cotacao")]
        public async Task DescobrirCotacao(IDialogContext context, LuisResult result)
        {
            var moedas = result.Entities.Select(e => e.Entity);
            await context.PostAsync($"Vou tentar descobrir as cotações das moedas: {string.Join(", ", moedas)} ");
        }
    }
}