Shader "Custom/MyDissolve" 
{ 
    Properties 
    { 
        _MainTex("Main Texture", 2D) = "white"{} 
        _Color ("Color", Color) = (1,1,1,1) 
        _Glossiness ("Smoothness", Range(0,1)) = 0.5 
        _Metallic ("Metallic", Range(0,1)) = 0.0 
        _SimpNoise ("Simple Noise (Noise Map for Dissolve)", 2D) = "gray" {} 
        // _Speed("Speed", Range(0,3)) = 1 
        _EmissionColor("Emission Color", Color) = (0,0,0,0) 
        _ControllerA("Emission Map Controller A", Range(0,1)) = 1 
        _ControllerB("Emission Map Controller B", Range(0,1)) = 0.95 
        _RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0) 
        _RimPower("Rim Power", Range(0.5,8.0)) = 3.0 
 
    } 
    SubShader 
    { 
        Tags {"RenderType"="Opaque" } 
        LOD 200 
        //Blend SrcAlpha OneMinusSrcAlpha 
 
        CGPROGRAM 
        // Physically based Standard lighting model, and enable shadows on all light types 
        #pragma surface surf Standard fullforwardshadows 
 
        // Use shader model 3.0 target, to get nicer looking lighting 
        #pragma target 3.0 
 
        sampler2D _SimpNoise; 
        sampler2D _MainTex; 
        half _Glossiness; 
        half _Metallic; 
        fixed4 _Color; 
        // float _Speed; 
        float4 _EmissionColor; 
        float4 _RimColor; 
        float _RimPower; 
        float _ControllerA; 
        float _ControllerB; 
   
        // Time variable later set by an attached c# script to control the step value of dissolve noise pattern 
        float _tConstant;
 
   
 
        struct Input 
        { 
           float2 uv_SimpNoise; 
           float2 uv_MainTex; 
           float3 viewDir; 
        }; 
 
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader. 
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing. 
        // #pragma instancing_options assumeuniformscaling 
        UNITY_INSTANCING_BUFFER_START(Props) 
         
        // put more per-instance properties here 
        UNITY_INSTANCING_BUFFER_END(Props) 
 
        void surf (Input IN, inout SurfaceOutputStandard o) 
        { 
           fixed noise = tex2D(_SimpNoise, IN.uv_SimpNoise); 
    
           // Step of noise map and our time constant that gives the dissolve effect 
           fixed noiseStep = step(noise, _tConstant); 
    
    
 
           fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color; 
           o.Albedo = c.rgb; 
           o.Metallic = _Metallic; 
           o.Smoothness = _Glossiness; 
 
           // Controllers to control the amount of emission on the game object during dissolve 
           fixed a = _ControllerA * _tConstant; 
           fixed b = _ControllerB * _tConstant; 
 
           // saturate() make sure the return value is 0 <= x <= 1 
           half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)); 
    
           /*  
           o.Emission contain 2 things: 
            1. Rim emission based on the dot() of viewDir and o.Normal 
            2. Subtraction of step() noise and a/b to achieve an outline emission effect  
             as alpha cutoff over time constant, value of a > b is required 
           */ 
           o.Emission = _RimColor.rgb * pow(rim, _RimPower) + (step(noise, a) - step(noise, b)) * _EmissionColor; 
    
           // clip() to simulate alpha cutoff for a opaque object 
           clip(noiseStep - c.rgb); 
        } 
        ENDCG 
    } 
    FallBack "Diffuse" 
}