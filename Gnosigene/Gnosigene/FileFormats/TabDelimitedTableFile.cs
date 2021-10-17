//Copyright 2021 Xaratan LLC
//Released under the MIT License (see LICENSE.txt)

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gnosigene.FileFormats
{
    public class TabDelimitedTableFile : IEnumerable<string[]>
    {
        private string[] Columns { get; }

        private List<string> Header { get; } = new List<string>();

        private List<string[]> Data { get; } = new List<string[]>();

        public int RowCount
        {
            get
            {
                return Data.Count;
            }
        }

        public int ColumnCount
        {
            get
            {
                return Columns.Length;
            }
        }

        public string[] this[int row]
        {
            get
            {
                return Data[row];
            }
        }

        public TabDelimitedTableFile(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                //Read the header
                string line = null;

                while (true)
                {
                    line = reader.ReadLine();

                    if (line is null)
                        break;

                    if (line.Length == 0)
                        continue;

                    if (line.StartsWith("#"))
                    {
                        Header.Add(line);
                    }
                    else
                    {
                        break; //exit on the first non-blank line that doesn't start with '#'
                    }
                }

                //Try and find the columns in the last header row
                if (Header.Count > 0)
                {
                    var lastLine = Header.Last();

                    if (lastLine.Length >= 2 && lastLine[0] == '#' && lastLine[1] != '#')
                    {
                        Columns = lastLine.Substring(1).Split('\t');
                    }
                }

                if (line is null)
                    return;  //We're at the end of the file

                //Note that there's one line left from the above read!

                do
                {
                    Data.Add(line.Split('\t'));

                    line = reader.ReadLine();
                } while (line != null);
            }
        }

        public IEnumerator<string[]> GetEnumerator()
        {
            foreach (var row in Data)
                yield return row;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var row in Data)
                yield return row;
        }

        public static void Write(Stream stream, string[] columns, IEnumerable<string[]> rows)
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.WriteLine($"#{string.Join("\t", columns)}");

                foreach (var row in rows)
                {
                    writer.WriteLine(string.Join("\t", row));
                }
            }
        }
    }
}
