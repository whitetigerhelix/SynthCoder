// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

Shader "SynthCoder/Colorful" {
    Properties{
        _Color("Main Color", Color) = (1, 1, 1, 1)
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 100

            CGPROGRAM
            #pragma surface surf Standard

            sampler2D _MainTex;

            struct Input {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutputStandard o) {
                o.Albedo = _Color.rgb;
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;

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
