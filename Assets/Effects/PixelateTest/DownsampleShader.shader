Shader "Custom/DownsampleShader"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_CellSize ("Cell Size", Float) = 0.02
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

			float _CellSize;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			
			float4 frag (v2f IN) : SV_TARGET
			{
				float2 sUV = IN.uv;
				sUV /= _CellSize;
				sUV = round(sUV);
				sUV *= _CellSize;
				return tex2D(_MainTex, sUV);
			}
			ENDCG
		}
	}
}
