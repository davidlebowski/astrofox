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
				fixed3 color : TEXCOORD0;
			};

			fixed4 _Color;
			static const fixed FRESNEL_POWER = 1.1;
			static const fixed INTENSITY = 2;

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				half3 normal = UnityObjectToWorldDir(v.normal);
				half3 viewDir = WorldSpaceViewDir(v.vertex);
				// Calculate simple fresnel
				o.color = lerp(0, _Color, FRESNEL_POWER - dot(normalize(normal), normalize(viewDir))) * INTENSITY;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
			    return fixed4(i.color, 1);
			}
			ENDCG
		}
	}
}
