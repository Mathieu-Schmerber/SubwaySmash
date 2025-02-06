Shader "Custom/XAxisTransparency_Texture"
{
    Properties
    {
        _Color ("Base Color", Color) = (1, 1, 1, 1)
        _MainTex ("Base Texture", 2D) = "white" { }
        _CenterTransparency ("Center Transparency", Range(0, 1)) = 0.0
        _EdgeTransparency ("Edge Transparency", Range(0, 1)) = 1.0
        _FadeStart ("Fade Start", Range(0, 1)) = 0.3 // Where the fadeout begins
        _FadeEnd ("Fade End", Range(0, 1)) = 0.7   // Where the fadeout finishes
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 objectPos : TEXCOORD1; // Object-space position
            };

            fixed4 _Color;
            float _CenterTransparency;
            float _EdgeTransparency;
            float _FadeStart;
            float _FadeEnd;
            sampler2D _MainTex; // Texture sampler

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                // Pass object-space position (without transformation) to fragment shader
                o.objectPos = v.vertex.xyz; 
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // Get the X position in object space
                float xPos = i.objectPos.x;

                // Normalize the X position relative to the fadeout range
                float fadeRange = _FadeEnd - _FadeStart;

                // Calculate the transparency based on the X position within the fade range
                float alpha = 0.0;
                
                if (abs(xPos) < _FadeStart)
                {
                    alpha = _EdgeTransparency;
                }
                else if (abs(xPos) > _FadeEnd)
                {
                    alpha = _CenterTransparency;
                }
                else
                {
                    // Smooth transition within the fade range
                    float t = (abs(xPos) - _FadeStart) / fadeRange;
                    alpha = lerp(_EdgeTransparency, _CenterTransparency, smoothstep(0.0, 1.0, t));
                }

                // Multiply the texture color with the calculated alpha
                texColor.a *= alpha;

                // Apply base color and texture color
                return texColor * _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}