//Copyright 2021 Xaratan LLC
//Released under the MIT License (see LICENSE.txt)

using System;
using System.Collections.Generic;
using System.Text;

namespace Gnosigene
{
    [Flags]
    public enum Nucleotide : int
    {
        _ = 0,
        A = 1,
        C = 1 << 1,
        G = 1 << 2,
        T = 1 << 3,
    }
}
