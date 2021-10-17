//Copyright 2021 Xaratan LLC
//Released under the MIT License (see LICENSE.txt)

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gnosigene.FileFormats
{
    public class SNPListWriter : IDisposable
    {
        StreamWriter writer;

        //bool HeaderWritten { get; } = false;

        public SNPListWriter(Stream stream)
        {
            writer = new StreamWriter(stream, Encoding.UTF8, 4096, true);
        }

        public void Dispose()
        {
            writer.Close();
        }

        public void WriteNext(SingleNucleotidePolymorphism snp)
        {
//            if (!HeaderWritten)
//            {
//                var header = @"# Gnosigene SNP File
//# $version: 1";
//            }

            var line = $"{snp.ReferenceIdentifier}\t{HumanChromosomeHelpers.ToDisplayString(snp.Chromosome)}\t{snp.Postion}\t{NucleotidePairExtensions.ToDisplayString(snp.Nucleotides)}";

            writer.WriteLine(line);
        }

        public void WriteAll(IEnumerable<SingleNucleotidePolymorphism> snps)
        {
            foreach (var snp in snps)
            {
                WriteNext(snp);
            }
        }
    }
}
