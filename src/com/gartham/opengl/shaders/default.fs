#version 330

out vec4 outputColor;
in vec3 pos;

uniform float time;

uniform float sphereX = .3f, sphereY = .4f, sphereZ = 3, r = .3f;
uniform vec3 lightSource = vec3(-1, 1, 0);

void main()
{
	float x = pos.x, y = pos.y;
	
	float trash = 9.25 + x*x + y*y - .6f * x -.8f * y - r*r;
	
	float partUnderSquareRoot =  36f- 4f * trash;
	
	if (partUnderSquareRoot < 0) {
		// Ray does not hit at all, make output color some ambient fog thing.
		outputColor = vec4(.2f, 0, 0, 1);
	} else {
		float chunk = sqrt(partUnderSquareRoot);
		
		float firstSol = 3 + chunk / 2, secondSol = 3 - chunk / 2;
		
		float intersectionT;
		if (firstSol < secondSol)
			intersectionT = firstSol;
		else
			intersectionT = secondSol;
		
		vec3 intersectionPoint = vec3(x, y, intersectionT);
		
		outputColor = vec4(vec3(1, 1, 1) - (lightSource - intersectionPoint), 1);
	}
}