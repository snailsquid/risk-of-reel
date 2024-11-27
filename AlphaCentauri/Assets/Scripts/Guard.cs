using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Guard : MonoBehaviour
{
    public float Guardspeed = 5f;
    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;
    public float caughtTime;
    float VisibleTimer;
    float viewAngle;
    float currentTime;
    [SerializeField] float checkInterval = 20f, firstHalf = 2;
    [SerializeField] Transform gameManager, referenceObject, susMeter, guardCaughtPosition;
    [SerializeField] Animator satpamAnimator;
    [SerializeField] CameraManager cameraManager;
    public enum GuardState { Neither, Staying, Patroling };
    GuardState guardState;
    public enum PlayerState { Playing, Waiting, getCaught };
    public PlayerState playerState;
    float waitTime;
    public float guardChecking;
    public List<GameObject> waypoints;
    int targetWaypointindex = 0;
    AudioSource audioSource;
    Transform player;
    float startTime;
    CentralStateManager centralStateManager;
    TimeManager timeManager;
    public bool canCatch { get; set; } = true;
    public bool canMove { get; private set; } = false;
    void Start()
    {
        timeManager = gameManager.GetComponent<TimeManager>();
        startTime = timeManager.startTime;
        audioSource = GetComponent<AudioSource>();
        playerState = PlayerState.Playing;
        player = referenceObject.GetComponent<ReferenceScript>().player;
        centralStateManager = gameManager.GetComponent<CentralStateManager>();
        viewAngle = spotlight.spotAngle;
    }
    bool CanSeePlayer()//Detect player by Guard vision
    {
        if (Vector3.Distance(transform.position, player.position) <= viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer <= viewAngle)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void Reset()
    {
        guardState = GuardState.Neither;
        guardChecking = 0;
        VisibleTimer = 0;
        waitTime = 0;
        targetWaypointindex = 1;
        transform.position = waypoints[1].transform.position;
        canMove = false;
    }
    public void SetMove(bool move)
    {
        canMove = move;
    }
    IEnumerator Stay()//Stay after reach checkingpoint
    {
        satpamAnimator.SetBool("walking", false);
        satpamAnimator.SetBool("flashlight", true);
        yield return guardState = GuardState.Staying;
        waitTime += Time.deltaTime;
        if (waitTime >= 8f)
        {
            guardState = GuardState.Patroling;
            waitTime = 0;
        }
    }
    float calc(float x)
    {
        Debug.Log(x);
        float l = 2.5f;
        float k = 4.3f;
        return (Mathf.Pow(l, k * x) - 1) / (Mathf.Pow(l, k) - 1);
    }
    void Update()
    {
        currentTime = timeManager.CurrentTime;
        if (centralStateManager.playerState == CentralStateManager.PlayerState.Rod && canMove)
        {
            guardChecking += Time.deltaTime;
            if (CanSeePlayer())
            {
                VisibleTimer += Time.deltaTime;//Slider go up when Guard see player
            }
            else
            {
                VisibleTimer -= Time.deltaTime;//Slider go up when Guard doesn't see player(Hide in bush)
            }
            VisibleTimer = Mathf.Clamp(VisibleTimer, 0, caughtTime);
            susMeter.GetComponent<SusMeter>().SetPercent(VisibleTimer / caughtTime);
            if (VisibleTimer >= caughtTime)
            {
                if (canCatch)
                {
                    canCatch = false;
                    VisibleTimer = 0;
                    playerState = PlayerState.Waiting;

                    transform.position = guardCaughtPosition.position;
                    cameraManager.SwitchToCaught();
                    canMove = false;

                    Debug.Log("GetCaught");//Add trigger gameover here
                    susMeter.GetComponent<SusMeter>().SetPercent(0);
                }
            }
            if (guardChecking >= checkInterval)
            {
                int check = Random.Range(0, 100);//Random number between 0-100 to calculate the chance
                Debug.Log(currentTime + " " + (timeManager.maxTime + 24 - timeManager.startTime));
                Debug.Log(check + " " + calc(currentTime / (timeManager.maxTime + 24 - timeManager.startTime)) * 100);
                if (check <= calc(currentTime / (timeManager.maxTime + 24 - timeManager.startTime)) * 100)// Compare to check for chance trigger patrol
                {
                    Debug.Log("Alert");//Add ui Guard go patrol
                    targetWaypointindex = 0;
                    audioSource.clip = AudioRegistry.Sounds[AudioManager.Sound.FootSteps];
                    audioSource.Play();
                    guardState = GuardState.Patroling;
                    guardChecking = -20f;
                }
                else//If not checking just restart timer and do nothing
                {
                    guardChecking = 0f;
                }

            }
            if (guardState == GuardState.Patroling)//from spawnpoint go to checkingpoint and go back to spawnpoint after 8 sec
            {
                satpamAnimator.SetBool("flashlight", false);
                satpamAnimator.SetBool("walking", true);
                Transform target = waypoints[targetWaypointindex].transform;
                Vector3 targetPosition = target.position;
                Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, Guardspeed * Time.deltaTime);
                RotateGuard(target.rotation.eulerAngles, 4);
                transform.position = newPosition;
                float distance = Vector3.Distance(transform.position, targetPosition);
                if (distance <= 0.05)
                {
                    if (targetWaypointindex < waypoints.Count - 1)
                    {
                        guardState = GuardState.Staying;
                        targetWaypointindex++;
                    }
                    else
                    {
                        guardState = GuardState.Neither;
                    }
                    audioSource.Stop();
                }
            }
            if (guardState == GuardState.Staying)
            {
                StartCoroutine(Stay());
            }
        }
        else
        {
            guardChecking = 0;
        }
    }
    bool rotateDebounce = true;
    void RotateGuard(Vector3 rotation, float duration)
    {
        if (rotateDebounce)
        {
            rotateDebounce = false;
            transform.DORotate(rotation, duration).OnComplete(() => rotateDebounce = true);
        }
    }
}


