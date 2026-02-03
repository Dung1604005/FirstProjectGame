Shader "Custom/FrameAnimation_Fixed"
{
    Properties
    {
        _MainTex ("Animation Texture", 2D) = "white" {}
        _Grid ("Grid Size (Columns, Rows)", Vector) = (4, 4, 0, 0)
        _FrameRate ("Frame Rate", Float) = 15
    }
    SubShader
    {
        // Chỉnh thành Transparent để Trail mượt và không có nền đen
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc" // Thư viện cần thiết cho các biến unity_

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; // Nhận màu từ Trail Renderer
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Grid;
            float _FrameRate;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;

                // LOGIC NHẢY FRAME NẰM Ở ĐÂY
                float totalFrames = _Grid.x * _Grid.y;
                float frameIndex = floor(_Time.y * _FrameRate % totalFrames);
                
                float frameX = fmod(frameIndex, _Grid.x);
                float frameY = floor(frameIndex / _Grid.x);

                // Thu nhỏ UV về kích thước 1 ô và dời đến đúng vị trí frame
                float2 size = float2(1.0 / _Grid.x, 1.0 / _Grid.y);
                float2 offset = float2(frameX * size.x, (_Grid.y - 1.0 - frameY) * size.y);
                
                o.uv = v.uv * size + offset;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return col;
            }
            ENDHLSL
        }
    }
}