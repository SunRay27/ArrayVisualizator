// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 Shader "Custom/PC2" {
 //based on http://forum.unity3d.com/threads/176317-Point-Sprite-automatic-texture-coords
     Properties {
         _PointSize("PointSize", Float) = 1
		 _Color("Color",Color) = (1.0,0.0,0.0,1.0)
     }
     SubShader {
        // Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        // 
         Pass 
	 {
		 
		// Cull Off ZWrite On Blend SrcAlpha OneMinusSrcAlpha
		 
			Cull Off
			ZWrite On
			//Blend OneMinusDstColor  One
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest NotEqual
         
			 CGPROGRAM
			 #pragma vertex vert
			 #pragma fragment frag
			 #pragma target 5.0
                                    
		struct VertexInput {
			float4 pos : POSITION;
			float4 color: COLOR;
		};
 
                 
	 struct VertexOutput {
		 float4 pos : SV_POSITION;
		 float size : PSIZE;
		 float4 color : COLOR;
	 };
                 float _PointSize;
				 float4 _Color;
				 VertexOutput vert(VertexInput  v)
				 {
					 VertexOutput o;
                     o.pos = UnityObjectToClipPos(v.pos);
                     o.size = _PointSize; 
                     o.color = v.color;
                     return o;
                 }
 
				 float4 frag(VertexOutput o) : COLOR
				 {
					 return o.color*_Color;
				 }
             ENDCG
         }
     } 
 }