shader_type sky;

/* --------- STARS --------- */
uniform int   vertical_sections     = 100;
uniform float min_star_size         = 0.001;
uniform float max_star_size         = 0.05;
uniform float star_falloff          = 10.0;
uniform float margin                = 0.05;
uniform float star_density          = 2.0;
uniform float glow_star_size_threshold   = 0.04;
uniform float big_glow_multiplier        = 2.0;
uniform float small_glow_multiplier      = 3.0;
uniform float small_glow_interval_min    = 2.5;
uniform float small_glow_interval_max    = 6.0;
uniform float pulse_duration = 0.15;
const float PULSE_GLOW_PROBABILITY = 0.01;

/* --------- BIG STARS --------- */
uniform int   vertical_sections_2       = 5;
uniform float min_star_size_2           = 0.02;
uniform float max_star_size_2           = 0.08;
uniform float star_density_2            = 0.3;
uniform float star_falloff_2          = 10.0;
uniform float big_glow_multiplier_2        = 1.5;

/* --------- CLOUDS --------- */
uniform float cloud_density      = 0.6;     // 0‑1, how “full” the sky is
uniform float cloud_scale        = 0.35;    // higher -> larger blobs
uniform vec3  cloud_color        = vec3(0.4, 0.5, 0.7);   // cool bluish tint
uniform float cloud_opacity      = 0.55;    // overall transparency
uniform float cloud_step_threshold1   = 0.55;      // tweak for more/less blobs
uniform float cloud_step_threshold2 = 0.95;

/* --------- HELPERS --------- */
float hash_float(vec2 p){
    return fract(sin(dot(p, vec2(12.9898,78.233))) * 43758.5453);
}

vec2 rand_vec2(vec2 xy){
    float r1 = fract(sin(dot(xy, vec2(11.9131,81.2317))) * 57183.77193);
    float r2 = fract(sin(dot(xy, vec2(16.8131,91.2327))) * 37113.66193);
    return vec2(r1, r2) * (1.0 - margin*2.0) + margin;
}

bool has_star_with_density(vec2 idx, float density){
    float rnd = hash_float(idx);
    float threshold = clamp(density * 0.1, 0.0, 1.0);
    return rnd < threshold;
}

vec2 get_partition_layer(vec2 sky_coords, int vert_sections){
    float y = sky_coords.y * 2.0 - 1.0;               // → [-1,1]
    float section_y = ceil(abs(y) * float(vert_sections))
                       / float(vert_sections);        // quantised Y
    float horizontal_sections =
        max(4.0,
            ceil(float(vert_sections) * 4.0 *
                 cos(section_y * PI / 2.0)));
    return vec2(sky_coords.x * horizontal_sections,
                y * float(vert_sections));
}

vec4 mod289(vec4 x){return x - floor(x * (1.0 / 289.0)) * 289.0;}
vec4 perm(vec4 x){return mod289(((x * 34.0) + 1.0) * x);}
float noise(vec3 p){
    vec3 a = floor(p);
    vec3 d = p - a;
    d = d * d * (3.0 - 2.0 * d);

    vec4 b = a.xxyy + vec4(0.0, 1.0, 0.0, 1.0);
    vec4 k1 = perm(b.xyxy);
    vec4 k2 = perm(k1.xyxy + b.zzww);

    vec4 c = k2 + a.zzzz;
    vec4 k3 = perm(c);
    vec4 k4 = perm(c + 1.0);

    vec4 o1 = fract(k3 * (1.0 / 41.0));
    vec4 o2 = fract(k4 * (1.0 / 41.0));

    vec4 o3 = o2 * d.z + o1 * (1.0 - d.z);
    vec2 o4 = o3.yw * d.x + o3.xz * (1.0 - d.x);

    return o4.y * d.y + o4.x * (1.0 - d.y);
}
/* --------- STAR LAYER --------- */
float layer_brightness(bool centered_star, vec2 sky_coords, int vert_sections,
                       float min_size, float max_size, float density,
                       float fall_off, float glow_threshold,
                       float big_glow_mul, float small_glow_mul,
                       float glow_interval_min, float glow_interval_max,
                       float pulse_time){
    vec2 partitions   = get_partition_layer(sky_coords, vert_sections);
    vec2 partition_idx= floor(partitions);          // integer cell index (as float)
    vec2 inner_coord  = fract(partitions);

    if (!has_star_with_density(partition_idx, density))
        return 0.0;

    vec2 star_coord = centered_star ? vec2(0.5) : rand_vec2(partition_idx);
    float dist      = length(inner_coord - star_coord);
    float unique_hash = hash_float(partition_idx);

    float star_size = mix(min_size, max_size, unique_hash);
    float base   = pow(star_size / dist, fall_off);
    base         = clamp(base, 0.0, 1.0);
    float brightness;

    // if (star_size > glow_threshold) {
    //     brightness = base * big_glow_mul;
    // } else {
    //     float interval  = mix(glow_interval_min, glow_interval_max, unique_hash);
    //     float cycle     = floor(TIME / interval);
    //     float seed      = hash_float(partition_idx + vec2(cycle, 0.0));
    //     bool  do_pulse  = seed < PULSE_GLOW_PROBABILITY;
    //     float t_in_cycle = mod(TIME, interval);

        // if (do_pulse && t_in_cycle < pulse_time) {
        //     brightness = base * small_glow_mul;
        // } else {
        brightness = base;
        // }
    // }

    return clamp(brightness, 0.0, 1.0);
}

/* --------- CLOUD LAYER --------- */
float cloud_brightness(vec3 dir) {
    /* ---- 1. Generate noise in a 3‑D space ---- */
    float n = noise(dir * cloud_scale + vec3(0.0, 0.0, 1));

    /* ---- 2. Convert [-1,1] → [0,1] ---- */
    n = clamp(n * 0.5 + 0.5, 0.0, 1.0);

    /* ---- 3. Optional turbulence for extra detail ---- */
    float t = noise(dir * cloud_scale * 2.0 + vec3(0.0, 0.0, 1));
    n += t * 0.15;
    n = clamp(n, 0.0, 1.0);
    /* ---- 4. Threshold to create gaps between clouds ---- */

    float mask = smoothstep(cloud_step_threshold1, cloud_step_threshold2, n);

    /* ---- 5. Final brightness (only where mask > 0) ---- */
    return clamp(mask * cloud_density, 0.0, 1.0);
}

void sky(){
    /* Layer 1 – small stars */
    float b1 = layer_brightness(false, SKY_COORDS,
                                vertical_sections, min_star_size, max_star_size,
                                star_density, star_falloff,
                                glow_star_size_threshold, big_glow_multiplier,
                                small_glow_multiplier, small_glow_interval_min,
                                small_glow_interval_max, pulse_duration);

    /* Layer 2 – big stars */
    float b2 = layer_brightness(true, SKY_COORDS,
                                vertical_sections_2, min_star_size_2, max_star_size_2,
                                star_density_2, star_falloff_2,
                                glow_star_size_threshold, big_glow_multiplier_2,
                                0.0, 0.0, 0.0, 0.0);
    /* Layer 3 – gas clouds */
    float cloud_b = cloud_brightness(EYEDIR);
    vec3  cloud_col = cloud_color * cloud_opacity;   // color * transparency

    /* Composite: add stars + soft‑additive blend with clouds */
    vec3 star_rgb = vec3(b1 + b2);                   // bright stars are already RGB
    vec3 sky_rgb  = mix(star_rgb, cloud_col, cloud_b);

    COLOR = vec3(clamp(sky_rgb, 0.0, 1.0));
}