Shader "Custom/gemBump"
{
	Properties
	{
		_DiffuseTex("Diffuse Texture", 2D) = "white" {}
		_BumpTex("Bump Texture", 2D) = "bump" {}
		_bumpMultiplier("Bump Amount", Range(0,20)) = 1
		_brightMultiplier("Brightness", Range(0,10)) = 1
		_Emission("Emissive Color", Color) = (1,1,1,1)
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Glossiness("Smoothness", Range(0,1)) = 0.5
	}
		SubShader
		{

			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows

			sampler2D _DiffuseTex;
			sampler2D _BumpTex;
			half _bumpMultiplier;
			half _brightMultiplier;
			fixed4 _Emission;
			half _Metallic;
			half _Glossiness;

			struct Input {
				float3 viewDir;
				float2 uv_DiffuseTex;
				float2 uv_BumpTex;
			};

			void surf(Input IN, inout SurfaceOutputStandard o) {
				half surfViewDotProd = dot(normalize(IN.viewDir), normalize(o.Normal));
				o.Albedo = tex2D(_DiffuseTex, IN.uv_DiffuseTex).rgb;
				o.Normal = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex)) * _brightMultiplier;
				o.Normal *= float3(_bumpMultiplier,_bumpMultiplier,1);
				o.Emission = _Emission.rgb * .5 * surfViewDotProd;
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
			}

		  ENDCG
		}
			Fallback "Diffuse"
}
