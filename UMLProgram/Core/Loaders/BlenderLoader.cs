using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Loaders {
    public class BlenderLoader {
        private const int MAX_LINE_CHARS=80;
        private const int CHAR_SIZE = 2;
        private const int CHUNK_SIZE = MAX_LINE_CHARS * CHAR_SIZE;

        struct ChunkResult {
            string CurrentLine;
            string NextLine;
        }

        public static bool Load(string filename) {
            FileStream stream = new FileStream(filename, FileMode.Open);
            byte[] chunk = new byte[CHUNK_SIZE];
            int offset = 0;
            while (stream.CanRead) {
                stream.Read(chunk, offset, MAX_LINE_CHARS);
                offset += MAX_LINE_CHARS; 
            }
            return true;
        }
    }
}
