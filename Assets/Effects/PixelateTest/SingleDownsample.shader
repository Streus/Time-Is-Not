Shader "Unlit/SingleDownsample"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_CellSize ("Cell Size", Float) = 0.02
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _CellSize;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f IN) : SV_Target
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
