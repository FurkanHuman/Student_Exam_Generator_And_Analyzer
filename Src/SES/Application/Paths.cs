namespace Application;

public record Paths
{
    public static string GetConfPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SES");

    public static string GetConfFile() => Path.Combine(GetConfPath(), "Conf.c");

}
