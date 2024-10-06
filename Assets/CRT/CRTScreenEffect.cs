using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CRTScreenEffect : MonoBehaviour
{
    public Shader crtShader;
    private Material crtMaterial;

    [Range(0, 1)] public float scanlineIntensity = 0.5f;
    [Range(0, 1)] public float distortion = 0.1f;
    [Range(0, 1)] public float curvature = 0.2f;
    [Range(0, 10)] public float scanlineSpeed = 1.0f;

    void Start()
    {
        if (crtShader == null)
        {
            Debug.LogError("CRT shader missing.");
            enabled = false;
            return;
        }

        crtMaterial = new Material(crtShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (crtMaterial != null)
        {
            crtMaterial.SetFloat("_ScanlineIntensity", scanlineIntensity);
            crtMaterial.SetFloat("_Distortion", distortion);
            crtMaterial.SetFloat("_Curvature", curvature);
            crtMaterial.SetFloat("_ScanlineSpeed", scanlineSpeed);

            Graphics.Blit(source, destination, crtMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}