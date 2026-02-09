using UnityEngine;
using System.Collections.Generic;

public class TunnelSpawnerScript : MonoBehaviour
{
    public GameObject TunnelPoolParent;
    public List<GameObject> TunnelPool;
    public Transform LastTunnel;
    float VerticalOffset;
    float HorizontalOffset;
    float TunnelLength;

    void Start()
    {
        for(int i = 0; i < TunnelPoolParent.transform.childCount; i++)
        {
            TunnelPool.Add(TunnelPoolParent.transform.GetChild(i).gameObject);
            // TunnelPool[i].SetActive(false);
        }
        TunnelLength = TunnelPool[0].GetComponent<MeshRenderer>().bounds.size.z;

        LastTunnel = TunnelPool[0].transform;

        for(int i = 1; i < TunnelPool.Count; i++)
        {
            SpawnNextTunnel(TunnelPool[i], LastTunnel);
        }
    }

    public void SpawnNextTunnel(GameObject tunnelToSpawn, Transform lastTunnel)
    {
        VerticalOffset = Random.Range(-1.5f, 1.5f);
        HorizontalOffset = Random.Range(-1.5f, 1.5f);
        Vector3 spawnPos = lastTunnel.position +
                           Vector3.forward * TunnelLength +
                           Vector3.up * VerticalOffset +
                           Vector3.right * HorizontalOffset;

        tunnelToSpawn.transform.position = spawnPos;
        tunnelToSpawn.SetActive(true);

        LastTunnel = tunnelToSpawn.transform;
    }
}
