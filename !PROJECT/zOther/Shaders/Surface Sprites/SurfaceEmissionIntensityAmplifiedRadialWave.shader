Shader "Surface Sprites/Emission/Intensified/2 Radial Light" {
	Properties {
		_MainTex ("Sprite Texture", 2D) = "white" {}
		[Normal] [NoScaleOffset] _BumpMap ("Bumpmap", 2D) = "bump" {}
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)

		[NoScaleOffset] _SecondaryColorMap ("Map (R)", 2D) = "" {}
		[HDR] _RedColorPulse ("Color on (R) While Pulsating", Color) = (1, 1, 1, 1)
		[HDR] _RedColor ("Color on Red Simple", Color) = (1, 1, 1, 1)

		_TimeSpeed ("Time Speed", Float) = 1
		_WaveLength ("Wave Length", Float) = 0.1
		_DistanceToMove ("Distance to Move from Center", Float) = 1
		_FadeDist ("Fade Length", Float) = 0.1
		_Center ("Offset Center", Vector) = (0, 0, 0, 0)
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

		uniform sampler2D _SecondaryColorMap;
		uniform half4 _RedColor;
		uniform half4 _RedColorPulse;
		uniform float4 _MainTex_TexelSize;
		uniform half _TimeSpeed;
		uniform half _WaveLength;
		uniform half _DistanceToMove;
		uniform half _FadeDist;
		uniform half3 _Center;

		struct Input {
			float2 uv_MainTex;
			half4 color;
			float3 localPos;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			v.vertex.xy *= _Flip.xy;

			#if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color * _Color * _RendererColor;
			o.localPos = v.vertex.xyz;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = SampleSpriteTexture (IN.uv_MainTex) * IN.color;
			o.Albedo = c.rgb * c.a;

			fixed4 e = tex2D(_SecondaryColorMap, IN.uv_MainTex);
			
			o.Albedo = (lerp(c.rgb, _RedColor, e.r)).rgb * e.a * c.a;
			o.Emission = (lerp(c.rgb, _RedColor, e.r) * c.a * e.r).rgb;

			
			half len = length(IN.localPos - _Center);
			half distToColor = frac(_Time.y * _TimeSpeed / _DistanceToMove) * _DistanceToMove;
			fixed atten = 1;

			if (len > distToColor - _WaveLength && len < distToColor) {
				if (len < distToColor && len > distToColor - _FadeDist)
					atten = (distToColor - len) / _FadeDist;

				if (len > distToColor - _WaveLength && len < distToColor - _WaveLength + _FadeDist)
					atten = (len - (distToColor - _WaveLength)) / _FadeDist;

				o.Emission = lerp(o.Emission, _RedColorPulse * e.r, atten);
			}

			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
