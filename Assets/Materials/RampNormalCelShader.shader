Shader "Lighting/RampNormalCelShader"
{
	Properties 
	{
		_Color  ("Color", Color) = (1,1,1,1)
        _DiffuseTex ("Diffuse Texture", 2D) = "" {}
        _BumpMultiplier ("Bump Multiplier", Range(0,5)) = 0.4
        _BumpMap ("Normal Map", 2D) = "" {}
		_RampTex ("Ramp Texture", 2D) = ""{}
	}
	
	SubShader 
	{
		CGPROGRAM
		#pragma surface surf RampCelShader

		float4 _Color;
        half _BumpMultiplier;
		sampler2D _RampTex;
        sampler2D _DiffuseTex;
        sampler2D _BumpMap;
		
		float4 LightingRampCelShader (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float dotProd = max(0.0, dot (s.Normal, lightDir)); // lightDir is already normalized
			float uvTexMapValue = dotProd * 0.5 + 0.4;
			float2 uvTexMapCoord = uvTexMapValue; // smearing

			// rh.x = uvTexMapValue;
			// rh.y = uvTexMapValue;

			// this is returning a color value between 0 and 1 from ramp texture.
			float3 rampTextureCol = tex2D(_RampTex, uvTexMapCoord).rgb; // tex2D returns a color from the texture -- vec4 rgba
			float4 col;
			// see docs for SurfaceOutput struct and _LightColor0 in UnityCG.cginc
			col.rgb = s.Albedo * _LightColor0.rgb * (rampTextureCol);
			col.a = s.Alpha;
			return col;
		}

		struct Input 
		{
			float2 uv_MainTex;
            float2 uv_DiffuseTex;
            float2 uv_BumpMap;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{			
			// o.Albedo = _Color.rgb;
            o.Albedo = tex2D(_DiffuseTex, IN.uv_DiffuseTex).rgb;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Normal *= float3(_BumpMultiplier,_BumpMultiplier,1);
		}
		
		ENDCG
	}
	FallBack "Diffuse"
}
