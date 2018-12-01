Shader "Point Cloud"
{
	Properties
	{
		colour("Color", Color) = (1.0,0.0,0.0,1.0)
		_PointSize("PointSize", Range(0.001,5)) = 1
	}
		SubShader
	{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		//LOD 1
		Pass
		{
			Cull Off ZWrite On Blend SrcAlpha OneMinusSrcAlpha
			ZTest Always
			CGPROGRAM
			#pragma vertex vertex_shader
			#pragma fragment pixel_shader
			#pragma target 5.0

			struct Point
			{
				
				float3 pos : POSITION;
				fixed4 color : COLOR;
			};

			
			float4 colour;
			StructuredBuffer<Point> cloud;

			struct type
			{
				float4 pos : SV_POSITION;
				float size : PSIZE;
				fixed4 color : COLOR;
				//float3 variable : TEXCOORD1;
			};
			float _PointSize;
			type vertex_shader(uint id : SV_VertexID)
			{
				type vs;
				Point p = cloud[id];

				
				vs.pos = UnityObjectToClipPos(p.pos);
				vs.size = _PointSize;
				vs.color = p.color;
				return vs;
			}

			half4 pixel_shader(type ps) : COLOR0
			{
				return  ps.color*colour;
				//return float4(colour.rgb,1.0);
				//else return float4(ps.variable,1.0);
			}

			ENDCG
		}
	}
}