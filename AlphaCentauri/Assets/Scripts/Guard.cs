using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guard : MonoBehaviour
{
    public float Guardspeed = 5f;
    public float inGameTime;
    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;
    public Slider susMeter;
    float VisibleTimer;
    float viewAngle;
    bool Staying;
    float waitTime;
    float guardChecking;
    public bool Patroling;
    public List<GameObject> waypoints;
    int targetWaypointindex=0;
    Transform Player;
    void Start()
    {
        Staying = false;
        Patroling=false;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
    }
    bool CanSeePlayer()//Detect player by Guard vision
    {
            if (Vector3.Distance(transform.position,Player.position) <= viewDistance)
            {
                Vector3 dirToPlayer = (Player.position - transform.position).normalized;
                float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward,dirToPlayer);
                if (angleBetweenGuardAndPlayer <= viewAngle/2f)
                {
                    if(!Physics.Linecast(transform.position,Player.position,viewMask))
                    {
                        return true;
                    }
                }
            }
            return false;
    }
    IEnumerator Stay()//Stay after reach checkingpoint
    {
        yield return Patroling = false;
        waitTime += Time.deltaTime;
        if (waitTime >= 8f)
        {
            Patroling = true;
            waitTime = 0;
            Staying = false;
        } 
    }
    void Update()
    {
        inGameTime += Time.deltaTime;
        guardChecking += Time.deltaTime;
        if (CanSeePlayer())
        {
            VisibleTimer += Time.deltaTime;//Slider go up when Guard see player
        }
        else
        {
            VisibleTimer -= Time.deltaTime;//Slider go up when Guard doesn't see player(Hide in bush)
        }
        VisibleTimer = Mathf.Clamp(VisibleTimer,0,8f);
        susMeter.value=VisibleTimer;
        if (VisibleTimer >= 8f)
        {
            Debug.Log("GetCaught");//Add trigger gameover here
        }
        if (guardChecking >=20f)
        {
            int check = Random.Range(0,100);//Random number between 0-100 to calculate the chance
            Debug.Log(check);
            if (inGameTime <600)//Chance Guard patrol from jam 20.00-00.00
            {
                if (check <= (Mathf.Floor(inGameTime/120))*5)// Compare to check for chance trigger patrol
                {
                    Debug.Log("Alert");//Add ui Guard go patrol
                    targetWaypointindex = 0;
                    Patroling=true;
                    guardChecking=-20f;
                }
                else//If not checking just restart timer and do nothing
                {
                    guardChecking=0f;
                }
            }
            else
            {
                if (check <= ((Mathf.Floor(inGameTime/120))*10)-20)//Chance Guard patrol from jam 00.00-02.00
                {
                    Debug.Log("Alert");//Add ui Guard go patrol
                    targetWaypointindex = 0;
                    Patroling=true;
                    guardChecking=-20f;
                }
                else//If not checking just restart timer and do nothing
                {
                    guardChecking=1f;
                }
            }
        }
        if(Patroling)//from spawnpoint go to checkingpoint and go back to spawnpoint after 8 sec
        {
            Vector3 targetPosition = waypoints[targetWaypointindex].transform.position;
            Vector3 newPosition = Vector3.MoveTowards(transform.position,targetPosition,Guardspeed* Time.deltaTime);
            transform.position = newPosition;
            float distance = Vector3.Distance(transform.position,targetPosition);
            if(distance <=0.05)
            {
                if(targetWaypointindex<waypoints.Count-1)
                {
                    Staying = true;
                    targetWaypointindex++;
                }
                else
                {
                    Patroling = false;
                }
            }
        }
        if(Staying)
        {
            StartCoroutine(Stay());
        }
    }   
}
