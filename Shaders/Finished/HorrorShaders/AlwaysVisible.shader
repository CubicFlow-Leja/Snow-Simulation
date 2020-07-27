Shader "LejashaderiHorror/AlwaysVisible"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)

	}
		SubShader
	{

		Pass
		{
			ZTest Always
			Cull Back
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			half4 _Color;
			struct v2f
			{
				half diff : COLOR0;
				half4 pos : SV_POSITION;
			};

			v2f vert(appdata_base  v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}

	}


}
