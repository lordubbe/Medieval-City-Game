// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:Particles/Alpha Blended Premultiply,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32724,y:32693,varname:node_4795,prsc:2|emission-1351-OUT,alpha-6048-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32007,y:32351,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6972-UVOUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32235,y:32772,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1351,x:32629,y:32353,varname:node_1351,prsc:2|A-5569-OUT,B-2053-RGB,C-8674-OUT,D-2053-R,E-2601-RGB;n:type:ShaderForge.SFN_Tex2d,id:9779,x:31396,y:32931,ptovrint:False,ptlb:TEX2,ptin:_TEX2,varname:node_9779,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5795-OUT;n:type:ShaderForge.SFN_Multiply,id:3598,x:32562,y:32221,varname:node_3598,prsc:2|A-6251-OUT,B-8674-OUT,C-2053-A;n:type:ShaderForge.SFN_Desaturate,id:6251,x:32328,y:32140,cmnt:Alpha,varname:node_6251,prsc:2|COL-9005-OUT;n:type:ShaderForge.SFN_Relay,id:2422,x:33028,y:32236,cmnt:ALPHA CHANNEL,varname:node_2422,prsc:2|IN-7049-OUT;n:type:ShaderForge.SFN_Relay,id:9208,x:33106,y:33235,varname:node_9208,prsc:2|IN-2422-OUT;n:type:ShaderForge.SFN_Relay,id:6048,x:32587,y:33242,varname:node_6048,prsc:2|IN-9208-OUT;n:type:ShaderForge.SFN_TexCoord,id:437,x:29699,y:32838,cmnt:z  random 0-1,varname:node_437,prsc:2,uv:0,uaff:True;n:type:ShaderForge.SFN_RemapRange,id:5054,x:29699,y:32999,cmnt:Random -1 to 1,varname:node_5054,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-437-Z;n:type:ShaderForge.SFN_Add,id:8707,x:30035,y:32843,cmnt:UV offset,varname:node_8707,prsc:2|A-437-UVOUT,B-5054-OUT;n:type:ShaderForge.SFN_Multiply,id:5013,x:30203,y:32843,cmnt:UV scale,varname:node_5013,prsc:2|A-8707-OUT,B-7477-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7477,x:30203,y:32974,ptovrint:False,ptlb:TEX2 Scale,ptin:_TEX2Scale,varname:node_7477,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Rotator,id:6408,x:30374,y:32843,varname:node_6408,prsc:2|UVIN-5013-OUT,ANG-8389-OUT;n:type:ShaderForge.SFN_Relay,id:6456,x:30041,y:33064,varname:node_6456,prsc:2|IN-5054-OUT;n:type:ShaderForge.SFN_Panner,id:9135,x:30916,y:32873,varname:node_9135,prsc:2,spu:0,spv:1|UVIN-293-OUT,DIST-9910-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9157,x:30540,y:32908,ptovrint:False,ptlb:TEX 2 U_Speed,ptin:_TEX2U_Speed,varname:node_9157,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:9910,x:30765,y:32893,varname:node_9910,prsc:2|A-9157-OUT,B-4601-T;n:type:ShaderForge.SFN_Time,id:4601,x:30540,y:32952,varname:node_4601,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:8952,x:30540,y:33078,ptovrint:False,ptlb:TEX 2 V_Speed,ptin:_TEX2V_Speed,varname:node_8952,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:7224,x:30765,y:33008,varname:node_7224,prsc:2|A-8952-OUT,B-4601-T;n:type:ShaderForge.SFN_Panner,id:4221,x:30916,y:33008,varname:node_4221,prsc:2,spu:1,spv:0|UVIN-293-OUT,DIST-7224-OUT;n:type:ShaderForge.SFN_ComponentMask,id:4742,x:31089,y:32873,varname:node_4742,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-9135-UVOUT;n:type:ShaderForge.SFN_Append,id:5795,x:31236,y:32931,varname:node_5795,prsc:2|A-4742-OUT,B-9869-OUT;n:type:ShaderForge.SFN_ComponentMask,id:9869,x:31089,y:33008,varname:node_9869,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-4221-UVOUT;n:type:ShaderForge.SFN_Relay,id:293,x:30569,y:32843,cmnt:UV coords goes here,varname:node_293,prsc:2|IN-6408-UVOUT;n:type:ShaderForge.SFN_Power,id:5805,x:32219,y:32582,varname:node_5805,prsc:2|VAL-4147-OUT,EXP-4091-OUT;n:type:ShaderForge.SFN_Slider,id:4091,x:31889,y:32776,ptovrint:False,ptlb:Power,ptin:_Power,varname:node_4091,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.6834477,max:20;n:type:ShaderForge.SFN_ComponentMask,id:8674,x:32417,y:32599,varname:node_8674,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-7145-OUT;n:type:ShaderForge.SFN_Tex2d,id:226,x:31403,y:33251,ptovrint:False,ptlb:TEX3,ptin:_TEX3,varname:node_226,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-2735-OUT;n:type:ShaderForge.SFN_Multiply,id:4147,x:32033,y:32582,varname:node_4147,prsc:2|A-8299-OUT,B-5065-OUT,C-2261-OUT,D-2053-A;n:type:ShaderForge.SFN_Vector1,id:2261,x:32060,y:32696,varname:node_2261,prsc:2,v1:2;n:type:ShaderForge.SFN_Clamp01,id:7145,x:32334,y:32418,varname:node_7145,prsc:2|IN-5805-OUT;n:type:ShaderForge.SFN_Relay,id:7725,x:30569,y:33187,cmnt:UV coords goes here,varname:node_7725,prsc:2|IN-4263-UVOUT;n:type:ShaderForge.SFN_Add,id:7772,x:30041,y:33188,cmnt:UV offset,varname:node_7772,prsc:2|A-437-UVOUT,B-2816-OUT;n:type:ShaderForge.SFN_Multiply,id:2811,x:30209,y:33188,cmnt:UV scale,varname:node_2811,prsc:2|A-7772-OUT,B-3430-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3430,x:30209,y:33319,ptovrint:False,ptlb:TEX3 Scale,ptin:_TEX3Scale,varname:_TEX2Scale_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Rotator,id:4263,x:30380,y:33188,varname:node_4263,prsc:2|UVIN-2811-OUT,ANG-5403-OUT;n:type:ShaderForge.SFN_Relay,id:8225,x:30041,y:33389,varname:node_8225,prsc:2|IN-2816-OUT;n:type:ShaderForge.SFN_RemapRange,id:2816,x:29699,y:33172,cmnt:Random -1 to 1,varname:node_2816,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-437-W;n:type:ShaderForge.SFN_Panner,id:3373,x:30916,y:33196,varname:node_3373,prsc:2,spu:0,spv:1|UVIN-7725-OUT,DIST-4657-OUT;n:type:ShaderForge.SFN_Panner,id:274,x:30916,y:33350,varname:node_274,prsc:2,spu:1,spv:0|UVIN-7725-OUT,DIST-8515-OUT;n:type:ShaderForge.SFN_Multiply,id:4657,x:30751,y:33218,varname:node_4657,prsc:2|A-5504-OUT,B-9542-T;n:type:ShaderForge.SFN_Multiply,id:8515,x:30751,y:33360,varname:node_8515,prsc:2|A-5063-OUT,B-9542-T;n:type:ShaderForge.SFN_Time,id:9542,x:30539,y:33296,varname:node_9542,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:5063,x:30539,y:33420,ptovrint:False,ptlb:TEX3 V_Speed,ptin:_TEX3V_Speed,varname:node_5063,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.03;n:type:ShaderForge.SFN_ValueProperty,id:5504,x:30539,y:33254,ptovrint:False,ptlb:TEX3 U_Speed,ptin:_TEX3U_Speed,varname:node_5504,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.03;n:type:ShaderForge.SFN_ComponentMask,id:9595,x:31098,y:33193,varname:node_9595,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-3373-UVOUT;n:type:ShaderForge.SFN_Append,id:2735,x:31245,y:33251,varname:node_2735,prsc:2|A-9595-OUT,B-8441-OUT;n:type:ShaderForge.SFN_ComponentMask,id:8441,x:31098,y:33328,varname:node_8441,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-274-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:457,x:31243,y:32340,varname:node_457,prsc:2,uv:0,uaff:True;n:type:ShaderForge.SFN_Rotator,id:6972,x:31467,y:32340,varname:node_6972,prsc:2|UVIN-457-UVOUT,ANG-1726-OUT,SPD-5931-OUT;n:type:ShaderForge.SFN_RemapRange,id:1726,x:31243,y:32491,varname:node_1726,prsc:2,frmn:0,frmx:1,tomn:0,tomx:360|IN-457-Z;n:type:ShaderForge.SFN_RemapRange,id:5403,x:30380,y:33319,varname:node_5403,prsc:2,frmn:0,frmx:1,tomn:0,tomx:360|IN-8225-OUT;n:type:ShaderForge.SFN_RemapRange,id:8389,x:30380,y:32994,varname:node_8389,prsc:2,frmn:0,frmx:1,tomn:0,tomx:360|IN-6456-OUT;n:type:ShaderForge.SFN_Tex2d,id:2601,x:32079,y:32950,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_2601,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b1d7fee26e54cc3498f6267f072a45b9,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:9005,x:32159,y:32140,varname:node_9005,prsc:2|A-5569-OUT,B-2601-RGB;n:type:ShaderForge.SFN_Clamp01,id:7049,x:32805,y:32236,varname:node_7049,prsc:2|IN-3598-OUT;n:type:ShaderForge.SFN_Multiply,id:5569,x:32209,y:32315,varname:node_5569,prsc:2|A-6074-RGB,B-5805-OUT,C-2826-OUT;n:type:ShaderForge.SFN_Slider,id:2826,x:31981,y:32056,ptovrint:False,ptlb:Strength,ptin:_Strength,varname:node_2826,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:20;n:type:ShaderForge.SFN_ToggleProperty,id:2513,x:31243,y:32296,ptovrint:False,ptlb:MainTex Random Rotation,ptin:_MainTexRandomRotation,cmnt:Rotate the Main Texture?,varname:node_2513,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True;n:type:ShaderForge.SFN_Multiply,id:5931,x:31432,y:32491,varname:node_5931,prsc:2|A-2513-OUT,B-1726-OUT;n:type:ShaderForge.SFN_Slider,id:8596,x:31476,y:32798,ptovrint:False,ptlb:TEX2 Influence,ptin:_TEX2Influence,varname:node_8596,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Lerp,id:8299,x:31647,y:32652,varname:node_8299,prsc:2|A-9514-RGB,B-9779-RGB,T-8596-OUT;n:type:ShaderForge.SFN_SceneColor,id:9514,x:31476,y:32652,cmnt:How much does this texture influence?,varname:node_9514,prsc:2;n:type:ShaderForge.SFN_Slider,id:6756,x:31643,y:33244,ptovrint:False,ptlb:TEX3 Influence,ptin:_TEX3Influence,varname:_TEX2Influence_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Lerp,id:5065,x:31814,y:33098,varname:node_5065,prsc:2|A-8420-RGB,B-226-RGB,T-6756-OUT;n:type:ShaderForge.SFN_SceneColor,id:8420,x:31643,y:33098,cmnt:How much does this texture influence?,varname:node_8420,prsc:2;proporder:6074-2513-9779-7477-9157-8952-8596-226-3430-5504-5063-6756-2601-4091-2826;pass:END;sub:END;*/

Shader "Particles/Jacob's Ultimate Particle Shader" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        [MaterialToggle] _MainTexRandomRotation ("MainTex Random Rotation", Float ) = 1
        _TEX2 ("TEX2", 2D) = "white" {}
        _TEX2Scale ("TEX2 Scale", Float ) = 1
        _TEX2U_Speed ("TEX 2 U_Speed", Float ) = 0.1
        _TEX2V_Speed ("TEX 2 V_Speed", Float ) = 0.1
        _TEX2Influence ("TEX2 Influence", Range(0, 1)) = 1
        _TEX3 ("TEX3", 2D) = "white" {}
        _TEX3Scale ("TEX3 Scale", Float ) = 0.5
        _TEX3U_Speed ("TEX3 U_Speed", Float ) = -0.03
        _TEX3V_Speed ("TEX3 V_Speed", Float ) = -0.03
        _TEX3Influence ("TEX3 Influence", Range(0, 1)) = 1
        _Mask ("Mask", 2D) = "white" {}
        _Power ("Power", Range(0, 20)) = 0.6834477
        _Strength ("Strength", Range(0, 20)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _TEX2; uniform float4 _TEX2_ST;
            uniform float _TEX2Scale;
            uniform float _TEX2U_Speed;
            uniform float _TEX2V_Speed;
            uniform float _Power;
            uniform sampler2D _TEX3; uniform float4 _TEX3_ST;
            uniform float _TEX3Scale;
            uniform float _TEX3V_Speed;
            uniform float _TEX3U_Speed;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _Strength;
            uniform fixed _MainTexRandomRotation;
            uniform float _TEX2Influence;
            uniform float _TEX3Influence;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float node_1726 = (i.uv0.b*360.0+0.0);
                float node_6972_ang = node_1726;
                float node_6972_spd = (_MainTexRandomRotation*node_1726);
                float node_6972_cos = cos(node_6972_spd*node_6972_ang);
                float node_6972_sin = sin(node_6972_spd*node_6972_ang);
                float2 node_6972_piv = float2(0.5,0.5);
                float2 node_6972 = (mul(i.uv0-node_6972_piv,float2x2( node_6972_cos, -node_6972_sin, node_6972_sin, node_6972_cos))+node_6972_piv);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_6972, _MainTex));
                float4 node_4601 = _Time + _TimeEditor;
                float node_5054 = (i.uv0.b*2.0+-1.0); // Random -1 to 1
                float node_6408_ang = (node_5054*360.0+0.0);
                float node_6408_spd = 1.0;
                float node_6408_cos = cos(node_6408_spd*node_6408_ang);
                float node_6408_sin = sin(node_6408_spd*node_6408_ang);
                float2 node_6408_piv = float2(0.5,0.5);
                float2 node_6408 = (mul(((i.uv0+node_5054)*_TEX2Scale)-node_6408_piv,float2x2( node_6408_cos, -node_6408_sin, node_6408_sin, node_6408_cos))+node_6408_piv);
                float2 node_293 = node_6408; // UV coords goes here
                float2 node_5795 = float2((node_293+(_TEX2U_Speed*node_4601.g)*float2(0,1)).g,(node_293+(_TEX2V_Speed*node_4601.g)*float2(1,0)).r);
                float4 _TEX2_var = tex2D(_TEX2,TRANSFORM_TEX(node_5795, _TEX2));
                float4 node_9542 = _Time + _TimeEditor;
                float node_2816 = (i.uv0.a*2.0+-1.0); // Random -1 to 1
                float node_8225 = node_2816;
                float node_4263_ang = (node_8225*360.0+0.0);
                float node_4263_spd = 1.0;
                float node_4263_cos = cos(node_4263_spd*node_4263_ang);
                float node_4263_sin = sin(node_4263_spd*node_4263_ang);
                float2 node_4263_piv = float2(0.5,0.5);
                float2 node_4263 = (mul(((i.uv0+node_2816)*_TEX3Scale)-node_4263_piv,float2x2( node_4263_cos, -node_4263_sin, node_4263_sin, node_4263_cos))+node_4263_piv);
                float2 node_7725 = node_4263; // UV coords goes here
                float2 node_2735 = float2((node_7725+(_TEX3U_Speed*node_9542.g)*float2(0,1)).g,(node_7725+(_TEX3V_Speed*node_9542.g)*float2(1,0)).r);
                float4 _TEX3_var = tex2D(_TEX3,TRANSFORM_TEX(node_2735, _TEX3));
                float3 node_4147 = (lerp(sceneColor.rgb,_TEX2_var.rgb,_TEX2Influence)*lerp(sceneColor.rgb,_TEX3_var.rgb,_TEX3Influence)*2.0*i.vertexColor.a);
                float3 node_5805 = pow(node_4147,_Power);
                float3 node_5569 = (_MainTex_var.rgb*node_5805*_Strength);
                float node_8674 = saturate(node_5805).r;
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float3 node_1351 = (node_5569*i.vertexColor.rgb*node_8674*i.vertexColor.r*_Mask_var.rgb);
                float3 emissive = node_1351;
                float3 finalColor = emissive;
                float node_6048 = saturate((dot((node_5569*_Mask_var.rgb),float3(0.3,0.59,0.11))*node_8674*i.vertexColor.a));
                fixed4 finalRGBA = fixed4(finalColor,node_6048);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
