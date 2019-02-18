Shader "Lighting/ProceduralCelShader" {
	Properties 
	{
		// hlsl convention to prepend exposed variables with _
		_Color  ("Color", Color) = (1,1,1,1)
        _nDiv ("No. Gradient Divisions", Range(1.0, 16)) = 2.0
	}
	
	SubShader 
	{
		CGPROGRAM
		// Syntax for precompiler directive:
		// surface shader | name of function | lighting model used -- prepend function with Lighting
		#pragma surface surfaceShaderFunction ProceduralCelShader

		float4 _Color;
		half _nDiv;

		float4 LightingProceduralCelShader (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			// prevents mapping for negative dot product values; i.e surface on the oposite side
			// of the light

			// scalar projection of two normalized vectors is always less than 1
			float dotProd = max(0.0, dot(s.Normal, lightDir)); // Normal and lightDir are already normal

            // SIMD structure of GPU makes logical branches a bad idea, can't use logical cutoffs
			
			// Owen, if you're curious how this is working:
			// https://www.desmos.com/calculator
			// plot function y = floor(x*n)/n 
			float3 functionOfdotProd = floor(dotProd * _nDiv) / _nDiv; // smearing of vector
								
			float4 col;
			// see docs for SurfaceOutput struct and _LightColor0
			col.rgb = s.Albedo * _LightColor0.rgb * functionOfdotProd * atten;
			col.a = s.Alpha;
			return col;
		}

		struct Input 
		{
            float2 uv_myDiffuse;
            float2 uv_myBump;
		};

		void surfaceShaderFunction (Input IN, inout SurfaceOutput o) 
		{			
			o.Albedo = _Color.rgb;
		}
		
		ENDCG
	}
	FallBack "Diffuse"
}
