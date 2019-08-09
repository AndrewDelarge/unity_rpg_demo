Shader "Test/PixelEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PixelSize("Pixel width", float) = 1.0
		
	}
	SubShader
	{

		Cull Off ZWrite Off ZTest Always
		Stencil{
			Ref 1
			ReadMask 1
			Comp Equal
			Pass Keep
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				half4 vertex : POSITION;
				half2 uv : TEXCOORD0;
			};

			struct v2f
			{
				half2 uv : TEXCOORD0;
				half4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			half _PixelSize;
		
		

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float2 pixels = _PixelSize * (1 / _ScreenParams.xy);
				float2 pixeluv = float2(pixels * round(i.uv / pixels));
				col = tex2D(_MainTex, pixeluv);
				return col;
			}
			ENDCG
		}
	}
}