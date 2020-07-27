Shader "LejashaderiHorror/CellShaderHorrorLejaShadows"
{
	Properties
	{

		_Color("Color", Color) = (1,1,1,1)
		_Cos("_Cos",float) = -0.12

		_Shade1("Shade1",float) = 1.46
		_Shade2("Shade2",float) = 0.72

		_MinRender("MinRenderDist",float) = 3
		
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
				

				half4 _IcingPos;
				half _IcingRadius;
				half _IcingIntensity;
				half4 _IceMiddle;
				half4 _IceRim;

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

					half Icing = length(i.wpos.xyz - _IcingPos.xyz)*(0.9 + 0.05*sin((i.wpos.x * i.wpos.z)*0.05) + 0.1*cos((i.wpos.x * i.wpos.z)*0.05));
					
					col= (Icing< _IcingRadius)? lerp(_IceMiddle*_IcingIntensity, _IceRim*_IcingIntensity,( Icing / _IcingRadius)*Icing / _IcingRadius):( (length(i.wpos.xyz - _Pos.xyz) < _Radius) ? lerp(_Intensity, 0, length(i.wpos.xyz - _Pos.xyz) / (_Radius)) : 0.0f)*_Color;
					//col=((length(i.wpos.xyz - _Pos.xyz) < _Radius) ? lerp(_Intensity, 0, length(i.wpos.xyz - _Pos.xyz) / (_Radius)) : 0.0f)*_Color;
				
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


			Pass
			{
				Tags {"Queue" = "Geometry+10"  }

					Cull Off
					CGPROGRAM

					#include "UnityCG.cginc"

					#pragma vertex vert
					#pragma fragment frag
					#pragma geometry geom

					#pragma target 4.0


					struct appdata
					{
						half4 vertex : POSITION;
					};


					struct v2g
					{
						half4  vertex : POSITION;
						half3 wpos : TEXCOORD0;
					};

					struct g2f
					{
						half4  pos : SV_POSITION;
					};

					half4 _Pos;
					half _Radius;

					half _MinRender;
					

					v2g vert(appdata v)
					{
						v2g o;

						o.wpos = mul(unity_ObjectToWorld, v.vertex);
						o.vertex = v.vertex;
						return o;
					}


					g2f MakeVertex(half3 v0, half3 RadialVec,float Dist)
					{
						g2f OUT;
						OUT.pos = UnityObjectToClipPos(v0 + RadialVec * Dist);
						return OUT;
					}



					[maxvertexcount(15)]
					void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
					{
						
						half3 v0 = IN[0].vertex.xyz;
						half3 v1 = IN[1].vertex.xyz;
						half3 v2 = IN[2].vertex.xyz;
						 

						half3 RadialVec1 = IN[0].wpos.xyz - _Pos.xyz;
						half3 RadialVec2 = IN[1].wpos.xyz - _Pos.xyz;
						half3 RadialVec3 = IN[2].wpos.xyz - _Pos.xyz;


						if (length(RadialVec1) > _Radius &&  length(RadialVec2) > _Radius && length(RadialVec3) > _Radius)
							return;

						if (length(RadialVec1) < _MinRender || length(RadialVec2) < _MinRender || length(RadialVec3) < _MinRender)
							return;


						half Dot1 = dot(normalize(RadialVec1), normalize(half3(RadialVec1.x, 0, RadialVec1.z)));
						half Dot2 = dot(normalize(RadialVec2), normalize(half3(RadialVec2.x, 0, RadialVec2.z)));
						half Dot3 = dot(normalize(RadialVec3), normalize(half3(RadialVec3.x, 0, RadialVec3.z)));

						
						half Dist1 = clamp(_Radius - length(RadialVec1),0,_Radius)*(2-Dot1);
						half Dist2 = clamp(_Radius - length(RadialVec2), 0, _Radius)*(2 - Dot2);
						half Dist3 = clamp(_Radius - length(RadialVec3), 0, _Radius)*(2- Dot3);


						if (RadialVec1.y > 0)
							RadialVec1.y = 0;

						if (RadialVec2.y > 0)
							RadialVec2.y = 0;

						if (RadialVec3.y > 0)
							RadialVec3.y = 0;


						RadialVec1 = normalize(RadialVec1);
						RadialVec2 = normalize(RadialVec2);
						RadialVec3 = normalize(RadialVec3);

						RadialVec1 = mul(unity_WorldToObject, RadialVec1);
						RadialVec2 = mul(unity_WorldToObject, RadialVec2);
						RadialVec3 = mul(unity_WorldToObject, RadialVec3);



						triStream.Append(MakeVertex(v0, RadialVec1, 0.1));
						triStream.Append(MakeVertex(v1, RadialVec2,0.1));
						triStream.Append(MakeVertex(v0, (RadialVec1), Dist1));
						triStream.Append(MakeVertex(v1, (RadialVec2), Dist2));
						triStream.RestartStrip();

						triStream.Append(MakeVertex(v1, RadialVec2, 0.1));
						triStream.Append(MakeVertex(v2, RadialVec3, 0.1));
						triStream.Append(MakeVertex(v1, (RadialVec2), Dist2));
						triStream.Append(MakeVertex(v2, (RadialVec3), Dist3));
						triStream.RestartStrip();

						triStream.Append(MakeVertex(v2, RadialVec3, 0.1));
						triStream.Append(MakeVertex(v0, RadialVec1, 0.1));
						triStream.Append(MakeVertex(v2, (RadialVec3), Dist3));
						triStream.Append(MakeVertex(v0, (RadialVec1), Dist1));
						triStream.RestartStrip();


						triStream.Append(MakeVertex(v0+ RadialVec1*Dist1, 0, 0));
						triStream.Append(MakeVertex(v1+ RadialVec2*Dist2, 0, 0));
						triStream.Append(MakeVertex(v2+ RadialVec3*Dist3, 0, 0));
						triStream.RestartStrip();


						

					}

					fixed4 frag(g2f i) : SV_Target
					{
					return  fixed4(0,0,0,1);
					}

				ENDCG
			}
		}


}
