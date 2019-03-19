Shader "Surface Sprites/Emission/Intensified/1 Aplified by sinus" {
	Properties {
		_MainTex ("Sprite Texture", 2D) = "white" {}
		[Normal] [NoScaleOffset] _BumpMap ("Bumpmap", 2D) = "bump" {}
		[NoScaleOffset] _LightMask ("Light Mask", 2D) = "black" {}
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		_ColorOff ("Off Color", Color) = (0.6, 0.6, 0.6, 1)
		[HDR] _ColorOn ("On Color", Color) = (1, 1, 1, 1)
		[MaterialToggle] _LightOn ("Light On", Float) = 0
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
		uniform sampler2D _LightMask;
		uniform half4 _ColorOn;
		uniform fixed4 _ColorOff;
		uniform fixed _LightOn;


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
			o.color = v.color * _RendererColor;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = SampleSpriteTexture (IN.uv_MainTex) * IN.color;
			fixed4 l = tex2D (_LightMask, IN.uv_MainTex);
			o.Albedo = c.g * c.a * (1 - l.r);
			if (_LightOn == 1) {
				//half adjuster = sin(_Time.z * 6) * 0.3f;
				o.Emission = _ColorOn * l.r;// + half4(adjuster, adjuster, adjuster, l.r) * l.r;
			} else {
				o.Albedo += _ColorOff * l.r;
			}

			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
