using UnityEngine;
using System.Collections;

public class EyeColor : MonoBehaviour
{
    MeshRenderer meshRenderer;
    
    [SerializeField]Material _lazerEye;
    [SerializeField]Material _normalEye;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = _normalEye;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        meshRenderer.material = _lazerEye;
        yield return new WaitForSeconds(0.8f);
        meshRenderer.material = _normalEye;
    }
}
