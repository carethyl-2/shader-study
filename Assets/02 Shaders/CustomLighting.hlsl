#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
#pragma multi_compile _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
#pragma multi_compile _ _CLUSTER_LIGHT_LOOP
#pragma multi_compile _ _LIGHTMAP_SHADOW_MIXING

#ifndef SHADERGRAPH_PREVIEW
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
    #if (SHADERPASS != SHADERPASS_FORWARD)
        #undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
    #endif
#endif

struct CustomLightingData
{
    // Position and Orientation
    float3 positionWorldSpace;
    float3 normalWorldSpace;
    float3 viewDirectionWorldSpace;
    float4 shadowCoordinate;

    // Surface properties
    float3 albedo;
    float smoothness;
    float ambientOcclusion;

    // Baked Lighting
    float3 bakedGI;
};

float GetSmoothnessPower(float _rawSmoothness)
{
    return exp2(10 * _rawSmoothness + 1);
}

#ifndef SHADERGRAPH_PREVIEW
float3 CustomGlobalIllumination(CustomLightingData _data)
{
    float3 indirectDiffuse = _data.albedo * _data.bakedGI * _data.ambientOcclusion;
    return indirectDiffuse;
}

float3 CustomLightHandling(CustomLightingData _data, Light _light)
{
    // Light base color
    float3 radiance = _light.color * (_light.distanceAttenuation * _light.shadowAttenuation);

    // Diffuse
    float diffuse = saturate(dot(_data.normalWorldSpace, _light.direction));

    // Specular
    float specularDot = saturate(dot(_data.normalWorldSpace, normalize(_light.direction + _data.viewDirectionWorldSpace)));
    float specular = pow(specularDot, GetSmoothnessPower(_data.smoothness))* diffuse;


    // Combined light outputs
    float3 color = _data.albedo * radiance * (diffuse + specular);
    return color;
}
#endif

float3 CustomLighting(CustomLightingData _data)
{
    // Shader graph preview estimate
#ifdef SHADERGRAPH_PREVIEW
    float3 lightDirection = float3(0.5, 0.5, 0);
    float intensity = saturate(dot(_data.normalWorldSpace, lightDirection)) + 
        pow(saturate(dot(_data.normalWorldSpace, normalize(_data.viewDirectionWorldSpace + lightDirection))), GetSmoothnessPower(_data.smoothness));
    return _data.albedo * intensity;
#else

    // Core functionality

    // Get main light
    Light mainLight = GetMainLight(_data.shadowCoordinate, _data.positionWorldSpace, 1);


    // Initialize Color with global illumination
    //MixRealtimeAndBakedGI(mainLight, _data.normalWorldSpace. _data.bakedGI);
    //float3 color = CustomGlobalIllumination(_data);
    float3 color = 0;
    
    // Shade the main light
    color += CustomLightHandling(_data, mainLight);

    #ifdef _ADDITIONAL_LIGHTS    
        // Shade all additional lights
        uint numAdditionalLights = GetAdditionalLightsCount();
        for (uint i = 0; i < numAdditionalLights; i++)
        {
            Light light = GetAdditionalLight(i, _data.positionWorldSpace, 1);
            color += CustomLightHandling(_data, light);
        }
    #endif

    // Return final color
    return color;
#endif
}

void CustomLighting_float(float3 PositionWS, float3 NormalWS, float3 ViewDirWS, float3 Albedo, float Smoothness, float AmbientOcclusion, float2 LightMapUV, out float3 Color)
{
    CustomLightingData data;

    data.positionWorldSpace = PositionWS;
    data.normalWorldSpace = NormalWS;
    data.viewDirectionWorldSpace = ViewDirWS;
    data.albedo = Albedo;
    data.smoothness = Smoothness;
    data.ambientOcclusion = AmbientOcclusion;

// Preview
#ifdef SHADERGRAPH_PREVIEW
    data.shadowCoordinate = 0;
    data.bakedGI = 0;

// Not preview
#else
    float4 positionCS = TransformWorldToHClip(PositionWS);
    #if SHADOWS_SCREEN
        data.shadowCoordinate = ComputeScreenPos(positionCS);
    #else
        data.shadowCoordinate = TransformWorldToShadowCoord(PositionWS);
    #endif



    float3 lightMapUV;
    OUTPUT_LIGHTMAP_UV(LightMapUV, unity_LightmapST, lightMapUV);
    
    float3 vertexSH;
    OUTPUT_SH(NormalWS, vertexSH);

    data.bakedGI = SAMPLE_GI(lightMapUV, vertexSH, NormalWS);
#endif

    Color = CustomLighting(data);
}
#endif // CUSTOM_LIGHTING_INCLUDED