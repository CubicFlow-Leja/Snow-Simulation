Shader "LejashaderiHorror/Tessallation"
{
	Properties{
		 _ColorFresh("_ColorFresh", Color) = (1, 1, 1, 1)
		 _ColorDeformed("_ColorDeformed", Color) = (1, 1, 1, 1)
		 _MainTex("Albedo", 2D) = "white" {}

		 _Displacement("_Displacement", 2D) = "white" {}
		 _DisplacementFac("_DisplacementFac", Float) = 1

		//_TessellationUniform("Tessellation Uniform", Range(1, 64)) = 1
		//_TessellationEdgeLength("Tessellation Edge Length", Range(0.1, 1)) = 0.5//world space
		_TessellationEdgeLength("Tessellation Edge Length", Range(5, 100)) = 50//screenspace
	}


    SubShader
    {
        Pass
        {
			CGPROGRAM

			#pragma target 4.6

			#pragma vertex Vert
			#pragma fragment Frag
			#pragma hull Hull
			#pragma domain Domain
			#pragma geometry Geom

			
			#include "TessallationCG.cginc"

			ENDCG
        }
		Pass
		{
			Tags {"LightMode" = "ShadowCaster"}
			CGPROGRAM

			#pragma target 4.6
			#pragma vertex Vert
			#pragma fragment ShadowFragmentProgram
			#pragma hull Hull
			#pragma domain Domain
			#pragma geometry Geom

			#include "ShadowsCG.cginc"
			#include "TessallationCG.cginc"

			ENDCG
		}
    }
}
