using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Loaders {
    public class BlenderLoader {
        public static bool Load(string filename) {
            FileStream stream = new FileStream(filename, FileMode.Open);
            ValidateBlenderFileStream(stream);
            return true;
        }
        private static void ValidateBlenderFileStream(FileStream stream) {
            if (stream.CanRead) {
                
            } else {
                throw new IOException(stream.Name + " does not support reading atm.");
            }
        }
    }
}
