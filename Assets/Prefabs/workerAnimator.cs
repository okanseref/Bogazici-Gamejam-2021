using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class workerAnimator : MonoBehaviour
{
    public GameObject particle;
    public void ParticleInstance()
    {
        Vector3 pos = transform.position;
        pos.y += 1.25f;
        pos.z += 2;
        Destroy(Instantiate(particle, pos, Quaternion.identity), 1f);
    }
}
