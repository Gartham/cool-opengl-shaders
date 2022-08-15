#version 330

out vec4 outputColor;
in vec3 pos;

uniform float time;

vec3 hsl2rgb( in vec3 c )
{
    vec3 rgb = clamp( abs(mod(c.x*6.0+vec3(0.0,4.0,2.0),6.0)-3.0)-1.0, 0.0, 1.0 );

    return c.z + c.y * (rgb-0.5)*(1.0-abs(2.0*c.z-1.0));
}

vec3 add(vec3 first, vec3 second)
{
	return vec3(min(1, max(0,first.x+second.x)), min(1, max(0, first.y + second.y)), min(1, max(0, first.z + second.z)));
}

vec4 renderOver(vec4 first, vec4 second)
{
	return first.x == 0 && first.y == 0 && first.z == 0 || first.w == 0 ? second : first;
}

vec4 clamp0(vec4 color)
{
	return vec4(clamp(color.r, 0, 1), clamp(color.g, 0, 1), clamp(color.b, 0, 1), clamp(color.a, 0, 1));
}

void main()
{
	vec4 f = clamp0(vec4(hsl2rgb(vec3(
		(sin(time/200000f) + 1) * 180,
		.5,
		min(1f, round(sin((pos.x + 1 + time / 1000f) * 20) + (sin(time/700f) + 1f) / 2f)) / 2f
	)), 1));
	
	vec4 s = vec4(hsl2rgb(vec3(sin(pos.x), 1f, .5f)), 1f);
	
	outputColor = renderOver(f, s);
}