//Copyright 2021 Xaratan LLC
//Released under the MIT License (see LICENSE.txt)

using System;
using System.Collections.Generic;
using System.Text;

namespace Gnosigene
{
    public static class Genome
    {
        private class SingleNucleotidePolymorphismComparer : IComparer<SingleNucleotidePolymorphism>
        {
            public int Compare(SingleNucleotidePolymorphism x, SingleNucleotidePolymorphism y)
            {
                var chromosome = x.Chromosome.CompareTo(y.Chromosome);

                if (chromosome != 0)
                {
                    return chromosome;
                }
                else
                {
                    var position = x.Postion.CompareTo(y.Postion);

                    if (position == 0)
                    {
                        return x.ReferenceIdentifier.CompareTo(y.ReferenceIdentifier);
                    }
                    else
                    {
                        return position;
                    }
                }
            }
        }


        public static List<SingleNucleotidePolymorphism> Combine(List<List<SingleNucleotidePolymorphism>> sources)
        {
            List<SingleNucleotidePolymorphism> results = new List<SingleNucleotidePolymorphism>();

            var comparer = new SingleNucleotidePolymorphismComparer();

            HashSet<string> rsids = new HashSet<string>();

            foreach (var source in sources)
            {
                foreach (var snp in source)
                {
                    if (rsids.Contains(snp.ReferenceIdentifier))
                    {
                        continue; //Don't put in duplicate SNPs
                    }

                    var location = results.BinarySearch(snp, comparer);

                    if (location < 0)
                    {
                        results.Insert(~location, snp);
                    }
                    else
                    {
                        Console.WriteLine("Found duplicate.");
                    }

                    rsids.Add(snp.ReferenceIdentifier);
                }
            }

            return results;
        }
    }
}
