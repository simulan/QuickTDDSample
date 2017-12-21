using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using UMLProgram.Core.Loaders;
using OpenTK;
using System.Globalization;
using UMLProgram.Core.Loaders.Files;

namespace UMLProgram.Core.Loaders {
    public class BlenderLoader {
        private const int MAX_LINE_CHARS=80;
        private const int CHAR_SIZE = 1;
        private const int CHUNK_SIZE = MAX_LINE_CHARS * CHAR_SIZE;
        private const char LINEFEED = '\n';
        private const char CARR_RETURN = '\r';
        private const char WHITESPACE = ' ';
        private const char FACE_INDEX_SEPERATOR = '/';
        
        public static IndexedObj Load(string file) {
            string[] lines = GetContent(file).Split(new Char[] { LINEFEED,CARR_RETURN });
            ObjImportCache temporaryData = ExtractData(lines);
            IndexedObj obj = IndexData(ProcessData(temporaryData));
            return obj;
        }
        private static string GetContent(string file) {
            byte[] readBuffer = new byte[MAX_LINE_CHARS];
            StringBuilder sb = new StringBuilder();
            using (FileStream stream = new FileStream(file, FileMode.Open)) {
                int bytesRead;
                do {
                    bytesRead = stream.Read(readBuffer, 0, MAX_LINE_CHARS);
                    if (bytesRead < MAX_LINE_CHARS) {
                        sb.Append(BytesToString(readBuffer.Take(bytesRead).ToArray()));
                    } else {
                        sb.Append(BytesToString(readBuffer));
                    }
                } while(bytesRead > 0);
            }
            return sb.ToString();
        }
        private static ObjImportCache ExtractData(string[] lines) {
            ObjImportCache data = new ObjImportCache();
            foreach (string line in lines) {
                string[] parts = line.Split(WHITESPACE);
                if (parts[0].Equals("v")) {
                    data.Vertices.Add(new Vector3(AsFloat(parts[1]), AsFloat(parts[2]), AsFloat(parts[3])));
                } else if (parts[0].Equals("vt")) {
                    data.UVs.Add(new Vector2(AsFloat(parts[1]), AsFloat(parts[2])));
                } else if (parts[0].Equals("vn")) {
                    data.Normals.Add(new Vector3(AsFloat(parts[1]), AsFloat(parts[2]), AsFloat(parts[3])));
                } else if (parts[0].Equals("f")) {
                    if (parts.Length > 4) {
                        throw new Exception("Loader does not support complex faces!");
                    }
                    data.Indices.Add(new ObjImportCache.TriangleIndices(parts[1].Split(FACE_INDEX_SEPERATOR), parts[2].Split(FACE_INDEX_SEPERATOR), parts[3].Split(FACE_INDEX_SEPERATOR)));
                }
            }
            return data;
        }
        private static ObjImport ProcessData(ObjImportCache data) {
            ObjImport processedData = new ObjImport(data.Indices.Count*3);
            for(int i=0; i<data.Indices.Count*3; i++ ) {
                processedData.Vertices[i] = data.Vertices[ (int) data.Indices[(int)(i / 3)].VertexIndices[i % 3]-1];
                if (data.UVs.Count != 0) {
                    processedData.UVs[i] = data.UVs[(int)data.Indices[(int)(i / 3)].UVIndices[i % 3] - 1];
                }
                processedData.Normals[i] = data.Normals[(int)data.Indices[(int)(i / 3)].NormalIndices[i % 3]-1];
            }
            return processedData;
        }
        private static IndexedObj IndexData(ObjImport data) {
            IndexedObj result = new IndexedObj();
            for (int i = 0; i < data.Vertices.Count(); i++) {
                int indexDuplicate = GetIndex(result, data.Vertices[i], data.UVs[i], data.Normals[i]);
                if (indexDuplicate > -1) {
                    result.Indices.Add(indexDuplicate);
                } else {
                    result.Indices.Add(result.Vertices.Count);
                    result.Vertices.Add(data.Vertices[i]);
                    result.UVs.Add(data.UVs[i]);
                    result.Normals.Add(data.Normals[i]);
                }
            }
            return result;
        }
        private static int GetIndex(IndexedObj output,Vector3 vertex,Vector2 uv,Vector3 normal) {
            int result = -1;
            for (int i = 0; i < output.Vertices.Count(); i++) {
                bool similarVertex = output.Vertices[i].Equals(vertex);
                bool similarUV = output.UVs[i].Equals(uv);
                bool similarNormal = output.Normals[i].Equals(normal);
                if (similarVertex && similarUV && similarNormal) {
                    return i;
                }   
            }
            return result;
        }

        private static float AsFloat(string number) {
            return float.Parse(number, CultureInfo.InvariantCulture);
        }
        private static String BytesToString(byte[] bytes) {
            string result = "";
            foreach (byte b in bytes) result += (char)b;
            return result;
        }
    }
}
