#if !defined(TESSELLATION_INCLUDED)
#define TESSELLATION_INCLUDED

#include "StructsCG.cginc"
#include "UnityCG.cginc"


float TessellationEdgeFactor(TessellationControlPoint cp0, TessellationControlPoint cp1)
{
	float3 p0 = mul(unity_ObjectToWorld, float4(cp0.vertex.xyz, 1)).xyz;
	float3 p1 = mul(unity_ObjectToWorld, float4(cp1.vertex.xyz, 1)).xyz;
	float edgeLength = distance(p0, p1);

	float3 edgeCenter = (p0 + p1) * 0.5;
	float viewDistance = distance(edgeCenter, _WorldSpaceCameraPos);

	return edgeLength * _ScreenParams.y / (_TessellationEdgeLength * viewDistance);
}

TessellationControlPoint Vert(VertexData v) {
	TessellationControlPoint p; 

#if !defined (SHADOWPASS)
	p.vertex = v.vertex;
	p.normal = v.normal;
	p.tangent = v.tangent;
	p.uv = v.uv;
	p.uv1 = v.uv1;
	p.uv2 = v.uv2;
#else
	p.vertex = v.vertex;
	p.normal = v.normal;
	p.uv = v.uv;
#endif

	return p;
}


TessellationFactors PatchConstFun(InputPatch<TessellationControlPoint, 3> patch) {
	TessellationFactors f;
	f.edge[0] = TessellationEdgeFactor(patch[1], patch[2]);
	f.edge[1] = TessellationEdgeFactor(patch[2], patch[0]);
	f.edge[2] = TessellationEdgeFactor(patch[0], patch[1]);
	f.inside = (f.edge[0] + f.edge[1] + f.edge[2]) * (1 / 3.0);
	return f;
}


//hull shader
[UNITY_domain("tri")]									
[UNITY_outputcontrolpoints(3)]							
[UNITY_outputtopology("triangle_cw")]					
[UNITY_partitioning("fractional_odd")]						
[UNITY_patchconstantfunc("PatchConstFun")]				
TessellationControlPoint Hull(InputPatch<TessellationControlPoint,3> patch, uint id : SV_OutputControlPointID)
{
	return patch[id];
}


InterpolatorsVertex MyVertexProgram(VertexData v) { 
	InterpolatorsVertex i;
	
	i.uv = TRANSFORM_TEX(v.uv, _MainTex);

	float displacement = tex2Dlod(_Displacement, float4(i.uv.xy, 0, 0)).r;
	displacement += (tex2Dlod(_BumpMap, float4(i.uv.xy*_BumpUVFactor, 0, 0)).r*2-1)*_BumpFactor;
	v.vertex.y += displacement* _DisplacementFac;


	
	i.pos = UnityObjectToClipPos(v.vertex);
	i.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex);

#if !defined (SHADOWPASS)
	i.Deformed = displacement;
	i.normal = UnityObjectToWorldNormal(v.normal);
	i.tangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
#endif

	return i;
}

//domain shader
[UNITY_domain("tri")]
InterpolatorsVertex  Domain( TessellationFactors factors, OutputPatch<TessellationControlPoint, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
{
	VertexData data;

		#define MY_DOMAIN_PROGRAM_INTERPOLATE(fieldName) data.fieldName = \
		patch[0].fieldName * barycentricCoordinates.x + \
		patch[1].fieldName * barycentricCoordinates.y + \
		patch[2].fieldName * barycentricCoordinates.z;

	MY_DOMAIN_PROGRAM_INTERPOLATE(vertex);
	MY_DOMAIN_PROGRAM_INTERPOLATE(normal)
	MY_DOMAIN_PROGRAM_INTERPOLATE(uv)

#if !defined (SHADOWPASS)
		MY_DOMAIN_PROGRAM_INTERPOLATE(tangent)
		MY_DOMAIN_PROGRAM_INTERPOLATE(uv1)
		MY_DOMAIN_PROGRAM_INTERPOLATE(uv2)
#endif


	return MyVertexProgram(data);
}


float4 Frag(Interpolators i) : SV_TARGET{

	half Param= ((length(i.worldPos.xyz - _Pos.xyz) < _Radius) ? lerp(_Intensity, 0, length(i.worldPos.xyz - _Pos.xyz) / (_Radius)) : 0.0f);

#if !defined (SHADOWPASS)
fixed4 col = lerp(_ColorDeformed,_ColorFresh , i.Deformed);
#else
fixed4 col = _ColorFresh;
#endif
	fixed4 Col = tex2D(_MainTex, i.uv)*col *Param;
	Col += (tex2D(_SparkleNoise, i.uv*_SparkleUVFactor).r > 0.5) ? _SparkleFactor * _SparkleCol : 0.0;

	return Col;
}


[maxvertexcount(3)]
void Geom(triangle InterpolatorsVertex i[3],inout TriangleStream<InterpolatorsGeometry> stream)
{
	float3 p0 = i[0].worldPos.xyz;
	float3 p1 = i[1].worldPos.xyz;
	float3 p2 = i[2].worldPos.xyz;

	float3 triangleNormal = normalize(cross(p1 - p0, p2 - p0));
	i[0].normal = triangleNormal;
	i[1].normal = triangleNormal;
	i[2].normal = triangleNormal;

	InterpolatorsGeometry g0, g1, g2;
	g0.data = i[0];
	g1.data = i[1];
	g2.data = i[2];

	g0.barycentricCoordinates = float2(1, 0);
	g1.barycentricCoordinates = float2(0, 1);
	g2.barycentricCoordinates = float2(0, 0);

	stream.Append(g0);
	stream.Append(g1);
	stream.Append(g2);
}




#endif