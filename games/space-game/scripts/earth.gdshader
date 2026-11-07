shader_type canvas_item;

uniform sampler2D earth;
uniform float follow = 0.8;
uniform int width = 1;

void fragment() {
	for(int i = -width; i <= width; i++) {
		for(int j = -width; j <= width; j++) {
			if(texture(TEXTURE, UV + vec2(float(j), float(i)) * TEXTURE_PIXEL_SIZE).w == 0.0 && texture(TEXTURE, UV).w != 0.0) {
				COLOR = vec4(0, 0, 0, 1);
				break;
			}
		}
	}
}
