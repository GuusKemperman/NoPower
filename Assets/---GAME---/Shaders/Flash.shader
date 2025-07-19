Shader "Custom/FlashShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Color ("Base Color Tint", Color) = (1,1,1,1)
        _FlashColor ("Flash Color", Color) = (1,1,1,1)
        _FlashAmount ("Flash Amount", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _FlashColor;
        float _FlashAmount;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            fixed4 finalColor = lerp(texColor, _FlashColor, _FlashAmount);
            o.Albedo = finalColor.rgb;
            o.Alpha = finalColor.a;
        }
        ENDCG
    }

    FallBack "Diffuse"
}