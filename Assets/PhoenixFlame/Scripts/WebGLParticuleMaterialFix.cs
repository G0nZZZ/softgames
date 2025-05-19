using UnityEngine;

/// <summary>
/// Swaps in a WebGL-safe particle material at runtime without touching existing shaders.
/// If `webGLMaterial` is not assigned, it falls back to Unity's built-in "Particles/Standard Unlit" shader.
/// Attach to any Particle System GameObject.
/// </summary>
[RequireComponent(typeof(ParticleSystemRenderer))]
public class WebGLParticleMaterialFix : MonoBehaviour
{
    [Tooltip("Optional WebGL-safe material (e.g., using 'Particles/Alpha Blended' shader). If empty, uses built-in unlit particle shader.")]
    [SerializeField] private Material webGLMaterial;

    private ParticleSystemRenderer _renderer;
    private static Material _fallback;

    private void Awake()
    {
        _renderer = GetComponent<ParticleSystemRenderer>();
        // Prepare fallback once
        if (_fallback == null)
        {
            var shader = Shader.Find("Particles/Standard Unlit");
            if (shader != null)
                _fallback = new Material(shader);
        }
    }

    private void Start()
    {
#if UNITY_WEBGL
        if (_renderer != null)
        {
            if (webGLMaterial != null)
            {
                _renderer.material = webGLMaterial;
            }
            else if (_fallback != null)
            {
                _renderer.material = _fallback;
            }
        }
#endif
    }
}