// uncomment different shaping functions if you're interested

Shader "Lighting/windWakerStyleCelShader" {
	Properties 
	{
		_Color  ("Color", Color) = (1,1,1,1)
        _DiffuseTex ("Diffuse Texture", 2D) = "white" {}
        _BumpMultiplier ("Bump Multiplier", Range(0,5)) = 0.4
        _BumpMap ("Normal Map", 2D) = "bump" {}
		_RampTex ("Ramp Texture", 2D) = "white"{}
        _BlurAmount("Blur Amount", Range(0,1.0)) = 0.1
		// _Ambient("Ambient Light", Range(0.2,1)) = 1.0
	}
	
	SubShader 
	{
		CGPROGRAM
		#pragma surface surfaceShaderFunction RampCelShader

		float4 _Color;
        half _BumpMultiplier;
		sampler2D _RampTex;
        sampler2D _DiffuseTex;
        sampler2D _BumpMap;
        half _BlurAmount;
		// half _Ambient;
		
		float4 LightingRampCelShader (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float dotProd = dot (s.Normal, lightDir);
			
            // so, the dot product just returns a value between 0, 1 based
            // on the angle between the viewDir and the surface Normal
            // let this angle be x and y = some shaping function such that
            // y is constrained between 0,1 this should also allow a step like gradient
			// with successive additions of smoothsteps & || logical cutoffs

			half epsilon = 0.001;
            float3 functionOfdotProd;

            // #-- 1 Naive solution from visual shaping function
            // even power to keep symetry for negative values
            // functionOfdotProd = -1/(1+ pow(2*(dotProd), 6))+1;

            // #-- 2
            // functionOfdotProd = smoothstep(0.3, 0.3 + epsilon, dotProd);

            // #-- 3 // SIMD structure of GPU makes logical branches a bad idea
			//			it forces pipeline to start ofver each time

            // if(dotProd < 0.3)
            // {
            //     functionOfdotProd = smoothstep(0.3, 0.3 + epsilon, dotProd) - 0.5;
            // }
            // else
            // {
            //     functionOfdotProd = smoothstep(0.5, 0.5 + epsilon, dotProd);
            // }

			// #-- 4 addition of designed smoothstep
			// 		 smoothstep is hardware accelerated
			// functionOfdotProd = smoothstep(0.35, 0.45+_BlurAmount, dotProd) * _Ambient;
            functionOfdotProd = smoothstep(0.35, 0.45+_BlurAmount, dotProd);

			float4 col;
			// see docs for SurfaceOutput struct and _LightColor0
			col.rgb = s.Albedo * _LightColor0.rgb * (functionOfdotProd);
			col.a = s.Alpha;
			return col;
		}

		struct Input 
		{
            float2 uv_DiffuseTex;
            float2 uv_BumpMap;
		};

		void surfaceShaderFunction (Input IN, inout SurfaceOutput o) 
		{			
			o.Albedo = _Color.rgb;
            // o.Albedo = tex2D(_DiffuseTex, IN.uv_DiffuseTex).rgb;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Normal *= float3(_BumpMultiplier,_BumpMultiplier,1);
		}
		
		ENDCG
	}
	FallBack "Diffuse"
}
