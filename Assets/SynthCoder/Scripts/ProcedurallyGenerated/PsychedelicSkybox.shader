// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!


//TODO: This doesn't work quite right yet - only is solid color of _Color1


// This shader creates a dynamic skybox that changes color and shape over time. 
// It takes in parameters for speed, scale, and three colors. 
// The skybox is created using simplex noise, which generates a pattern of colors and shapes. 
// The noise function is used to modify the vertex positions of the skybox, creating a wavy effect. 
// Finally, the colors of the skybox are determined by interpolating between the three colors based on the noise value, 
// and raising the noise to the power of three. The shader falls back to a default skybox if it cannot be used.
Shader "SynthCoder/PsychedelicSkybox"
{
    Properties
    {
        _Speed("Speed", Range(0, 10)) = 1
        _Scale("Scale", Range(0, 10)) = 1
        _Color1("Color 1", Color) = (1, 0, 0, 1)
        _Color2("Color 2", Color) = (0, 1, 0, 1)
        _Color3("Color 3", Color) = (0, 0, 1, 1)
    }

    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Background" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f 
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float _Speed;
            float _Scale;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;

            float3 mod289(float3 x)
            {
                return x - floor(x * (1.0 / 289.0)) * 289.0;
            }

            float3 permute(float3 x)
            {
                return mod289(((x * 34.0) + 1.0) * x);
            }

            float3 taylorInvSqrt(float3 r)
            {
                return 1.79284291400159 - 0.85373472095314 * r;
            }

            float3 fade(float3 t)
            {
                return t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
            }

            float3 rand3(float3 seed)
            {
                return frac(sin(dot(seed, float3(12.9898, 78.233, 45.164))) * 43758.5453);
            }

            float rand(float2 seed)
            {
                return frac(sin(dot(seed.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            float rand(float3 seed)
            {
                return frac(sin(dot(seed.xyz, float3(12.9898, 78.233, 45.164))) * 43758.5453);
            }

            float simplexNoise(float3 v)
            {
                const float F3 = 0.3333333;
                const float G3 = 0.1666667;
                float3 s = floor(v + dot(v, float3(F3, F3, F3)));
                float3 x = v - s + dot(s, float3(G3, G3, G3));
                float3 e = step(float3(0.0, 0.0, 0.0), x - x.yzx);
                float3 i1 = e * (1.0 - e.zxy);
                float3 i2 = 1.0 - e.zxy * (1.0 - e);
                float3 x1 = x - i1 + G3;
                float3 x2 = x - i2 + 2.0 * G3;
                float3 x3 = x - 1.0 + 3.0 * G3;
                float4 w, d;
                w.x = dot(x, x);
                w.y = dot(x1, x1);
                w.z = dot(x2, x2);
                w.w = dot(x3, x3);
                w = max(0.6 - w, 0.0);
                d.x = dot(rand(float3(s)), x);
                d.y = dot(rand(float3(s) + i1), x1);
                d.z = dot(rand(float3(s) + i2), x2);
                d.w = dot(rand(float3(s) + 1.0), x3);
                d = max(0.5 - d, 0.0);
                w *= w;
                w = (w * w) * w;
                return dot(d, w);
            }

            void vert(inout appdata v)
            {
                v.vertex.xyz *= 1.0 + 0.1 * (simplexNoise(v.vertex.xyz * _Scale * _Speed) - 0.5);
                v.vertex = UnityObjectToClipPos(v.vertex);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 noisePos = i.worldPos * _Scale * _Speed;
                float noise = simplexNoise(noisePos);
                float4 color = lerp(_Color1, _Color2, noise);
                color = lerp(color, _Color3, pow(noise, 3.0));
                return color;
            }

            ENDCG
        }
    }
    FallBack "Skybox/Cubemap"
}
