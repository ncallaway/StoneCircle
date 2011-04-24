uniform extern texture ScreenTexture;	

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;	
};



float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(ScreenS, texCoord);
	
	
	color.rgb -= .75f * float3(1.0f, 1.0f, .9f);
	return color;
}

technique Technique1
{
    pass Pass0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
