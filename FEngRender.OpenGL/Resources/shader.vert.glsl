#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec4 color; 
layout(location = 2) in vec2 aTexCoord;

out vec4 outColor;
out vec2 texCoord;

void main(void)
{
    outColor = color;
    texCoord = aTexCoord;

    gl_Position = vec4(aPosition, 1.0);
}