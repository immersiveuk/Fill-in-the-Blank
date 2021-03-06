﻿Shader "Postprocessing/FadeToBlackShader"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		
		_FadeLevel("Fade Level", Range(0,1)) = 0
		_TintColor("Color", Color) = (0,0,0,1)
	}

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _FadeLevel;
			fixed4 _TintColor;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col =  tex2D(_MainTex, i.uv);
				return lerp(_TintColor, col, (1-_FadeLevel));
			}
			ENDCG
        }
    }
}
