using Microsoft.Extensions.Configuration;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace aziotedge_modinit;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddCommandLine(args)
            .Build();

        string connectionString = configuration.GetConnectionString("IoTEdge")!;

        if (string.IsNullOrEmpty(connectionString)) PrintUsageAndExit("ConnectionString:IoTEdge");
        IDictionary<string, string> map = connectionString.ToDictionary(';', '=');
        if (!map.TryGetValue("HostName", out string? hostname)) PrintUsageAndExit("HostName");
        if (!map.TryGetValue("DeviceId", out string? deviceId)) PrintUsageAndExit("DeviceId");        
        if (!map.TryGetValue("SharedAccessKey", out string? sasKey)) PrintUsageAndExit("SharedAccessKey");
        string modId = configuration.GetValue<string>("moduleId")!;

        if (string.IsNullOrEmpty(hostname) 
            || string.IsNullOrEmpty(deviceId) 
            || string.IsNullOrEmpty(sasKey) 
            || string.IsNullOrEmpty(modId)) PrintUsageAndExit("empty connection string values");
        else await InitModuleAsync(hostname, deviceId, sasKey, modId, modId.StartsWith('$'));
    }

    private static async Task InitModuleAsync(string hostname, string edgeId, string sasKey, string moduleId, bool withEtag = false)
    {
        const string Api_Version_2021_04_12 = "api-version=2021-04-12";

        string putUrl = $"https://{hostname}/devices/{edgeId}/modules/{moduleId}?{Api_Version_2021_04_12}";
        using HttpClient putClient = new();
        HttpRequestMessage reqPut = new(HttpMethod.Put, putUrl);
        reqPut.Headers.Add(HttpRequestHeader.Authorization.ToString(), Sas.GetToken(hostname, sasKey));
        if (withEtag)
        {
            reqPut.Headers.IfMatch.Add(new System.Net.Http.Headers.EntityTagHeaderValue("\"*\""));
        }

        Module modIdentity = new()
        {
            Authentication = new Authentication { Type = "sas", SymmetricKey = new SymmetricKey { PrimaryKey = null } },
            ModuleId = moduleId,
            ManagedBy = "IotEdge",
            DeviceId = edgeId,
            ConnectionState = "Disconnected"
        };

        string modJson = JsonSerializer.Serialize(modIdentity);

        reqPut.Content = new StringContent(modJson, System.Text.Encoding.UTF8, "application/json");
        HttpResponseMessage respPut = await putClient.SendAsync(reqPut);
        if (!respPut.IsSuccessStatusCode)
        {
            if (respPut.StatusCode == HttpStatusCode.Conflict) 
            { 
                await InitModuleAsync(hostname, edgeId, sasKey, moduleId, true);
            }
            else
            {
                await Console.Out.WriteLineAsync($"[ERROR] {respPut.StatusCode} {respPut.ReasonPhrase}");
            }
        }
        else
        {
            string putRespJson = await respPut.Content.ReadAsStringAsync();
            JsonDocument doc = JsonDocument.Parse(putRespJson);
            string respJson = JsonSerializer.Serialize(doc, new JsonSerializerOptions() { WriteIndented = true });
            await Console.Out.WriteLineAsync(respJson);
        }
    }

    private static void PrintUsageAndExit(string err = "")
    {
        Console.WriteLine($"\n{Assembly.GetExecutingAssembly().GetName().Name} tool");
        Console.WriteLine($" [ERROR] {err}");
        Console.WriteLine(" requires IoTEdge device connection string, and module id.");
        Console.WriteLine(" eg. aziotedge-modinit --ConnectionStrings:IoTEdge=<connectionstring> --moduleId=<$edgeHub|custom>");
        Console.WriteLine("exiting. \n\n");
        Environment.Exit(0);
    }
}