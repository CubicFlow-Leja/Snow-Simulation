Shader "LejashaderiHorror/NoShadingNoClip"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		//_Ref("Ref",int) = 1
		
	}
		SubShader
		{
			/* Stencil
			{
				Ref[_Ref]
				Comp always
				Pass replace
			}*/

			Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }

			Pass
			{
				Tags {"LightMode" = "ForwardBase"}


				Cull Off
				ZWrite On
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
			

				struct v2f
				{
					half4 pos : SV_POSITION;
				};


				
				fixed4 _Color;

				v2f vert(appdata_base  v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					return o;
				}

			
				fixed4 frag(v2f i) : SV_Target
				{

					fixed4 col = _Color;
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

				struct v2f {
				V2F_SHADOW_CASTER;
				};

				v2f vert(appdata_base v)
				{
					v2f o;
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					SHADOW_CASTER_FRAGMENT(i)
				}

				ENDCG
			}
		}


}
