uniform extern texture ScreenTexture;	

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;	
};
float health;
float fatigue;
int index;
float2 Position[2];
float2 player;
float Radius[2];
float3 AMBColor;
float AMBStrength;
float3 GLOColor;


float4 LightSource(float2 texCoord: TEXCOORD0) : COLOR0
{	

	float4 color = tex2D(ScreenS, texCoord);
	float blur = distance(player, texCoord);
	blur *= blur*blur/250;
	
	//color = tex2D(ScreenS, float2(texCoord.x+blur, texCoord.y+blur));
	//color += tex2D(ScreenS, float2(texCoord.x-blur, texCoord.y-blur));
	//color += tex2D(ScreenS, float2(texCoord.x-blur, texCoord.y+blur));
	//color += tex2D(ScreenS, float2(texCoord.x+blur, texCoord.y-blur));	
	//color += tex2D(ScreenS, float2(texCoord.x+blur, texCoord.y));
	//color += tex2D(ScreenS, float2(texCoord.x-blur, texCoord.y));
	//color += tex2D(ScreenS, float2(texCoord.x, texCoord.y+blur));
	//color += tex2D(ScreenS, float2(texCoord.x, texCoord.y-blur));
	
	//color/=8;
	
	color.rgb -= AMBStrength * AMBColor;
	
	
	for (int i =0; i<index; i++){
	float dist = distance(Position[i], texCoord);
	if ( dist < Radius[i])  color.rgb += (Radius[i]-dist) * 1.5 * GLOColor;
	}
	
	float dist2 = distance(float2(.5, .5), texCoord);
	if(dist2>health) color.rgb += (dist2-health) * (9 -((dist2 > (1-health/2))*3)) *float3(.3f,-.2f,-.2f);
	if(dist2>fatigue) color.rgb -= ((dist2-fatigue))*2*float3(1.0f, 1.0f, 1.0f);

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
