#include "UnityCG.cginc"


bool Calc(float3 d1, float3 d2, float3 d3, float3 d4,float3 d,float Factor)
{
	/*
	if (1.0-dot(d ,d1) < Factor)
		return true;
	if (1.0 - dot(d, d2) < Factor)
		return true;
	if (1.0 - dot(d, d3) < Factor)
		return true;
	if (1.0 - dot(d, d4) < Factor)
		return true;

	return false;*/
	/*
	float3 D1 = d1 - d3;
	float3 D2 = d2 - d4;
	return abs(sqrt(dot(D1,D1)+dot(D2,D2)))> Factor;*/

	float D = abs(dot(d, d1)) + abs(dot(d, d2)) + abs(dot(d, d3)) + abs(dot(d, d4));
	return D > Factor;
}


bool CalculateNormalsOutline(sampler2D NormalsTex,float2 PixelCoord,float4 _TexelSize,float Factor,float Dist,float4x4 _viewToWorld)
{	
	float depth;
	float3 d;
	float3 d1;
	float3 d2;
	float3 d3;
	float3 d4;
	DecodeDepthNormal( tex2D(NormalsTex, PixelCoord), depth,d);
	DecodeDepthNormal( tex2D(NormalsTex, PixelCoord + float2(_TexelSize.x*Dist, _TexelSize.y*Dist)), depth,d1);
	DecodeDepthNormal( tex2D(NormalsTex, PixelCoord + float2(-_TexelSize.x*Dist, _TexelSize.y*Dist)), depth,d2);
	DecodeDepthNormal( tex2D(NormalsTex, PixelCoord + float2(_TexelSize.x*Dist, -_TexelSize.y*Dist)), depth,d3);
	DecodeDepthNormal( tex2D(NormalsTex, PixelCoord + float2(-_TexelSize.x*Dist, -_TexelSize.y*Dist)), depth,d4);
	d= mul((float3x3)_viewToWorld, d);
	d1= mul((float3x3)_viewToWorld, d1);
	d2= mul((float3x3)_viewToWorld, d2);
	d3= mul((float3x3)_viewToWorld, d3);
	d4= mul((float3x3)_viewToWorld, d4);

	/*float3 d1= tex2D(NormalsTex, PixelCoord+float2(_TexelSize.x*Dist, _TexelSize.y*Dist)).rgb;
	float3 d2= tex2D(NormalsTex, PixelCoord+float2(-_TexelSize.x*Dist, _TexelSize.y*Dist)).rgb;
	float3 d3= tex2D(NormalsTex, PixelCoord+float2(_TexelSize.x*Dist, -_TexelSize.y*Dist)).rgb;
	float3 d4= tex2D(NormalsTex, PixelCoord+float2(-_TexelSize.x*Dist,-_TexelSize.y*Dist)).rgb;*/
	
	return Calc(d1,d2,d3,d4,d, Factor);
}