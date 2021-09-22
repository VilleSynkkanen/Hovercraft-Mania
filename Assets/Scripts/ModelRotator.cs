using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    float rotation;

    void Start()
    {
        rotation = transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        rotation += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }
}
