//------------------------------------------------------
//--                                                	--
//--				www.riemers.net			--
//--				Basic shaders			--
//--			Use/modify as you like              --
//--                                                	--
//------------------------------------------------------

struct VertexToPixel
{
    float4 Position   	: POSITION;    
    float4 Color		: COLOR0;
    float LightingFactor: TEXCOORD0;
    float2 TextureCoords: TEXCOORD1;
};

struct PixelToFrame
{
    float4 Color : COLOR0;
};

//------- XNA-to-HLSL variables --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float3 xLightDirection;
float xAmbient;
bool xEnableLighting;
bool xShowNormals;

//------- Texture Samplers --------

Texture xTexture;
float   xTextureWidth;
float   xTextureHeight;

sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

//------- Technique: Pretransformed --------

VertexToPixel PretransformedVS( float4 inPos : POSITION, float4 inColor: COLOR)
{	
	VertexToPixel Output = (VertexToPixel)0;
	
	Output.Position = inPos;
	Output.Color = inColor;
    
	return Output;    
}

PixelToFrame PretransformedPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
	
	Output.Color = PSIn.Color;

	return Output;
}

technique Pretransformed_2_0
{
	pass Pass0
	{   
		VertexShader = compile vs_2_0 PretransformedVS();
		PixelShader  = compile ps_2_0 PretransformedPS();
	}
}

//------- Technique: Colored --------

VertexToPixel ColoredVS( float4 inPos : POSITION, float4 inColor: COLOR, float3 inNormal: NORMAL)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);
	Output.Color = inColor;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
	Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor = saturate(dot(Normal, -xLightDirection));
    
	return Output;    
}

PixelToFrame ColoredPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
    
	Output.Color = PSIn.Color;
	Output.Color.rgb *= saturate(PSIn.LightingFactor + xAmbient);
	
	return Output;
}

technique Colored_2_0
{
	pass Pass0
	{   
		VertexShader = compile vs_2_0 ColoredVS();
		PixelShader  = compile ps_2_0 ColoredPS();
	}
}



// Technique: VerticalEdge

VertexToPixel VerticalEdgeVS( float4 inPos : POSITION, float2 inTexCoords : TEXCOORD0 )
{
	VertexToPixel Output = (VertexToPixel)0;
	
	Output.Position = inPos;
	Output.TextureCoords = inTexCoords;
	
	return Output;
}

PixelToFrame VerticalEdgePS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;
	
	Output.Color.rgb = float3(0, 0, 0);
	
	float4 hit = tex2D(TextureSampler, PSIn.TextureCoords);
	if (hit.r >= 0.5) {
		float pixDeltaY = 1.0 / xTextureHeight;
		float2 uv = PSIn.TextureCoords;
		uv.y += pixDeltaY * 2;
	
		hit = tex2D(TextureSampler, uv);
		
		if (hit.r >= 0.5) {
			Output.Color.rgb = float3(0.0, 1.0, 0.0);
		}
	}
	
	return Output;
}

technique VerticalEdge_2_0
{
	pass Pass0
	{   
		VertexShader = compile vs_2_0 VerticalEdgeVS();
		PixelShader  = compile ps_2_0 VerticalEdgePS();
	}
}


//------- Technique: Textured --------
VertexToPixel TexturedVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	//Output.Position = mul(inPos, preWorldViewProjection);	
	Output.Position = inPos;
	Output.TextureCoords = inTexCoords;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
	Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor = saturate(dot(Normal, -xLightDirection));
    
	return Output;    
}


PixelToFrame TexturedPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;

	float2 uv = PSIn.TextureCoords;
	
	Output.Color = tex2D(TextureSampler, uv);
	Output.Color.rgb *= saturate(PSIn.LightingFactor + xAmbient);

	return Output;
}

technique Textured_2_0
{
	pass Pass0
	{   
		VertexShader = compile vs_2_0 TexturedVS();
		PixelShader  = compile ps_2_0 TexturedPS();
	}
}




