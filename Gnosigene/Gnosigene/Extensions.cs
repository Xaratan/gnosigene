//Copyright 2021 Xaratan LLC
//Released under the MIT License (see LICENSE.txt)

using System;
using System.Collections.Generic;
using System.Text;

namespace Gnosigene.Extensions
{
    public static class Extensions
    {
        public static bool IsNotNull(this object obj)
        {
            return !(obj is null);
        }
    }
}
