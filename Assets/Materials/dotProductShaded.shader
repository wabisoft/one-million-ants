Shader "surfaceShaders/dotProductShaded"
{
    Properties
    {
        _Color ("Diffuse Color", Color) = (1,1,1,1)
        _Emission ("Emissive Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        CGPROGRAM
            #pragma surface surfaceShaderFunc Lambert

            struct Input
            {
                // magic global stuff
                float3 viewDir;
            };
            // I don't have to declare my own surfaceoutput struct
            // this is with precompiler directive?

            // struct SurfaceOutput
            // {
            //     fixed3 Albedo;  // diffuse color
            //     fixed3 Normal;  // tangent space normal, if written
            //     fixed3 Emission; // 
            //     half Specular;  // specular power in 0..1 range
            //     fixed Gloss;    // specular intensity
            //     fixed Alpha;    // alpha for transparencies
            // };


            // by convention, gpu programming likes single variable names...
            // surface shader function:

            fixed4 _Emission;
            fixed4 _Color;

            void surfaceShaderFunc (Input IN, inout SurfaceOutput o)
            {
                // intensity * color
                half surfViewDotProd = dot(normalize(IN.viewDir), normalize(o.Normal));
                // o.Albedo = float3(surfViewDotProd, surfViewDotProd, surfViewDotProd);
                // o.Albedo = float3(0.0, 1-surfViewDotProd, 1.0);
                // o.Albedo = float3(surfViewDotProd, 1.0, 1.0-surfViewDotProd);
                o.Emission = _Emission.rgb * 1-surfViewDotProd;
                o.Albedo = _Color.rgb * 1-surfViewDotProd;
            }
        ENDCG  
    }
    Fallback "Diffuse"
}