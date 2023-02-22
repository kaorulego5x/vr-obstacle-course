using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monolith_move : MonoBehaviour
{
    public float speed;
    public float delaySecond;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {  
        transform.position = new Vector3(5f * Mathf.Sin(speed * Time.time + delaySecond), 0, 0) + offset;
    }
}
