using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Render.TexturedCube.Programs;


namespace UMLProgram.Core.Render.TexturedCube {
    public class TexturedCubeRenderer {
        private static Matrix4 projectionMatrix, viewMatrix, modelMatrix;
        private static int vertexBufferHandle,
            textureUVHandle,
            colorBufferHandle,
            vertexShaderHandle,
            textureHandle,
            fragmentShaderHandle,
            shaderProgramHandle,
            projectionMatrixLocation,
            modelMatrixLocation,
            viewMatrixLocation;

        public static void Load(Size clientSize) {
            LoadTexture();
            CreateBuffersForShaders();
            CreateShaders(clientSize);   
        }
        private static void LoadTexture() {

        }
        private static void CreateBuffersForShaders() {
            BufferVertices();
            BufferTextureUV();
        }
        private static void BufferVertices() {
            GL.GenBuffers(0, out vertexBufferHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(TexturedCubeData.Vertices.Length * Vector3.SizeInBytes), TexturedCubeData.Vertices, BufferUsageHint.StaticDraw);
        }
        private static void BufferTextureUV() {
            GL.GenBuffers(1, out textureUVHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 1);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, new IntPtr(TexturedCubeData.Texture.UVs.Length * Vector2.SizeInBytes), TexturedCubeData.Texture.UVs, BufferUsageHint.StaticDraw);
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
            fragmentShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(fragmentShaderHandle, FragmentShader.Text);
            GL.CompileShader(fragmentShaderHandle);
            Trace.WriteLine(GL.GetShaderInfoLog(vertexShaderHandle));
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
        public static void Render() {
        
        }
    }
}
