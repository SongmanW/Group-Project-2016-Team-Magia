/*
 * Author:  NMCG
 * Date:    28/3/15
 * Function: Skinned model shader with diffuse, normal and spot lights
 * Amended: None
 */

#define MaxBones 59
float4x4 Bones[MaxBones];    //replaces world transform since each bone needs a world transform
float4x4 World;
float4x4 View;
float4x4 Projection;

/*************************************************************************************/
/*								TEXTURES											 */
/*************************************************************************************/
texture DiffuseMapTexture;
sampler DiffuseMapSampler = sampler_state 
{
	 Texture = <DiffuseMapTexture>; 
	 MinFilter = Linear; 
	 MagFilter = Linear;  
	 MipFilter = Linear; 
	 AddressU = Wrap;
     AddressV = Wrap;	
 };

float Alpha;
 /*************************************************************************************/
/*								STRUCTURES											 */
/*************************************************************************************/
struct VS_INPUT 
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;

	//animation specific
	float4 BoneIndices : BLENDINDICES;
	float4 BoneWeights : BLENDWEIGHT;
};

struct VS_OUTPUT 
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
};

/*************************************************************************************/
/*								LIGHTS 												 */
/*************************************************************************************/


/***************************************************************************************/
/*							TWEAKABLES												   */
/***************************************************************************************/


/*************************************************************************************/
/*								VERTEX SHADERS										 */
/*************************************************************************************/

VS_OUTPUT vertexShaderCommon(VS_INPUT In) 
{
	VS_OUTPUT Out = (VS_OUTPUT) 0;

	//calculate the transform to be applied - for each axis - to the bone under consideration 
	float4x4 skinTransform = 0;
	skinTransform += Bones[In.BoneIndices.x] * In.BoneWeights.x;
	skinTransform += Bones[In.BoneIndices.y] * In.BoneWeights.y;
	skinTransform += Bones[In.BoneIndices.z] * In.BoneWeights.z;
	skinTransform += Bones[In.BoneIndices.w] * In.BoneWeights.w;
	
	
	//get wsp for view vector
	float4 worldSpacePos = mul(In.Position, skinTransform);

	//standard wvp
	Out.Position = mul( mul(worldSpacePos,View),Projection);

	//texture mapping coords
	Out.TexCoord = In.TexCoord;
	return Out;
}
/*************************************************************************************/
/*								PIXEL SHADERS										 */
/*************************************************************************************/

//reads color from a map and paints on surface
float4 pixelShaderSimple(VS_OUTPUT In) : COLOR0
{
	float4 Color = tex2D(DiffuseMapSampler, In.TexCoord);
	Color.a *= Alpha;
	return Color;
}

/*************************************************************************************/
/*								TECHNIQUES											 */
/*************************************************************************************/
technique SimpleTexture
{
	pass One
	{
		VertexShader = compile vs_2_0 vertexShaderCommon();
		PixelShader = compile ps_2_0 pixelShaderSimple();
	}
}
