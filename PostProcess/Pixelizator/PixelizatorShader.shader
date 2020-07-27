Shader "Hidden/PixelizatorShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		//_Columns("Columns",int)= 256
		//_Rows("Rows",int)= 256

    }
    SubShader
    {
        
        Cull Off ZWrite Off ZTest Always



        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
			sampler2D _TempTex;
			int _Columns;
			int _Rows;

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				uv.y *= _Rows;
				uv.x *= _Columns;
				uv.y = round(uv.y);
				uv.x = round(uv.x);
				uv.y /= _Rows;
				uv.x /= _Columns;

                fixed4 col = tex2D(_TempTex, uv);
				
                return col;
            }
            ENDCG
        }
    }
}
