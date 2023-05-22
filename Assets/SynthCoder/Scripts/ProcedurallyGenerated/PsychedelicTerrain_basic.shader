// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

Shader "SynthCoder/PsychedelicTerrain_basic"
{
    Properties
    {
        _MainTex ("Terrain Texture", 2D) = "white" {}
        _Amplitude("Amplitude", Range(0, 1)) = 0.1
        _Frequency("Frequency", Range(0, 10)) = 1
        _Speed("Speed", Range(-1, 1)) = 0.1
        _Distortion("Distortion", Range(0, 1)) = 0.5
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 1, 0, 1)
        _Color3 ("Color 3", Color) = (0, 0, 1, 1)
    }
 
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
 
        CGPROGRAM
        #pragma surface surf Lambert
 
        sampler2D _MainTex;
        float _Amplitude;
        float _Frequency;
        float _Speed;
        float _Distortion;
        float4 _Color1;
        float4 _Color2;
        float4 _Color3;
 
        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };
 
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Calculate the displacement vector
            float3 disp = float3(0, 0, 0);
            float3 noisePos = IN.worldPos * _Frequency + _Speed * _Time.y;
            //TODO: Missing noise texture
            /*float noise = _NoiseTex.Sample(_NoiseTexSampler, noisePos).r - 0.5;
            disp.x += noise;
            disp.y += noise;
            disp.z += noise;*/
            disp *= _Amplitude * _Distortion;
 
            // Apply the displacement vector to the vertex
            IN.worldPos += disp;
 
            // Calculate the color based on the world position
            float3 color = float3(0, 0, 0);
            color.x = sin(IN.worldPos.x * _Distortion) * _Color1.r + cos(IN.worldPos.x * _Distortion) * _Color2.r + sin(IN.worldPos.z * _Distortion) * _Color3.r;
            color.y = sin(IN.worldPos.y * _Distortion) * _Color1.g + cos(IN.worldPos.y * _Distortion) * _Color2.g + sin(IN.worldPos.x * _Distortion) * _Color3.g;
            color.z = sin(IN.worldPos.z * _Distortion) * _Color1.b + cos(IN.worldPos.z * _Distortion) * _Color2.b + sin(IN.worldPos.y * _Distortion) * _Color3.b;
 
            o.Albedo = color;
            o.Alpha = 1;
            o.Emission = color;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

