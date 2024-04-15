using System.Collections.Generic;

using AssemblyCSharp.Assets.Tools;

using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class CloudsGenerator : MonoBehaviour
{
    [SerializeField]
    Sprite[] cloudSprites;

    [SerializeField]
    int maxClouds = 5;
    List<GameObject> cloudObjects = new List<GameObject>();

    [Header("Cloud size")]
    [SerializeField]
    float minCloudSize = 0.3f;
    [SerializeField]
    float maxCloudSize = 0.8f;

    [Header("Cloud X offset")]
    [SerializeField]
    float minXOffset = 40f;
    [SerializeField]
    float maxXOffset = 160f;

    [Header("Cloud speed")]
    [SerializeField]
    bool sizeInfluenceOnSpeed = true;
    [SerializeField]
    float minCloudSpeed = 2.0f;
    [SerializeField]
    float maxCloudSpeed = 10.0f;


    [Header("Spawn time")]
    [SerializeField]
    float minSpawnTime = 1.0f;
    [SerializeField]
    float maxSpawnTime = 3.0f;

    float nextSpawnTime = 0f;
    float spawnTimer = 0f;

    int cloudSpawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        CheckClouds();


        if (spawnTimer > nextSpawnTime && cloudObjects.Count < maxClouds)
        {
            CreateCloud();
            spawnTimer = 0f;
            nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
        else
        {
            spawnTimer += Time.fixedDeltaTime;
        }
    }

    private void CreateCloud()
    {
        // Create object
        GameObject cloud = new GameObject($"Cloud_{cloudSpawned}");
        cloud.transform.parent = transform;
        cloudSpawned++;

        // Add cloud image
        Image cloudImage = cloud.AddComponent<Image>();
        Sprite cloudSprite = cloudSprites.RandomOne();
        cloudImage.sprite = cloudSprite;
        cloudImage.preserveAspect = true;
        cloudImage.SetNativeSize();

        float cloudSize = Random.Range(minCloudSize, maxCloudSize);
        cloud.transform.localScale = new Vector3(cloudSize, cloudSize, 1f);

        // Set start and direction
        int direction = Random.Range(0, 100) < 50 ? -1 : 1;
        cloud.GetComponent<RectTransform>().anchoredPosition = new Vector3()
        {
            x = direction * (0.5f * (cloud.GetComponent<RectTransform>().rect.width + UnityEngine.Screen.width) + Random.Range(minXOffset, maxXOffset)),
            y = Random.Range(UnityEngine.Screen.height / 4, UnityEngine.Screen.height / 2),
            z = 1.0f
        };

        //Debug.Log($"Created cloud at: {cloud.transform.position.x}, {cloud.transform.position.y} -> rect width: {cloud.GetComponent<RectTransform>().rect.width}, screen {UnityEngine.Screen.width}");

        // Move it
        Rigidbody2D cloudRb = cloud.AddComponent<Rigidbody2D>();
        cloudRb.gravityScale = 0f;
        cloudRb.velocityX = -direction * Random.Range(minCloudSpeed, maxCloudSpeed);
        if (sizeInfluenceOnSpeed)
            cloudRb.velocityX *= cloudSize;


        cloudObjects.Add(cloud);
    }

    private void CheckClouds()
    {
        if (cloudObjects.Count == 0) return;

        for (int i = cloudObjects.Count - 1; i >= 0; i--)
        {
            GameObject cloud = cloudObjects[i];
            RectTransform rectTransform = cloud.GetComponent<RectTransform>();
            int direction = cloud.GetComponent<Rigidbody2D>().velocityX > 0 ? 1 : -1;
            float border = rectTransform.rect.width * 0.5f + 20f;

            // Going left
            if (direction < 0)
            {
                if (rectTransform.anchoredPosition.x < -border)
                {
                    cloudObjects.Remove(cloud);
                    Destroy(cloud, 1f);
                }
            }
            else // going right
            {
                if (rectTransform.anchoredPosition.x > border + UnityEngine.Screen.width)
                {
                    cloudObjects.Remove(cloud);
                    Destroy(cloud, 1f);
                }
            }
        }
    }
}
