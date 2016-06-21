Shader "Unlit/spriteshader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma multi_compile DUMMY PIXELSNAP_ON
	#include "UnityCG.cginc"

		struct appdata_t
		{
			float4 vertex   : POSITION;
			float4 color    : COLOR;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f
		{
			float4 vertex   : SV_POSITION;
			fixed4 color : COLOR;
			half2 texcoord  : TEXCOORD0;
		};

		fixed4 _Color;

		v2f vert(appdata_t IN)
		{
			v2f OUT;
			OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
			OUT.texcoord = IN.texcoord;
			OUT.color = IN.color * _Color;
	#ifdef PIXELSNAP_ON
			OUT.vertex = UnityPixelSnap(OUT.vertex);
	#endif

			return OUT;
		}

		sampler2D _MainTex;

		fixed4 frag(v2f IN) : SV_Target
		{
			fixed4 c = fixed4(0,0,0,1);
			fixed4 tex = tex2D(_MainTex, IN.texcoord);
		
			if (tex.r > 0.5) {
				c.r = 1 - ((1 - 2 * (tex.r-0.5)) * (1 - IN.color.r ));
			}
			else if (tex.r <= 0.5) {
				c.r = 2*tex.r * IN.color.r ;
			}

			if (tex.g > 0.5) {
				c.g = 1 - ((1 - 2 * (tex.g - 0.5)) * (1 - IN.color.g));
			}
			else if (tex.g <= 0.5) {
				c.g = 2 * tex.g * IN.color.g;
			}

			if (tex.b > 0.5) {
				c.b = 1 - ((1 - 2 * (tex.b - 0.5)) * (1 - IN.color.b));
			}
			else if (tex.b <= 0.5) {
				c.b = 2 * tex.b * IN.color.b;
			}
			/*if (IN.color.r > 0.5) {
				c.r = 1 - ((1 - tex.r) * (1 - (IN.color.r - 0.5)));
			}
			else if (IN.color.r <= 0.5) {
				c.r = tex.r * (IN.color.r + 0.5);
			}

			if (IN.color.g > 0.5) {
				c.g = 1 - ((1 - tex.g) * (1 - (IN.color.g - 0.5)));
			}
			else if (IN.color.g <= 0.5) {
				c.g = tex.g * (IN.color.g + 0.5);
			}

			if (IN.color.b > 0.5) {
				c.b = 1 - ((1 - tex.b) * (1 - (IN.color.b - 0.5)));
			}
			else if (IN.color.b <= 0.5) {
				c.b = tex.b * (IN.color.b + 0.5);
			}*/
			/*c.r = SoftLight(IN.color.r, tex.r);
			c.g = SoftLight(IN.color.g, tex.g);
			c.b = SoftLight(IN.color.b, tex.b);*/
			/*float SoftLight(float target, float tex) {
				float ret;

				if (target > 0.5) {
					ret = 1 - ((1 - tex) * (1 - (target - 0.5)));
				}
				else if (target <= 0.5) {
					ret = tex * (target + 0.5);
				}
				return ret;
			}*/
		
		c.rgb *= tex.a;
		c.a = tex.a;

		return c;
		}

			ENDCG
		}
	}

	
}

