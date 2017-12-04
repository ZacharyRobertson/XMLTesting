using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    public float health = 100;
    public Vector3 location;

    // Use this for initialization
    void Start()
    {
        playerName = gameObject.name;
        location = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
