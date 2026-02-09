using UnityEngine;

public class TunnelMoveScript : MonoBehaviour
{
    public TunnelSpawnerScript TunnelSpawner;
    public Transform DissappearPoint;
    public float MoveSpeed;
    public float RotationSpeed;

    void Update()
    {
        foreach(GameObject tunnel in TunnelSpawner.TunnelPool)
        {
            if(tunnel.activeSelf == true)
            {
                tunnel.transform.Translate(-tunnel.transform.forward * Time.deltaTime * MoveSpeed);
                tunnel.transform.rotation = Quaternion.Euler(tunnel.transform.rotation.eulerAngles + new Vector3(tunnel.transform.rotation.x + RotationSpeed * Time.deltaTime, 0f, 0f));
                
                if(tunnel.transform.position.z <= DissappearPoint.position.z)
                {
                    tunnel.SetActive(false);
                    TunnelSpawner.SpawnNextTunnel(tunnel, TunnelSpawner.LastTunnel);
                }
            }
        }
    }
}
