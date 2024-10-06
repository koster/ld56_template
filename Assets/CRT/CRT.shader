Shader "Hidden/CRTShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _ScanlineIntensity ("Scanline Intensity", Range(0, 1)) = 0.5
        _Distortion ("Distortion", Range(0, 1)) = 0.1
        _Curvature ("Curvature", Range(0, 1)) = 0.2
        _ScanlineSpeed ("Scanline Speed", Range(0, 10)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZTest Always Cull Off ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _ScanlineIntensity;
            float _Distortion;
            float _Curvature;
            float _ScanlineSpeed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Apply screen curvature
                float2 centeredUV = v.texcoord - 0.5;
                centeredUV *= 1.0 + _Curvature * dot(centeredUV, centeredUV);
                o.uv = centeredUV + 0.5;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture with UV distortion
                float2 uv = i.uv;
                uv.x += _Distortion * (uv.y - 0.5) * (uv.y - 0.5);
                fixed4 col = tex2D(_MainTex, uv);

                // Add moving scanlines effect
                float scanline = sin((i.uv.y + _Time.y * _ScanlineSpeed) * _ScreenParams.y * 3.14 * 2) * 0.5 + 0.5;
                col.rgb *= 1.0 - _ScanlineIntensity * scanline;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
