Shader "Toon/Lit Stencil" {
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_RampStrength("Ramp Strength", Range(0,1)) = 0.4
		_MainTex("Base (RGB)", 2D) = "white" {}
	    _BumpMap("Bumpmap", 2D) = "bump" {}
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200
		Stencil
			{
				Ref 1
				Comp Always
				Pass Replace
			}
			CGPROGRAM
			#pragma surface surf ToonRamp fullforwardshadows

			sampler2D _Ramp;
	float _RampStrength;
	// custom lighting function that uses a texture ramp based
	// on angle between light direction and normal
	#pragma lighting ToonRamp exclude_path:prepass
	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten) {
		#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
		#endif

		half d = dot(s.Normal, lightDir);
		half ramp = smoothstep(0,0.05, d);

		half4 c;
		c.rgb = (s.Albedo) * _LightColor0.rgb * (ramp * _RampStrength) * (atten * 2);

		c.a = 0;
		return c;
	}

	sampler2D _MainTex;
	float4 _Color;
	sampler2D _BumpMap;
	struct Input {
		float2 uv_MainTex : TEXCOORD0;
		float2 uv_BumpMap;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	}

	ENDCG
	}

		Fallback "Diffuse"
}