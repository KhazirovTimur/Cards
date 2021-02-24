using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterBehaviourScript : MonoBehaviour
{
    public Text valueText;
    public float moveSpeed = 5f;
    private void Start()
    {
        
    }

    private void Update()
    {
        transform.position += new Vector3(moveSpeed, 0, 0) * Time.deltaTime;
    }

}
