//Copyright 2021 Xaratan LLC
//Released under the MIT License (see LICENSE.txt)

using CommandLine;
using Gnosigene;
using Gnosigene.Extensions;
using Gnosigene.FileFormats;
using System;
using System.Collections.Generic;
using System.IO;

namespace GnosigeneCLI
{
    [Verb("scan", HelpText = "Scan a genome for a SNP.")]
    public class ScanOptions
    {
        [Option('f', "format", Default = "snplist", HelpText = "Input file format. Supports: 23andMe, AncestryDNA, and SNPList Raw SNP file (23andme, ancestrydna, snplist).")]
        public string Format { get; set; }

        [Option('s', "source", Required = true)]
        public string Source { get; set; }

        [Option('c', "chromosome")]
        public string Chromosome { get; set; }

        [Option('a', "start", Default = 0)]
        public long StartPosition { get; set; }

        [Option('z', "end", Default = long.MaxValue)]
        public long EndPosition { get; set; }
    }
    
    [Verb("recode", HelpText = "Changes the encoding and combines one or more SNP files.")]
    public class RecodeOptions
    {
        [Option("1p", HelpText = "Path for file 1.")]
        public string Path1 { get; set; }

        [Option("1f", HelpText = "File format for file 1.")]
        public string Format1 { get; set; }

        [Option("2p", HelpText = "Path for file 2.")]
        public string Path2 { get; set; }

        [Option("2f", HelpText = "File format for file 2.")]
        public string Format2 { get; set; }

        [Option("3p", HelpText = "Path for file 3.")]
        public string Path3 { get; set; }

        [Option("3f", HelpText = "File format for file 3.")]
        public string Format3 { get; set; }

        [Option("op", HelpText = "Output file path.")]
        public string OutputPath { get; set; }

        [Option("of", HelpText = "Output file format.")]
        public string OutputFormat { get; set; }
    }

    class Program
    {
        static int Main(string[] args)
        {
            return CommandLine.Parser.Default.ParseArguments<ScanOptions, RecodeOptions>(args)
                .MapResult(
                    (ScanOptions opts) => Scan(opts),
                    (RecodeOptions opts) => Recode(opts),
                    (errs => HandleErrors(errs))
                );
        }

        private static int HandleErrors(IEnumerable<Error> errs)
        {
            Console.WriteLine("Errors");

            return 1;
        }

        private static int Recode(RecodeOptions options)
        {
            List<SingleNucleotidePolymorphism> LoadFile(string path, string format)
            {
                List<SingleNucleotidePolymorphism> results = new List<SingleNucleotidePolymorphism>();

                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    switch (format)
                    {
                        case "23andme":
                            {
                                using (var twentyReader = new TwentyThreeAndMeSNPFileReader(stream))
                                {
                                    SingleNucleotidePolymorphism snp = null;

                                    do
                                    {
                                        snp = twentyReader.GetNext();

                                        if (snp.IsNotNull())
                                        {
                                            results.Add(snp);
                                        }

                                    } while (snp.IsNotNull());
                                }
                            }
                            break;
                        case "ancestry":
                            {
                                using (var ancestryReader = new AncestryDNASNPFileReader(stream))
                                {
                                    SingleNucleotidePolymorphism snp = null;

                                    do
                                    {
                                        snp = ancestryReader.GetNext();

                                        if (snp.IsNotNull())
                                        {
                                            results.Add(snp);
                                        }

                                    } while (snp.IsNotNull());
                                }
                            }
                            break;
                        case "snplist":
                            {
                                using (var snplist = new SNPListReader(stream))
                                {
                                    SingleNucleotidePolymorphism snp = null;

                                    do
                                    {
                                        snp = snplist.GetNext();

                                        if (snp.IsNotNull())
                                        {
                                            results.Add(snp);
                                        }

                                    } while (snp.IsNotNull());
                                }
                            }
                            break;
                        default:
                            throw new Exception();
                    }
                }

                return results;
            }

            try
            {
                if (options.OutputPath is null)
                    return 1;

                var snpSets = new List<List<SingleNucleotidePolymorphism>>();

                if (options.Path1.IsNotNull())
                {
                    snpSets.Add(LoadFile(options.Path1, options.Format1));
                }

                if (options.Path2.IsNotNull())
                {
                    snpSets.Add(LoadFile(options.Path2, options.Format2));
                }

                if (options.Path3.IsNotNull())
                {
                    snpSets.Add(LoadFile(options.Path3, options.Format3));
                }

                var combined = Genome.Combine(snpSets);

                using (var outStream = new FileStream(options.OutputPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (var writer = new SNPListWriter(outStream))
                    {
                        writer.WriteAll(combined);
                    }
                }

                Console.WriteLine(snpSets.Count);

                Console.WriteLine();

                foreach (var snpSet in snpSets)
                {
                    Console.WriteLine(snpSet.Count);
                }

                return 0;
            }
            catch
            {
                return 1;
            }
        }

        private static int Scan(ScanOptions options)
        {
            if (!HumanChromosomeHelpers.TryParse(options.Chromosome, out var humanChromosome))
            {
                Console.WriteLine("Couldn't interpret the provided chromosome.");
                return 1;
            }


            using (var fs = new FileStream(options.Source,FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                switch (options.Format)
                {
                    case "23andMe":
                        {
                            using (var rawFile = new TwentyThreeAndMeSNPFileReader(fs))
                            {
                                var done = false;

                                do
                                {
                                    var snp = rawFile.GetNext();

                                    if (snp.IsNotNull())
                                    {
                                        if (snp.Chromosome == humanChromosome)
                                        {
                                            if (snp.Postion >= options.StartPosition && snp.Postion <= options.EndPosition)
                                            {
                                                Console.WriteLine(snp.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        done = true;
                                    }

                                } while (!done);
                            }
                        }
                        break;
                    case "ancestry":
                        {
                            using (var rawFile = new AncestryDNASNPFileReader(fs))
                            {
                                var done = false;

                                do
                                {
                                    var snp = rawFile.GetNext();

                                    if (snp.IsNotNull())
                                    {
                                        if (snp.Chromosome == humanChromosome)
                                        {
                                            if (snp.Postion >= options.StartPosition && snp.Postion <= options.EndPosition)
                                            {
                                                Console.WriteLine(snp.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        done = true;
                                    }

                                } while (!done);
                            }
                        }
                        break;
                    case "snplist":
                        {
                            using (var rawFile = new SNPListReader(fs))
                            {
                                var done = false;

                                do
                                {
                                    var snp = rawFile.GetNext();

                                    if (snp.IsNotNull())
                                    {
                                        if (snp.Chromosome == humanChromosome)
                                        {
                                            if (snp.Postion >= options.StartPosition && snp.Postion <= options.EndPosition)
                                            {
                                                Console.WriteLine(snp.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        done = true;
                                    }

                                } while (!done);
                            }
                        }
                        break;
                }
                
            }

            return 0;
        }
    }
}
