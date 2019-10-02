﻿Shader "Hidden/Roystan/Normals Texture"
{
    Properties
    {
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType" = "Opaque" 
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
            };
			
			// Vertex structure
            struct v2f
            {
                float4 vertex : SV_POSITION;
				float3 viewNormal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			// Vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.viewNormal = COMPUTE_VIEW_NORMAL;
                return o;
            }

			// Fragment shader
            float4 frag (v2f i) : SV_Target
            {
                return float4(i.viewNormal, 0);
            }
            ENDCG
        }
    }
}

// Credit: Free online tutorial by Erik Roystan Ross.
// https://roystan.net/articles/toon-water.html