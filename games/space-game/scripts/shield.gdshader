shader_type canvas_item;

uniform bool ready = true;

void fragment() {
	if(!ready) {
		COLOR = vec4(0.8, 0.2, 0.2, 1.0) * texture(TEXTURE, UV).w;
	}
}
