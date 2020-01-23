using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pabx.Model
{
    public class AzureSubscriptionKey
    {
        public string SubscriptionKey { get; set; }

        public static async Task<string> LoadFromJson(string jsonFilename = "../../../azure-credentials.json")
        {
            AzureSubscriptionKey credentials;

            using (FileStream fs = File.OpenRead(jsonFilename))
            {
                credentials = await JsonSerializer.DeserializeAsync<AzureSubscriptionKey>(fs);
            }

            return credentials.SubscriptionKey;
        }
    }
}
