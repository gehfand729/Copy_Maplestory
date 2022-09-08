Shader "Unlit/sShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
#if 1
#define vec2 float2
#define vec3 float3
#define vec4 float4
#define fract(x) (x - floor(x))
//#define mix lerp
#define mix(x, y, a) ((x) * (1-(a)) + (y) * (a))
#define mat2 float2x2
#define mat3 float3x3
#define mod(x, y) ((x) - (y) * floor((x)/(y)))
#define int1 INT1
#define int2 INT2
#define atan atan2
#define inversesqrt(x) (1./sqrt(x))

#define lowp
#define mediump

            vec3 iResolution;
            vec4 _MainTex_TexelSize;// x, y, z(width), w(height)
            float iTime;
            vec4 iDate;
            vec4 iMouse;
#endif


            fixed4 frag(v2f i) : SV_Target
            {
                iResolution = vec3(_MainTex_TexelSize.zw, 0);

                vec4 c;
                vec2 fragCoord = i.uv.xy * iResolution.xy;
                mainImage(c, fragCoord);

                fixed4 col = c;// tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
