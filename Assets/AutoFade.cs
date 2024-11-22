using UnityEngine;
using System.Collections;

public class AutoFade : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }

}
