// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

Shader "SynthCoder/Colorful_simple"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Opaque"}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Input structure for vertex shader
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            // Output structure for vertex shader
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            // Main color property
            float4 _MainColor;

            // Vertex shader
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = mul(unity_WorldToObject, v.normal).xyz;
                return o;
            }

            // Fragment shader
            float4 frag(v2f i) : SV_Target
            {
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 normal = normalize(i.normal);
                float diffuse = max(0.0, dot(lightDir, normal));
                return float4(_MainColor.rgb * diffuse, _MainColor.a);
            }

            ENDCG
        }
    }
}

