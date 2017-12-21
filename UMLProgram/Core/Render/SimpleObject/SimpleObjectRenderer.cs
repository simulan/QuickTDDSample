using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Input;
using UMLProgram.Core.Loaders;
using UMLProgram.Core.Loaders.Files;
using UMLProgram.Core.Render.SimpleObject.Programs;

namespace UMLProgram.Core.Render.SimpleObject {
    public class SimpleObjectRenderer {
        private static Matrix4 projectionMatrix, viewMatrix, modelMatrix;
        private static Vector3 lightColorUniform = new Vector3(0.8f, 0.8f, 0.8f);
        private static Vector3 lightPositionUniform = new Vector3(5, 5, 0);
        private static int[] indices;
        private static int lightPowerUniform = 6;
        private static int vertexBufferHandle,
            uvBufferHandle,
            normalBufferHandle,
            indicesBufferHandle,
            textureHandle,
            vertexShaderHandle,
            fragmentShaderHandle,
            shaderProgramHandle,
            projectionMatrixLocation,
            modelMatrixLocation,
            viewMatrixLocation,
            lightColorUniformLocation,
            lightPowerUniformLocation,
            lightPositionUniformLocation;

        public static void Load(Size clientSize) {
            //LoadTexture();
            IndexedObj importModel = LoadObj();
            indices = importModel.Indices.ToArray(); 
            CreateBuffersForShaders(importModel);
            CreateShaders(clientSize);
        }
        private static void LoadTexture() {
            String file = "C:\\Work\\My CSharp\\UMLcreator\\UMLProgram\\texture.dds";
            textureHandle = DDSLoader.Load(file);
        }
        private static IndexedObj LoadObj() {
            String file = "C:\\Work\\My CSharp\\UMLcreator\\UMLProgram\\box.obj";
            return BlenderLoader.Load(file);
        }
        private static void CreateBuffersForShaders(IndexedObj model) {
            //BufferIndices(model.Indices);
            BufferVertices(model.Vertices);
            BufferUVs(model.UVs);
            BufferNormals(model.Normals);
        }
        private static void BufferIndices(List<int> indices) {
            GL.GenBuffers(1, out indicesBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, indicesBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count() * sizeof(int), indices.ToArray(), BufferUsageHint.StaticDraw);
        }
        private static void BufferVertices(List<Vector3> vertices) {
            GL.GenBuffers(1, out vertexBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(vertices.Count() * Vector3.SizeInBytes), vertices.ToArray(), BufferUsageHint.StaticDraw);
        }
        private static void BufferUVs(List<Vector2> uvs) {
            GL.GenBuffers(1, out uvBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBufferHandle);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, new IntPtr(uvs.Count() * Vector2.SizeInBytes), uvs.ToArray(), BufferUsageHint.StaticDraw);
        }
        private static void BufferNormals(List<Vector3> normals) {
            GL.GenBuffers(1, out normalBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBufferHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(normals.Count() * Vector3.SizeInBytes), normals.ToArray(), BufferUsageHint.StaticDraw);
        }
        private static void CreateShaders(Size clientSize) {
            CompileVertexShader();
            CompileFragmentShader();
            SetShaderProgram();
            SupplyShaderMatrices(clientSize);
            SupplyShaderVars();
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
        private static void SupplyShaderVars() {
            lightColorUniformLocation = GL.GetUniformLocation(shaderProgramHandle, "light_color");
            lightPowerUniformLocation = GL.GetUniformLocation(shaderProgramHandle, "light_power");
            lightPositionUniformLocation = GL.GetUniformLocation(shaderProgramHandle, "light_position_worldspace");
            GL.Uniform3(lightColorUniformLocation, ref lightColorUniform); 
            GL.Uniform1(lightPowerUniformLocation, lightPowerUniform);
            GL.Uniform3(lightPositionUniformLocation, lightPositionUniform);
        }
        public static void Update(Controller.ControllerData data) {
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(data.FOV), 4 / 3, 0.1f, 100, out projectionMatrix);
            Vector3 up = Vector3.Cross(data.Right, data.Direction);
            viewMatrix = Matrix4.LookAt(data.Position, data.Position + data.Direction, up);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);
        }
        public static void Draw() {
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBufferHandle);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
            GL.EnableVertexAttribArray(2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBufferHandle);
            GL.VertexAttribPointer(2, 3,  VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, indices);
            GL.DisableVertexAttribArray(2);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);
        }
    }
}
