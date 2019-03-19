// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Surface Sprites/Visual Surface Wire" {
	Properties {
		_MainTex ("Sprite Texture", 2D) = "white" {}
		[Normal] [NoScaleOffset] _BumpMap ("Bumpmap", 2D) = "bump" {}
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		_ColorOff ("Off Color", Color) = (0.6, 0.6, 0.6, 1)
		_ColorOn ("On Color", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		_StripeTexture ("Stripe Texture", 2D) = "white" {}
		[HDR] _LightColor ("Light Color", Color) = (1, 1, 1, 1)
		[MaterialToggle] _MovingLight ("Light Moving", Float) = 0
		_SpeedTime ("Light Speed", Float) = 1


		//[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		//[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		//[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
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
		uniform sampler2D _StripeTexture;
		uniform float4 _StripeTexture_ST;
		uniform fixed _MovingLight;
		uniform half4 _LightColor;
		uniform fixed4 _ColorOn;
		uniform fixed4 _ColorOff;
		uniform half _SpeedTime;


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
			if (_MovingLight == 1) {
				o.color *= _ColorOn;
			} else {
				o.color *= _ColorOff;
			}
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = SampleSpriteTexture (IN.uv_MainTex) * IN.color;
			o.Albedo = c.rgb * c.a;

			if (_MovingLight == 1) {
				fixed4 e = mul(unity_ObjectToWorld, tex2D(_StripeTexture, float2(IN.uv_MainTex.x * _StripeTexture_ST.x, IN.uv_MainTex.y * _StripeTexture_ST.y) + fixed2(_Time.z * _SpeedTime, 0)));
				o.Emission = _LightColor.rgb * e.a;
			} /*else {
				fixed4 e = mul(unity_ObjectToWorld, tex2D(_StripeTexture, float2(IN.uv_MainTex.x * _StripeTexture_ST.x, IN.uv_MainTex.y * _StripeTexture_ST.y)));
				o.Albedo += _LightColor.rgb * e.a * .2f;
			}*/

			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
