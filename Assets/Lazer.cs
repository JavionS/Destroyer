using UnityEngine;
using UnityEngine.VFX;

public class Lazer : MonoBehaviour
{
    public VisualEffect vfx;
    public LayerMask hitLayer;
    public float laserLength = 100f;
    
    public Transform laser;
    
    void Start()
    {
        hitLayer = LayerMask.GetMask("Buildings");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = laser.position;
        Vector3 direction = laser.forward;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(origin, direction, out RaycastHit hit, laserLength, hitLayer))
            {
                if (hit.collider.CompareTag("Destructible"))
                {
                    MeshDestroy meshDestroy = hit.collider.gameObject.GetComponent<MeshDestroy>();
                    meshDestroy.DestroyMesh();
                
                }
            }
        }
        
        
    }
}
