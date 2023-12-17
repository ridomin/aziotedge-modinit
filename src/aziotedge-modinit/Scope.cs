using System.Text.Json.Serialization;

namespace aziotedge_modinit
{
    public class Scope
    {
        [JsonPropertyName("devices")]
        public List<Device>? Devices { get; set; }

        [JsonPropertyName("modules")]
        public List<Module>? Modules { get; set; } 

        [JsonPropertyName("continuationLink")]
        public string? ContinuationLink { get; set; }

        public Scope()
        {
            Devices = new List<Device>();
            Modules = new List<Module>();
        }
    }

    public class Capabilities
    {
        [JsonPropertyName("iotEdge")]
        public bool? IotEdge { get; set; }
    }

    public class Device
    {
        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }

        [JsonPropertyName("generationId")]
        public string? GenerationId { get; set; }

        [JsonPropertyName("etag")]
        public string? Etag { get; set; }

        [JsonPropertyName("connectionState")]
        public string? ConnectionState { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("statusReason")]
        public string? StatusReason { get; set; }

        [JsonPropertyName("connectionStateUpdatedTime")]
        public DateTime? ConnectionStateUpdatedTime { get; set; }

        [JsonPropertyName("statusUpdatedTime")]
        public DateTime? StatusUpdatedTime { get; set; }

        [JsonPropertyName("lastActivityTime")]
        public DateTime? LastActivityTime { get; set; }

        [JsonPropertyName("cloudToDeviceMessageCount")]
        public int? CloudToDeviceMessageCount { get; set; }

        [JsonPropertyName("authentication")]
        public Authentication? Authentication { get; set; }

        [JsonPropertyName("capabilities")]
        public Capabilities? Capabilities { get; set; }

        [JsonPropertyName("deviceScope")]
        public string? DeviceScope { get; set; }

        [JsonPropertyName("parentScopes")]
        public List<string>? ParentScopes { get; set; }
    }


    public class Authentication
    {

        [JsonPropertyName("symmetricKey")]
        public SymmetricKey? SymmetricKey { get; set; }


        [JsonPropertyName("x509Thumbprint")]
        public X509Thumbprint? X509Thumbprint { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }

    public class Module
    {
        [JsonPropertyName("moduleId")]
        public string? ModuleId { get; set; }

        [JsonPropertyName("managedBy")]
        public string? ManagedBy { get; set; }

        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }

        [JsonPropertyName("generationId")]
        public string? GenerationId { get; set; }

        [JsonPropertyName("etag")]
        public string? Etag { get; set; }

        [JsonPropertyName("connectionState")]
        public string? ConnectionState { get; set; }

        [JsonPropertyName("connectionStateUpdatedTime")]
        public DateTime? ConnectionStateUpdatedTime { get; set; }

        [JsonPropertyName("lastActivityTime")]
        public DateTime? LastActivityTime { get; set; }


        [JsonPropertyName("cloudToDeviceMessageCount")]
        public int? CloudToDeviceMessageCount { get; set; }


        [JsonPropertyName("authentication")]
        public Authentication? Authentication { get; set; }
    }

    public class SymmetricKey
    {
        [JsonPropertyName("primaryKey")]
        public string? PrimaryKey { get; set; }


        [JsonPropertyName("secondaryKey")]
        public string? SecondaryKey { get; set; }
    }

    public class X509Thumbprint
    {
        [JsonPropertyName("primaryThumbprint")]
        public string? PrimaryThumbprint { get; set; }

        [JsonPropertyName("secondaryThumbprint")]
        public string? SecondaryThumbprint { get; set; }
    }
}