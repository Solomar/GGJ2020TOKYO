Shader "Unlit/Pixelization"
{
	Properties
	{
		_CelLevel("Cel Level", range(0, 20)) = 2
		_MainTex("Texture", 2D) = "white" {}
		_CelOffset("Cel Offset", range(0, 1)) = 0.5
		_PixelatedLevel("Pixel Level", int) = 500
		_PixelSize("Pixel Size", int) = 5
	}

	SubShader
		{

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				float _CelLevel;
				float _CelOffset;
				int _PixelatedLevel;
				int _PixelSize;
				float4 _LightColor0;
				sampler2D _MainTex;
				float4 _MainTex_ST;

				struct v2f
				{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float3 worldNorm : NORMAL;
				};

				v2f vert(appdata_full m)
				{
					v2f c; // vertices in clip space
					c.pos = UnityObjectToClipPos(m.vertex);
					c.uv = TRANSFORM_TEX(m.texcoord, _MainTex);
					c.worldNorm = mul(m.normal.xyz, (float3x3) unity_WorldToObject);
					return c;
				}

				// cel shading effect
				float cel_shading(float3 norm, float3 lightDir)
				{
					float NDotL = max(0, dot(normalize(norm), normalize(lightDir)));
					return floor(NDotL * _CelLevel) / (_CelLevel - _CelOffset);
				}

				// pixelated shading effect
				float pixel_shading(float v)
				{
					float output = v - fmod(v * _PixelatedLevel, _PixelSize) / _PixelatedLevel;
					return output;
				}

				float4 frag(v2f v) : SV_Target
				{
					/*float4 col = tex2D(_MainTex, v.uv);
					col.rgb *= cel_shading(v.worldNorm, _WorldSpaceLightPos0.xyz) * _LightColor0.rgb;*/
					float4 col = tex2D(_MainTex, float2(pixel_shading(v.uv.x), pixel_shading(v.uv.y)));
					return col;
				}
				ENDCG
			}
		}
}
