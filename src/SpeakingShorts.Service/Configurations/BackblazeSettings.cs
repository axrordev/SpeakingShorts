namespace SpeakingShorts.Service.Configurations;

public class BackblazeSettings
{
public string ApplicationKeyId { get; set; } = string.Empty;
public string ApplicationKey { get; set; } = string.Empty;
public string BucketName { get; set; } = string.Empty;
public string ServiceUrl { get; set; } = string.Empty;
public string PublicUrlBase { get; set; } = string.Empty;
public int MaxFileSizeInMb { get; set; } = 100;
public List<string> AllowedAudioExtensions { get; set; } = new();
}
