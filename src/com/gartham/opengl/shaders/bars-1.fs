#version 330

out vec4 outputColor;
in vec3 pos;

uniform float time;

void main()
{
	float val = sin(time/2000f) * round(sin((pos.x + 1 + time / 1000f) * 20));
	outputColor = vec4(val, val, val, 1);
}