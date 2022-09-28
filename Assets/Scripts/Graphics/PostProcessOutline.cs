using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.BeforeStack, "Roystan/Post Process Outline")]
public sealed class PostProcessOutline : PostProcessEffectSettings {
    public IntParameter scale = new IntParameter { value = 1 };
    public ColorParameter color = new ColorParameter { value = Color.white };
    [Range(0f, 1f)]
    public FloatParameter normalThreshold = new FloatParameter { value = 0.5f };
    // [Range(0f, 1f)]
    // public FloatParameter depthThreshold = new FloatParameter { value = 0.5f };
    [Range(0f, 1f)]
    public FloatParameter depthNormalThreshold = new FloatParameter { value = 0.5f };
    public FloatParameter depthNormalThresholdScale = new FloatParameter { value = 0.5f };
}

public sealed class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutline> {

    public float CalculateDepthThreshold() {
        return 0.161f;
        //return 1 - (float)Screen.currentResolution.width / 10000f;
    }

    public override void Render(PostProcessRenderContext context) {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Roystan/Outline Post Process"));
        sheet.properties.SetFloat("_Scale", settings.scale);
        sheet.properties.SetFloat("_NormalThreshold", settings.normalThreshold);
        sheet.properties.SetFloat("_DepthThreshold", CalculateDepthThreshold());
        sheet.properties.SetFloat("_DepthNormalThreshold", settings.depthNormalThreshold);
        sheet.properties.SetFloat("_DepthNormalThresholdScale", settings.depthNormalThresholdScale);

        Matrix4x4 clipToView = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, true).inverse;
        sheet.properties.SetMatrix("_ClipToView", clipToView);

        // sheet.properties.SetFloat("_Time", Time.time);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}