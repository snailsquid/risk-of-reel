using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Bobber : MonoBehaviour
{
    [SerializeField] Transform Particle, fishableArea, bobberTop;
    ParticleSystem ps;
    [SerializeField] Transform debugObject;
    float firstPositionX;
    float firstPositionZ;
    public float moveRamge;
    public float minShake;
    public float maxShake;
    [SerializeField] bool debug = false;
    [SerializeField] float timeToBob = 7.5f, damping = 0.4f, freq = 0.4f, multiplier = 2f, jumpHeight = 5f;
    public float duration = 2f;
    public bool IsTouchingWater { get; private set; } = false;
    Rigidbody rigidBody;
    float yTouchWater = 0;
    float x = 0;
    LineRenderer lr;
    void Start()
    {
        ps = Particle.GetComponent<ParticleSystem>();
        lr = GetComponent<LineRenderer>();
        ps.Stop();
        fishableArea = GameObject.Find("Fishable Area").transform;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "water" && !IsTouchingWater)
        {
            Debug.Log("touched water");
            AudioManager.Instance.PlaySFX(AudioRegistry.Sounds[AudioManager.Sound.WaterSplashBobber]);
            IsTouchingWater = true;
            rigidBody = transform.GetComponent<Rigidbody>();
            yTouchWater = rigidBody.position.y;
            rigidBody.velocity = Vector3.zero;
            rigidBody.useGravity = false;
        };
    }
    Vector3 GetRandomFishableArea()
    {
        Debug.Log(fishableArea);
        Collider collider = fishableArea.GetComponent<Collider>();
        Debug.Log(collider.bounds.min.x + " " + collider.bounds.max.x + " " + collider.bounds.min.z + " " + collider.bounds.max.z);
        return new Vector3(UnityEngine.Random.Range(collider.bounds.min.x, collider.bounds.max.x), transform.position.y, UnityEngine.Random.Range(collider.bounds.min.z, collider.bounds.max.z));
    }
    public void Splash()
    {
        ps.Play();
        firstPositionX = transform.position.x;
        firstPositionZ = transform.position.z;
        DG.Tweening.Sequence sequence = DOTween.Sequence().SetLoops(-1);
        sequence.Append(transform.DOMove(GetRandomFishableArea(), 1).SetEase(Ease.InOutBack));
        sequence.Append(transform.DOMove(GetRandomFishableArea(), 2).SetEase(Ease.InOutBack));
        sequence.Append(transform.DOMove(GetRandomFishableArea(), 2).SetEase(Ease.InOutBack)).SetLoops(1, LoopType.Yoyo);
        sequence.Append(transform.DOMove(GetRandomFishableArea(), 1).SetEase(Ease.InOutBack));
        sequence.Append(transform.DOLocalMove(GetRandomFishableArea(), 2).SetEase(Ease.Linear).SetLoops(1, LoopType.Yoyo));
        sequence.Append(transform.DOLocalMove(new Vector3(firstPositionX, transform.position.y, firstPositionZ), 1).SetEase(Ease.Linear));
    }
    public static double BobEase(double t, double damping = 0.4f, double frequency = 2.0)
    {
        // Apply the bobbing formula
        return Math.Exp(-damping * t) * Math.Sin(2 * Math.PI * frequency * t) * -1;
    }
    float deltaTime = 0;
    void Update()
    {
        if (debug)
        {
            Transform clone = Instantiate(debugObject);
            clone.position = transform.position;
        }
        if (IsTouchingWater && deltaTime < timeToBob)
        {
            deltaTime += Time.deltaTime;
            float x = (float)BobEase(deltaTime, damping, freq) * multiplier;
            rigidBody.position = new Vector3(rigidBody.position.x, yTouchWater + x, rigidBody.position.z);
        }
        lr.positionCount = 2;
        lr.SetPosition(0, bobberTop.position);
        lr.SetPosition(1, GameObject.Find("RodAttach").transform.position);
    }
    public void FishLaunch(FishGenerator.FishType fishType)
    {
        GameObject fishModel = FishGenerator.GetFishModel(fishType);
        if (fishModel != null)
        {
            GameObject clone = Instantiate(fishModel);
            clone.transform.position = transform.position + new Vector3(0, -5, 0);
            clone.transform.Rotate(-90, 0, 0);
            clone.transform.DOMove(clone.transform.position + new Vector3(0, jumpHeight, 0), duration).SetEase(Ease.InOutQuint).onComplete = () => Destroy(clone.gameObject);
        }
        Finish();
    }
    public void Finish()
    {
        Destroy(gameObject);
    }
}
