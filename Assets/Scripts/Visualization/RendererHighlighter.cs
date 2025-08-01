using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RendererHighlighter : MonoBehaviour
{
    private const string EMISSION_COLOR = "_EmissionColor";
    private const string EMISSION_KEYWORD = "_EMISSION";

    private readonly Dictionary<Renderer, Color[]> _originalEmissionColors = new();

    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private Color _highlightEmissionColor = new(0.25f, 0.25f, 0.25f);
    private MaterialPropertyBlock _materialPropertyBlock;
    private int _materialCount;

    public readonly ReactiveVariable<bool> Highlighted = new();

    private void Awake()
    {
        Highlighted.Changed += (_, highlight) => Highlight(highlight);
        _materialPropertyBlock = new();

        foreach (Renderer renderer in _renderers)
        {
            Material[] materials = renderer.materials;
            _materialCount = materials.Length;
            _originalEmissionColors.Add(renderer, new Color[_materialCount]);

            for (int i = 0; i < _materialCount; i++)
            {
                Material material = materials[i];
                if (material.shaderKeywords.Any(k => k == EMISSION_KEYWORD))
                    _originalEmissionColors[renderer][i] = material.GetColor(EMISSION_COLOR);
                else
                {
                    material.EnableKeyword(EMISSION_KEYWORD);
                    _originalEmissionColors[renderer][i] = Color.black;
                }
            }
        }
    }

    public void Highlight(bool highlight)
    {   
        foreach (Renderer renderer in _renderers)
        {
            for (int i = 0; i < _materialCount; i++)
            {
                renderer.GetPropertyBlock(_materialPropertyBlock, i);
                Color emissionColor = highlight ? _highlightEmissionColor : _originalEmissionColors[renderer][i];
                _materialPropertyBlock.SetColor("_EmissionColor", emissionColor);
                renderer.SetPropertyBlock(_materialPropertyBlock, i);
            }
        }
    }
}