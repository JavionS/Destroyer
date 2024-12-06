using UnityEngine;
using System.Collections;

public class AutoFade : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    MeshRenderer meshRenderer;
    private Material _originalMaterials;
    private Rigidbody rb;
    private Collider _collider;
    private bool _fading = false;

    private Coroutine _blinkingCoroutine;
    private bool _flashing = false;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        _originalMaterials = meshRenderer.material;
        
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<MeshCollider>();
        StartCoroutine(Fade());
        
    }

    void Update()
    {
        if (_fading)
        {
            if (!_flashing)
            {
                // meshRenderer.material = _originalMaterials;
                _blinkingCoroutine = StartCoroutine(Blink());
            }
        }
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(5f);
        rb.isKinematic = true;
        _collider.isTrigger = true;
        
        yield return new WaitForSeconds(8f);
        
        _fading = true;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        _fading = false;
    }

    IEnumerator Blink()
    {
        _flashing = true;
        
        meshRenderer.enabled = !meshRenderer.enabled;
        
        // meshRenderer.material = _blackMaterial;
        
        yield return new WaitForSeconds(0.4f); // Wait for 0.2 seconds

        _flashing = false;
    }

}
