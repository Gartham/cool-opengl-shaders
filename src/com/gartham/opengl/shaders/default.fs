#version 330

out vec4 outputColor;
in vec3 pos;

uniform float time;

void main()
{
	float val = round(sin((pos.x + 1) * 20));
	outputColor = vec4(val, val, val, 1);
}