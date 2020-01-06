using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ExampleBot.Dialogs
{
    [Serializable]
    public class ConversationDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(P);

            return Task.CompletedTask;
        }

        private async Task P(IDialogContext context, IAwaitable<object> result)
        {
            var act = (await result) as Activity;

            if (act.Text.ToLowerInvariant() == "oi")
            {
                await context.PostAsync("Olá! :)  \n\nEm que posso ajudar? ^^");
            }
            if (act.Text.ToLowerInvariant().Contains("viajar"))
            {
                await context.PostAsync("claro! ^^  \n\nDeixa eu só ver os locais mais buscados...");

                var response = context.MakeMessage();
                response.Type = ActivityTypes.Typing;
                await context.PostAsync(response);
            }

            context.Wait(P);
        }


            private async Task AskUserForAnAttachment(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Attachment(
                context: context,
                resume: ResumeAfterUserSendAttachment,
                prompt: "Envia uma imagem bem loka pra mim",
                retry: "Por favor, manda via anexo, ok?",
                attempts: 1);
        }

        private async Task ResumeAfterUserSendAttachment(IDialogContext context, IAwaitable<IEnumerable<Attachment>> result)
        {
            try
            {
                var attachment = await result;
                var myImageInfo = attachment.FirstOrDefault();

                var client = new HttpClient();

                var image = await client.GetAsync(myImageInfo.ContentUrl);

                if (image.IsSuccessStatusCode)
                {
                    await context.PostAsync("Olha... imagem bem loca mesmo, hein? =D");
                }
                else
                {
                    await context.PostAsync("Cara... não consegui receber sua imagem x.x");
                }
            }
            catch (TooManyAttemptsException e)
            {
                await context.PostAsync("Eita... você só manda errado :/");
            }
        }

        private async Task AskUserToChoose(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Choice(
                context: context,
                resume: ResumeAfterChoiseHasBeenMade,
                prompt: "Para onde você quer viajar?",
                options: new string[] { "Teste numero um", "Eita", "Doido, né?" },
                retry: "Por favor, escolha uma das opções fornecidas.",
                attempts: 1,
                promptStyle: PromptStyle.Keyboard
            );
        }

        private async Task AskUserToChoose(IDialogContext context)
        {
            PromptDialog.Choice(
                context: context,
                resume: ResumeAfterChoiseHasBeenMade,
                promptOptions: new PromptOptions<OpcoesDeViagem>(
                    prompt: "Para onde você quer ir?",
                    retry: "Por favor, escolha uma das opções válidas.",
                    tooManyAttempts: "Cara... tu não quer escolher uma certa, né?",
                    options: new OpcoesDeViagem[] { OpcoesDeViagem.Brazilia, OpcoesDeViagem.Chicago, OpcoesDeViagem.Tokio, OpcoesDeViagem.Tokio, OpcoesDeViagem.Viena },
                    attempts: 1
                    )
            );
        }

        private async Task ResumeAfterChoiseHasBeenMade(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var place = await result;


            }
            catch (TooManyAttemptsException e)
            {
                await context.PostAsync("Acho que o lugar pra onde você quer ir, eu não quero estar >.>");
            }
        }

        private async Task AskForAgeAsync(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Number(
                context: context, 
                resume: ResumeAfterAgeAsking, 
                prompt: "Qual sua idade?", 
                min: 6, 
                max: 140, 
                retry: "Não entendi. Por favor, insira um número entre 6 e 140...",
                attempts: 2);
        }

        private async Task ResumeAfterAgeAsking(IDialogContext context, IAwaitable<double> userAnswer)
        {
            try
            {
                var answer = await userAnswer;
                await context.PostAsync($"Só {answer} anos? Como é novinho!");
            }
            catch (IndexOutOfRangeException e)
            {
                await context.PostAsync("Sua idade não está no range aceitável!");
            }
            catch (TooManyAttemptsException e)
            {
                await context.PostAsync("Cê não sabe o próprio nome?");
            }
            catch (Exception e)
            {

            }
        }


        private async Task AskUserNameAsync(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Text(
                context: context,
                resume: ResumeAfterNameAsking,
                prompt: "Oi, tudo bem? Diz aí... qual seu nome?",
                retry: "Não entendi cara... pode repetir?",
                attempts: 2);
        }

        private async Task ResumeAfterNameAsking(IDialogContext context, IAwaitable<string> userAnswer)
        {
            var answer = await userAnswer;

            PromptDialog.Confirm(
                context: context,
                resume: ResumeAfterNameConfirmation,
                promptOptions: new PromptOptions<string>(
                    prompt: $"Tem certeza que '{answer}' é seu nome?", 
                    retry: "Desculpe, não entendi sua resposta. Pode escolher uma das opções?",
                    tooManyAttempts: "Cara... você não sabe seu próprio nome?",
                    options: new List<string>() { "É meu nome sim", "Eita, é não. Cê tá doido?" }, 
                    attempts: 1
                )
            );
        }

        private async Task ResumeAfterNameConfirmation(IDialogContext context, IAwaitable<bool> userConfirmed)
        {
            try
            {
                var userAnswer = await userConfirmed;

                if (userAnswer)
                {
                    await context.PostAsync("Beleza então, é um prazer conhecê-lo ^^");
                }
                else
                {

                }
            }
            catch (TooManyAttemptsException e)
            {
                await context.PostAsync("Ei... cê não lembra seu próprio nome?");
            }
            catch (Exception e)
            {
                await context.PostAsync("Algo deu muito errado x.x");
            }
        }
    }

    enum OpcoesDeViagem
    {
        Viena,
        Brazilia,
        Chicago,
        Pernambuco,
        Tokio
    }
}