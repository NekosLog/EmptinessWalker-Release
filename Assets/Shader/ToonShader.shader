Shader "Custom/ToonShader"
{
    Properties
    {
        // ���C���e�N�X�`��
        _MainTex ("Texture", 2D) = "white" {}

        // �����v�e�N�X�`��
        _RampTex ("RampMap", 2D) = "white" {}
    }
    SubShader
    {
        Tags {
            // �����_�[�^�C�v
            "RenderType"="Opaque"

            // �����_�[�p�C�v���C����URP���g�p
            "RenderPipeline"="UniversalPipeline"
        }

        // �f�B�t���[�Y�V�F�[�_�𗘗p�����g�D�[���V�F�[�_
        LOD 200

        Pass
        {
            // ���C�g���[�h�̎w��@����͕��ʂ̃t�H���[�h�����_�����O
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert         // ���_
            #pragma fragment frag       // �t���O�����g
            #pragma multi_compile_fog   // �t�H�O�̎w��

            // �C���N���[�h
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // ���_���
            struct appdata
            {
                float4 vertex : POSITION;      // �I�u�W�F�N�g�̒��_�ʒu
                float2 uv : TEXCOORD0;         // UV���W
                float3 normal : NORMAL;        // �I�u�W�F�N�g�̖@��
            };

            // �t���O�����g���
            struct v2f
            {
                float2 uv : TEXCOORD0;         // UV���W
                float fogFactor: TEXCOORD1;    // �t�H�O�̉e���x
                float4 vertex : SV_POSITION;   // �f�o�C�X���W�n�̒��_�ʒu
                float3 normal : NORMAL;        // �t���O�����g�̖@��
            };

            // �e�N�X�`�����
            TEXTURE2D(_MainTex);               // ���C���e�N�X�`��
            TEXTURE2D(_RampTex);               // �����v�e�N�X�`��
            SAMPLER(sampler_MainTex);          // ���C���e�N�X�`���p�̃T���v���[
            SAMPLER(sampler_RampTex);          // �����v�e�N�X�`���p�̃T���v���[

            // �o�b�t�@�̐ݒ�
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;                // ���C���e�N�X�`���̃X�P�[�����O�ƃI�t�Z�b�g
            CBUFFER_END

            // ���_�����t���O�����g�V�F�[�_�ɓn������
            v2f vert (appdata v)
            {
                // �ϐ��̒�`
                v2f o;

                // ���_�ʒu���I�u�W�F�N�g���W�n����N���b�v���W�n�֕ϊ�
                o.vertex = TransformObjectToHClip(v.vertex.xyz);

                // UV���W�����C���e�N�X�`���̃X�P�[�����O�ƃI�t�Z�b�g�ɕϊ�
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // �t�H�O�̉e���x���v�Z
                o.fogFactor = ComputeFogFactor(o.vertex.z);

                // �@�����I�u�W�F�N�g���W���烏�[���h���W�n�֕ϊ�
                o.normal = TransformObjectToWorldNormal(v.normal);

                // �쐬�������_����Ԃ�
                return o;
            }

            // �t���O�����g
            float4 frag (v2f i) : SV_Target
            {
                // ���C���e�N�X�`������F���擾
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // ���C�g�����擾
                Light light = GetMainLight();

                // �s�N�Z���̖@���ƃ��C�g�̌����̓��ς��v�Z����
                float t = dot(i.normal, light.direction);

                // ���ς̒l��0���傫���l�ɂ���@0���ƃe�N�X�`���̖��Ő������`��ɂȂ�Ȃ�
                t = max(0.01f, t);

                // ���ς��烉���v�}�b�v�̍��W���擾
                float4 b = SAMPLE_TEXTURE2D(_RampTex, sampler_RampTex, float2(t, 0.5f));

                // �����v�}�b�v�����������邳���|�����킹��
                col *= b;

                // �t�H�O��K��
                col.rgb = MixFog(col.rgb, i.fogFactor);

                // �F����Ԃ�
                return col;
            }
            ENDHLSL
        }
    }
}