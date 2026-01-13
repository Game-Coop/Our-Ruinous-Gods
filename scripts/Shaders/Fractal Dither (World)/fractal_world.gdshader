shader_type spatial;
render_mode depth_prepass_alpha;

// Base color/albedo texture (sRGB):
uniform sampler2D albedo_tex : source_color;
// Albedo UV transform
uniform vec2 albedo_uv_offset = vec2(0.0);
uniform vec2 albedo_uv_scale  = vec2(1.0);

uniform vec3 col1:source_color;
uniform vec3 col2:source_color;
uniform float doFresnel = 0.5;
uniform float fresnelStrength = 0.5;
uniform sampler2D Depth:hint_depth_texture;
uniform sampler2D DitherMap:repeat_enable,hint_roughness_gray;
// 1D brightness ramp (in a 2D image, e.g. 256×1), set to CLAMP.
uniform sampler2D dither_ramp_tex;
uniform float ditherScale = 1.0;
uniform float ditherStrength = 1.0;
uniform bool invertDots = false;

// Adjust the incoming color brightness
uniform float input_exposure = 1.0;
uniform float input_offset   = 0.0;

uniform float uv1_blend_sharpness;
varying vec3 uv1_power_normal;
varying vec3 uv1_triplanar_pos;
varying vec2 suv;
varying vec2 tsuv;
varying mat4 inv;
varying float fresn;

float smoothRound(float gradient, float steps, float factor) {
	float m1a = floor(gradient * steps + 1.0)/steps;
	float m1b = floor(gradient * steps)/steps;
	float m2a = fract(gradient * steps);
	m2a = smoothstep(0.0, 1.0, pow(m2a, factor));
	return mix(m1b, m1a, m2a);
}

float depth() {
	float d = texture(Depth, tsuv).r;
	// NDC == Normalized Device Coordinates
	vec3 ndc = vec3((tsuv * 2.0) - 1.0, d);
	vec4 view = inv * vec4(ndc, 1.0);
	view.xyz /= view.w;
	return -view.z;
}

float getGrayscale(vec3 c) {
    // Standard luminance256
    return clamp(dot(c, vec3(0.299, 0.587, 0.114)), 0.0, 1.0);
}
// Sample brightness ramp similarly to RuneVision (with half‐texel offset)
float sampleBrightnessRamp(float brightness) {
    float w = float(textureSize(dither_ramp_tex, 0).x);
    float inv_w = 1.0 / w;
    float samp_x = 0.5 * inv_w + (1.0 - inv_w) * brightness;
    return texture(dither_ramp_tex, vec2(samp_x, 0.5)).r;
}
float linearLight(float a, float b, float f) {
	return a+(b-a)*f;
}
float split_specular(float specular) {
	return step(0.15f, specular);
}
float fresnel(float amount, vec3 normal, vec3 view) {
	return pow((1.0 - clamp(dot(normalize(normal), normalize(view)), 0.0, 1.0)), amount);
}
float twoTone(float gradient, float threshold) {
	return clamp((gradient - threshold) * ( (1.0 - fresn) * 35.0), 0, 1);
}
float blackWhite(float gradient) {
	return gradient > 0.0? 0.0 : 1.0;
}

void vertex() {
	vec3 normal = MODEL_NORMAL_MATRIX * NORMAL;
	
	TANGENT = vec3(0.0, 0.0, -1.0) * abs(normal.x);
	TANGENT += vec3(1.0, 0.0, 0.0) * abs(normal.y);
	TANGENT += vec3(1.0, 0.0, 0.0) * abs(normal.z);
	TANGENT = inverse(MODEL_NORMAL_MATRIX) * normalize(TANGENT);
	
	BINORMAL = vec3(0.0, 1.0, 0.0) * abs(normal.x);
	BINORMAL += vec3(0.0, 0.0, -1.0) * abs(normal.y);
	BINORMAL += vec3(0.0, 1.0, 0.0) * abs(normal.z);
	BINORMAL = inverse(MODEL_NORMAL_MATRIX) * normalize(BINORMAL);
	
	// UV1 Triplanar: Enabled
	uv1_power_normal = pow(abs(NORMAL), vec3(uv1_blend_sharpness));
	uv1_power_normal /= dot(uv1_power_normal, vec3(1.0));
	uv1_triplanar_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	uv1_triplanar_pos = VERTEX;
	uv1_triplanar_pos *= vec3(1.0, -1.0, 1.0);
}

//Sample our triplanar
vec4 triplanar_texture(sampler2D p_sampler, vec3 p_weights, vec3 p_triplanar_pos) {
	vec4 samp = vec4(0.0);
	samp += texture(p_sampler, p_triplanar_pos.xy) * p_weights.z;
	samp += texture(p_sampler, p_triplanar_pos.xz) * p_weights.y;
	samp += texture(p_sampler, p_triplanar_pos.zy * vec2(-1.0, 1.0)) * p_weights.x;
	return samp;
}

float dither(float brightness) { //TODO HERE!
	float b = invertDots ? (1.0 - brightness) : brightness;
	float corrected_b = sampleBrightnessRamp(b);
	
	float q = 0.005;
	float d0 = depth() * 50.0;
	float d1 = d0;
	float d2 = d0;
	
	d0 = floor(d0 * q)/q; d0 = max(d0, 100.0);
	d1 = fract(d1 * q);
	d2 = floor(d2 * q + 1.0)/q; d2 = max(d2, 100.0);
	
	//Brightness fix
	//return -(Math.cos(Math.PI * x) - 1) / 2;
	d1 = -(cos(d1 * PI) - 1.0) / 2.0;
	float s = sin(d1 * PI);
	
	float dither1 = (triplanar_texture(
		DitherMap, 
		uv1_power_normal, 
		uv1_triplanar_pos * ditherScale / (d0)).r);
	float dither2 = (triplanar_texture(
		DitherMap, 
		uv1_power_normal, 
		uv1_triplanar_pos * ditherScale / (d2)).r);
	//d1 = clamp((d1 - 0.5) * 100.5 + 0.5, 0.0, 1.0);
	//float dither = mix(dither1, dither2, d1) + (dither1 + (dither2 - dither1) * (d1));
	float dither = mix(dither1, dither2, d1) + abs(dither1 - dither2) * s * 0.1;
	dither *= (1.0 - pow(fresn, 20.0));
	dither = clamp(dither, 0, 1);
	dither -= 0.5;
	return dither;
}

// Finds brightness val to pass into dithering
vec3 applyDither(vec3 col_in) {
    // Keep the same exposure + offset from the original:
    //   col_in -> col_in * input_exposure + input_offset
    //   (then clamp 0..1 so we don't feed negative or >1 into dithering)
    vec3 col = col_in * input_exposure + input_offset;
    col = clamp(col, 0.0, 1.0);
	
	float lum = getGrayscale(col);
	float v = dither(lum);
	return vec3(v);
}

//  FRAGMENT
//     Here we simply set the base ALBEDO and PBR properties.
//     We do NOT do dithering here. That happens in light().
//     TODO: Add in main tex and roughness
// ----------------------------------------------------
void fragment() {
	// Set some vars for dithering
	inv = INV_PROJECTION_MATRIX;
	suv = SCREEN_UV * normalize(vec2(16.0, 9.0));
	tsuv = SCREEN_UV;
	fresn = fresnel(1.0, NORMAL, VIEW);
	
	// Albedo UV transform
    vec2 uv_albedo = UV * albedo_uv_scale + albedo_uv_offset;
    // Sample base color from texture
    vec3 base_col = texture(albedo_tex, uv_albedo).rgb;
	float lum = getGrayscale(base_col);
	ALBEDO = vec3(lum);
}

// ----------------------------------------------------
//  LIGHT
//    Called once per light. We compute the lambert (or
//    other) contribution, pass it into our dither,
//    then add it into DIFFUSE_LIGHT.
//    If multiple lights overlap, each will run this code
//    and sum into DIFFUSE_LIGHT.
// ----------------------------------------------------
void light() {
	// Basic lambert
	float NdotL = max(dot(NORMAL, LIGHT), 0.0);
	vec3 lambert_color = ALBEDO * LIGHT_COLOR * ATTENUATION * NdotL;
	float base = clamp(dot(NORMAL, LIGHT), -1, 1.0);
	base = max((base+1.0) * 0.5, 0.0);
	base = clamp(base, 0, 1);
	base *= ATTENUATION;
	
	// Crossfade Code
	//float base1 = base + (dither());
	//base = twoTone(base);

	// Morph Code
	//base += (dither());

	// Final Code
	vec3 dith = applyDither(lambert_color);
	dith = clamp(dith, 0.0, 1.0);
	base *= dith[0];
	//DIFFUSE_LIGHT = lambert_color;
	//DIFFUSE_LIGHT *= dith;

	// apply threshold
	base = twoTone(base, 0.02);
	//base = blackWhite(base);

	//base1 = clamp(smoothRound(base1, 4.0, 80.0) 0, 1);
	//base = clamp(smoothRound(base, 4.0, 80.0), 0, 1);

	vec3 col = mix(col1, col2, base);
	DIFFUSE_LIGHT += mix(vec3(0.0), col * 2.0, base) * (LIGHT_COLOR) * 1.0;
	//DIFFUSE_LIGHT += lambert_color;
}



