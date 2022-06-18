Shader "Kotikov/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
        _OutlineColor2("Outline Color2", Color) = (1,1,1,1)
        _OutlineWidth("Outline Width", Range(0, 1)) = 0.01
        _MovingSpeed("Moving Speed", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float4 _OutlineColor2;
            float _OutlineWidth;
            float _MovingSpeed;

            static float _diagonal = 0.7;
            static float2 _dirs[8] =
                {float2(1, 0), float2(-1, 0), float2(0, 1), float2(0, -1),
                float2(_diagonal, _diagonal), float2(-_diagonal, _diagonal), float2(_diagonal, -_diagonal), float2(-_diagonal, -_diagonal)};
            
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            float GetMaxAlpha(float2 uv)
            {
                float result = 0;
                for (uint i = 0; i < 8; i++)
                {
                    float2 sUV = uv + _dirs[i] * _OutlineWidth;
                    result = max(result, tex2D(_MainTex, sUV).a);
                }
                return result;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= i.color;

                float2 noiseUV = i.uv;
                noiseUV.y += _Time.y * _MovingSpeed;
                fixed3 noiseColor = tex2D(_NoiseTex, noiseUV).rgb;
                fixed3 invertedNoiseColor = 1 - noiseColor;
                fixed3 outlineColor = _OutlineColor.rgb * noiseColor +  _OutlineColor2.rgb * invertedNoiseColor;
                
                col.rgb = lerp(outlineColor, col.rgb, col.a);
                col.a = GetMaxAlpha(i.uv);
                
                return col;
            }
            ENDCG
        }
    }
}
