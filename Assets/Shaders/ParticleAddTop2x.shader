Shader "Custom/ParticleAddTop2x" 
{

Properties 
{
	_MainTex ("Particle Texture", 2D) = "white" {}
}

Category 
{
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	Cull Off Lighting Off ZWrite Off ZTest Always
	Fog { Mode Off }
	
	BindChannels 
	{
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				combine texture * primary DOUBLE
			}
		}
	}
}
}
