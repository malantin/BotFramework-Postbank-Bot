using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace PostbankBot.Models
{
    [Serializable]

    public class EchoDialog : IDialog<object>

    {

        public async Task StartAsync(IDialogContext context)

        {

            context.Wait(MessageReceivedAsync);

        }



        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<Message> argument)

        {

            var message = await argument;

            await context.PostAsync("You said: " + message.Text);

            context.Wait(MessageReceivedAsync);

        }

    }


}