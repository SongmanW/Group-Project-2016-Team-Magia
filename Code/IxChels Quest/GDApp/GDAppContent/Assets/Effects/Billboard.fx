//Author: NMCG
//Date: 23.4.15

//------- Variables --------//
//used for WVP calculation
float4x4 View;
float4x4 Projection;
float4x4 World;

//used by normal billboard
float3 Right;
float3 Up;

//used by animated billboard
bool IsAnimated = false;
float2 InverseFrameCount;
int CurrentFrame;

//used by scrolling billboard
bool IsScrolling = false;
float2 scrollRate = float2(0,0);

float Alpha = 1;
float AlphaThreshold = 0.01f;

//------- Texture Samplers --------//
Texture DiffuseTexture;
sampler textureSampler = sampler_state 
{ 
	texture = <DiffuseTexture>;
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter = LINEAR; 
	AddressU = Wrap; 
	AddressV = Wrap; 
};

//------- Structures --------//
struct VS_INPUT
{
	float3 Position				:	POSITION0;
	float4 TexCoordAndOffset	:	TEXCOORD0;
};

struct VS_OUTPUT
{
	float4 Position :	POSITION;
	float2 TexCoord	:	TEXCOORD0;
};

//------- Functions --------//
float2 GetTexCoords(float2 texCoords)
{
	if (IsAnimated)
		return (texCoords + float2(CurrentFrame, CurrentFrame)) * InverseFrameCount;
	else
		return texCoords;
}

float4 GetFinalPosition(float3 center, float2 texCoords, float2 scale, float3 up, float3 side)
{
	float3 finalPosition = center;
	finalPosition += (texCoords.x - 0.5f)*side*scale.x;
	finalPosition += (0.5f - texCoords.y)*up*scale.y;
	return mul(float4(finalPosition, 1), mul(View, Projection));
}
//
////------- Vertex Shaders --------//
VS_OUTPUT NormalBillboardVS(VS_INPUT In)
{
	VS_OUTPUT Output = (VS_OUTPUT)0;
	float3 finalPosition = In.Position;
	finalPosition += (In.TexCoordAndOffset.z * -Right) + (In.TexCoordAndOffset.w * Up);
	Output.Position = mul(float4(finalPosition, 1), mul(World, mul(View, Projection)));
	Output.TexCoord = GetTexCoords(In.TexCoordAndOffset.xy);
	return Output;
}
VS_OUTPUT CylindricalBillboardVS(VS_INPUT In)
{
	VS_OUTPUT Output = (VS_OUTPUT)0;
	float3 finalPosition = In.Position;
	finalPosition += (In.TexCoordAndOffset.z * float3(View._11, View._21, View._31)) + (In.TexCoordAndOffset.w * normalize(Up));
	Output.Position = mul(float4(finalPosition, 1), mul(World, mul(View, Projection)));
	Output.TexCoord = GetTexCoords(In.TexCoordAndOffset.xy);
	return Output;
}
VS_OUTPUT SphericalBillboardVS(VS_INPUT In)
{
	VS_OUTPUT Output = (VS_OUTPUT)0;
	float3 finalPosition = In.Position;
	finalPosition += (In.TexCoordAndOffset.z * float3(View._11, View._21, View._31)) + (In.TexCoordAndOffset.w * float3(View._12, View._22, View._32));
	Output.Position = mul(float4(finalPosition, 1), mul(World, mul(View, Projection)));
	Output.TexCoord = GetTexCoords(In.TexCoordAndOffset.xy);
	return Output;
}
//------- Pixel Shader ------------------
float4 BillboardPS(VS_OUTPUT In) : COLOR0
{
	float2 texCoord = In.TexCoord;

	if (IsScrolling)
	{
		texCoord.x -= scrollRate.x;
		texCoord.y += scrollRate.y;
		texCoord.x %= 1;
		texCoord.y %= 1;
	}
	float4 Color = tex2D(textureSampler, texCoord);
	Color.a *= Alpha; 

	clip(Color.a < AlphaThreshold ? -1 : 1);

	return Color;
}

//------- Techniques --------------------

technique Normal
{
	pass Pass0
    {        
    	VertexShader = compile vs_2_0 NormalBillboardVS();
        PixelShader  = compile ps_2_0 BillboardPS();        
    }
}
technique Cylindrical
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 CylindricalBillboardVS();
		PixelShader = compile ps_2_0 BillboardPS();
	}
}
technique Spherical
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 SphericalBillboardVS();
		PixelShader = compile ps_2_0 BillboardPS();
	}
}