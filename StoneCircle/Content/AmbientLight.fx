float health;


float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR0
{
	float dist = distance(float2(.5,.5), texCoord);	
	float4 color = float4(0,0,0,0);
	if(dist>health){ color.r = 2*( dist-health); color.a= 2*( dist-health);}
	
	
		

	
	
	return color;
}

technique Technique1
{
    pass Pass0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
