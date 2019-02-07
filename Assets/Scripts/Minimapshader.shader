Shader "Minimap/Minimapshader"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_Color("Collision colour", Color) = (1,0,0,1)
		_Height("Subarine height", Range(0,1)) = 1
    }
	
		Category{
			Lighting off
			Cull off
			Tags{ "Queue" = "Background" }

		SubShader{
			ZWrite On
			Alphatest Less[_Height]
			Pass{SetTexture[_MainTex]}
		}
	}
}
