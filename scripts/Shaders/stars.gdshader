shader_type sky;

/* ------------------------------------------------------------------ */
/* USER‑EDITABLE PARAMETERS (Layer 1 – the original “small stars”)   */
/* ------------------------------------------------------------------ */
uniform int   vertical_sections     = 100;          // many partitions → many stars
uniform float min_star_size         = 0.001;
uniform float max_star_size         = 0.05;

uniform float star_falloff          = 10.0;
uniform float margin                = 0.05;

/* density – a value of 1 ≈ 1 star per partition cell (clamped in code) */
uniform float star_density          = 2.0;

/* glow threshold & multipliers */
uniform float glow_star_size_threshold   = 0.04;     // stars larger than this glow
uniform float big_glow_multiplier        = 2.0;
uniform float small_glow_multiplier      = 3.0;

/* small‑star pulse parameters (seconds) */
uniform float small_glow_interval_min    = 2.5;
uniform float small_glow_interval_max    = 6.0;
uniform float pulse_duration = 0.15;

const float PULSE_GLOW_PROBABILITY = 0.01;

/* ------------------------------------------------------------------ */
/* USER‑EDITABLE PARAMETERS (Layer 2 – scarce, big stars)              */
/* ------------------------------------------------------------------ */
uniform int   vertical_sections_2       = 5;          // few partitions → few stars
uniform float min_star_size_2           = 0.02;
uniform float max_star_size_2           = 0.08;

uniform float star_density_2            = 0.3;
uniform float star_falloff_2          = 10.0;

uniform float big_glow_multiplier_2        = 1.5;

/* ------------------------------------------------------------------ */
/* INTERNAL HELPERS                                                     */
/* ------------------------------------------------------------------ */

/* 2‑D hash → [0,1] */
float hash_float(vec2 p) {
    return fract(sin(dot(p, vec2(12.9898,78.233))) * 43758.5453);
}

/* Random point inside a partition cell (scaled to [margin,1-margin]) */
vec2 rand_vec2(vec2 xy) {
    float r1 = fract(sin(dot(xy, vec2(11.9131,81.2317))) * 57183.77193);
    float r2 = fract(sin(dot(xy, vec2(16.8131,91.2327))) * 37113.66193);
    return vec2(r1, r2) * (1.0 - margin*2.0) + margin;
}

/* Does a star exist in this cell?  Uses the supplied density */
bool has_star_with_density(vec2 idx, float density) {
    float rnd = hash_float(idx);
    /* clamp to [0,1] – density is applied here */
    float threshold = clamp(density * 0.1, 0.0, 1.0);
    return rnd < threshold;
}

/* Partition the unit sphere into a grid that becomes finer near the poles
   (vertical_sections is passed as an int) ---------------------------------*/
vec2 get_partition_layer(vec2 sky_coords, int vert_sections) {
    float y = sky_coords.y * 2.0 - 1.0;               // → [-1,1]
    float section_y = ceil(abs(y) * float(vert_sections))
                       / float(vert_sections);        // quantised Y

    /* The number of horizontal cells depends on latitude (cosine law) */
    float horizontal_sections =
        max(4.0,
            ceil(float(vert_sections) * 4.0 *
                 cos(section_y * PI / 2.0)));

    return vec2(sky_coords.x * horizontal_sections,
                y * float(vert_sections));
}

/* ------------------------------------------------------------------ */
/* CALCULATE BRIGHTNESS FOR ONE LAYER                                 */
/* ------------------------------------------------------------------ */
float layer_brightness(bool centered_star,
                       vec2 sky_coords,
                       int  vert_sections,
                       float min_size,  float max_size,
                       float density,
                       float fall_off,
                       float glow_threshold,
                       float big_glow_mul,
                       float small_glow_mul,
                       float glow_interval_min,
                       float glow_interval_max,
                       float pulse_time) {

    vec2 partitions   = get_partition_layer(sky_coords, vert_sections);
    vec2 partition_idx= floor(partitions);          // integer cell index (as float)
    vec2 inner_coord  = fract(partitions);

    /* No star in this cell → zero brightness */
    if (!has_star_with_density(partition_idx, density))
        return 0.0;

    vec2 star_coord;
    if(centered_star){
        star_coord = vec2(0.5);
    } else {
        star_coord = rand_vec2(partition_idx);
    }

    float dist      = length(inner_coord - star_coord);
    float unique_hash = hash_float(partition_idx);

    float star_size = mix(min_size, max_size, unique_hash);
    float base   = pow(star_size / dist, fall_off);
    base         = clamp(base, 0.0, 1.0);
    float brightness;

    if (star_size > glow_threshold) {
        brightness = base * big_glow_mul;
    } else {
        /* ----- Pulse logic -----
           interval  – a random duration per star
           cycle     – which burst we’re in
           seed      – decides if this star will spike this cycle
         */
        float interval  = mix(glow_interval_min, glow_interval_max, unique_hash);
        float cycle     = floor(TIME / interval);

        float seed      = hash_float(partition_idx + vec2(cycle, 0.0));
        bool  do_pulse  = seed < PULSE_GLOW_PROBABILITY;

        float t_in_cycle = mod(TIME, interval);

        if (do_pulse && t_in_cycle < pulse_time) {
            brightness = base * small_glow_mul;
        } else {
            brightness = base;
        }
    }

    return clamp(brightness, 0.0, 1.0);
}

/* ------------------------------------------------------------------ */
/* SKY ENTRY POINT                                                     */
/* ------------------------------------------------------------------ */
void sky() {
    float b1 = layer_brightness(false, SKY_COORDS,
                                vertical_sections, min_star_size, max_star_size, star_density, star_falloff,
                                glow_star_size_threshold, big_glow_multiplier, small_glow_multiplier,
                                small_glow_interval_min, small_glow_interval_max, pulse_duration);

    float b2 = layer_brightness(true, SKY_COORDS,
                                vertical_sections_2, min_star_size_2, max_star_size_2, star_density_2, star_falloff_2,
                                glow_star_size_threshold, big_glow_multiplier_2, 0.0,   // small_glow_mul unused
                                0.0, 0.0, 0.0);   // pulse params unused

    float total = clamp(b1 + b2, 0.0, 1.0);
    COLOR = vec3(total);
}
