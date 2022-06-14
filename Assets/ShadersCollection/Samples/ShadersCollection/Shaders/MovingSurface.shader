Shader "Kotikov/MovingSurface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _OffsetX ("Offset X", Range(-100,100)) = 0
		_OffsetY ("Offset Y", Range(-100,100)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;
        half _OffsetX;
        half _OffsetY;
        fixed4 _Color;

		struct Input
        {
            float2 uv_MainTex;
        };
        
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float varX = _OffsetX;// * _Time.y; //Uncomment if you want move surface by Time
            float varY = _OffsetY;// * _Time.y; //Uncomment if you want move surface by Time
            fixed2 uv_Tex = IN.uv_MainTex + fixed2(varX, varY);
            half4 c = tex2D(_MainTex, uv_Tex);
           
			o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
