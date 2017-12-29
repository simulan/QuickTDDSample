using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Loaders.Files;

namespace UMLProgram.Core.Render.Common {
    public class ModelWorker {
        public static IndexedD3Model IndexData(D3Model data) {
            List<int> indices = new List<int>();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            for (int i = 0; i < data.Vertices.Count(); i++) {
                int indexDuplicate = GetIndex(vertices, uvs, normals, data.Vertices[i], data.UVs[i], data.Normals[i]);
                if (indexDuplicate > -1) {
                    indices.Add(indexDuplicate);
                } else {
                    indices.Add(vertices.Count);
                    vertices.Add(data.Vertices[i]);
                    uvs.Add(data.UVs[i]);
                    normals.Add(data.Normals[i]);
                }
            }
            return new IndexedD3Model(vertices.ToArray(), uvs.ToArray(), normals.ToArray(), indices.ToArray());
        }
        private static int GetIndex(List<Vector3> vertices, List<Vector2> uvs, List<Vector3> normals, Vector3 vertex, Vector2 uv, Vector3 normal) {
            int result = -1;
            for (int i = 0; i < vertices.Count(); i++) {
                bool similarVertex = vertices[i].Equals(vertex);
                bool similarUV = uvs[i].Equals(uv);
                bool similarNormal = normals[i].Equals(normal);
                if (similarVertex && similarUV && similarNormal) {
                    return i;
                }
            }
            return result;
        }
    }
}
