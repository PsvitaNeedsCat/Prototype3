Shader "Roystan/Toon/Water"
{
	Properties
	{
		// Default values for gradient
		_DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
		_DepthGradientDeep("Depth Gradient Deep", Color) =  (0.086, 0.407, 1, 0.789)
		_DepthMaxDistance("Depth Maximum Distance", Float) = 1

		// For custom foam colour
		_FoamColor("Foam Color", Color) = (1, 1, 1, 1) // R,G,B,Alpha - Doesn't change shit?

		// For noise texture
		_SurfaceNoise("Surface Noise", 2D) = "white" {}

		// For waves
		_SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777

		// Shoreline
		//_FoamDistance("Foam Distance", Float) = 0.04
		_FoamMaxDistance("Foam Maximum Distance", Float) = 0.4
		_FoamMinDistance("Foam Minimum Distance", Float) = 0.04

		// Controls speed in UVs per second (Of Animation)
		_SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)

		// 2 channel distortion texture
		_SurfaceDistortion("Surface Distortion", 2D) = "white" {}

		// Control to multiply the strength of the distortion
		_SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27
    }
    SubShader
    {
		// For transparency
		Tags
		{
			"Queue" = "Transparent"
		}

        Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha // Enables blend
			ZWrite Off // Stops object being written to depth buffer so that blending can occur

			CGPROGRAM
			#define SMOOTHSTEP_AA 0.01
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			// Blends top and bottom colours together
			float4 alphaBlend(float4 top, float4 bottom)
			{
				float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
				float alpha = top.a + bottom.a * (1 - top.a);

				return float4(color, alpha);
			}

            struct appdata
            {
                float4 vertex : POSITION;

				float4 uv : TEXCOORD0;

				float3 normal : NORMAL;
            };

			// Vertex structure
            struct v2f
            {
                float4 vertex : SV_POSITION;

				float4 screenPosition : TEXCOORD2;

				float2 noiseUV : TEXCOORD0;

				float2 distortUV : TEXCOORD1;

				float3 viewNormal : NORMAL;
            };

			// Need to be defined for vertex shader
			sampler2D _SurfaceNoise;
			float4 _SurfaceNoise_ST;
			sampler2D _SurfaceDistortion;
			float4 _SurfaceDistortion_ST;

			// Vertex shader
            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPosition = ComputeScreenPos(o.vertex);
				o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise); // TRANFORM_TEX modifies the UV with tiling and offset
				o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);
				o.viewNormal = COMPUTE_VIEW_NORMAL;

                return o;
            }

			// Properties that need to be defined
			float4 _DepthGradientShallow;
			float4 _DepthGradientDeep;
			float _DepthMaxDistance;
			sampler2D _CameraDepthTexture;
			float _SurfaceNoiseCutoff;
			//float _FoamDistance;
			float _FoamMaxDistance;
			float _FoamMinDistance;
			float2 _SurfaceNoiseScroll;
			float _SurfaceDistortionAmount;
			sampler2D _CameraNormalsTexture;
			float4 _FoamColor;

			// Fragment shader
            float4 frag (v2f i) : SV_Target
            {
				// tex2Dproj is just like tex2D but it converts it from ortho to persp
				float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
				float existingDepthLinear = LinearEyeDepth(existingDepth01);

				// Calculate difference between top of water and the depth value
				float depthDifference = existingDepthLinear - i.screenPosition.w;

				// To calculate water colour
				float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance); // Clamps between 0 and 1
				float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference01);

				// View normal of the water's surface and the object behind it
				float3 existingNormal = tex2Dproj(_CameraNormalsTexture, UNITY_PROJ_COORD(i.screenPosition));
				float3 normalDot = saturate(dot(existingNormal, i.viewNormal));
				// Shoreline
				//_FoamDistance /= 10; // Patch shorline size
				float foamDistance = lerp(_FoamMaxDistance, _FoamMinDistance, normalDot); // Changes foam size
				float foamDepthDifference01 = saturate(depthDifference / foamDistance);
				float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;

				// Create distortion
				float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;
				// Creates animation
				float2 noiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x, (i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);
				// To create waves
				float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;
				//float surfaceNoise = surfaceNoiseSample > surfaceNoiseCutoff ? 1 : 0;
				float surfaceNoise = smoothstep(surfaceNoiseCutoff - SMOOTHSTEP_AA, surfaceNoiseCutoff + SMOOTHSTEP_AA, surfaceNoiseSample);

				// Foam Colour
				float4 surfaceNoiseColor = _FoamColor;
				surfaceNoiseColor.a *= surfaceNoise;

				return alphaBlend(surfaceNoiseColor, waterColor);
            }
            ENDCG
        }
    }
}

// Credit: Free online tutorial by Erik Roystan Ross.
// https://roystan.net/articles/toon-water.html