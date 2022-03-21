Shader "Hidden/Roystan/Outline Post Process"
{
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			// Custom post processing effects are written in HLSL blocks,
			// with lots of macros to aid with platform differences.
			// https://github.com/Unity-Technologies/PostProcessing/wiki/Writing-Custom-Effects#shader
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	// _CameraNormalsTexture contains the view space normals transformed
	// to be in the 0...1 range.
	TEXTURE2D_SAMPLER2D(_CameraNormalsTexture, sampler_CameraNormalsTexture);
	TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);

	// Data pertaining to _MainTex's dimensions.
	// https://docs.unity3d.com/Manual/SL-PropertiesInPrograms.html
	float4 _MainTex_TexelSize;
	float _Scale;
	float4 _Color;
	float _NormalThreshold;
	float _DepthThreshold;
	float _DepthNormalThreshold;
	float _DepthNormalThresholdScale;

	float4x4 _ClipToView;

	// Combines the top and bottom colors using normal blending.
	// https://en.wikipedia.org/wiki/Blend_modes#Normal_blend_mode
	// This performs the same operation as Blend SrcAlpha OneMinusSrcAlpha.
	float4 alphaBlend(float4 top, float4 bottom) {
		float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
		float alpha = top.a + bottom.a * (1 - top.a);

		return float4(color, alpha);
	}

	struct VertexData {
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoordStereo : TEXCOORD1;
		float3 viewSpaceDir : TEXCOORD2;
#if STEREO_INSTANCING_ENABLED
		uint stereoTargetEyeIndex : SV_RenderTargetArrayIndex;
#endif
	};

	VertexData Vert(AttributesDefault v) {
		VertexData o;
		// clip space position (all on plane z = 0.0)
		o.vertex = float4(v.vertex.xy, 0.0, 1.0);
		o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);

#if UNITY_UV_STARTS_AT_TOP
		o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
#endif
		o.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);
		o.viewSpaceDir = mul(_ClipToView, o.vertex).xyz;
		return o;
	}

	float4 Frag(VertexData i) : SV_Target
	{
		// Sample depth/normal buffer pixels in X shape (top left/right, bottom left/right).
		float halfScaleFloor = floor(_Scale * 0.5);
		float halfScaleCeil = ceil(_Scale * 0.5);

		float2 bottomLeftUV = i.texcoord - float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleFloor;
		float2 topRightUV = i.texcoord + float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleCeil;
		float2 bottomRightUV = i.texcoord + float2(_MainTex_TexelSize.x * halfScaleCeil, -_MainTex_TexelSize.y * halfScaleFloor);
		float2 topLeftUV = i.texcoord + float2(-_MainTex_TexelSize.x * halfScaleFloor, _MainTex_TexelSize.y * halfScaleCeil);

		float depth0 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, bottomLeftUV).r;
		float depth1 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, topRightUV).r;
		float depth2 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, bottomRightUV).r;
		float depth3 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, topLeftUV).r;

		float3 normal0 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, bottomLeftUV).rgb;
		float3 normal1 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, topRightUV).rgb;
		float3 normal2 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, bottomRightUV).rgb;
		float3 normal3 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, topLeftUV).rgb;

		float depthFiniteDifference0 = depth1 - depth0;
		float depthFiniteDifference1 = depth3 - depth2;

		// Roberts Cross
		float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 100;
		float3 viewNormal = normal0 * 2 - 1;
		float NdotV = 1 - dot(viewNormal, -i.viewSpaceDir);
		float normalThreshold01 = saturate((NdotV - _DepthNormalThreshold) / (1 - _DepthNormalThreshold));
		float normalThreshold = normalThreshold01 * _DepthNormalThresholdScale + 1;
		float finalDepthThreshold = normalThreshold * _DepthThreshold * depth0;
		edgeDepth = edgeDepth > finalDepthThreshold ? 1 : 0;

		float3 normalFiniteDifference0 = normal1 - normal0;
		float3 normalFiniteDifference1 = normal3 - normal2;

		// Difference in normals must exceed a threshold in order to qualify as plane change.
		float edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0) + dot(normalFiniteDifference1, normalFiniteDifference1));
		edgeNormal = edgeNormal > _NormalThreshold ? 1 : 0;

		// Inverts colors (white background, black outlines)
		float edge = max(edgeNormal, edgeDepth);

		// Replace the existing return depth0 line.
		// return abs(depthFiniteDifference1) * 100;

		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		float4 edgeColor = float4(_Color.rgb, edge);
		return alphaBlend(edgeColor, color);
	}
	ENDHLSL
}
	}
}