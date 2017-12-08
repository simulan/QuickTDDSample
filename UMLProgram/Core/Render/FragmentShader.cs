﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Cube.Programs {
    public static class FragmentShader {
        public static readonly string Text = @"
            #version 130
            precision highp float;
            const vec3 ambient = vec3(0.1, 0.1, 0.1);
            const vec3 lightVecNormalized = normalize(vec3(0.5, 0.5, 2.0));
            const vec3 lightColor = vec3(0.9, 0.9, 0.7);
            in vec3 normal;
            out vec4 out_frag_color;
            void main(void)
            {
              float diffuse = clamp(dot(lightVecNormalized, normalize(normal)), 0.0, 1.0);
              out_frag_color = vec4(ambient + diffuse * lightColor, 1.0);
            }";
    }
}
