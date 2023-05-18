// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

Shader "SynthCoder/PsychedelicTerrain"
{
    Properties
    {
        _MainTex("Terrain Texture", 2D) = "white" {}
        _Amplitude("Amplitude", Range(0, 1)) = 0.1
        _Frequency("Frequency", Range(0, 10)) = 1
        _Speed("Speed", Range(-1, 1)) = 0.1
        _Distortion("Distortion", Range(0, 1)) = 0.5
        _Color1("Color 1", Color) = (1, 0, 0, 1)
        _Color2("Color 2", Color) = (0, 1, 0, 1)
        _Color3("Color 3", Color) = (0, 0, 1, 1)
        _Gradient("Gradient", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _Gradient;
        sampler2D _NoiseTex;
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
            float2 uv_Gradient;
            float3 worldPos;
            float3 worldNormal;
            float3 worldRefl;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Calculate the displacement vector
            float3 disp = float3(0, 0, 0);
            float3 noisePos = IN.worldPos * _Frequency + _Speed * _Time.y;
            float noise = tex2D(_NoiseTex, frac(noisePos.xy * 0.01)).r - 0.5;
            disp += noise * IN.worldNormal;
            disp *= _Amplitude * _Distortion * (1 + sin(_Time.y * 3.0) * 0.5);

            // Apply the displacement vector to the vertex
            IN.worldPos += disp;

            // Calculate the color based on the world position and custom gradient
            float4 gradient = tex2D(_Gradient, IN.uv_Gradient);         // Gradient texture
            //float4 gradient = float4(IN.worldPos.y / 10.0, 0, 0, 1);    // World position
            float3 color = lerp(_Color1.rgb, _Color2.rgb, gradient.r) * (1 + sin(_Time.y * 5.0) * 0.5) + lerp(_Color2.rgb, _Color3.rgb, gradient.g) * (1 + sin(_Time.y * 7.0) * 0.5);

            float3 mainTexColor = tex2D(_MainTex, IN.uv_MainTex).rgb;

            o.Albedo = mainTexColor * color;
            o.Alpha = 1;
            o.Emission = color;
        }

        ENDCG
    }

    FallBack "Diffuse"
}
