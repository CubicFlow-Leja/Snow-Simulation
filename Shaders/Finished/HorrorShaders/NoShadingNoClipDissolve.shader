Shader "LejashaderiHorror/NoShadingNoClipDissolve"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_ClippingTreshold("Clipping Treshold",Range(0.0,3.0)) = 0.01

		//_Ref("Ref",int) = 1
	}
		SubShader
		{
			/* Stencil 
			{
				Ref[_Ref]
				Comp always
				Pass replace
			}

*/
			Tags { "RenderType" = "Opaque" "Queue" = "Geometry"}

			Pass
			{
				Tags {"LightMode" = "ForwardBase"}


				Cull Off
				ZWrite On
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
					half4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				sampler2D _MainTex;
				half4 _MainTex_ST;

				half _ClippingTreshold;

				fixed4 _Color;

				v2f vert(appdata v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

			
				fixed4 frag(v2f i) : SV_Target
				{

					fixed4 col = _Color;
					
					clip(-tex2D(_MainTex, i.uv).r + _ClippingTreshold);

					return col;
				}
				ENDCG
			}

			
			Pass{
				Tags {"LightMode" = "ShadowCaster"}
				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_shadowcaster
				#include "UnityCG.cginc"


				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					V2F_SHADOW_CASTER;
				};
				


				sampler2D _MainTex;
				half4 _MainTex_ST;
				half _ClippingTreshold;

				v2f vert(appdata_base v)
				{
					v2f o;
					o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					clip(-tex2D(_MainTex, i.uv).r + _ClippingTreshold);
					SHADOW_CASTER_FRAGMENT(i)
				}

				ENDCG
			}
		}


}
