float health;
float fatigue;

float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR0
{
	float dist = distance(float2(.5,.5), texCoord);	
	float4 color = float4(0,0,0,0);
	
	if(dist>fatigue) { color.a = 3*(dist-fatigue);}
	if(dist>health){ color.r = ( dist-health) -.5f* (dist>fatigue)* (dist-fatigue); if(health < fatigue) color.a= 3*( dist-health);}
	
	
		

	
	
	return color;
}

technique Technique1
{
    pass Pass0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
