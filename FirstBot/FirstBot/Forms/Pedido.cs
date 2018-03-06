using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstBot.Forms
{
    [Serializable]
    public class Pedido
    {
        public Comidas Comidas { get; set; }
        public Bebidas Bebidas { get; set; }
        public Pagamento Pagamento { get; set; }
        public Retirada Retirada { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }

        public static IForm<Pedido> BuildForm()
        {
            var form = new FormBuilder<Pedido>();

            form.Configuration.DefaultPrompt.ChoiceStyle = ChoiceStyleOptions.Buttons;

            form.OnCompletion(async (context, pedido) => 
            {
                //Fazer coisas quando a todos os botões do formulário já tiverem sido definidos
                //Meter o loko geral;

                await context.PostAsync("Seu pedido está a caminho... obrgado");
            });

            return form.Build();
        }

    }

    public enum Bebidas
    {
        Refrigerante = 1,
        Suco
    }

    public enum Comidas
    {
        Coxinha = 1,
        Lanchenatural,

        [Terms("Pizza", "Fatia", "a pizza", "piza")]
        [Describe("Fatia de Pizza")]
        FatiaPizza,
        Fogazza,
        Quibe
    }

    public enum Pagamento
    {
        Cartao = 1,
        Dinheiro
    }

    public enum Retirada
    {
        Motoboy,
        NoBalcao
    }
}