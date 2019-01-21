Shader "Lighting/LambertRolled"
{
    Properties
    {
        // exposed in inspector
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surfaceShaderFunction RollMyOwnLambert // surface|name|lighting function
        
        // #-- Note
        // Normally, I need to include UnityCG for _LightColor0, so surface includes this?

        // #--
        // struct SurfaceOutput
        // {
        //     fixed3 Albedo;  // diffuse color
        //     fixed3 Normal;  // tangent space normal, if written
        //     fixed3 Emission;
        //     half Specular;  // specular power in 0..1 range
        //     fixed Gloss;    // specular intensity
        //     fixed Alpha;    // alpha for transparencies
        // };

        // #-- Lighting Model is Lambert
        half4 LightingRollMyOwnLambert(SurfaceOutput s, half3 lightDir, half attentuationValue)
        {
            half normalLightDotProd = max(0.0, dot(s.Normal, lightDir)); // normal and lightDir are normalized already
            half4 col;
            col.a = s.Alpha;
            col.rgb = s.Albedo * _LightColor0.rgb * normalLightDotProd * attentuationValue;
            return col;
        }

        struct Input
        {
            float2 uv_diffuseTex;
            // float2 uv_emissiveTex;
            float2 uv_bumpMap;
        };

        fixed4 _Color;

        void surfaceShaderFunction(Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color.rgb;
		}
		ENDCG
    }
    FallBack "Diffuse"
}
