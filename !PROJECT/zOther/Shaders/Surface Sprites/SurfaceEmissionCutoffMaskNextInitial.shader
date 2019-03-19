Shader "Surface Sprites/Emission/Cutoff/2.b Mask Next Initial" {
	Properties {
		_MainTex ("Sprite Texture", 2D) = "white" {}
		[Normal] [NoScaleOffset] _BumpMap ("Bumpmap", 2D) = "bump" {}
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)

		[NoScaleOffset] _SecondaryColorMap ("Mask (RG)", 2D) = "" {}
		_EmissionOpacity ("Emission Opacity", Range(0,1)) = 0

		_AlbedoBehindEmission ("Albedo Behind Emission", Color) = (0, 0, 0, 1)
		[HDR] _EmissionColorNow ("Emission Color Now", Color) = (0, 0, 0, 1)
		[HDR] _EmissionColorNext ("Emission Color Next", Color) = (0, 0, 0, 1)

		_Cutoff ("Cutoff from (G)", Range(0,1)) = 0
		_CutoffRange ("Line Fade", Float) = 0
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
		uniform half4 _EmissionColorNow;
		uniform half4 _EmissionColorNext;
		uniform fixed4 _AlbedoBehindEmission;
		uniform float4 _MainTex_TexelSize;
		uniform fixed _Cutoff;
		uniform half _CutoffRange;

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
				half largerCutoff = (_Cutoff * (1 + _CutoffRange)) - _CutoffRange;

				o.Emission = lerp(_EmissionColorNext, _EmissionColorNow, saturate((e.g - largerCutoff) / _CutoffRange) ) * e.r * _EmissionOpacity ;
			}


			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));

			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
