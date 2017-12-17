using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Loaders;
using UMLProgram.Core.Loaders.Files;
using UMLProgram.Core.Render.SimpleObject.Programs;

namespace UMLProgram.Core.Render.SimpleObject {
    public class SimpleObjectRenderer {
        private static Matrix4 projectionMatrix, viewMatrix, modelMatrix;
        private static int vertexBufferHandle,
            vertexShaderHandle,
            fragmentShaderHandle,
            shaderProgramHandle,
            projectionMatrixLocation,
            modelMatrixLocation,
            viewMatrixLocation;
        private static int vertexCount;

        public static void Load(Size clientSize) {
            ObjImport importModel = LoadObj();
            vertexCount = importModel.Vertices.Count();
            CreateBuffersForShaders(importModel);
            CreateShaders(clientSize);
        }
        private static ObjImport LoadObj() {
            String file = "C:\\Work\\My CSharp\\UMLcreator\\UMLProgram\\box.obj";
            return BlenderLoader.Load(file);
        }
        private static void CreateBuffersForShaders(ObjImport model) {
            BufferVertices(model);
        }
        private static void BufferVertices(ObjImport model) {
            GL.GenBuffers(1, out vertexBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(model.Vertices.Length * Vector3.SizeInBytes), model.Vertices, BufferUsageHint.StaticDraw);
        }
        private static void CreateShaders(Size clientSize) {
            CompileVertexShader();
            CompileFragmentShader();
            SetShaderProgram();
            SupplyShaderMatrices(clientSize);
        }
        private static void CompileVertexShader() {
            vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, VertexShader.Text);
            GL.CompileShader(vertexShaderHandle);
            Trace.WriteLine(GL.GetShaderInfoLog(vertexShaderHandle));
        }
        private static void CompileFragmentShader() {
            fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle, FragmentShader.Text);
            GL.CompileShader(fragmentShaderHandle);
            Trace.WriteLine(GL.GetShaderInfoLog(fragmentShaderHandle));
        }
        private static void SetShaderProgram() {
            shaderProgramHandle = GL.CreateProgram();
            GL.AttachShader(shaderProgramHandle, vertexShaderHandle);
            GL.AttachShader(shaderProgramHandle, fragmentShaderHandle);
            GL.LinkProgram(shaderProgramHandle);
            GL.UseProgram(shaderProgramHandle);
            Trace.WriteLine(GL.GetProgramInfoLog(shaderProgramHandle));
        }
        private static void SupplyShaderMatrices(Size clientSize) {
            projectionMatrixLocation = GL.GetUniformLocation(shaderProgramHandle, "projection_matrix");
            viewMatrixLocation = GL.GetUniformLocation(shaderProgramHandle, "view_matrix");
            modelMatrixLocation = GL.GetUniformLocation(shaderProgramHandle, "model_matrix");

            float aspectRatio = clientSize.Width / (float)(clientSize.Height);
            Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, aspectRatio, 0.1f, 100, out projectionMatrix);
            viewMatrix = Matrix4.LookAt(new Vector3(4, 3, -3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            modelMatrix = Matrix4.Identity;

            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);
            GL.UniformMatrix4(modelMatrixLocation, false, ref modelMatrix);
        }

        public static void Update() {
        }
        public static void Draw() {
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
            GL.DisableVertexAttribArray(0);
        }

    }
}
