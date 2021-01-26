// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:Sprites/Default,iptp:1,cusa:True,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:True,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1873,x:33730,y:32927,cmnt:Created By Mike Jakubowski,varname:node_1873,prsc:2|emission-9613-OUT,alpha-4805-A;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32620,y:32963,ptovrint:False,ptlb:MainTex,ptin:_MainTex,cmnt:Input Texture,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:8041,x:32620,y:33140,cmnt:Color from Sprite Renderer,varname:node_8041,prsc:2;n:type:ShaderForge.SFN_RgbToHsv,id:3046,x:32782,y:32963,varname:node_3046,prsc:2|IN-4805-RGB;n:type:ShaderForge.SFN_Slider,id:1860,x:32620,y:33310,ptovrint:False,ptlb:Saturation Bound,ptin:_SaturationBound,cmnt:Saturation bound to skip gray pixels,varname:node_1860,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1,max:1;n:type:ShaderForge.SFN_Code,id:9613,x:33200,y:33027,varname:node_9613,prsc:2,code:aQBmACAAKABzAGEAdAB1AHIAYQB0AGkAbwBuACAAPgAgAHMAYQB0AHUAcgBhAHQAaQBvAG4AQgBvAHUAbgBkACAAJgAmACAAIQBpAG4AdgBlAHIAcwBlACkAIAByAGUAdAB1AHIAbgAgAHAAaQB4AGUAbAA7AA0ACgBpAGYAIAAoAHMAYQB0AHUAcgBhAHQAaQBvAG4AIAA8ACAAcwBhAHQAdQByAGEAdABpAG8AbgBCAG8AdQBuAGQAIAAmACYAIABpAG4AdgBlAHIAcwBlACkAIAByAGUAdAB1AHIAbgAgAHAAaQB4AGUAbAA7AA0ACgBmAGwAbwBhAHQAIABnAHIAYQB5ACAAPQAgADAALgAzACAAKgAgAHAAaQB4AGUAbAAuAHIAIAArACAAMAAuADUAOQAgACoAIABwAGkAeABlAGwALgBnACAAKwAgADAALgAxADEAIAAqACAAcABpAHgAZQBsAC4AYgA7AA0ACgByAGUAdAB1AHIAbgAgAGMAbwBsAG8AcgBNAHUAbAB0AGkAcABsAGkAZQByACAAKgAgAGMAbwBsAG8AcgAgACoAIABnAHIAYQB5ADsA,output:2,fname:ToGrayscale,width:461,height:168,input:2,input:2,input:0,input:0,input:0,input:4,input_1_label:pixel,input_2_label:color,input_3_label:saturation,input_4_label:saturationBound,input_5_label:colorMultiplier,input_6_label:inverse|A-4805-RGB,B-8041-RGB,C-3046-SOUT,D-1860-OUT,E-7569-OUT,F-6479-OUT;n:type:ShaderForge.SFN_Slider,id:7569,x:32620,y:33400,ptovrint:False,ptlb:Color Multiplier,ptin:_ColorMultiplier,varname:node_7569,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:2;n:type:ShaderForge.SFN_ToggleProperty,id:6479,x:32777,y:33490,ptovrint:False,ptlb:Inverse,ptin:_Inverse,varname:node_6479,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;proporder:4805-1860-7569-6479;pass:END;sub:END;*/

Shader "Hero Editor/Gray Paint" {
    Properties {
        [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
        _SaturationBound ("Saturation Bound", Range(0, 1)) = 0.1
        _ColorMultiplier ("Color Multiplier", Range(1, 2)) = 1
        [MaterialToggle] _Inverse ("Inverse", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

		// these six unused properties are required when a shader is used in the UI system, or you get a warning.
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            ZTest [unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            
            Stencil {
                Ref [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilOp]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _SaturationBound;
            float3 ToGrayscale( float3 pixel , float3 color , float saturation , float saturationBound , float colorMultiplier , half inverse ){
            if (saturation > saturationBound && !inverse) return pixel;
            if (saturation < saturationBound && inverse) return pixel;
            float gray = 0.3 * pixel.r + 0.59 * pixel.g + 0.11 * pixel.b;
            return colorMultiplier * color * gray;
            }
            
            uniform float _ColorMultiplier;
            uniform fixed _Inverse;
            float4 _ClipRect;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;               
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                float4 worldPosition : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.worldPosition = v.vertex;
                o.pos = UnityObjectToClipPos( o.worldPosition );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex)); // Input Texture
                float4 node_3046_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_3046_p = lerp(float4(float4(_MainTex_var.rgb,0.0).zy, node_3046_k.wz), float4(float4(_MainTex_var.rgb,0.0).yz, node_3046_k.xy), step(float4(_MainTex_var.rgb,0.0).z, float4(_MainTex_var.rgb,0.0).y));
                float4 node_3046_q = lerp(float4(node_3046_p.xyw, float4(_MainTex_var.rgb,0.0).x), float4(float4(_MainTex_var.rgb,0.0).x, node_3046_p.yzx), step(node_3046_p.x, float4(_MainTex_var.rgb,0.0).x));
                float node_3046_d = node_3046_q.x - min(node_3046_q.w, node_3046_q.y);
                float node_3046_e = 1.0e-10;
                float3 node_3046 = float3(abs(node_3046_q.z + (node_3046_q.w - node_3046_q.y) / (6.0 * node_3046_d + node_3046_e)), node_3046_d / (node_3046_q.x + node_3046_e), node_3046_q.x);;
                float3 node_9613 = ToGrayscale( _MainTex_var.rgb , i.vertexColor.rgb , node_3046.g , _SaturationBound , _ColorMultiplier , _Inverse );
                float3 emissive = node_9613;
                float3 finalColor = emissive;
				_MainTex_var.a -= 1 - i.vertexColor.a;

                #ifdef UNITY_UI_CLIP_RECT
                finalColor.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (finalColor.a - 0.001);
                #endif

                return fixed4(finalColor,_MainTex_var.a);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off

			Stencil {
                Ref [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilOp]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 2.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
				float4 worldPosition : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
				o.worldPosition = v.vertex;
                o.pos = UnityObjectToClipPos( o.worldPosition );                
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
    //CustomEditor "ShaderForgeMaterialInspector"
}
