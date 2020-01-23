using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using System.Collections.Generic;
using Pabx.Model;

namespace Pabx
{

    public class PabxRecordings
    {
        // public string InputFileName { get; set; }
        public string OutputFileName { get; set; }
        public string TextToSpeech { get; set; }
    }

    class Program
    {
        public static async Task SynthesisToAudioFileAsync(string filename, string text)
        {
            var config = SpeechConfig.FromSubscription("24af634c423b42de910d4c84c9607f0f", Region.USWest.ToString());
            config.SpeechSynthesisVoiceName = "pt-BR-HeloisaRUS";

            if (filename == null)
            {
                filename = "helloworld.wav";
            }

            if (text == null)
            {
                text = text = "Olá! A AlphaGraphics Faria Lima 24 horas agradece a sua ligação. Entre com o ramal desejado, ou, digite: 1, para solicitar um orçamento. Para outros assuntos, por favor aguarde na linha.";
            }             
            
            using (var fileOutput = AudioConfig.FromWavFileOutput(filename))
            {
                using (var synthesizer = new SpeechSynthesizer(config, fileOutput))
                {
                    var result = await synthesizer.SpeakTextAsync(text);

                    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    {
                        Console.WriteLine($"Speech synthesized to [{filename}] for text [{text}]");
                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }
                    }
                }
            }


        }

        public static async Task MultipleSythesisToAudioFileAsync(List<PabxRecordings> recordings)
        {
            foreach (var item in recordings)
            {
                await SynthesisToAudioFileAsync(item.OutputFileName, item.TextToSpeech);
            }
        }

        public static void Main(string[] args)
        {
            TextToSpeech tts = TextToSpeech.TextToSpeechFactory().GetAwaiter().GetResult();

            TextToSpeech.SynthesisToAudioFileAsync(tts).Wait();



            //var addressRec = new PabxRecordings
            //{
            //    TextToSpeech = "Nosso endereço é: Avenida Brigadeiro Faria Lima, número dois nove quatro um, esquina com a rua Jerônimo da Veiga.",
            //    OutputFileName = "FL-Address.wav"
            //};

            //var parkingRec = new PabxRecordings
            //{
            //    TextToSpeech = "Temos estacionamento conveniado. Ele está localizado na rua Jerônimo da Veiga número, quatro meia um, bem ao lado da AlphaGraphics. Obtendo o selo de desconto ao pagar seu serviço no caixa, você paga apenas 10 reais pelo primeiro período.",
            //    OutputFileName = "FL-Parking.wav"
            //};

            //var whatsAppRec = new PabxRecordings
            //{
            //    TextToSpeech = "Por favor, anote o nosso WhatsApp. Ele é igual ao nosso número fixo: onze, trinta sete oito, quatro nove, zero zero. Vou repetir. Onze, trinta sete oito, quatro nove, zero zero.",
            //    OutputFileName = "FL-WhatsApp.wav"
            //};

            //var emailRec = new PabxRecordings
            //{
            //    TextToSpeech = "O nosso email é: contato arroba, A G 24 horas, ponto com, ponto br.",
            //    OutputFileName = "FL-Email.wav"
            //};

            //var workingHoursRec = new PabxRecordings
            //{
            //    TextToSpeech = "Nossa unidade é 24 horas todos os dias, de segunda a domingo, inclusive aos feriados. Só fechamos alguns dias durante o natal, ano novo e carnaval. Estamos de portas abertas 24 horas em todos os demais dias do ano.",
            //    OutputFileName = "FL-WorkingHours.wav"
            //};

            //var orderStatusRec = new PabxRecordings
            //{
            //    TextToSpeech = "Se você deseja uma posição sobre o seu material, por favor certifique-se de ter em mãos: o número da Ordem de Produção e o nome da pessoa que lhe atendeu. Obrigada!",
            //    OutputFileName = "FL-OrderStatus.wav"
            //};

            //var recordings = new List<PabxRecordings>();

            //recordings.Add(addressRec);
            //recordings.Add(parkingRec);
            //recordings.Add(whatsAppRec);
            //recordings.Add(emailRec);
            //recordings.Add(workingHoursRec);
            //recordings.Add(orderStatusRec);

            //MultipleSythesisToAudioFileAsync(recordings).Wait();


            // Console.WriteLine("Hello World!");
            //SynthesisToAudioFileAsync().Wait();

            // Console.ReadKey();


        }
    }
}
