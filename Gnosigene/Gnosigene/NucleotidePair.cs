//Copyright 2021 Xaratan LLC
//Released under the MIT License (see LICENSE.txt)

using System;
using System.Collections.Generic;
using System.Text;

namespace Gnosigene
{
    [Flags]
    public enum NucleotidePair : int
    {
        _ = 0,

        //Single Nucleotides
        _A = 1,
        _C = 1 << 1,
        _G = 1 << 2,
        _T = 1 << 3,

        A_ = _A << 4,
        C_ = _C << 4,
        G_ = _G << 4,
        T_ = _T << 4,

        //Pairs
        AA = A_ | _A,
        AC = A_ | _C,
        AG = A_ | _G,
        AT = A_ | _T,

        CA = C_ | _A,
        CC = C_ | _C,
        CG = C_ | _G,
        CT = C_ | _T,

        GA = G_ | _A,
        GC = G_ | _C,
        GG = G_ | _G,
        GT = G_ | _T,

        TA = T_ | _A,
        TC = T_ | _C,
        TG = T_ | _G,
        TT = T_ | _T,

        Missing = 1 << 31
    }

    public static class NucleotidePairExtensions
    {
        public static string ToDisplayString(this NucleotidePair pair)
        {
            switch (pair)
            {
                case NucleotidePair._:
                    return "--";
                case NucleotidePair._A:
                case NucleotidePair.A_:
                    return "A";
                case NucleotidePair._C:
                case NucleotidePair.C_:
                    return "C";
                case NucleotidePair._G:
                case NucleotidePair.G_:
                    return "G";
                case NucleotidePair._T:
                case NucleotidePair.T_:
                    return "T";
                case NucleotidePair.AA:
                    return "AA";
                case NucleotidePair.AC:
                    return "AC";
                case NucleotidePair.AG:
                    return "AG";
                case NucleotidePair.AT:
                    return "AT";
                case NucleotidePair.CA:
                    return "CA";
                case NucleotidePair.CC:
                    return "CC";
                case NucleotidePair.CG:
                    return "CG";
                case NucleotidePair.CT:
                    return "CT";
                case NucleotidePair.GA:
                    return "GA";
                case NucleotidePair.GC:
                    return "GC";
                case NucleotidePair.GG:
                    return "GG";
                case NucleotidePair.GT:
                    return "GT";
                case NucleotidePair.TA:
                    return "TA";
                case NucleotidePair.TC:
                    return "TC";
                case NucleotidePair.TG:
                    return "TG";
                case NucleotidePair.TT:
                    return "TT";
                case NucleotidePair.Missing:
                    return "??";
                default:
                    return "ERROR";
            }
        }
    }
}
