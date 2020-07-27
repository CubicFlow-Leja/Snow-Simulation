Shader "LejashaderiHorror/CellShaderHorrorLeja"
{
	Properties
	{

		_Color("Color", Color) = (1,1,1,1)
		_Cos("_Cos",float) = -0.12

		_Shade1("Shade1",float) = 1.46
		_Shade2("Shade2",float) = 0.72
	}
		SubShader
		{
			

			Tags { "RenderType" = "Opaque"}

			Pass
			{
				Tags {"LightMode" = "ForwardBase"}


				Cull Back
				ZWrite On
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
			

				struct v2f
				{
					half diff : COLOR0;
					half3 ambient : COLOR1;
					half4 pos : SV_POSITION;
					half3 wpos : TEXCOORD1;
					
				};

				//boja
				fixed4 _Color;

				//kosinusi i pripadni intenziteti
				half _Shade1;
				half _Shade2;
				half _Cos;


				half4 _Pos;
				half _Radius;
				half _Intensity;
				

				//half4 _IcingPos;
				//half _IcingRadius;
				//half _IcingIntensity;
				//half4 _IceMiddle;
				//half4 _IceRim;

				v2f vert(appdata_base  v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.wpos = mul(unity_ObjectToWorld, v.vertex);
				
					half3 worldNormal = UnityObjectToWorldNormal(v.normal);

					half3 RadialVec1 = o.wpos.xyz - _Pos.xyz;

					half Dot = dot(worldNormal, RadialVec1);

				
					o.diff =  (Dot > _Cos) ?  _Shade2 : _Shade1;


					//ambient lighting
					o.ambient = ShadeSH9(half4(worldNormal, 1));
					return o;
				}

				//fragment shader
				fixed4 frag(v2f i) : SV_Target
				{
					fixed3 lighting = i.diff+ i.ambient;

					fixed4 col;// = _Color;

					//half Icing = length(i.wpos.xyz - _IcingPos.xyz)*(0.9 + 0.05*sin((i.wpos.x * i.wpos.z)*0.05) + 0.1*cos((i.wpos.x * i.wpos.z)*0.05));
					
					//col= (Icing< _IcingRadius)? lerp(_IceMiddle*_IcingIntensity, _IceRim*_IcingIntensity,( Icing / _IcingRadius)*Icing / _IcingRadius):( (length(i.wpos.xyz - _Pos.xyz) < _Radius) ? lerp(_Intensity, 0, length(i.wpos.xyz - _Pos.xyz) / (_Radius)) : 0.0f)*_Color;
					col=((length(i.wpos.xyz - _Pos.xyz) < _Radius) ? lerp(_Intensity, 0, length(i.wpos.xyz - _Pos.xyz) / (_Radius)) : 0.0f)*_Color;
				
					return col*half4(lighting,0);
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
