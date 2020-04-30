using System;
using System.Collections.Generic;

using System.Text;

namespace Collector
{
   internal class Parameters
    {
        internal static string ConfigPath = @"./Collector.ini";

        internal static IniOper iniOper = new IniOper(ConfigPath);
    }
}
