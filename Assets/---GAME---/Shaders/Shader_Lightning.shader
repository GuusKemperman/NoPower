Shader "Unlit/Shader_Lightning"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Glow("Glow Intensity", Float) = 5
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
                float noise = sin(_Time.y * 40 + i.uv.x * 20);
                float brightness = saturate(noise * 0.5 + 0.5);
                fixed4 col = _Color * brightness * _Glow;
                col.a *= brightness;
                return col;
            }
            ENDCG
        }
    }
}
