using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Triangle.Programs {
    public class VertexShader {
        public static readonly string Text = @"
            #version 330 core
            layout(location = 0) in vec3 vertexPosition_modelspace;

            void main(){
              gl_Position.xyz = vertexPosition_modelspace;
              gl_Position.w = 1.0;
            }
        ";
    }
}
