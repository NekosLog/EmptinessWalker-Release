Shader "Custom/ToonShader"
{
    Properties
    {
        // メインテクスチャ
        _MainTex ("Texture", 2D) = "white" {}

        // ランプテクスチャ
        _RampTex ("RampMap", 2D) = "white" {}
    }
    SubShader
    {
        Tags {
            // レンダータイプ
            "RenderType"="Opaque"

            // レンダーパイプラインはURPを使用
            "RenderPipeline"="UniversalPipeline"
        }

        // ディフューズシェーダを利用したトゥーンシェーダ
        LOD 200

        Pass
        {
            // 
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float fogFactor: TEXCOORD1;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            TEXTURE2D(_MainTex);
            TEXTURE2D(_RampTex);
            SAMPLER(sampler_MainTex);
            SAMPLER(sampler_RampTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.fogFactor = ComputeFogFactor(o.vertex.z);
                o.normal = TransformObjectToWorldNormal(v.normal);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // ライト情報を取得
                Light light = GetMainLight();

                // ピクセルの法線とライトの方向の内積を計算する
                float t = dot(i.normal, light.direction);

                // 内積の値を0以上の値にする
                t = max(0.01f, t);

                // 内積からランプマップの座標を取得
                float4 b = SAMPLE_TEXTURE2D(_RampTex, sampler_MainTex, float2(t, 0.5f));

                // 拡散反射光を反映
                col *= b;

                // apply fog
                col.rgb = MixFog(col.rgb, i.fogFactor);
                return col;
            }
            ENDHLSL
        }
    }
}