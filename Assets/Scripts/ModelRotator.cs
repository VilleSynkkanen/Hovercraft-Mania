using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    float rotation;

    void Start()
    {
        rotation = transform.localRotation.eulerAngles.z;
    }

    void Update()
    {
        rotation += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }
}
