using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guard : MonoBehaviour
{
    public float Guardspeed = 5f;
    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;
    public Slider susMeter;
    public float caughtTime;
    float VisibleTimer;
    float viewAngle;
    float currentTime;
    [SerializeField] float checkInterval = 20f, firstHalf = 2;
    [SerializeField] Transform gameManager, referenceObject;
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
            if (angleBetweenGuardAndPlayer <= viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }
    IEnumerator Stay()//Stay after reach checkingpoint
    {
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
        return Mathf.Log10(Mathf.Pow(x, 0.5f) + 1);
    }
    void Update()
    {
        currentTime = timeManager.CurrentTime;
        if (centralStateManager.playerState == CentralStateManager.PlayerState.Rod)
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
            VisibleTimer = Mathf.Clamp(VisibleTimer, 0, 8f);
            susMeter.value = VisibleTimer;
            if (VisibleTimer >= caughtTime)
            {

                playerState = PlayerState.Waiting;
                gameManager.GetComponent<CentralStateManager>().FinishRun(true);
                Debug.Log("GetCaught");//Add trigger gameover here
                VisibleTimer = 0;
            }
            if (guardChecking >= checkInterval)
            {
                int check = Random.Range(0, 100);//Random number between 0-100 to calculate the chance
                Debug.Log(check);
                check = 0;
                Debug.Log(currentTime + " " + startTime);
                Debug.Log(currentTime - (24 - startTime));
                if (currentTime + startTime - 24 < firstHalf)
                {
                    if (check <= calc(currentTime) * 100)// Compare to check for chance trigger patrol
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
                else
                {
                    if (check <= (Mathf.Pow((currentTime - firstHalf) / (timeManager.maxTime - firstHalf), 0.25f) * (1 - calc(timeManager.maxTime)) + calc(timeManager.maxTime)) * 100)
                    {
                        Debug.Log("Alert");//Add ui Guard go patrol
                        targetWaypointindex = 0;
                        guardState = GuardState.Patroling;
                        audioSource.clip = AudioRegistry.Sounds[AudioManager.Sound.FootSteps];
                        audioSource.Play();
                        guardChecking = -20f;
                    }
                    else//If not checking just restart timer and do nothing
                    {
                        guardChecking = 1f;
                    }
                }
            }
            if (guardState == GuardState.Patroling)//from spawnpoint go to checkingpoint and go back to spawnpoint after 8 sec
            {
                Vector3 targetPosition = waypoints[targetWaypointindex].transform.position;
                Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, Guardspeed * Time.deltaTime);
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
}
