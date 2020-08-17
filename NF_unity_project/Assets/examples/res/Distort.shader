Shader "Unlit/Distort"
{
	Properties {
		_DispMap ("Displacement Map (RG)", 2D) = "white" {}
		_MaskTex ("Mask (R)", 2D) = "white" {}
		_DispScrollSpeedX  ("Map Scroll Speed X", Float) = 0
		_DispScrollSpeedY  ("Map Scroll Speed Y", Float) = 0
		_StrengthX  ("Displacement Strength X", Float) = 1
		_StrengthY  ("Displacement Strength Y", Float) = -1
	}

	Category
	{
		Tags
		{
			"Queue"="Transparent+99"
			"RenderType"="Transparent"
		}

		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Lighting Off
		ZWrite Off
		ZTest Always
		
		SubShader
		{
			Pass
			{
				Name "Distort"
				Tags
				{
					"LightMode" = "UniversalForward"
				}

				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

				TEXTURE2D(_DispMap);
				SAMPLER(sampler_DispMap);
				float4 _DispMap_ST;
				TEXTURE2D(_MaskTex);
				SAMPLER(sampler_MaskTex);
				float4 _MaskTex_ST;
				
				half _DispScrollSpeedY;
				half _DispScrollSpeedX;
				half _StrengthX;
				half _StrengthY;

				//TEXTURE2D(_CameraDepthTexture);
				//SAMPLER(sampler_CameraDepthTexture);
				//float4 _CameraDepthTexture_TexelSize;

				TEXTURE2D(_CameraOpaqueTexture);
				SAMPLER(sampler_CameraOpaqueTexture);
				float4 _CameraOpaqueTexture_TexelSize;
				

				struct appdata
				{
					float4 vertex : POSITION;
					half4 color : COLOR;
					float2 texcoord: TEXCOORD0;
					float2 param : TEXCOORD1;
				};

				struct v2f
				{
					float4 vertex : POSITION;
					half4 color : COLOR;
					float2 uvmain : TEXCOORD0;
					float2 param : TEXCOORD1;
					float4 uvgrab : TEXCOORD2;
				};

				
				v2f vert (appdata v)
				{
					v2f o;
					ZERO_INITIALIZE(v2f, o);

					o.vertex = TransformObjectToHClip(v.vertex.xyz);
					#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
					#else
					float scale = 1.0;
					#endif
					o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
					o.uvgrab.zw = o.vertex.zw;
					o.uvmain = TRANSFORM_TEX(v.texcoord, _DispMap);
					o.color = v.color;
					o.param = v.param;
					return o;
				}


				half4 frag(v2f i) : SV_Target
				{
					half2 mapoft = half2(_Time.y*_DispScrollSpeedX, _Time.y*_DispScrollSpeedY);

					half4 offsetColor = SAMPLE_TEXTURE2D(_DispMap, sampler_DispMap, i.uvmain + mapoft);

					half oftX =  offsetColor.r * _StrengthX * i.param.x;
					half oftY =  offsetColor.g * _StrengthY * i.param.x;

					i.uvgrab.x += oftX;
					i.uvgrab.y += oftY;
					
					//half4 col = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.uvgrab.xy / i.uvgrab.w);
					half4 col = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, i.uvgrab.xy / i.uvgrab.w);
					col.a = i.color.a;

					half4 tint = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uvmain);
					col.a *= tint.r;

					return col;
				}
				ENDHLSL
			}
		}
	}
}
