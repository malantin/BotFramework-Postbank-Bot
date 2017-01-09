using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Postbank;

namespace PostbankBot.Models
{
    [LuisModel("", "")]
    [Serializable]
    public class BankingDialog : LuisDialog<object>
    {


        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var message = "... *sleeping*";
            if (result.Query.ToLower().Contains("hi") || result.Query.ToLower().Contains("hi ") || result.Query.ToLower().Contains("hi!"))
            {
                message = "Hi! Welcome at Postbank! How may I assist you?";
            }
            else
            {
                //string message = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
                message = $"Sorry I did not understand: \"" + result.Query + "\" You can say \"Hi\" to start a dialog.";
            }
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("ShowBalance")]
        public async Task ShowBalance(IDialogContext context, LuisResult result)
        {
            PostbankClient client = new PostbankClient("Hackathon5", "test12345");

            try
            {
                client = await client.GetAccountInformationAsnyc();
                EntityRecommendation ent = null;
                if (result.TryFindEntity("AccountNumber", out ent))
                {
                    var account = client.IDInfo.accounts.Where(i => i.iban.ToLower() == ent.Entity).First();
                    await context.PostAsync($"The balance for your {account.productDescription} account with IBAN {account.iban} is {account.amount} {account.currency}.");
                }
                else
                {
                    await context.PostAsync($"For what account do you want to display your balance? You have got {client.IDInfo.accounts.Count} account(s) with the following IBAN:");
                    foreach (var item in client.IDInfo.accounts)
                    {
                        await context.PostAsync(item.iban);
                    }
                    await context.PostAsync("Which account do you want to use?");
                }
            }
            catch (Exception ex)
            {
                await context.PostAsync($"I am sorry. :( There has been some problems in getting your account information. Please try again later. The error message is {ex.Message}.");
            }
            finally
            {
                context.Wait(MessageReceived);
            }

        }

        [LuisIntent("ShowTransactions")]
        public async Task ShowTransactions(IDialogContext context, LuisResult result)
        {
            PostbankClient client = new PostbankClient("Hackathon5", "test12345");

            try
            {
                client = await client.GetAccountInformationAsnyc();
                EntityRecommendation accountNumber = null;
                EntityRecommendation amount = null;
                result.TryFindEntity("AccountNumber", out accountNumber);
                result.TryFindEntity("builtin.number", out amount);
                if (accountNumber != null && amount != null)
                {
                    var account = client.IDInfo.accounts.Where(i => i.iban.ToLower() == accountNumber.Entity).First();
                    var amountNumber = int.Parse(amount.Entity);
                    var transactions = await client.GetTransactionForAccount(account);
                    await context.PostAsync($"The balance for your {account.productDescription} account with IBAN {account.iban} is {account.amount} {account.currency}. These are the last {amount.Entity} transactions:");
                    for (int i = 0; i < amountNumber; i++)
                    {
                        if (transactions.content[i] != null)
                        {
                            var currentTrans = transactions.content[i];
                            await context.PostAsync($"{DateTime.FromFileTime(currentTrans.valutaDate)} {currentTrans.transactionType}: {currentTrans.amount} {currentTrans.currency} {String.Join("", currentTrans.purpose)}");
                        }
                    }
                }
                else
                {
                    await context.PostAsync($"For which account do you want to display your transaction? You have got {client.IDInfo.accounts.Count} account(s) with the following IBAN:");
                    foreach (var item in client.IDInfo.accounts)
                    {
                        await context.PostAsync(item.iban);
                    }
                    await context.PostAsync("Which account do you want to use? Please state the IBAN and the number of transactions you want to see.");
                }
            }
            catch (Exception ex)
            {
                await context.PostAsync($"I am sorry. :( There has been some problems in getting your account information. Please try again later. The error message is {ex.Message}.");
            }
            finally
            {
                context.Wait(MessageReceived);
            }

        }

        public BankingDialog(ILuisService service = null) : base(service)
        {

        }

    }
}

