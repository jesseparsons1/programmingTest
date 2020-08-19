Shader "Custom/Outline"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _DotProduct("Outline", Range(-1,1)) = 0.25
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
        float _DotProduct;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = _Color;
            o.Albedo = c.rgb;

            float border = 1 - (abs(dot(IN.viewDir, IN.worldNormal)));
            float alpha = (border * (1 - _DotProduct) + _DotProduct);
            
            o.Alpha = c.a * alpha;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
