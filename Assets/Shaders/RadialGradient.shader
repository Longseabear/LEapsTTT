
Shader "Unlit/Gradient"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _Radius ("Radius", Float) = 0.5
        _ColorPivot ("Color Pivot", Vector) = (0.5, 0.5, 0, 0)
        _Color1 ("Inner Color", Color) = (1,0,0,1)
        _Color2 ("Outer Color", Color) = (0,0,1,1)
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "TilemapInstanced"="True"
        }
        LOD 200

        Blend One OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                half2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _Radius;
            float4 _ColorPivot;
            fixed4 _Color1;
            fixed4 _Color2;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 pivot = _ColorPivot.xy;
                float dist = distance(IN.texcoord, pivot);
                float t = saturate(dist / _Radius);
                fixed4 gradientColor = lerp(_Color1, _Color2, t);

                fixed4 texColor = tex2D(_MainTex, IN.texcoord);
                texColor *= IN.color;
                texColor.rgb *= texColor.a;
                return texColor * gradientColor;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"

}
