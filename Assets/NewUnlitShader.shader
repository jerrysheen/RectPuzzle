Shader"Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaValue ("Alpha", Float) = 1.0
        _Bounds ("Bounds", Vector) = (0.0, 6.0, 0.0, 0.0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent"  "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            
            float _AlphaValue;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
};

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Bounds;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(UNITY_MATRIX_M, float4(v.vertex.xyz, 1.0f));
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                //return i.worldPos.y;
                col.a = 1.0 -  (i.worldPos.y - _Bounds.x) / (_Bounds.y - _Bounds.x);
                return col;
            }
            ENDCG
        }
    }
}
