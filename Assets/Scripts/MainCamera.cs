using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,0,speed*Time.deltaTime);   
    }

    private void onTriggerEnter(Collider other) 
    {
        Debug.Log("hello");
        if(other.CompareTag("Obstacle")) {
            Destroy(other);
        }
    }
}
