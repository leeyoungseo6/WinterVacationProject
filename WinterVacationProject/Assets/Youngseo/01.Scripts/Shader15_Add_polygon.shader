Shader "Gondr/Shader15"
{
    Properties
    {
        _AxisColor("Axis Color", Color) = (0.8, 0.8,0.8,1)
        _SweepColor("Sweep Color", Color) = (0.1, 0.3,1.0, 1)
    }
        SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"}
        LOD 100
        //PI, TWO_PI
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;  //위치
                float2 texcoord     : TEXCOORD0;                
            };
            
            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;  //호모지니어스 클립스페이스
                float4 positionOS   : TEXCOORD1;
                float2 uv           : TEXCOORD0;
            };

    
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionOS = IN.positionOS;
                OUT.uv = IN.texcoord;
                return OUT;
            }

            CBUFFER_START(UnityPerMaterial)
            float4 _AxisColor;
            float4 _SweepColor;
            CBUFFER_END

            float sweep(float2 pt, float2 center, float radius, float line_width, float edge_thickness)
            {
                float2 d = pt - center;
                float theta = _Time.z; //2배시간
                float2 p = float2 (cos(theta), -sin(theta)) * radius;  //원위의 점을 구하고 
                float h = clamp( dot(d, p) / dot(p, p), 0.0, 1.0);  //90~ 180까지는 0이 나오고 0 ~90까지는 0~1
                    float l = length(d - p * h);

                //그라디언트를 먹이는 코드
                float gradient = 0.0;
                const float gradient_angle = PI * 0.5; // 45도 각도로

                if(length(d) < radius)
                {
                    float angle = fmod(theta + atan2(d.y, d.x), TWO_PI);
                    gradient = clamp(gradient_angle - angle, 0.0, gradient_angle) / gradient_angle * 0.5;
                }
                return gradient + 1.0 - smoothstep(line_width, line_width + edge_thickness, l);
            }
            

            float circle(float2 pt, float2 center, float radius, float line_width, float edge_thickness)
            {
                pt = pt -center;
                float len = length(pt);
                float result = smoothstep(radius - line_width * 0.5f - edge_thickness, radius - line_width * 0.5, len)
                        - smoothstep(radius + line_width * 0.5f, radius + line_width * 0.5 + edge_thickness, len);

                return result;
            }
            
            
            float onLine(float a, float b, float line_width, float edge_thickness)
            {
                float half_line_width = line_width * 0.5;
                return smoothstep(a - half_line_width - edge_thickness, a - half_line_width, b)
                    - smoothstep(a + half_line_width, a + half_line_width + edge_thickness, b);
            }

            float getDelta(float x)
            {
                return (sin(x) + 1.0) / 2.0;  //0~ 1로 값 리맵
            }

            float polygon(float2 pt, float2 center, float radius, int sides, float rotate, float edge_thick)
            {
                pt = pt - center; //원점이동 시켜주고
                float theta = atan2(pt.y, pt.x) + rotate; // 현재 점이 중심점으로부터 각도가 몇인지
                float rad = PI * 2 / float(sides); //3각형이면 120도씩

                float d = cos(floor(0.5 + theta / rad) * rad - theta) * length(pt);
                return 1.0 - smoothstep(radius, radius + edge_thick, d);
            }
            
            float4 frag(Varyings i) : SV_Target
            {
                float3 color = onLine(i.uv.y, 0.5, 0.002, 0.001) * _AxisColor.rgb;
                color += onLine(i.uv.x, 0.5, 0.002, 0.001) * _AxisColor.rgb;
                float2 center = 0.5;
                color += circle(i.uv, center, 0.3, 0.002, 0.001) * _AxisColor.rgb;
                color += circle(i.uv, center, 0.2, 0.002, 0.001) * _AxisColor.rgb;
                color += circle(i.uv, center, 0.1, 0.002, 0.001) * _AxisColor.rgb;
                color += sweep(i.uv, center, 0.3, 0.003, 0.001) * _SweepColor.rgb;

                color += polygon(i.uv, float2(0.9 - sin(_Time.w)*0.01, 0.5), 0.01, 3, 0, 0.001)* _AxisColor.rgb;
                color += polygon(i.uv, float2(0.1 - sin(_Time.w + PI)*0.01, 0.5), 0.01, 3, PI, 0.001)* _AxisColor.rgb;
                return float4(color, 1.0);
            }
            ENDHLSL
        }
    }
}



