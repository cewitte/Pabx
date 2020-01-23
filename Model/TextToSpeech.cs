using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace Pabx.Model
{
    public class TextToSpeech
    {
        public Dictionary<string,string> Texts { get; set; }
        public string AzureVoice { get; set; }

        private static string _path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        private static string _outputDir = "Output/";

        public static async Task<TextToSpeech> TextToSpeechFactory(string jsonFilename = "../../../texts-to-speech.json")
        {
            TextToSpeech tts;

            Console.WriteLine(_path);

            try
            {
                using (FileStream fs = File.OpenRead(jsonFilename))
                {
                    tts = await JsonSerializer.DeserializeAsync<TextToSpeech>(fs);
                    Console.WriteLine(tts);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return tts;
        }

        public static async Task SynthesisToAudioFileAsync(TextToSpeech tts, string azureSubscriptionKey = null, string azureRegion = null)
        {
            if (azureSubscriptionKey == null)
            {
                azureSubscriptionKey = await AzureSubscriptionKey.LoadFromJson();
            }

            if (azureRegion == null)
            {
                azureRegion = Region.USWest.ToString();
            }
            
            var config = SpeechConfig.FromSubscription(azureSubscriptionKey, azureRegion);
            config.SpeechSynthesisVoiceName = tts.AzureVoice;

            String filename;
            Directory.CreateDirectory(_outputDir);

            foreach (var item in tts.Texts)
            {
                // WAV is the standard format and file extension has to be checked.
                if (string.IsNullOrEmpty(new FileInfo(item.Key).Extension))
                {
                    filename = string.Concat(_outputDir, item.Key, ".wav");
                }
                else
                {
                    filename = string.Concat(_outputDir, item.Key);
                }

                using (var fileOutput = AudioConfig.FromWavFileOutput(filename))
                {
                    using (var synthesizer = new SpeechSynthesizer(config, fileOutput))
                    {
                        var result = await synthesizer.SpeakTextAsync(item.Value);

                        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                        {
                            Console.WriteLine($"Speech synthesized to [{filename}] for text [{item.Value}]");
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
        }
    }
}
