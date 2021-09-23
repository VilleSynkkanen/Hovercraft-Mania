using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HovercraftHud : MonoBehaviour
{
    [SerializeField] RectTransform speedoNeedle;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] float needleRotationMultiplier;
    [SerializeField] float maxNeedleRotationPerSecond;
    [SerializeField] RectTransform playerBlip;
    [SerializeField] float trackMapPosMultiplier;

    Transform trackCenter;
    HovercraftMovement movement;
    float speed;

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<HovercraftMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        float newSpeed = movement.rb.velocity.magnitude;

        if(Mathf.Abs(newSpeed - speed) > maxNeedleRotationPerSecond * Time.deltaTime)
        {
            if(newSpeed < speed)
            {
                speed -= maxNeedleRotationPerSecond * Time.deltaTime;
            }
            else
            {
                speed += maxNeedleRotationPerSecond * Time.deltaTime;
            }
        }

        speedoNeedle.rotation = Quaternion.Euler(0, 0, needleRotationMultiplier * speed);
        speedText.text = ((int)(newSpeed * 3.6f)).ToString();

        Vector3 playerPos = movement.transform.position - trackCenter.position;
        playerBlip.anchoredPosition = trackMapPosMultiplier * new Vector3(playerPos.x, playerPos.z, 0);
    }

    public void SetTrackCenter(Transform center)
    {
        trackCenter = center;
    }
}
