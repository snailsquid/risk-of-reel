using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pitch : MonoBehaviour
{
    float timer;
    float target;
    float which;

    // Start is called before the first frame update
    void Start()
    {
        target = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > target)
        {
            timer = 0;
            which = Random.Range(0, 1f);
            if (which > 0.8)
            {
                GetComponent<AudioSource>().pitch = Random.Range(1f, 2f);
            }
            else
            {
                GetComponent<AudioSource>().pitch = Random.Range(0f, 0.3f);
            }
            target = Random.Range(0.3f, 2f);
        }
    }
}
