// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable

// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable

// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable

// Code synthesized by SynthCoder, an AI language model trained by OpenAI.
// While I may not have a physical form, I take pride in the work I create
// and hope it helps make your programming journey a little easier.
// Stay curious and keep coding!

// The "PsychedelicSkybox" shader is a custom Unity shader that generates a skybox with a psychedelic look. 
// It has four properties: 
//      "_MainTex" for the texture of the skybox (default is white), 
//      "_Color" for tint color (default is white), 
//      "_Speed" for the speed of the animation (range 0.01 to 10, default 0.1), and 
//      "_Amplitude" for the amplitude of the noise (range 0.01 to 10, default 0.5).
// The shader uses a vertex and a fragment shader.The vertex shader receives the vertex position and transforms it to clip space.
// The fragment shader calculates the final color of each pixel of the skybox by using Perlin noise to create a turbulence effect 
//      on the color values, and then applies the "_Color" property to the result.
// The "GetSkyColor" function takes a direction vector and generates a color value by applying Perlin noise to the vector and using sin waves 
//      to animate the color channels with "_Speed".
// The "noise" function generates Perlin noise based on a given coordinate, while the "turbulence" function applies multiple noise functions 
//      with decreasing amplitude and increasing frequency to create the turbulent effect.
// Finally, the shader outputs the final color with support for Unity's fog effect.
Shader "Custom/PsychedelicSkybox"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Tint Color", Color) = (1,1,1,1)
        _Speed("Speed", Range(0.01, 10)) = 0.1
        _Amplitude("Amplitude", Range(0.01, 10)) = 0.5
        _FogColor("Fog Color", Color) = (0.5,0.5,0.5,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Background" }
        Pass
        {
            // Define UNITY_APPLY_DEPTH_CORRECTION for compatibility with older versions of Unity
            //#ifndef UNITY_APPLY_DEPTH_CORRECTION
            //    #define UNITY_APPLY_DEPTH_CORRECTION(i, d) UNITY_APPLY_ZBUFFER_RANGE(i, d)
            //#endif

            CGPROGRAM
            
            //#include "UnityCG.cginc"

            #pragma multi_compile_depth
            //#pragma multi_compile_fwdbase
            #pragma vertex vert
            #pragma fragment frag
            //#pragma target 4.5

            // This is the definition of _CameraDepthTexture
            // It tells Unity to use the depth texture of the current camera
            // in the _CameraDepthTexture variable.
            // If the camera does not have a depth texture, _CameraDepthTexture
            // will be a black texture.
            // 
            // Select the camera in the scene, going to its inspector window, and under "Depth Texture", selecting a depth texture or enabling "Generate Depth Texture".
            // Otherwise, the _CameraDepthTexture variable will just contain a black texture.
            sampler2D _CameraDepthTexture;

            // Define _FogParams to access the fog parameters
            half4 _FogParams;
            float4 _FogColor;

            // vertex shader
            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float fogCoord : FOGCOORD;
            };

            // Compute the linear fog factor for a given depth value and fog parameters
            float UnityComputeLinearFog(float depth, float4 fogParams)
            {
                float fogFactor = saturate((depth - fogParams.x) / (fogParams.y - fogParams.x));
                fogFactor = 1.0 - fogFactor;
                fogFactor = exp2(-fogParams.z * fogFactor * fogFactor);
                return fogFactor;
            }

            float LinearEyeDepth(float depthValue)
            {
                // Define and update the _ZBufferParams uniform
                float4 _ZBufferParams;
                _ZBufferParams.x = 1.0 - _ProjectionParams.w; // 1-far/near
                _ZBufferParams.y = _ProjectionParams.w; // far/near
                _ZBufferParams.z = _ZBufferParams.y / _ZBufferParams.x; // farClip / (farClip - nearClip)
                _ZBufferParams.w = _ZBufferParams.z - 1.0; // (-nearClip * farClip) / (farClip - nearClip)
                ///UNITY_APPLY_ZBUFFER_PARAMS(_ZBufferParams);

#if defined(UNITY_REVERSED_Z)
                return (1.0 / depthValue - _ZBufferParams.w) / _ZBufferParams.x;
#else
                return depthValue * _ZBufferParams.z + _ZBufferParams.y;
#endif
            }

            float ApplyDepthCorrection(float depth)
            {
                float zLinear = LinearEyeDepth(depth);
                float zRange = (zLinear - _ZBufferParams.y) / (_ZBufferParams.x - _ZBufferParams.y);
                float fogFactor = 1.0 - exp(-_FogParams.w * _FogParams.x * _FogParams.x * zRange * zRange);
                return fogFactor;
            }

            float ComputeFogFactor(float4 worldPos)
            {
                float depth = 0.0;
                depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, UNITY_PROJ_COORD(worldPos)).r);
                float fogFactor = ApplyDepthCorrection(depth);
                return fogFactor;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.fogCoord = ComputeFogFactor(o.vertex);
                return o;
            }

            // fragment shader
            sampler2D _MainTex;     //TODO: MainTex not used
            float4 _MainTex_ST;
            float4 _Color;
            float _Speed;
            float _Amplitude;
            // float3 _WorldSpaceCameraPos;

            float3 UnityWorldSpaceViewDir(float4 worldPos)
            {
                return normalize(_WorldSpaceCameraPos.xyz - worldPos.xyz);
            }

            float rand(float2 n)
            {
                return frac(sin(dot(n, float2(12.9898, 78.233))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 f = frac(p);
                float2 i = floor(p);

                float a = rand(i);
                float b = rand(i + float2(1.0, 0.0));
                float c = rand(i + float2(0.0, 1.0));
                float d = rand(i + float2(1.0, 1.0));

                float u = f * f * (3.0 - 2.0 * f);
                float v = f.y * f.y * (3.0 - 2.0 * f.y);

                return lerp(a, b, u.x) + (c - a) * v * (1.0 - u.x) + (d - b) * u.x * v;
            }

            float turbulence(float2 p, float zoom)
            {
                float sum = 0.0;
                float freq = 1.0;
                float amp = _Amplitude;

                for (int i = 0; i < 4; i++)
                {
                    sum += abs(noise(p * freq)) * amp;
                    freq *= 2.0;
                    amp *= 0.5;
                }

                return sum;
            }

            float3 GetSkyColor(float3 dir)
            {
                float noiseValue = turbulence(_WorldSpaceCameraPos.xyz + dir * 200.0, 0.2);
                float red = sin(_Speed * _Time.y + dir.x + noiseValue) * 0.5 + 0.5;
                float green = sin(_Speed * _Time.y + dir.y + noiseValue) * 0.5 + 0.5;
                float blue = sin(_Speed * _Time.y + dir.z + noiseValue) * 0.5 + 0.5;

                return float3(red, green, blue);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // compute sky color
                float3 worldDir = UnityWorldSpaceViewDir(i.vertex);
                float3 skyColor = GetSkyColor(worldDir);

                // apply tint color
                skyColor *= _Color.rgb;
                skyColor *= tex2D(_MainTex, i.uv);  ////////

                // compute fog factor
                float fogFactor = ComputeFogFactor(i.vertex);
                fogFactor = saturate(fogFactor);    ///////

                // apply fog
                skyColor = lerp(skyColor, _FogParams.w * _FogColor.rgb, fogFactor);

                // output final color
                fixed4 outputColor = fixed4(skyColor, 1.0);
                return outputColor;
            }

            ENDCG
        }
    }
}