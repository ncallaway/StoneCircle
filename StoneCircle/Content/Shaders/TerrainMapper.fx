uniform extern texture ScreenTexture;	

sampler ScreenS = sampler_state
{
		
};

sampler pallette : register(s1);

float4 LightSource(float2 texCoord: TEXCOORD0) : COLOR0
{
	float4 tMap = tex2D(ScreenS, texCoord);
	float2 newCoord = texCoord *4095%512/4096;
	
	float4 color = tex2D(pallette, float2(newCoord.x, newCoord.y));

	//float4 color = tex2D(pallette, texCoord/8);
	
	
	return color;


}
float4 Ambient(float2 texCoord: TEXCOORD0) : COLOR0
{	

	float4 color = tex2D(ScreenS, texCoord);
	
	color.rgb -= .8* float3(1.0f, 1.0f, .8f);
	color.a  = .5f;
	return color;
}

technique Technique1
{
    pass Pass0
    {	
        PixelShader = compile ps_2_0 LightSource();
    }
    pass Pass1
    {	AlphaBlendEnable = true;
        PixelShader = compile ps_2_0 LightSource();
        SrcBlend = SrcAlpha;
        DestBlend = InvSrcAlpha;
    }
    
    pass Pass1
    {	AlphaBlendEnable = true;
        PixelShader = compile ps_2_0 LightSource();
          SrcBlend = SrcAlpha;
        DestBlend = InvSrcAlpha;
    }
  }
