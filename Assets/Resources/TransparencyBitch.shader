Shader "Custom/TransparencyBitch" {
	Properties { 
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BlendTex ("Overlay", 2D) = "white" {}
        _Ratio ("Ratio", Float) = 0
    }

	CGINCLUDE

	#include "UnityCG.cginc"

	uniform sampler2D _MainTex;
	uniform sampler2D _BlendTex;
    uniform float _Ratio;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 tex : TEXCOORD0;
	};

	v2f vert(appdata_base v) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.tex = v.texcoord;
		return o;
	}

	float4 frag(v2f i) : COLOR {
		float4 color = tex2D(_MainTex, i.tex);
        float4 color2 = tex2D(_BlendTex, i.tex);
		return float4(color.rgb * (1 - _Ratio) + color2.rgb * (_Ratio), 1);
	}

	ENDCG

	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}
