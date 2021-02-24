using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOutDestruction : MonoBehaviour
{
    public float destroyAfterSeconds;
    private float lifeTime;

    private void Start()
    {
        lifeTime = Time.time + destroyAfterSeconds;
    }
    void Update()
    {
        if (Time.time > lifeTime)
            Destroy(this.gameObject);
    }
}
