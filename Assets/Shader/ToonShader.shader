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
            // ライトモードの指定　今回は普通のフォワードレンダリング
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert         // 頂点
            #pragma fragment frag       // フラグメント
            #pragma multi_compile_fog   // フォグの指定

            // インクルード
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // 頂点情報
            struct appdata
            {
                float4 vertex : POSITION;      // オブジェクトの頂点位置
                float2 uv : TEXCOORD0;         // UV座標
                float3 normal : NORMAL;        // オブジェクトの法線
            };

            // フラグメント情報
            struct v2f
            {
                float2 uv : TEXCOORD0;         // UV座標
                float fogFactor: TEXCOORD1;    // フォグの影響度
                float4 vertex : SV_POSITION;   // デバイス座標系の頂点位置
                float3 normal : NORMAL;        // フラグメントの法線
            };

            // テクスチャ情報
            TEXTURE2D(_MainTex);               // メインテクスチャ
            TEXTURE2D(_RampTex);               // ランプテクスチャ
            SAMPLER(sampler_MainTex);          // メインテクスチャ用のサンプラー
            SAMPLER(sampler_RampTex);          // ランプテクスチャ用のサンプラー

            // バッファの設定
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;                // メインテクスチャのスケーリングとオフセット
            CBUFFER_END

            // 頂点情報をフラグメントシェーダに渡す処理
            v2f vert (appdata v)
            {
                // 変数の定義
                v2f o;

                // 頂点位置をオブジェクト座標系からクリップ座標系へ変換
                o.vertex = TransformObjectToHClip(v.vertex.xyz);

                // UV座標をメインテクスチャのスケーリングとオフセットに変換
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // フォグの影響度を計算
                o.fogFactor = ComputeFogFactor(o.vertex.z);

                // 法線をオブジェクト座標からワールド座標系へ変換
                o.normal = TransformObjectToWorldNormal(v.normal);

                // 作成した頂点情報を返す
                return o;
            }

            // フラグメント
            float4 frag (v2f i) : SV_Target
            {
                // メインテクスチャから色を取得
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // ライト情報を取得
                Light light = GetMainLight();

                // ピクセルの法線とライトの向きの内積を計算する
                float t = dot(i.normal, light.direction);

                // 内積の値を0より大きい値にする　0だとテクスチャの問題で正しい描画にならない
                t = max(0.01f, t);

                // 内積からランプマップの座標を取得
                float4 b = SAMPLE_TEXTURE2D(_RampTex, sampler_RampTex, float2(t, 0.5f));

                // ランプマップから取った明るさを掛け合わせる
                col *= b;

                // フォグを適応
                col.rgb = MixFog(col.rgb, i.fogFactor);

                // 色情報を返す
                return col;
            }
            ENDHLSL
        }
    }
}