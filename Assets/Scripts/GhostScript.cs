using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    public class GhostScript : MonoBehaviour
    {
        private Animator Anim;

        [SerializeField] float _speed = 7.0f;
        [SerializeField] float _obstacleRange = 5.0f;

        private readonly float _sphereRadius = 0.75f;
        [HideInInspector]public bool _isAlive;
        
        private float timer = 0f;
        private float _changeTime;

        public int score = 100;

        [SerializeField]private LayerMask _wall;

        void Start()
        {
            Anim = this.GetComponent<Animator>();
            _isAlive = true;
            _changeTime = Random.Range(5f, 13f);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                _isAlive = false;
                // StartCoroutine(Die());
            }
            if (_isAlive)
            {
                transform.Translate(0, 0, _speed * Time.deltaTime);
                Ray ray = new(transform.position, transform.forward);

                timer += Time.deltaTime;
                if (timer >= _changeTime)
                {
                    if (Random.Range(0f, 1f) < 0.9f)
                    {
                        float theta = Random.Range(-180, 180);
                        transform.Rotate(0, theta, 0);
                    }

                    _changeTime = Random.Range(2f, 18f);
                    timer = 0f;
                }
                
                if (Physics.SphereCast(ray, _sphereRadius, out RaycastHit hit, _obstacleRange, _wall))
                {
                    
                    float theta = Random.Range(-110, 110);
                    transform.Rotate(0, theta, 0);
                }
            }
            else
            {
                Quaternion targetRotation = Quaternion.Euler(-180f, 0, 0); 
                transform.rotation = Quaternion.Lerp(
                    transform.rotation, targetRotation, 1f * Time.deltaTime);
                StartCoroutine(Die());
            }
        }

        IEnumerator Die()
        {
            Anim.SetBool("Die",true);
            yield return new WaitForSeconds(2f);
            Destroy(this.gameObject);
        }
    }
}