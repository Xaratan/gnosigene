//Copyright 2021 Xaratan LLC
//Released under the MIT License (see LICENSE.txt)

//NOTE: This functionality in file is not completed.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gnosigene.FileFormats
{
    public static class TabixFileReader
    {
        private static bool ReadInt(Stream stream, out int result)
        {
            byte[] data = new byte[4];

            var size = stream.Read(data, 0, 4);

            if (size < 4)
            {
                result = 0;
                return false;
            }

            result = data[3] << (8 * 3);
            result += data[2] << (8 * 2);
            result += data[1] << 8;
            result += data[0];

            return true;
        }

        public static Tabix Read(Stream stream)
        {
            try
            {
                byte[] magic = new byte[4];

                stream.Read(magic, 0, magic.Length);

                if (magic[0] != (byte)'T'
                    || magic[1] != (byte)'B'
                    || magic[2] != (byte)'I'
                    || magic[3] != (byte)1)
                {
                    return null;
                }

                if (!ReadInt(stream, out var numberOfSequences)) //n_ref
                    return null;

                if (!ReadInt(stream, out var format)) //format
                    return null;

                if (!ReadInt(stream, out var sequenceNameColumn)) //col_seq
                    return null;

                if (!ReadInt(stream, out var regionStartColumn)) //col_beg
                    return null;

                if (!ReadInt(stream, out var regionEndColumn)) //col_end
                    return null;

                if (!ReadInt(stream, out var commentCharacter)) //meta
                    return null;

                if (!ReadInt(stream, out var skipLineCount)) //skip
                    return null;

                if (!ReadInt(stream, out var sequenceNamesLength)) //l_nm
                    return null;

                //##NAMES##
                List<string> names = new List<string>();

                int sequenceNamesCount = 0;

                while (sequenceNameColumn < sequenceNamesLength)
                {
                    
                }
            }
            catch
            {
                return null;
            }

            throw new Exception(); //Unfinished code
        }
    }
}
