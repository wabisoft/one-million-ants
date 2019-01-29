Shader "Lighting/RampDiffuseCelShader" {
	Properties 
	{
		_Color  ("Color", Color) = (1,1,1,1)
		_RampTex ("Ramp Texture", 2D) = ""{}
	}
	
	SubShader 
	{
		
		CGPROGRAM
		#pragma surface surfaceShaderFunction RampCelShader

		float4 _Color;
		sampler2D _RampTex;
		
		float4 LightingRampCelShader (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float NdotL = dot(s.Normal, lightDir);
			float NDotLClamped = max(0.0, NdotL); // prevents shading on side with negative dotproduct
			float2 rampUVCoords = NdotL*.5 +.5; // sample coords for texture
			float3 celValue = tex2D(_RampTex, rampUVCoords).rgb;
			
			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (celValue);
			col.a = s.Alpha;
			return col;
		}

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surfaceShaderFunction (Input IN, inout SurfaceOutput o) 
		{			
			o.Albedo = _Color.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
