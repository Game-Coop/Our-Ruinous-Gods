shader_type sky;

uniform int vertical_sections = 21;
uniform float min_star_size = 0.001;
uniform float max_star_size = 0.05;
uniform float star_falloff = 10.0;
uniform float margin = 0.05;

uniform float blink_period_min = 5.0;
uniform float blink_period_max = 10.0;
uniform float blink_min_brightness = 0.001;

uniform float star_density = 2;

uniform float cluster_scale = 2.0;
uniform float cluster_contrast = 3.0;

vec2 get_partition(vec2 sky_coords) {
	float y = sky_coords.y * 2.0 - 1.0;
	float section_y = ceil(abs(y) * float(vertical_sections)) / float(vertical_sections);
	float horizontal_sections = max(4.0, ceil(float(vertical_sections) * 4.0 * cos(section_y * PI / 2.0)));
	horizontal_sections = horizontal_sections > 0.0 ? horizontal_sections : 1.0;
	return vec2(sky_coords.x * horizontal_sections, y * float(vertical_sections));
}

vec2 rand_vec2(vec2 xy) {
    float rand1 = fract(sin(dot(xy, vec2(11.9131, 81.2317))) * 57183.77193);
    float rand2 = fract(sin(dot(xy, vec2(16.8131, 91.2327))) * 37113.66193);
	return vec2(rand1, rand2) * (1.0 - margin * 2.0) + margin;
}

float hash_float(vec2 p) {
    return fract(sin(dot(p, vec2(12.9898, 78.233))) * 43758.5453);
}
float value_noise(vec2 p) {
    vec2 i = floor(p);
    vec2 f = fract(p);
    vec2 u = f * f * (3.0 - 2.0 * f);
    float a = hash_float(i);
    float b = hash_float(i + vec2(1.0, 0.0));
    float c = hash_float(i + vec2(0.0, 1.0));
    float d = hash_float(i + vec2(1.0, 1.0));
    return mix(mix(a, b, u.x), mix(c, d, u.x), u.y);
}

float random_range(float a, float b, vec2 idx) {
    float h1 = hash_float(idx);
    float h2 = hash_float(idx + vec2(37.0, 73.0));
    float rand01 = fract(h1 * 1000.0 + h2 * 10000.0);
    return mix(a, b, rand01);
}

bool has_star(vec2 idx) {
    float rnd = hash_float(idx);
    float noise_val = value_noise(idx / cluster_scale);
    noise_val = pow(noise_val, cluster_contrast);
    float local_density = star_density * noise_val;

    return rnd < local_density;
}

float blink_factor(float t, float phase, float period, float min_brightness) {
    return mix(min_brightness, 1.0, 0.5 + 0.5 * sin(2.0 * PI * (t / period + phase)));
}

float star_brightness(vec2 sky_coords) {
	vec2 partitions = get_partition(sky_coords);
	vec2 partition_index = floor(partitions);
	vec2 inner_coord = fract(partitions);

    if (!has_star(partition_index))
        return 0.0;

	vec2 star_coord = rand_vec2(partition_index);
	float dist = length(inner_coord - star_coord);
	float star_size = random_range(min_star_size, max_star_size, partition_index);
	float base = pow(star_size / dist, star_falloff);

  	float phase = hash_float(partition_index);
	float blink_period = random_range(blink_period_min, blink_period_max, partition_index);
    float fade = blink_factor(TIME, phase, blink_period, blink_min_brightness);

    return base * fade;
}

void sky() {
	COLOR = vec3(star_brightness(SKY_COORDS));
}
