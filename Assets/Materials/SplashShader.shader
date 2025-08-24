Shader "Custom/InnerOutlineSoftShader" {
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)

        _EdgeAntiAliasing("Edge Anti-Aliasing", Range(0.0, 5.0)) = 1.0

        // --- Inner Outline Settings ---
        _OutlineEnabled ("Enable Outline (0=Off, 1=On)", Float) = 0
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness ("Outline Thickness", Float) = 5
        _OutlineSoftness ("Outline Softness", Float) = 0.5
        _OutlineBlendMode ("Blend Mode (0=Mix, 1=Mult, 2=Add)", Float) = 0
        
        // --- UV Ripple Deformation Settings ---
        _DeformationStrength ("Deformation Strength", Float) = 0.02
        _DeformationFrequency ("Deformation Frequency", Float) = 20.0
        _DeformationSpeed ("Deformation Speed", Float) = 2.0
    }

    SubShader {
       Tags {
           "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"
           "PreviewType"="Plane" "CanUseSpriteAtlas"="True"
       }
       Pass {
           Stencil { Ref 4 Comp NotEqual Pass Replace }
           Cull Off Lighting Off ZWrite Off
           Blend SrcAlpha OneMinusSrcAlpha

           CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
           #pragma target 3.0
           #include "UnityCG.cginc"

           // --- Variable Declarations ---
           uniform sampler2D _MainTex;
           uniform half4 _MainTex_TexelSize;
           fixed4 _Color;
           half _EdgeAntiAliasing;
           fixed4 _OutlineColor;
           half _OutlineThickness;
           float _OutlineEnabled;
           float _OutlineSoftness;
           float _OutlineBlendMode;
           float _DeformationStrength;
           float _DeformationFrequency;
           float _DeformationSpeed;

           struct v2f {
               half4 pos : POSITION;
               half2 uv : TEXCOORD0;
               fixed4 color : COLOR;
           };

           v2f vert(appdata_img v) {
               v2f o;
               o.pos = UnityObjectToClipPos(v.vertex);
               o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
               o.color = _Color;
               return o;
           }

           half4 frag(v2f i) : COLOR {
               
               // --- RIPPLE DEFORMATION LOGIC ---
               float sineWave = sin(i.uv.y * _DeformationFrequency + _Time.y * _DeformationSpeed);
               float horizontalOffset = sineWave * _DeformationStrength;

               // --- FIX FOR EDGE ARTIFACT ---
               // Create a falloff gradient that is 0 at the horizontal edges (x=0, x=1) and 1 in the middle.
               // This prevents the UV coordinates from being pushed out of bounds, which causes clamping/stretching.
               float edgeFalloff = 4.0 * i.uv.x * (1.0 - i.uv.x);
               
               // Apply the falloff to the offset.
               float finalOffset = horizontalOffset * edgeFalloff;
               
               // Create the new, deformed UV coordinate using the corrected offset.
               float2 deformedUV = i.uv + float2(finalOffset, 0);

               // --- ALL TEXTURE LOOKUPS NOW USE THE DEFORMED UV ---
               half4 textureColor = tex2D(_MainTex, deformedUV);
               half3 finalRGB = textureColor.rgb * i.color.rgb;

               if (_OutlineEnabled > 0.5 && textureColor.a > 0.01) {
                   half2 offset = _MainTex_TexelSize.xy * _OutlineThickness;
                   
                   float neighborAlpha = 0.0;
                   neighborAlpha += tex2D(_MainTex, deformedUV + half2(0, offset.y)).a;
                   neighborAlpha += tex2D(_MainTex, deformedUV - half2(0, offset.y)).a;
                   neighborAlpha += tex2D(_MainTex, deformedUV + half2(offset.x, 0)).a;
                   neighborAlpha += tex2D(_MainTex, deformedUV - half2(offset.x, 0)).a;
                   neighborAlpha += tex2D(_MainTex, deformedUV + offset).a;
                   neighborAlpha += tex2D(_MainTex, deformedUV - offset).a;
                   neighborAlpha += tex2D(_MainTex, deformedUV + half2(offset.x, -offset.y)).a;
                   neighborAlpha += tex2D(_MainTex, deformedUV + half2(-offset.x, offset.y)).a;

                   float softnessRange = 8.0 * _OutlineSoftness;
                   float upperBound = 7.99;
                   float lowerBound = upperBound - softnessRange;
                   float blendFactor = 1.0 - smoothstep(lowerBound, upperBound, neighborAlpha);

                   if (blendFactor > 0.0) {
                       half3 blendedColor;
                       half3 baseColorForBlend = finalRGB;
                       if (_OutlineBlendMode < 0.5) { // Mix
                           blendedColor = lerp(baseColorForBlend, _OutlineColor.rgb, _OutlineColor.a);
                       } else if (_OutlineBlendMode < 1.5) { // Multiply
                           blendedColor = baseColorForBlend * _OutlineColor.rgb;
                       } else { // Additive
                           blendedColor = baseColorForBlend + _OutlineColor.rgb;
                       }
                       finalRGB = lerp(baseColorForBlend, blendedColor, blendFactor);
                   }
               }
               
               half derivative = fwidth(textureColor.a);
               half shapeAlpha = smoothstep(0.5 - derivative * _EdgeAntiAliasing, 0.5 + derivative * _EdgeAntiAliasing, textureColor.a);
               half finalAlpha = shapeAlpha * i.color.a;

               clip(finalAlpha - 0.001);

               return half4(finalRGB, finalAlpha);
           }
           ENDCG
       }
    }
    Fallback off
}