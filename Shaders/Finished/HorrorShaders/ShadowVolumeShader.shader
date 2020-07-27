Shader "LejashaderiHorror/ShadowVolumeShader"
{
	Properties
	{
		_MinRender("MinRenderDist",float) = 3
	}
		SubShader
		{
	

			Pass
			{
			Tags {"Queue" = "Geometry+10"  }

			Cull Off
			//ZWrite On

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
				OUT.pos = UnityObjectToClipPos(v0 +RadialVec *Dist);
				return OUT;
			}



			[maxvertexcount(9)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
			{
				//extrude trokute?
				half3 v0 = IN[0].vertex.xyz;
				half3 v1 = IN[1].vertex.xyz;
				half3 v2 = IN[2].vertex.xyz;



				
				half3 RadialVec1 = IN[0].wpos.xyz -_Pos.xyz;
				half3 RadialVec2 = IN[1].wpos.xyz - _Pos.xyz;
				half3 RadialVec3 = IN[2].wpos.xyz - _Pos.xyz;


				if (length(RadialVec1) > _Radius &&  length(RadialVec2) > _Radius &&  length(RadialVec3) > _Radius)
					return;

				if (length(RadialVec1) < _MinRender &&  length(RadialVec2) < _MinRender && length(RadialVec3) < _MinRender)
					return;


				half Dist1 = clamp( _Radius - length(RadialVec1),0,_Radius);
				half Dist2 = clamp(_Radius - length(RadialVec2), 0, _Radius);;
				half Dist3 =  clamp(_Radius - length(RadialVec3), 0, _Radius);;

				
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
				
				//dobra ideja ali triba bolje namistit redoslijed vertexi
				//bolje je 2x triangle napravit zasebno
				//i onda zasebno 3 quada , bit ce vise vertexi al eto

				triStream.Append(MakeVertex(v0, RadialVec1, 0.1));
				triStream.Append(MakeVertex(v1, RadialVec2,0.1));
				triStream.Append(MakeVertex(v2, RadialVec3,0.1));
				triStream.Append(MakeVertex(v0, (RadialVec1), Dist1));
				triStream.Append(MakeVertex(v1, (RadialVec2), Dist2));
				triStream.Append(MakeVertex(v2, (RadialVec3), Dist3));
				//triStream.Append(MakeVertex(v0, RadialVec1, 0.1));
				
				
				
				triStream.Append(MakeVertex(v0, RadialVec1, 0.1));
				triStream.Append(MakeVertex(v2, RadialVec3, 0.1));
				triStream.Append(MakeVertex(v1, RadialVec2, 0.1));

				
				/*
				triStream.Append(MakeVertex(v0+ RadialVec1*Dist1, half3(0,-1,0), 20.0f));
				triStream.Append(MakeVertex(v1 + RadialVec2 * Dist2, half3(0, -1, 0), 20.0f));
				triStream.Append(MakeVertex(v2 + RadialVec3 * Dist3, half3(0, -1, 0), 20.0f));*/

				triStream.RestartStrip();

			}

			fixed4 frag(g2f i) : SV_Target
			{
			return  fixed4(1,0,0,1);
			}

			ENDCG
			}
			
		}

}