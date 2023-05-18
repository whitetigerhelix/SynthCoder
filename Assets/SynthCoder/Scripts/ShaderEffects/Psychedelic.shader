// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

Shader "SynthCoder/Psychedelic"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Speed("Speed", Range(0.1, 10.0)) = 1.0
        _Scale("Scale", Range(1, 10)) = 1
        _Distortion("Distortion", Range(0.1, 10.0)) = 1.0
        _DistortionSpeed("Distortion Speed", Range(0.1, 10.0)) = 1.0
        _Color1("Color 1", Color) = (1, 0.5, 0, 1)
        _Color2("Color 2", Color) = (1, 0, 0.5, 1)
        _Color3("Color 3", Color) = (1, 1, 0, 1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Speed;
            float _Scale;
            float _Distortion;
            float _DistortionSpeed;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;

            float4 ApplyNoise(float4 color, float2 uv)
            {
                float noise = 0;
                float frequency = 0.05;
                float amplitude = 1;
                for (int i = 0; i < 4; i++) {
                    noise += amplitude * tex2D(_MainTex, uv * frequency).r;
                    frequency *= 2;
                    amplitude *= 0.5;
                }
                return color * noise;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float2 distortedUv = v.uv + _Distortion * float2(
                    sin(_DistortionSpeed * _Time.y + v.vertex.x),
                    cos(_DistortionSpeed * _Time.y + v.vertex.y)
                );
                o.uv = distortedUv * _Scale;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv += _Speed * _Time.y;

                float r = sin(uv.x * 10.0 + _Time.y) * 0.5 + 0.5;
                float g = sin(uv.y * 10.0 + _Time.y) * 0.5 + 0.5;
                float b = (r + g) * 0.5;

                float4 color1 = ApplyNoise(_Color1, uv * 2);
                float4 color2 = ApplyNoise(_Color2, uv * 4);
                float4 color3 = ApplyNoise(_Color3, uv * 6);

                float4 color = color1 * r + color2 * g + color3 * b;

                return color;
            }

            ENDCG
        }
    }

    FallBack "Diffuse"
}
