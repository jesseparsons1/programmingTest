Shader "Custom/Outline"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MinAlpha("Outline", Range(-1 , 0)) = 0.25
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True"}
        LOD 200

        CGPROGRAM

        #pragma surface surf Lambert alpha:fade nolighting

        #pragma target 3.0

        struct Input
        {
            float3 worldNormal;
            float3 viewDir;
        };

        fixed4 _Color;
        float _MinAlpha;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = _Color;
            o.Albedo = c.rgb;

            float border = 1 - (abs(dot(IN.viewDir, IN.worldNormal)));
            float alphaScale = _MinAlpha + (1 - _MinAlpha) * border;
            
            o.Alpha = alphaScale;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
