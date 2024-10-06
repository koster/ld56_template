using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[ExecuteInEditMode]
[AddComponentMenu("UI/Mask Graphic Inverter", 11)]
[RequireComponent(typeof(MaskableGraphic))]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class MaskGraphicInverter : MonoBehaviour, IMaterialModifier
{
    [NonSerialized]
    private Graphic m_Graphic;

    [NonSerialized]
    private Material m_OriginalMaterial;

    [NonSerialized]
    private Material m_CustomMaterial;

    public Graphic graphic
    {
        get { return this.m_Graphic ?? (this.m_Graphic = this.GetComponent<Graphic>()); }
    }

    public Material GetModifiedMaterial(Material baseMaterial)
    {
        if (!this.isActiveAndEnabled)
            return baseMaterial;

        if (m_OriginalMaterial == baseMaterial && m_CustomMaterial != null)
        {
            return m_CustomMaterial;
        }

        m_OriginalMaterial = baseMaterial;
        m_CustomMaterial = new Material(baseMaterial);
        m_CustomMaterial.SetInt("_StencilComp", (int)CompareFunction.NotEqual);

        return m_CustomMaterial;
    }

    protected void OnEnable()
    {
        if ((Object)this.graphic != (Object)null)
        {
            this.graphic.canvasRenderer.hasPopInstruction = true;
            this.graphic.SetMaterialDirty();
        }

        MaskUtilities.NotifyStencilStateChanged((Component)this);
    }

    protected void OnDisable()
    {
        if ((Object)this.graphic != (Object)null)
        {
            this.graphic.SetMaterialDirty();
            this.graphic.canvasRenderer.hasPopInstruction = false;
            this.graphic.canvasRenderer.popMaterialCount = 0;
        }

        m_OriginalMaterial = (Material)null;
        m_CustomMaterial = (Material)null;
        MaskUtilities.NotifyStencilStateChanged((Component)this);
    }

    protected void OnValidate()
    {
        if (!this.isActiveAndEnabled)
            return;
        if ((Object)this.graphic != (Object)null)
            this.graphic.SetMaterialDirty();
        MaskUtilities.NotifyStencilStateChanged((Component)this);
    }
}