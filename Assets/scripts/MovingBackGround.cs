using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class MovingBackGround : MonoBehaviour
{
    [SerializeField] CinemachineCamera cinemachineCamera;
    [SerializeField] List<Material> materials;
    [SerializeField] float moveSpeed;
    float speed;

    List<float> distances;
    bool isWalking, isBlocked;
    float moveDirection, cameraSpeed;
    Vector3 lastPosition;
    public static MovingBackGround Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
        distances = new List<float> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }
    private void Start()
    {
        for (int i = 0; i < materials.Count; i++)
            materials[i].SetTextureOffset("_MainTex", Vector2.zero);
    }
    void FixedUpdate()
    {
        if (cinemachineCamera != null)
        {
            Vector3 currentPosition = cinemachineCamera.transform.position;
            cameraSpeed = (currentPosition.x - lastPosition.x) / Time.deltaTime;
            lastPosition = currentPosition;
        }
        if (isWalking && !isBlocked && Mathf.Abs(cameraSpeed)> 2)
        {
            speed = moveSpeed;
            for (int i = 0; i < materials.Count; i++)
            {
                Debug.Log($"background {i} is moving with speed {speed}");
                distances[i] += Time.deltaTime * speed * -moveDirection;
                distances[i] %= 1.0f;
                materials[i].SetTextureOffset("_BaseMap", new Vector2(distances[i], 0));
                speed /= 2;
            }
        }
    }
    public void IsWalkingTrigger(bool state, bool state2, float playerDir)
    {
        isWalking = state;
        isBlocked = state2;
        moveDirection = playerDir;
    }
}
