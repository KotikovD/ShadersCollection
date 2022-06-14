Shader "Kotikov/RoundedWorld"
{
    Properties
    {
        _CurvatureX ("CurvatureX", Float) = 0.0000
        _CurvatureY ("CurvatureY", Float) = 0.0001
        _MainTex ("Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200


        CGPROGRAM
        #pragma surface surf Standard vertex:vert addshadow
        #pragma target 3.0
        
        sampler2D _MainTex;
        float _CurvatureX;
        float _CurvatureY;
        half _Glossiness;
        half _Metallic;
        
        struct Input
        {
            float2 uv_MainTex;
        };

        void vert(inout appdata_full v)
        {
            float4 vv = mul(unity_ObjectToWorld, v.vertex);
            vv.xyz -= _WorldSpaceCameraPos.xyz;
            float z = vv.z * vv.z;
            vv = float4(z * -_CurvatureX, z * -_CurvatureY, 0.0f, 0.0f);
            v.vertex += mul(unity_WorldToObject, vv);
        }
        
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
}
