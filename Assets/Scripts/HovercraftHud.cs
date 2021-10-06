using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HovercraftHud : MonoBehaviour
{
    [SerializeField] RectTransform speedoNeedle;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] float needleRotationMultiplier;
    [SerializeField] float maxNeedleRotationPerSecond;
    [SerializeField] RectTransform playerBlip;
    [SerializeField] Image trackMap;

    Transform trackCenter;
    HovercraftMovement movement;
    float speed;
    float trackMapPosMultiplier;

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<HovercraftMovement>();
        trackMap.sprite = TimeTrialController.instance.TrackMapImage;
        trackMapPosMultiplier = TimeTrialController.instance.TrackMapPosMultiplier;
        trackMap.GetComponent<RectTransform>().sizeDelta = TimeTrialController.instance.TrackMapDimensions;
        trackMap.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, TimeTrialController.instance.TrackMapRotation);
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
