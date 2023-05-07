// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

Shader "Custom/ColorBarShader Transparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Range(0, 10)) = 0.5
        _Scale ("Scale", Range(-100,100)) = 5
    }
    SubShader
    {
        Tags {"Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
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
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float4 color;
                color.r = sin(i.uv.y * _Scale + _Time.y * _Speed);
                color.g = sin(i.uv.y * _Scale + 2 * _Time.y * _Speed);
                color.b = sin(i.uv.y * _Scale + 4 * _Time.y * _Speed);
                color.a = 0.5;
                return color;
            }
            ENDCG
        }
    }
}
