Shader "LejashaderiHorror/Tessallation"
{
	Properties{
		 _SparkleCol("_SparkleCol", Color) = (1, 1, 1, 1)
		 _ColorFresh("_ColorFresh", Color) = (1, 1, 1, 1)
		 _ColorDeformed("_ColorDeformed", Color) = (1, 1, 1, 1)

		 _MainTex("MainTex", 2D) = "white" {}
		 _SparkleNoise("Sparkle", 2D) = "white" {}
		 _BumpMap("Bump", 2D) = "white" {}

		 _Displacement("_Displacement", 2D) = "white" {}

		_TessellationEdgeLength("Tessellation Edge Length", Range(5, 100)) = 15

		_DisplacementFac("SnowHeight", Range(0, 25)) = 2.5//needs to match camera end clip plane!!!!!

		_BumpFactor("_BumpFactor", Range(0, 0.2)) = 0.1
		_BumpUVFactor("_BumpUVFactor", Range(0, 25)) = 2

		_SparkleFactor("_SparkleFactor", Range(0, 0.2)) = 0.1
		_SparkleUVFactor("_SparkleUVFactor", Range(5, 300)) = 2
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
