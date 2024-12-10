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
        if (Input.GetMouseButton(0))
        {
            meshRenderer.material = _lazerEye;
        }
        else
        {
            meshRenderer.material = _normalEye;
        }

        if (GameBehavior.Instance.LaserEye)
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
