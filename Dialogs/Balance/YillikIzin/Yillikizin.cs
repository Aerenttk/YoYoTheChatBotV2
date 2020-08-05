using System;
using System.Collections.Generic;
using System.Linq;
using demo7dialogs.Dialogs;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using AdaptiveCards;

namespace YoYoBot.Dialogs.Balance.YillikIzin
{
    public class Yillikizin : WaterfallDialog
    {
        public Yillikizin(string dialogId, IEnumerable<WaterfallStep> steps = null) : base(dialogId, steps)
        {
            Request worker = new Request();
            //Giriş
            AddStep(async (stepContext, cancellationToken) =>
            {
                return await stepContext.PromptAsync("textPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"Yıllık İzin almanız için sizden 5 farklı " +
                                                                          $"bilgi isteyeceğim bunlar:{Environment.NewLine}" +
                                                                          $"    1.Adınız{Environment.NewLine}" +
                                                                          $"    2.Soyadınız{Environment.NewLine}" +
                                                                          $"    3.Sigorta Sicil Numaranız{Environment.NewLine}" +
                                                                          $"    4.İzine çıkacağınız tarih{Environment.NewLine}" +
                                                                          $"    5.İzinden Döneceğiniz Tarih{Environment.NewLine}" +
                                                                          $"Öncelikle İsminiz Nedir?")
                    });
            });
            //İsim Alındı
            AddStep(async (stepContext, cancellationToken) =>
            {
                var response = stepContext.Result as string;
                worker.UserName = response;
                return await stepContext.NextAsync();
            });
            //Soyad Sorgusu
            AddStep(async (stepContext, cancellationToken) =>
            {
                return await stepContext.PromptAsync("textPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"Peki {worker.UserName}" +
                                                                          $"{Environment.NewLine}Soyadınız nedir?")
                    });
            });
            //Soyad Alındı
            AddStep(async (stepContext, cancellationToken) =>
            {
                var response = stepContext.Result as string;
                worker.RequestArg1 = response;
                return await stepContext.NextAsync();
            });
            //İzin başlangıç Tarihi sorgusu
            AddStep(async (stepContext, cancellationToken) =>
            {
                return await stepContext.PromptAsync("textPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"Tamamdır Sayın {worker.UserName} {worker.RequestArg1}" +
                                                                          $"{Environment.NewLine}İzine ne zaman çıkıyorsunuz?")
                    });
            });
            //İzin başlangıç tarihi alındı
            AddStep(async (stepContext, cancellationToken) =>
            {
                var response = stepContext.Result as string;
                worker.RequestArg2 = response;
                return await stepContext.NextAsync();
            });
            //İzin dönüş tarihi sorgusu
            AddStep(async (stepContext, cancellationToken) =>
            {
                return await stepContext.PromptAsync("textPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"Çok Güzel!! {Environment.NewLine} " +
                                                                          $"O zaman.. Sayın {worker.UserName} {worker.RequestArg1} " +
                                                                          $"{Environment.NewLine}{worker.RequestArg2}" +
                                                                          $" Tarihinde izine çıkarsanız ne zaman geleceksiniz?")
                    });
            });
            //İzin dönüş tarihi alındı
            AddStep(async (stepContext, cancellationToken) =>
            {
                var response = stepContext.Result as string;
                worker.RequestArg3 = response;
                return await stepContext.NextAsync();
            });
            //Sigorta sicil no sorgusu
            AddStep(async (stepContext, cancellationToken) =>
            {
                return await stepContext.PromptAsync("textPrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"Peki Sayın {worker.UserName} {worker.RequestArg1} " +
                                                                          $"{Environment.NewLine} {worker.RequestArg2}" +
                                                                          $"-{worker.RequestArg3} tarihleri arasında izinli olacaksınız" +
                                                                          $"{Environment.NewLine}Son bir bilgi kaldı.. " +
                                                                          $"{Environment.NewLine}Bana Sigorta sicil numaranı da " +
                                                                          $"verebilir misin?")
                    });
            });
            //Sigorta sicil no alındı
            AddStep(async (stepContext, cancellationToken) =>
            {
                var response = stepContext.Result as string;
                worker.RequestArg4= response;
                return await stepContext.NextAsync();
            });
            //Genel kontrol
            AddStep(async (stepContext, cancellationToken) =>
            {
                return await stepContext.PromptAsync("choicePrompt",
                    new PromptOptions
                    {
                        Prompt = stepContext.Context.Activity.CreateReply($"Son kez kontrol edelim{Environment.NewLine}" +
                                                                          $"Sayın {worker.UserName} {worker.RequestArg1}{Environment.NewLine}" +
                                                                          $"{worker.RequestArg2}-{worker.RequestArg3} tarihleri " +
                                                                          $"arasında izinli olacaksınız{Environment.NewLine}" +
                                                                          $"Ve sigorta sicil numaranız da {worker.RequestArg4}{Environment.NewLine}" +
                                                                          $"Doğru mu?"),
                        Choices = new[] { new Choice { Value = "Evet" }, new Choice { Value = "Hayır" },}.ToList()
                    });

            });
            //Hata varsa geri dönüş gerçekleştiriliyor aynı zamanda api dan gelen responsa olumlu ise user a dönüş verilecek
            AddStep(async (stepContext, cancellationToken) =>
            {
                var response = stepContext.Result as FoundChoice;
                               
                if (response.Value == "Evet")
                {
                    //Api result kontrol edilecek
                    return await stepContext.PromptAsync("textPrompt",
                        new PromptOptions
                        {
                            Prompt = stepContext.Context.Activity.CreateReply($"Yıllık izininiz alınmıştır.. İyi Günler.")
                        });
                }
                else if (response.Value == "Hayır")
                {
                    return await stepContext.BeginDialogAsync(Yillikizin.Id);
                }
                return await stepContext.NextAsync();
            });
            //Main diyalog dönüşü
            AddStep(async (stepContext, cancellationToken) =>
            {
                return await stepContext.BeginDialogAsync(MainDialog.Id);
            });
            AddStep(async (stepContext, cancellationToken) => { return await stepContext.ReplaceDialogAsync(Id); });
        }

        public static string Id => "Yillikizin";

        public static Yillikizin Instance { get; } = new Yillikizin(Id);
    }
}
