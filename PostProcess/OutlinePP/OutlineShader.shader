Shader "Hidden/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
     
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma target 3.0
			#include "OutlinePPcginc.cginc"
			#include "OutlinePPcgincNormals.cginc"
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

			//depth
			sampler2D _CameraDepthTexture;
			float4 _CameraDepthTexture_TexelSize;
			float _Scale;
			float _Dist;

			//normals
			//float _Dist2;
			//float _AngleFactor;
			//sampler2D _CameraDepthNormalsTexture;
			//float4 _CameraNormalsTexture_TexelSize;

			//float4x4 _viewToWorld;

			fixed4 _OutlineColor;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
             
			/*if (CalculateOutline(_CameraDepthTexture,i.uv, _CameraDepthTexture_TexelSize, _Scale, _Dist) || 
				CalculateNormalsOutline(_CameraDepthNormalsTexture, i.uv, _CameraNormalsTexture_TexelSize, _AngleFactor, _Dist2, _viewToWorld))
			{
				col = _OutlineColor;
			}*/


			col -= _OutlineColor * CalculateOutline(_CameraDepthTexture, i.uv, _CameraDepthTexture_TexelSize, _Scale, _Dist);
            return col;
            }
            ENDCG
        }
    }
}
