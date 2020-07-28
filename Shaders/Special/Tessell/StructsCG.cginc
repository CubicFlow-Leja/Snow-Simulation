#if !defined(STRUCTS_INCLUDED)
#define STRUCTS_INCLUDED

#define CUSTOM_GEOMETRY_INTERPOLATORS \
	float2 barycentricCoordinates : TEXCOORD9;


float _TessellationEdgeLength;
fixed4 _ColorFresh;
fixed4 _ColorDeformed;
fixed4 _SparkleCol;

half4 _Pos;
half _Radius;
half _Intensity;


#if !defined (MainTex)
	#define MainTex
	sampler2D _MainTex;
	float4 _MainTex_ST;
#endif

#if !defined (BumpMap)
#define BumpMap
	sampler2D _BumpMap;
	float4 _BumpMap_ST;
#endif

#if !defined (SparkleNoise)
#define SparkleNoise
	sampler2D _SparkleNoise;
	float4 _SparkleNoise_ST;
#endif


#if !defined (Displacement)
#define Displacement
	sampler2D _Displacement;
	float4 _Displacement_ST;
#endif

//
//sampler2D _Displacement;
//float4 _Displacement_ST;
float _DisplacementFac;
float _BumpUVFactor;
float _BumpFactor;

float _SparkleUVFactor;
float _SparkleFactor;



struct VertexData {
#if !defined (SHADOWPASS)
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
	float2 uv : TEXCOORD0;
	float2 uv1 : TEXCOORD1;
	float2 uv2 : TEXCOORD2;
#else
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float2 uv : TEXCOORD0;
#endif
};

struct TessellationControlPoint {
#if !defined (SHADOWPASS)
	float4 vertex : INTERNALTESSPOS;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
	float2 uv : TEXCOORD0;
	float2 uv1 : TEXCOORD1;
	float2 uv2 : TEXCOORD2;
#else
	float4 vertex : INTERNALTESSPOS;
	float3 normal : NORMAL;
	float2 uv : TEXCOORD0;
#endif

	
};

struct TessellationFactors 
{
	float edge[3] : SV_TessFactor;			 
	float inside : SV_InsideTessFactor;		 
};


struct InterpolatorsVertex {
#if !defined (SHADOWPASS)
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : TEXCOORD1;
	float3 tangent : TEXCOORD2;
	float4 worldPos : TEXCOORD4;
	float Deformed : TEXCOORD5;
#else
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : TEXCOORD1;
	float4 worldPos : TEXCOORD4;
#endif



};


struct Interpolators { 
#if !defined (SHADOWPASS)
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : TEXCOORD1;
	float3 tangent : TEXCOORD2;
	float3 worldPos : TEXCOORD4;
	float Deformed : TEXCOORD5;
#else
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 worldPos : TEXCOORD4;
#endif

	

};

struct InterpolatorsGeometry {
	InterpolatorsVertex data;
	CUSTOM_GEOMETRY_INTERPOLATORS
};



#endif