# Pabx - Notas da versão *alpha* (pt-BR)
Esse código C#/.NET Core permite executar uma aplicação de console que transforma texto escrito em fala utilizando os serviços do Microsoft Azure. Eu utilizo esse código para gerar textos de boa qualidade para URA.
Para utilizar esse código, é preciso configurar dois arquivos:
```
azure-credentials.json
texts-to-speech.json
```
O ```azure-credentials.json``` não é fornecido, então você precisará criar um arquivo com exatamente o mesmo nome e a seguinte estrutura:
```
{
  "SubscriptionKey": "<Sua chave de assinatura do serviço de conversão de texto em fala do Azure>"
}
```
Já o ```texts-to-speech.json``` é fornecido e já possui um exemplo de texto que utilizei para a empresa para a qual trabalho. A estrutura desse arquivo é a seguinte:
```
{
  "Texts": {
    "<Nome do Arquivo>": "<Texto a converter em fala>",
    "<Nome do Arquivo 2>": "<Texto a converter em fala>",
    "<Nome do Arquivo 3>": "<Texto a converter em fala>"
  },
  "AzureVoice":"pt-BR-HeloisaRUS"
}
```
O código criará um diretório ```Output``` na mesma pasta do executável contendo todos os arquivos especificados em ```Text``` com a extensão ```.wav```. 

*Obs.: Não é necessário incluir a extensão ```.wav``` no ```texts-to-speech.json```, porém utilize nomes simples com caracteres compatíveis com o sistema de arquivos do seu sistema operacional. Esse código ainda não faz tratamento de erros e exceções como deveria.*

Quanto ao campo ```AzureVoice```, você pode selecionar outras vozes na [documentação do Azure](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support#standard-voices), se disponíveis (no momento em que escrevo esse texto, há apenas essa opção disponível).

## Dica

Como mencionado anteriormente, o Azure gera arquivos de áudio de alta qualidade em [formato WAVE](https://pt.wikipedia.org/wiki/WAV), o que provavelmente não será compatível com sua URA. Muitas URAs utilizam o formato de GSM, um formato de áudio comprimido que oferece qualidade aceitável para ligações telefônicas. Você pode converter de ```.wav``` para ```.gsm``` gratuitamente com o [Convertio](https://convertio.co/pt/wav-gsm/).
