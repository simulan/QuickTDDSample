using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Triangle {
    public class TriangleRenderer {
        private static int vaoHandle;
        private static int vertexBufferHandle;

        public static void Load(Size clientSize) {
            CreateShaders(clientSize);
            CreateVertexBuffers();
            BindBuffersToShaders();
        }
        private static void CreateShaders(Size clientSize) {
            //compile shader vert,frag, setshaderprogram,linkmatrix
        }
        private static void CreateVertexBuffers() {
            BufferVertices();
        }
        private static void BufferVertices() {
            GL.GenBuffers(1,out vertexBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(TriangleVertexData.Vertex.Length * Vector3.SizeInBytes), TriangleVertexData.Vertex, BufferUsageHint.StaticDraw);
        }
        private static void BindBuffersToShaders() {
            GL.GenVertexArrays(1, out vaoHandle);
            GL.BindVertexArray(vaoHandle);
        }
        public static void Render() {
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DisableVertexAttribArray(0);
        }
    }
}
