﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvest.Net
{
    internal static class Extensions
    {
        internal static long? ParseNullableLong(this string input)
        {
            long output;
            if (long.TryParse(input, out output))
                return output;
            return null;
        }

        internal static string ToYesNo(this bool input)
        {
            return input ? "yes" : "no";
        }
    }
}
