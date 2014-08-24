Shader "Custom/Unculled" 
{
	Properties 
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
	}

	Category 
	{
		Cull Off 
		Lighting Off 
		Fog { Mode Off }
		ZWrite Off
		
		BindChannels 
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		SubShader {
			Material 
			{
	 			Diffuse [_Color]
	 		}
			Pass 
			{
				SetTexture [_MainTex] 
				{
					constantColor [_Color]
					Combine texture * primary, texture * constant 
				} 
			}
		}
	}
}
