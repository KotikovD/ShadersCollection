Shader "Kotikov/Noiser"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Intensity ("Intensity", Range(0.01, 10.0)) = 2.0
        _Pixilization ("Pixilization", Range(1, 400.0)) = 20.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _Intensity;
        float _Pixilization;

        
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float random (float2 x)
        {
            const float multiplier = 555.0;
            const float offsetX = 1224.0;
            const float offsetY = 1242.0;
            
            float d = dot(x, float2(offsetX, offsetY));
            float s = sin(d);
            return frac(s * multiplier);
        }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            float2 uv = IN.uv_MainTex * _Pixilization;
            float2 id = _Pixilization == 1 ? IN.uv_MainTex : floor(uv);
            float rnd = random(id);
            
            o.Albedo = c.rgb - rnd * _Intensity;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
