#version 330

out vec4 outputColor;
in vec3 pos;

uniform float time;
uniform float SPEC_COEFF = 0.4, DIFF_COEFF = 0.2;


float checkSphereIntersection(vec3 sphereCoordinates, float sphereRadius, vec3 rayStart, vec3 rayDir, out bool intersected)
{
	rayDir = normalize(rayDir);
	float b = 2 * dot(rayDir, rayStart - sphereCoordinates);
	float c = dot(rayStart, rayStart) + dot(sphereCoordinates, sphereCoordinates) - 2 * dot(rayStart, sphereCoordinates) - sphereRadius * sphereRadius;
	
	float fc = 4 * c, bs = b * b;
	if (fc > bs) {
		// No intersection.
		intersected = false;
		return 0;
	} else {
		float s = sqrt(bs - fc);
		float first = (b - s) / 2, second = (s + b) / -2;
		intersected = true;
		
		if (abs(first) < abs(second))
			return first;
		else return second;
	}
}

// sphereCoordinates are for the center of the sphere; they are used to calculate the normal.
vec3 getReflectedRayDir(vec3 sphereCoordinates, vec3 intersectionPoint, vec3 previousDir) {
	vec3 normal = normalize(intersectionPoint - sphereCoordinates);
	return previousDir - 2 * dot(previousDir, normal) * normal;
}

// recursionDepth is decremented upon each call.
vec3 shootRayAndCalculateColor(vec3 rayStart, vec3 rayDir, vec3 lightSource, vec3 sphereCoords, float sphereRadius, int recursionDepth) {

	vec3 resultColor = vec3(0, 0, 0);

	bool intersected;
	float t = checkSphereIntersection(sphereCoords, sphereRadius, rayStart, rayDir, intersected);
	
	if (intersected) {
		vec3 intersectionPoint = rayStart + rayDir * t;
		
		// Shadow ray.
		vec3 dirToLight = normalize(lightSource - intersectionPoint);
		float t = checkSphereIntersection(sphereCoords, sphereRadius, intersectionPoint, dirToLight, intersected);
		if (!intersected)
			resultColor += lightSource * DIFF_COEFF;
			
		if (recursionDepth != 0) {
			vec3 reflectionRayDir = getReflectedRayDir(sphereCoords, intersectionPoint, rayDir);
			resultColor += SPEC_COEFF * shootRayAndCalculateColor(intersectionPoint, reflectionRayDir, lightSource, sphereCoords, sphereRadius, recursionDepth - 1);
		}
	}
	
	return resultColor;
}

void main()
{
	vec3 LIGHT_SOURCE = vec3(1, 1, 1);
	vec3 SPHERE_COORDS = vec3(0, 0, 10);
	
	outputColor = vec4(0, 0, 0, 1);
	
	float SPHERE_RADIUS = 1;
	
	// For this pixel, we shoot a ray outwards, straight, away from the camera (in the direction we're looking).
	vec3 rayStart = pos;
	vec3 rayDir = vec3(0, 0, 1);
	
	float factor = SPEC_COEFF;
	
	
	for (int i = 0; i >= 0; i--) {
		bool intersected;
		float t = checkSphereIntersection(SPHERE_COORDS, SPHERE_RADIUS, rayStart, rayDir, intersected);
		
		if (intersected) {
			vec3 intersectionPoint = rayStart + rayDir * t;
			
			// Shadow ray.
			vec3 dirToLight = normalize(LIGHT_SOURCE - intersectionPoint);
			float t = checkSphereIntersection(SPHERE_COORDS, SPHERE_RADIUS, intersectionPoint, dirToLight, intersected);
			if (!intersected || t>-0.01)
				outputColor.xyz += LIGHT_SOURCE * factor * DIFF_COEFF * ;
			
			// Reflection ray.
			vec3 reflectionRayDir = getReflectedRayDir(SPHERE_COORDS, intersectionPoint, rayDir);
			
			rayStart = intersectionPoint;
			rayDir = reflectionRayDir;
			factor *= SPEC_COEFF;
		}
	}
	
	// outputColor = vec4(shootRayAndCalculateColor(pos, vec3(0, 0, 1), LIGHT_SOURCE, SPHERE_COORDS, SPHERE_RADIUS, 4), 1);
	
	
	
	
	
	

	/*

	// We check if there was an intersection with the sphere.
	bool intersected;
	float t = checkSphereIntersection(SPHERE_COORDS, SPHERE_RADIUS, pos, rayDir, intersected);
	
	if (intersected) {
		// If there was an intersection, we want to shoot two rays: a shadow ray and a reflection ray. The reflection ray will recurse.
		
		vec3 intersectionPoint = rayStart + rayDir * t;
		
		// First shoot the shadow ray; it goes directly to the light source.
		vec3 dirToLight = normalize(LIGHT_SOURCE - intersectionPoint);
		float t = checkSphereIntersection(SPHERE_COORDS, SPHERE_RADIUS, intersectionPoint, dirToLight, intersected);
		
		// (Just checking for any intersection does not work if the sphere is behind the light source. :-)
		if (!intersected) {
			// If the shadow ray was not intercepted by any scene objects, then it has a direct path to the light source. This means that we can add the shadow light contribution directly.
			// Add the color from the light source. We're assuming there is only one light source right now, so we just add that one light's color.
			outputColor.xyz += LIGHT_SOURCE.xyz * DIFF_COEFF;
		}
		
		// Now shoot a reflection ray. We need to call a recursive function for this. (It'll basically do what we did before this IF statement, but recursively a few times.
		// We need to get the outgoing ray.
		vec3 reflectionRayDir = getReflectedRayDir(SPHERE_COORDS, intersectionPoint, rayDir);
		
		
		
	} else
		outputColor = vec4(0, 0, 0, 1);
		
	*/
}