Shader "Unlit/Shader_Lightning"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Glow("Glow Intensity", Float) = 5.0
        _Fade("Fade Duration", Float) = 1.0
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha One
        ZTest Always
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _Color;
            float _Glow;
            float _Fade;
            sampler2D _MainTex;

            // -------------------------
            // Structs
            // -------------------------
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            // -------------------------
            // Shaders
            // -------------------------
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
               // Flickering
               float noise = sin(_Time.y * 40 + i.uv.x * 20);
               float brightness = saturate(noise * 0.5 + 0.5);
               fixed4 col = _Color * brightness * _Glow;

               // Fade from left (uv.x = 0) to right (uv.x = 1) over time
               float fadeProgress = saturate(_Time.y / _Fade);
               float edgeWidth = 0.1; // controls softness of the fade edge
               float alphaMask = 1.0 - smoothstep(fadeProgress - edgeWidth, fadeProgress + edgeWidth, i.uv.x);

               col.a *= brightness * alphaMask;
               return col;
            }
            ENDCG
        }
    }
}
