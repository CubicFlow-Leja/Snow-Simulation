#include "UnityCG.cginc"

/*
bool CheckDelta(float d1,float d2,float _Scale)
{
	if (abs(d1-d2)> _Scale)
	{
		return true;
	}
	else
	{
		return false;
	}
}*/

float Calc(float d1, float d2, float d3, float d4, float d,float _Scale)
{
	/*
	if(CheckDelta(d,d1, _Scale))
		return true;
	if (CheckDelta(d, d2, _Scale))
		return true;
	if (CheckDelta(d, d3, _Scale))
		return true;
	if (CheckDelta(d, d4, _Scale))
		return true;
	
	return false;
	*/
	/*
	float D1 = d1 - d3;
	float D2 = d2 - d4;
	return abs(D1 + D2) > _Scale;
	*/
	//float D = abs(d - d1) + abs(d - d2) + abs(d - d3) + abs(d - d4);

	float D = abs(4.0*d - d1  - d2  - d3 - d4);//puno mocnija metoda i optimiziranija

	return D *_Scale;
	
}


float CalculateOutline(sampler2D DepthTex,float2 PixelCoord,float4 _TexelSize,float _Scale,float Dist)
{
	//persp
	/*float d = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2D(DepthTex, PixelCoord)));
	float d1= LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2D(DepthTex, PixelCoord+float2(_TexelSize.x*Dist, _TexelSize.y*Dist))));
	float d2= LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2D(DepthTex, PixelCoord+float2(-_TexelSize.x*Dist, _TexelSize.y*Dist))));
	float d3= LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2D(DepthTex, PixelCoord+float2(_TexelSize.x*Dist, -_TexelSize.y*Dist))));
	float d4= LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2D(DepthTex, PixelCoord+float2(-_TexelSize.x*Dist,-_TexelSize.y*Dist))));*/

	//ortho
	float d = UNITY_SAMPLE_DEPTH(tex2D(DepthTex, PixelCoord));
	float d1 = UNITY_SAMPLE_DEPTH(tex2D(DepthTex, PixelCoord + float2(_TexelSize.x*Dist, _TexelSize.y*Dist)));
	float d2 = UNITY_SAMPLE_DEPTH(tex2D(DepthTex, PixelCoord + float2(-_TexelSize.x*Dist, _TexelSize.y*Dist)));
	float d3 = UNITY_SAMPLE_DEPTH(tex2D(DepthTex, PixelCoord + float2(_TexelSize.x*Dist, -_TexelSize.y*Dist)));
	float d4 = UNITY_SAMPLE_DEPTH(tex2D(DepthTex, PixelCoord + float2(-_TexelSize.x*Dist, -_TexelSize.y*Dist)));

	return Calc(d1,d2,d3,d4,d, _Scale);
}