Shader "Particles/Emission with Mask" {
	Properties {
		_EmissionTex ("Emision (R)", 2D) = "white" {}
		[HDR] _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
	}
	SubShader {
		Tags { 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		LOD 200
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma surface surf Lambert fullforwardshadows vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
		#include "UnitySprites.cginc"

		#pragma target 3.0

		uniform sampler2D _EmissionTex;

		struct Input {
			float2 uv_EmissionTex;
		};

		uniform half _Speed;
		uniform half4 _EmissionColor;

		void vert (inout appdata_full v, out Input o)
		{
			v.vertex.xy *= _Flip.xy;

			#if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = mul(unity_ObjectToWorld, tex2D (_EmissionTex, IN.uv_EmissionTex));
			o.Emission = c.r * _EmissionColor;
			o.Albedo = c.r * _EmissionColor;
			o.Alpha = c.r;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
