#if !defined(STRUCTS_INCLUDED)
#define STRUCTS_INCLUDED

#define CUSTOM_GEOMETRY_INTERPOLATORS \
	float2 barycentricCoordinates : TEXCOORD9;


//float _TessellationUniform;
float _TessellationEdgeLength;
fixed4 _ColorFresh;
fixed4 _ColorDeformed;

half4 _Pos;
half _Radius;
half _Intensity;


#if !defined (MainTex)
	#define MainTex
	sampler2D _MainTex;
	float4 _MainTex_ST;
#endif



sampler2D _Displacement;
float4 _Displacement_ST;
float _DisplacementFac;



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
	float edge[3] : SV_TessFactor;			 //faktori za svaki edge
	float inside : SV_InsideTessFactor;		 //faktor za centar trisa
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