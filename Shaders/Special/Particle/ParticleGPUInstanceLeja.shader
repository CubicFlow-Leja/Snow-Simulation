Shader "LejaGPU/GPUParticleShader"
{
	Properties
	{
		_Color("Color",Color) = (1,1,1,1)
	}
		SubShader
		{
			Tags{ "RenderType" = "Opaque" }
			LOD 100
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#pragma multi_compile_instancing
				#pragma instancing_options procedural:vertInstancingSetup

				#include "UnityCG.cginc"
				#include "UnityStandardParticleInstancing.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};
				struct v2f
				{
					float4 vertex : SV_POSITION;
					half3 wpos : TEXCOORD1;
					
				};
			
				fixed4 _Color;

				half4 _Pos;
				half _Radius;

			
				v2f vert(appdata v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);

					o.wpos = mul(unity_ObjectToWorld, v.vertex);

					o.vertex = UnityObjectToClipPos(v.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					
					float Length = length(i.wpos.xyz - _Pos.xyz);

					fixed LenFac = (Length < _Radius) ? lerp(1, 0.3, Length / (_Radius)) : 0.05f;
					
					return _Color *LenFac;
				}

				ENDCG
			}
		}
}