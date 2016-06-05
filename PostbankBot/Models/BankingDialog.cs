using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace PostbankBot.Models
{
    [LuisModel("b76c4455-7602-477b-a922-6520787eac41", "0e4fea57c762484fb2e92fcb2c144228")]
    [Serializable]
    public class BankingDialog : LuisDialog<object>
    {

        //private readonly Dictionary<string, Alarm> alarmByWhat = new Dictionary<string, Alarm>();
        //public const string DefaultAlarmWhat = "default";

        //public bool TryFindAlarm(LuisResult result, out Alarm alarm)
        //{
        //    alarm = null;
        //    string what;
        //    EntityRecommendation title;

        //    if (result.TryFindEntity(Entity_Alarm_Title, out title))
        //    {
        //        what = title.Entity;
        //    }
        //    else
        //    {
        //        what = DefaultAlarmWhat;
        //    }
        //    return this.alarmByWhat.TryGetValue(what, out alarm);
        //}

        //public const string Entity_Alarm_Title = "builtin.alarm.title";
        //public const string Entity_Alarm_Start_Time = "builtin.alarm.start_time";
        //public const string Entity_Alarm_Start_Date = "builtin.alarm.start_date";

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

        public BankingDialog(ILuisService service = null) : base(service)
        {

        }

        //[LuisIntent("builtin.intent.alarm.delete_alarm")]
        //public async Task DeleteAlarm(IDialogContext context, LuisResult result)
        //{
        //    Alarm alarm;
        //    if (TryFindAlarm(result, out alarm))
        //    {
        //        this.alarmByWhat.Remove(alarm.What);
        //        await context.PostAsync($"alarm {alarm} deleted");
        //    }
        //    else
        //    {
        //        await context.PostAsync("did not find alarm");
        //    }
        //    context.Wait(MessageReceived);
        //}

        //[LuisIntent("builtin.intent.alarm.find_alarm")]
        //public async Task FindAlarm(IDialogContext context, LuisResult result)
        //{
        //    Alarm alarm;
        //    if (TryFindAlarm(result, out alarm))
        //    {
        //        await context.PostAsync($"found alarm {alarm}");
        //    }
        //    else
        //    {
        //        await context.PostAsync("did not find alarm");
        //    }
        //    context.Wait(MessageReceived);
        //}

        //[LuisIntent("builtin.intent.alarm.set_alarm")]
        //public async Task SetAlarm(IDialogContext context, LuisResult result)
        //{
        //    EntityRecommendation title;

        //    if (!result.TryFindEntity(Entity_Alarm_Title, out title))
        //    {
        //        title = new EntityRecommendation(type: Entity_Alarm_Title) { Entity = DefaultAlarmWhat };
        //    }

        //    EntityRecommendation date;

        //    if (!result.TryFindEntity(Entity_Alarm_Start_Date, out date))
        //    {
        //        date = new EntityRecommendation(type: Entity_Alarm_Start_Date) { Entity = string.Empty };
        //    }

        //    EntityRecommendation time;

        //    if (!result.TryFindEntity(Entity_Alarm_Start_Time, out time))
        //    {
        //        time = new EntityRecommendation(type: Entity_Alarm_Start_Time) { Entity = string.Empty };
        //    }

        //    var parser = new Chronic.Parser();
        //    var span = parser.Parse(date.Entity + " " + time.Entity);

        //    if (span != null)
        //    {
        //        var when = span.Start ?? span.End;
        //        var alarm = new Alarm() { What = title.Entity, When = when.Value };
        //        this.alarmByWhat[alarm.What] = alarm;
        //        string reply = $"alarm {alarm} created";
        //        await context.PostAsync(reply);
        //    }
        //    else
        //    {
        //        await context.PostAsync("could not find time for alarm");
        //    }
        //    context.Wait(MessageReceived);
        //}

        //[LuisIntent("builtin.intent.alarm.snooze")]
        //public async Task AlarmSnooze(IDialogContext context, LuisResult result)
        //{
        //    Alarm alarm;
        //    if (TryFindAlarm(result, out alarm))
        //    {
        //        alarm.When = alarm.When.Add(TimeSpan.FromMinutes(7));
        //        await context.PostAsync($"alarm {alarm} snoozed!");
        //    }
        //    else
        //    {
        //        await context.PostAsync("did not find alarm");
        //    }

        //    context.Wait(MessageReceived);
        //}

        //[LuisIntent("builtin.intent.alarm.time_remaining")]
        //public async Task TimeRemaining(IDialogContext context, LuisResult result)
        //{
        //    Alarm alarm;

        //    if (TryFindAlarm(result, out alarm))
        //    {
        //        var now = DateTime.UtcNow;
        //        if (alarm.When > now)
        //        {
        //            var remaining = alarm.When.Subtract(DateTime.UtcNow);
        //            await context.PostAsync($"There is {remaining} remaining for alarm {alarm}.");
        //        }
        //        else
        //        {
        //            await context.PostAsync($"The alarm {alarm} expired already.");
        //        }
        //    }
        //    else
        //    {
        //        await context.PostAsync("did not find alarm");
        //    }
        //    context.Wait(MessageReceived);
        //}

        //private Alarm turnOff;

        //[LuisIntent("builtin.intent.alarm.turn_off_alarm")]
        //public async Task TurnOffAlarm(IDialogContext context, LuisResult result)
        //{
        //    if (TryFindAlarm(result, out this.turnOff))
        //    {
        //        PromptDialog.Confirm(context, AfterConfirming_TurnOffAlarm, "Are you sure?");
        //    }
        //    else
        //    {
        //        await context.PostAsync("did not find alarm");
        //        context.Wait(MessageReceived);
        //    }
        //}

        //public async Task AfterConfirming_TurnOffAlarm(IDialogContext context, IAwaitable<bool> confirmation)
        //{
        //    if (await confirmation)
        //    {
        //        this.alarmByWhat.Remove(this.turnOff.What);
        //        await context.PostAsync($"Ok, alarm {this.turnOff} disabled.");
        //    }
        //    else
        //    {
        //        await context.PostAsync("Ok! We haven't modified your alarms!");
        //    }
        //    context.Wait(MessageReceived);
        //}

        //[LuisIntent("builtin.intent.alarm.alarm_other")]
        //public async Task AlarmOther(IDialogContext context, LuisResult result)
        //{
        //    await context.PostAsync("what ?");
        //    context.Wait(MessageReceived);
        //}

        //[Serializable]
        //public sealed class Alarm : IEquatable<Alarm>
        //{
        //    public DateTime When { get; set; }
        //    public string What { get; set; }

        //    public override string ToString()
        //    {
        //        return $"[{this.What} at {this.When}]";
        //    }

        //    public bool Equals(Alarm other)
        //    {
        //        return other != null
        //            && this.When == other.When
        //            && this.What == other.What;
        //    }

        //    public override bool Equals(object other)
        //    {
        //        return Equals(other as Alarm);
        //    }

        //    public override int GetHashCode()
        //    {
        //        return this.What.GetHashCode();
        //    }
        //}
    }
}

