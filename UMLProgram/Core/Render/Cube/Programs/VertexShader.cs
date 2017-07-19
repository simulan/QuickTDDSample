﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Cube.Programs {
    public static class VertexShader {
        public static readonly string Text = @"
            #version 130
            precision highp float;
            uniform mat4 projection_matrix;
            uniform mat4 modelview_matrix;
            in vec3 in_position;
            in vec3 in_normal;
            out vec3 normal;
            void main(void)
            {
              //works only for orthogonal modelview
              normal = (modelview_matrix * vec4(in_normal, 0)).xyz;
  
              gl_Position = projection_matrix * modelview_matrix * vec4(in_position, 1);
            }";
    }
}
