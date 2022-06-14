Shader "Kotikov/Rotation"
{
    Properties
    {
        _Angle ("Angle", Range(-30, 30)) = 0.0
        _TimeScale("Time scale", Range(-100, 100)) = 0
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Angle;
            float _TimeScale;
            fixed4 _Color;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                float2 pivot = float2(0.5, 0.5);

                //float speed = _TimeScale;
                // Uncomment if you want show animation by Time
                float speed = _Time.y * _TimeScale;
                
                float cosAngle = cos(_Angle + speed);
                float sinAngle = sin(_Angle + speed);
                float2x2 rot = float2x2(cosAngle, -sinAngle, sinAngle, cosAngle);
                
                float2 uv = TRANSFORM_TEX(v.uv, _MainTex) - pivot;
                o.uv = mul(rot, uv);
                o.uv += pivot;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
