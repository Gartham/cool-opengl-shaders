#version 330

out vec4 outputColor;
in vec3 pos;

uniform float time;

vec3 hsl2rgb( in vec3 c )
{
    vec3 rgb = clamp( abs(mod(c.x*6.0+vec3(0.0,4.0,2.0),6.0)-3.0)-1.0, 0.0, 1.0 );

    return c.z + c.y * (rgb-0.5)*(1.0-abs(2.0*c.z-1.0));
}

void main()
{
	outputColor = vec4(hsl2rgb(vec3(
		(sin(time/200000f) + 1) * 180,
		.5,
		round(sin((pos.x + 1 + time / 1000f) * 20)) / 2f
	)), 1);
}