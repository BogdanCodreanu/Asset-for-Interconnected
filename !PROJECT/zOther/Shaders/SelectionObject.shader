Shader "Other/Selection" {
	Properties {
		_MainTex ("Texture", 2D) = "black" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Transparency("Transparency", Range(0, 1)) = 1
	}
	SubShader {
		Tags{"Queue" = "Transparent"}
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform fixed4 _Color;
			uniform fixed _Transparency;
			uniform fixed _ColorOverlap;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR {
				fixed mask = tex2D(_MainTex, i.uv).a;


				return _Color * _Transparency * mask;
			}
			ENDCG
		}
	}
}
