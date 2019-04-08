////////////////////////////////////////////////////////////////////////////////////////////////////
//
// エッジ用ピクセルシェーダー
//
////////////////////////////////////////////////////////////////////////////////////////////////////

#include "MaterialTexture.hlsli"
#include "DefaultVS_OUTPUT.hlsli"
#include "GlobalParameters.hlsli"


float4 main(VS_OUTPUT IN) : SV_TARGET
{
    return EdgeColor;
}
