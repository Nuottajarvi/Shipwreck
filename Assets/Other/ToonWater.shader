Shader "Roystan/Toon/Water"
{
    Properties
    {
        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777
        _SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)
        _SurfaceDistortion("Surface Distortion", 2D) = "white" {}
        _SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27
    }
    SubShader
    {
        Pass
        {
			CGPROGRAM
            #define SMOOTHSTEP_AA 0.01
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 noiseUV : TEXCOORD0;
                float2 distortUV : TEXCOORD1;
            };

            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;
            float _SurfaceNoiseCutoff;
            float2 _SurfaceNoiseScroll;

            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;
            float _SurfaceDistortionAmount;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
                o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float tileMultiplierBase = 10;
                float tileMultiplierDistort = 6;

                float4 waterColor = .5 * float4(0, 0.9, 1, 1) - tex2D(_SurfaceNoise, i.noiseUV * .5).r * .3 + float4(1,1,1,1) * i.noiseUV.y;
                float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV * tileMultiplierDistort).xy * 2 - 1) * _SurfaceDistortionAmount;
                float2 noiseUV = float2((i.noiseUV.x * tileMultiplierBase + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x, (i.noiseUV.y * tileMultiplierBase + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);
                float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;
                float surfaceNoise = smoothstep(_SurfaceNoiseCutoff - SMOOTHSTEP_AA, _SurfaceNoiseCutoff + SMOOTHSTEP_AA, surfaceNoiseSample);
				return waterColor + surfaceNoise;
            }
            ENDCG
        }
    }
}