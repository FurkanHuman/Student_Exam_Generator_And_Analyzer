using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES_GUI;

public record Paths
{
    public static string GetConfPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SES");

    public static string GetConfFile() => Path.Combine(GetConfPath(), "Conf.c");

}
