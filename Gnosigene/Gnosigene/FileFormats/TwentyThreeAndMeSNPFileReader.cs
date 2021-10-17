//Copyright 2021 Xaratan LLC
//Released under the MIT License (see LICENSE.txt)

using Gnosigene.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gnosigene.FileFormats
{
    public class TwentyThreeAndMeSNPFileReader : IDisposable
    {
        Stream Source { get; }
        StreamReader Reader { get; }

        public TwentyThreeAndMeSNPFileReader(Stream stream)
        {
            Source = stream;
            Reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true);
        }

        private SingleNucleotidePolymorphism ParseLine(string line)
        {
            if (line.Length == 0)
                return null;

            var firstChar = line[0];

            switch (firstChar)
            {
                case '#':
                    return null;
                default:
                    {
                        var parts = line.Split('\t');

                        if (parts.Length != 4)
                            return null;

                        var rsid = parts[0];
                        var chromosomeText = parts[1];
                        var positionText = parts[2];
                        var genotypeText = parts[3];

                        HumanChromosome humanChromosome;

                        switch (chromosomeText)
                        {
                            case "1":
                                humanChromosome = HumanChromosome.Chromosome1;
                                break;
                            case "2":
                                humanChromosome = HumanChromosome.Chromosome2;
                                break;
                            case "3":
                                humanChromosome = HumanChromosome.Chromosome3;
                                break;
                            case "4":
                                humanChromosome = HumanChromosome.Chromosome4;
                                break;
                            case "5":
                                humanChromosome = HumanChromosome.Chromosome5;
                                break;
                            case "6":
                                humanChromosome = HumanChromosome.Chromosome6;
                                break;
                            case "7":
                                humanChromosome = HumanChromosome.Chromosome7;
                                break;
                            case "8":
                                humanChromosome = HumanChromosome.Chromosome8;
                                break;
                            case "9":
                                humanChromosome = HumanChromosome.Chromosome9;
                                break;
                            case "10":
                                humanChromosome = HumanChromosome.Chromosome10;
                                break;
                            case "11":
                                humanChromosome = HumanChromosome.Chromosome11;
                                break;
                            case "12":
                                humanChromosome = HumanChromosome.Chromosome12;
                                break;
                            case "13":
                                humanChromosome = HumanChromosome.Chromosome13;
                                break;
                            case "14":
                                humanChromosome = HumanChromosome.Chromosome14;
                                break;
                            case "15":
                                humanChromosome = HumanChromosome.Chromosome15;
                                break;
                            case "16":
                                humanChromosome = HumanChromosome.Chromosome16;
                                break;
                            case "17":
                                humanChromosome = HumanChromosome.Chromosome17;
                                break;
                            case "18":
                                humanChromosome = HumanChromosome.Chromosome18;
                                break;
                            case "19":
                                humanChromosome = HumanChromosome.Chromosome19;
                                break;
                            case "20":
                                humanChromosome = HumanChromosome.Chromosome20;
                                break;
                            case "21":
                                humanChromosome = HumanChromosome.Chromosome21;
                                break;
                            case "22":
                                humanChromosome = HumanChromosome.Chromosome22;
                                break;
                            case "X":
                                humanChromosome = HumanChromosome.ChromosomeX;
                                break;
                            case "Y":
                                humanChromosome = HumanChromosome.ChromosomeY;
                                break;
                            case "MT":
                                humanChromosome = HumanChromosome.ChromosomeMitochondria;
                                break;
                            default:
                                throw new Exception($"Error in 23andMe Raw SNP File. Unknown chromosome '{chromosomeText}'.");
                        }

                        if (!long.TryParse(positionText, out var position))
                        {
                            throw new Exception($"Error in 23andMe Raw SNP File. Invalid position '{positionText}'.");
                        }

                        if (genotypeText.Length > 2)
                            throw new Exception($"Error in 23andMe Raw SNP File. Invalid genotype '{genotypeText}'.");

                        NucleotidePair genotype = 0;

                        foreach (var nucleotide in genotypeText)
                        {
                            genotype = (NucleotidePair)((int)genotype << 4);

                            switch (nucleotide)
                            {
                                case 'A':
                                    genotype |= NucleotidePair._A;
                                    break;
                                case 'C':
                                    genotype |= NucleotidePair._C;
                                    break;
                                case 'G':
                                    genotype |= NucleotidePair._G;
                                    break;
                                case 'T':
                                    genotype |= NucleotidePair._T;
                                    break;
                                case '-':
                                    genotype |= NucleotidePair._;
                                    break;
                            }
                        }

                        return new SingleNucleotidePolymorphism(rsid, humanChromosome, position, genotype);
                    }
            }
        }

        public SingleNucleotidePolymorphism GetNext()
        {
            do
            {
                var line = Reader.ReadLine();

                if (line is null)
                {
                    return null;
                }
                else
                {
                    var snp = ParseLine(line);

                    if (snp.IsNotNull())
                    {
                        return snp;
                    }
                }

            } while (true);
        }

        public void Dispose()
        {
            Reader.Dispose();
        }
    }
}
