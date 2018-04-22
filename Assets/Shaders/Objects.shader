Shader "Astrofox/Objects"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				half3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				half3 normal : TEXCOORD0;
				half3 viewDir : TEXCOORD1;
			};

			fixed4 _Color;

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldDir(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
			    fixed4 col = _Color;
			    return lerp(0, _Color, 1.1 - dot(normalize(i.normal), normalize(i.viewDir))) * 2;
			}
			ENDCG
		}
	}
}
