shader_type canvas_item;

uniform sampler2D earth;
uniform int width = 1;

void fragment() {
	vec2 centerUV = (UV - vec2(0.5)) * 2.0;
	
	vec2 transformCenteredUV = centerUV / 1.2;
	
	bool outside = length(transformCenteredUV) > 1.0 / 1.2;
	
	vec2 transformUV = (transformCenteredUV + vec2(1.0)) / 2.0;
	
	COLOR = texture(earth, transformUV).w * texture(TEXTURE, UV);
	
	if(!outside) {
		for(int i = -width; i <= width; i++) {
			for(int j = -width; j <= width; j++) {
				if(texture(earth, transformUV + vec2(float(j), float(i)) * TEXTURE_PIXEL_SIZE).w == 0.0 && texture(earth, transformUV).w != 0.0) {
					COLOR = vec4(0, 0, 0, 1);
					break;
				}
			}
		}
	}
	else if(abs(length(transformCenteredUV) - 1.0 / 1.2) < 0.02 && texture(earth, transformUV).w != 0.0) {
		COLOR = vec4(0, 0, 0, 1);
	}
}
