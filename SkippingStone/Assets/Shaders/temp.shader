Shader "Custom/Temp"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DisplacementTex ("DisplacementTex", 2D) = "white" {}
		_WaterTex ("WaterTex", 2D) = "white" {}
		_MaskTex ("MaskTex", 2D) = "white" {}
		_BaseHeight ("BaseHeight", float) = 0.27
		_Turbulence ("Turbulence", float) = 1
		_ScrollOffset ("ScrollOffset", float) = 0
	}
	SubShader
	{
		Cull Off
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		//LOD 100
		
		GrabPass { }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _DisplacementTex;
			sampler2D _WaterTex;
			sampler2D _MaskTex;
			sampler2D _GrabTexture;
			float4 _MainTex_ST;
			fixed4 _MainTex_TexelSize;
			float _BaseHeight;
			fixed _Turbulence;
			fixed _ScrollOffset;

			struct appdata
			{
				float4 vertex : POSITION;
				half2 uv : TEXCOORD0;
			};

			struct v2f
			{
				half2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 uvgrab : TEXCOORD1;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uvgrab = ComputeGrabScreenPos(o.vertex);
				return o;
			}

			float wave(float x) {
				fixed waveOffset =  cos((x - _Time + _ScrollOffset) * 60) * 0.004
									+ cos((x - 2 * _Time + _ScrollOffset) * 20) * 0.008
									+ sin((x + 2 * _Time + _ScrollOffset) * 35) * 0.01
									+ cos((x + 4 * _Time + _ScrollOffset) * 70) * 0.001;
				return _BaseHeight + waveOffset * _Turbulence;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{				
				fixed4 waterCol = tex2D(_WaterTex, i.uv);
				//fixed4 main = tex2Dproj(_MainTex, UNITY_PROJ_COORD(i.uvgrab));
				fixed maskValue = tex2D(_MaskTex, i.uv).r;

				float waveHeight = wave(i.uv.x);
				fixed isTexelAbove = step(waveHeight, i.uv.y);
				fixed isTexelBelow = 1 - isTexelAbove;

				fixed2 disPos = i.uv;
				disPos.x += (_Time[0]) % 2;
				disPos.y += (_Time[0]) % 2;
				
				half4 col = tex2D(_MainTex, i.uv);//tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));				

				float topDist = abs(i.uv.y - waveHeight);
				fixed isNearTop = 1 - step(abs(_MainTex_TexelSize.y * 6), topDist);

				fixed topColorBlendFac = isNearTop * isTexelBelow * maskValue;
				col.r = lerp(col.r, waterCol.r , topColorBlendFac);
				col.g = lerp(col.g, waterCol.g , topColorBlendFac);
				col.b = lerp(col.b, waterCol.b , topColorBlendFac);

				return col + half4(0,0,0,0);
			}
			ENDCG
		}
	}
}
