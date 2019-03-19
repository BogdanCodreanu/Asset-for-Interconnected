Shader "Surface Sprites/Emission/Cutoff/1 Vector Direction Right" {
	Properties {
		_MainTex ("Sprite Texture", 2D) = "white" {}
		[Normal] [NoScaleOffset] _BumpMap ("Bumpmap", 2D) = "bump" {}
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)

		[NoScaleOffset] _SecondaryColorMap ("Mask (R)", 2D) = "" {}
		_EmissionOpacity ("Emission Opacity", Range(0,1)) = 0

		_AlbedoBehindEmission ("Albedo Behind Emission", Color) = (0, 0, 0, 1)
		[HDR] _EmissionColor ("Emission Color", Color) = (0, 0, 0, 1)

		_Cutoff ("Cutoff", Range(0,1)) = 0
		_LineFade ("Line Fade", Float) = 0
		//_CutoffDirection ("Cutoff Direction", Vector) = (1, 0, 0, 0)
	}

	SubShader {
		Tags { 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
		#pragma multi_compile _ PIXELSNAP_ON
		#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
		#include "UnitySprites.cginc"

		uniform sampler2D _BumpMap;

		uniform fixed _EmissionOpacity;
		uniform sampler2D _SecondaryColorMap;
		uniform half4 _EmissionColor;
		uniform fixed4 _AlbedoBehindEmission;
		uniform float4 _MainTex_TexelSize;
		uniform fixed _Cutoff;
		uniform half _LineFade;
		uniform fixed _LowGraphicsMode;
		//uniform half3 _CutoffDirection;

		struct Input {
			float2 uv_MainTex;
			half4 color;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			v.vertex.xy *= _Flip.xy;

			#if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color * _Color * _RendererColor;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = SampleSpriteTexture (IN.uv_MainTex) * IN.color;
			fixed4 e = tex2D(_SecondaryColorMap, IN.uv_MainTex);
			o.Albedo = (lerp(c.rgb, _AlbedoBehindEmission, e.r)).rgb * c.a;

			if (_EmissionOpacity != 0) {
				fixed atten = 1;
				if (_Cutoff != 1 && _Cutoff != 0) {

					if (_Cutoff < IN.uv_MainTex.x + _LineFade && _Cutoff > IN.uv_MainTex.x - _LineFade) {
						atten = (IN.uv_MainTex.x - (_Cutoff - _LineFade)) / (_LineFade * 2);
					}

					if (_Cutoff >= IN.uv_MainTex.x + _LineFade) {
						atten = 0;
					}
				} else {
					atten = 1 - _Cutoff;
				}
				/*
				//fixed2 cutoffDirection = fixed2(normalize(_CutoffDirection).xy);
				fixed atten = 1;
				if (length(IN.uv_MainTex * cutoffDirection) < _Cutoff * sign(cutoffDirection.x * cutoffDirection.y))
					atten = 0;
				atten = 0;*/
				o.Emission = _EmissionColor * e.r * c.a * _EmissionOpacity * atten;
			}

			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
