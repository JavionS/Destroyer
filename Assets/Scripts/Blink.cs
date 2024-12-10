using UnityEngine;
using System.Collections;
using TMPro;

public class Blink : MonoBehaviour
{
    private TextMeshProUGUI _messageGUI;

    private bool _inCoroutine = false;
    [SerializeField] private float _blinkRate = 0.5f;

    private Coroutine _myStoppableCoroutine;
    private void Start()
    {
        _messageGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!_inCoroutine)
        {
            _myStoppableCoroutine = StartCoroutine(BlinkMenu());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StopCoroutine(_myStoppableCoroutine);
            _inCoroutine = false;
        }
    }
    
    IEnumerator BlinkMenu()
    {
        _inCoroutine = true;
        
        _messageGUI.enabled = !_messageGUI.enabled;
        
        yield return new WaitForSeconds(_blinkRate); // Wait for 0.5 seconds

        _inCoroutine = false;
    }
}
