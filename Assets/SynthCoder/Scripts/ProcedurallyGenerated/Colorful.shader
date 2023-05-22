// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

// This shader uses a Perlin noise function to generate a colorful pattern that is multiplied by a factor of 2 and used as the emission color. 
// This results in a bright and colorful mesh that is easy to see, even with lighting. The main color of the mesh can be set using the _Color property, 
// and the Smoothness and Metallic properties can be adjusted to control the reflectivity and metallic properties of the mesh.
Shader "SynthCoder/Colorful"
{
    Properties
    {
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Standard

        float fract(float x) { return x - floor(x); }
        float4 mod289(float4 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
        float3 mod289(float3 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
        float2 mod289(float2 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
        float4 permute(float4 x) { return ((x * 34.0) + 1.0) * mod289(x); } //{ return ((x * 34.0) + 1.0) * x; }
        float3 permute(float3 x) { return ((x * 34.0) + 1.0) * mod289(x); }
        float4 taylorInvSqrt(float4 r) { return 1.79284291400159 - 0.85373472095314 * r; }
        float3 taylorInvSqrt(float3 r) { return 1.79284291400159 - 0.85373472095314 * r; }

        struct Input
        {
            float2 uv_MainTex;
            float4 _Color;
            float _Glossiness;
            float _Metallic;
        };

        // This implementation of the noise function is adapted from the book "The Book of Shaders" by Patricio Gonzalez Vivo and Jen Lowe.
        float noise(float2 P)
        {
            const float2 C = float2(0.211324865405187, 0.366025403784439);
            float2 i = floor(P + dot(P, C.yy));
            float2 x0 = P - i + dot(i, C.xx);
            float2 i1;
            i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
            float4 x12 = float4(x0.xy, x0.xy);
            x12 = x12 + float4(C.x, C.y, C.x, C.y); // This line adds a small offset to the coordinates to help break up any symmetry in the noise.
            x12.xy -= i1;
            i = mod289(i);
            float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0)) + i.x + float3(0.0, i1.x, 1.0));
            float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
            m = m * m;
            m = m * m;
            float3 x = 2.0 * fract(p * float3(1.0 / 41.0, 1.0 / 953.0, 1.0 / 337.0)) - 1.0;
            float3 h = abs(x) - 0.5;
            float3 ox = floor(x + 0.5);
            float3 a0 = x - ox;
            m *= taylorInvSqrt(a0 * a0 + h * h);
            float4 g = float4(0.0, 0.5, 1.0, 2.0) - float4(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw), dot(x12.xy + x0.xy, x12.xy + x0.xy));
            float4 norm = taylorInvSqrt(g);
            g *= norm;
            return 2.2 * dot(m, g.yzw);
        }

        void surf(Input IN, inout SurfaceOutputStandard o) 
        {
            o.Albedo = IN._Color.rgb;
            o.Metallic = IN._Metallic;
            o.Smoothness = IN._Glossiness;

            // Use a Perlin noise function to create a colorful pattern
            float r = noise(IN.uv_MainTex * 5.0);
            float g = noise((IN.uv_MainTex + float2(1.2345, 6.7890)) * 7.0);
            float b = noise((IN.uv_MainTex + float2(9.8765, 4.3210)) * 11.0);

            o.Emission = (r * float3(1.0, 0.0, 0.0) + g * float3(0.0, 1.0, 0.0) + b * float3(0.0, 0.0, 1.0)) * 2.0;
        }

        ENDCG
    }

    FallBack "Diffuse"
}
