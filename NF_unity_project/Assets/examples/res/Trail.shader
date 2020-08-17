Shader "Unlit/Trail"
{
    // ref: unity's Particle Add.shader
    Properties
    {
        _TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
        _MainTex ("Texture", 2D) = "white" {}
    }

    Category
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane" 
        }

        SubShader
        {
            Pass
            {
                Name "Trail"
                Tags
                {
                    "LightMode" = "UniversalForward"
                }
                Blend SrcAlpha One
                Cull Off
                ColorMask RGB
                Lighting Off
                ZWrite Off

                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #pragma multi_compile_fog
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                float4 _MainTex_ST;
                half4 _TintColor;

                struct appdata
                {
                    float4 vertex : POSITION;
                    half4 color : COLOR;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    half4 color : COLOR;
                    float2 uv : TEXCOORD0;
                    float fogCoord : TEXCOORD1;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    ZERO_INITIALIZE(v2f, o);

                    o.vertex = TransformObjectToHClip(v.vertex.xyz);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = v.color;
                    o.fogCoord = ComputeFogFactor(o.vertex.z);

                    return o;
                }

                half4 frag(v2f Input) : SV_Target
                {
                    half4 col = 2.0f * Input.color * _TintColor * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, Input.uv);
                    col.a = saturate(col.a);
                    col.rgb = MixFogColor(col.rgb, half3(0, 0, 0), Input.fogCoord);
                    return col;
                }
                ENDHLSL
            }
        }
    }
}
