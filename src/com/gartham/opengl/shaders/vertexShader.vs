#version 330

layout (location = 0) in vec2 position;

out vec3 pos;

void main()
{
	gl_Position = vec4(position, 0, 1.0f);
	pos = gl_Position.xyz;
}