using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Render.ColorCube;
using UMLProgram.Core.Render.Cube;
using UMLProgram.Core.Render.Rectangle;
using UMLProgram.Core.Render.TexturedCube;
using UMLProgram.Core.Render.Triangle;

namespace UMLProgram.Core {
    public class UmlWindow : GameWindow {

        public UmlWindow() {
        }
        protected override void OnLoad(EventArgs e) {
            VSync = VSyncMode.On;
            GL.Enable(EnableCap.DepthTest);
            TexturedCubeRenderer.Load(ClientSize);
            GL.ClearColor(System.Drawing.Color.MidnightBlue);
        }
        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 0.1f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }
        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            TexturedCubeRenderer.Render();
            SwapBuffers();
        }
    }
}
