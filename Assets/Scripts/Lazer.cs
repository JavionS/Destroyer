using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;
using System.Collections;
using Sample;

public class Lazer : MonoBehaviour
{
    [SerializeField] private VisualEffect _vfxDetect;
    [SerializeField] private VisualEffect[] _vfxHits = new VisualEffect[2];
    public LayerMask hitLayer;
    public float laserLength = 100f;
    
    public Transform laser;
    
    void Start()
    {
        hitLayer = LayerMask.GetMask("Buildings");
        foreach (var lazer in _vfxHits)
        {
            lazer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = laser.position;
        Vector3 direction = laser.forward;
        

        if (GameBehavior.Instance.isAlive && Input.GetMouseButton(1) && !Input.GetMouseButton(0))
        {
            _vfxDetect.enabled = true;
        }
        else
        {
            _vfxDetect.enabled = false;
        }
       
        if (GameBehavior.Instance.isAlive && GameBehavior.Instance.LaserEye)
        {
            foreach (var lazer in _vfxHits)
            {
                lazer.enabled = true;
            }
            if (Physics.Raycast(origin, direction, out RaycastHit hit, laserLength, hitLayer))
            {
                if (hit.collider.CompareTag("Destructible"))
                {
                    MeshDestroy meshDestroy = hit.collider.gameObject.GetComponent<MeshDestroy>();
                    meshDestroy.DestroyMesh();
                    GameBehavior.Instance.Score(meshDestroy.score);
                
                }
                if (hit.collider.CompareTag("Ghost"))
                {
                    GhostScript ghost = hit.collider.gameObject.GetComponent<GhostScript>();
                    if (ghost._isAlive)
                    {
                        ghost._isAlive = false;
                        GameBehavior.Instance.Score(ghost.score);
                    }
                }
            }
        }else
        {
            foreach (var lazer in _vfxHits)
            {
                lazer.enabled = false;
            }
        }
        
        
    }
}
