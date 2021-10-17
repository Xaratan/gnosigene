using Gnosigene.FileFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTesting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var stream = File.OpenRead(@"E:\Genetics\Reference\seq_gene.md"))
            {
                var tabs = new TabDelimitedTableFile(stream);

                using (var geneStream = new FileStream(@"E:\Genetics\Reference\genes.md", FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    TabDelimitedTableFile.Write(geneStream,
                        new string[]
                        {
                            "gene",
                            "chromosome",
                            "start",
                            "stop"
                        },
                        tabs.Where(row => row[11] == "GENE" && row[12].Contains("GRCh37")).Select(row => new string[] { row[9], row[1], row[2], row[3] })
                        );
                }
            }
        }
    }
}
