#if !defined(Shadows_INCLUDED)
#define Shadows_INCLUDED


#if !defined (MainTex)
	#define MainTex
	sampler2D _MainTex;
	float4 _MainTex_ST;
#endif


#if !defined(SHADOWPASS)
	#define SHADOWPASS
#endif

#include "UnityCG.cginc"
#include "StructsCG.cginc"



InterpolatorsVertex MyShadowVertexProgram(VertexData v) {
	InterpolatorsVertex i;
	i.pos = UnityObjectToClipPos(v.vertex);
	return i;
}

float4 ShadowFragmentProgram(Interpolators i) : SV_TARGET
{
	return 0;
}


#endif