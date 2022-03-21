using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcessHandDrawn : MonoBehaviour
{
    public Material EffectMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, EffectMaterial);
    }
}
