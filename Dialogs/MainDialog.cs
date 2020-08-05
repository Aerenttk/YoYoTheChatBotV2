using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using YoYoBot.Dialogs.Balance.YillikIzin;

namespace demo7dialogs.Dialogs
{
    public class MainDialog : WaterfallDialog
    {
        public MainDialog(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            AddStep(async (stepContext, cancellationToken) =>
            {
                return await stepContext.PromptAsync("choicePrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply("Selam Ben YoYo Sizin sanal asistanınızım. Size BOSCH BuP1 İçerisinde yardımcı olabilirim. Şu an sizin için yıllık izin alabilirim. Bunun için Yıllık İzin yazabilirsin"),
                        Choices = new[] {new Choice {Value = "Yıllık İzin"},}.ToList()
                    });
            });
            AddStep(async (stepContext, cancellationToken) =>
            {
                var response = (stepContext.Result as FoundChoice)?.Value;

                if (response == "Yıllık İzin")
                {
                    return await stepContext.BeginDialogAsync(Yillikizin.Id);
                }
                return await stepContext.NextAsync();
            });

            AddStep(async (stepContext, cancellationToken) => { return await stepContext.ReplaceDialogAsync(Id); });
        }


        public static string Id => "mainDialog";

        public static MainDialog Instance { get; } = new MainDialog(Id);
    }
}