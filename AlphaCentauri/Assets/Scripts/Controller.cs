using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{
    int influence = 1;
    float rotX = 0f;
    float rotY = 0f;
    float sens = 1f;
    float horizontal = 0f;
    float vertical = 0f;
    float walkVelocity = 4f;
    float acceleration = 40f;
    float aT = 40f;
    float sprintVelocity = 0f;
    float verticalVelocity = 0f;
    float jumpTolerance = 0f;
    float jumped = 0f;
    float groundError = 0f;
    float sprintInfluence = 1f;
    float slopeFriction = 0.4f;
    float M = 0f;
    float F = 0f;
    bool grounded = false;
    bool sprinting = false;
    bool W = false;
    bool A = false;
    bool S = false;
    bool D = false;
    bool autoHop = false;
    Vector2 fatigue;
    Vector3 a1;
    Vector3 v1;
    Vector3 a2;
    Vector3 v2;
    Vector3 aS;
    Vector3 vS;
    Vector3 velocity;
    Vector3 v;
    Vector3 a;
    Vector3 V;
    Rigidbody rb;
    RaycastHit ground;
    RaycastHit normal;
    private TextMeshProUGUI text;
    private GameObject aligner;
    private GameObject slopeDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        text = GameObject.Find("debug text").GetComponent<TMPro.TextMeshProUGUI>();
        aligner = GameObject.Find("slopeAlign");
        slopeDirection = GameObject.Find("slopeDirection");
        Cursor.visible = false;
        fatigue = new Vector2(1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //rotation
        rotX = Mathf.Clamp((rotX + (-Input.GetAxis("Mouse Y") * sens)), -90, 90);
        rotY += Input.GetAxis("Mouse X") * sens;

        transform.rotation = Quaternion.Euler(0, rotY, 0);
        gameObject.transform.GetChild(0).localRotation = Quaternion.Euler(Mathf.Clamp(rotX, -90, 90), 0, 0);

        //movement
        W = Input.GetKey("w");
        A = Input.GetKey("a");
        S = Input.GetKey("s");
        D = Input.GetKey("d");
        
        if (!A && D)
        {
            horizontal = 1;
        }
        else if (A && !D)
        {
            horizontal = -1;
        }

        if (!W && S)
        {
            vertical = -1;
        }
        else if (W && !S)
        {
            vertical = 1;
        }
        
        if ((A && D) || (!A && !D))
        {
            horizontal = 0;
        }

        if ((W && S) || (!W && !S))
        {
            vertical = 0;
        }

        //sprint
        if (Input.GetKey("left shift")  && grounded && !Input.GetKey("s"))
        {
            sprinting = true;
            sprintVelocity = 0.5f*walkVelocity;
        }
        else if ((!Input.GetKey("left shift") && grounded) || (Input.GetKey("s") && grounded))
        {
            sprinting = false;
            sprintVelocity = 0f;
        }

        //grounded
        if (Physics.SphereCast(transform.position, 0.4f, -transform.up, out ground, 0.75f))
        {
            if (Vector3.Angle(Vector3.up, ground.normal) > 46)
            {
                grounded = false;
            }
            else
            {
                grounded = true;
            }
        }
        else
        {
            grounded = false;
        }
        
        if (grounded)
        {
            acceleration = acceleration + (aT - acceleration)*Time.deltaTime*0.5f;
            fatigue = Vector2.MoveTowards(fatigue, new Vector2(1, 0), Time.deltaTime*3f);
            sprintInfluence = 1f;
            influence = 1;
            groundError = 0.65f - ground.distance;

            //slope
            if (Physics.Raycast(transform.position, -transform.up, out normal, 1f))
            {
                aligner.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal.normal);
                slopeDirection.transform.localRotation = Quaternion.Euler(0, gameObject.transform.eulerAngles.y, 0);
            }
        }
        else if (!grounded)
        {
            acceleration = 0.625f*aT;
            if (sprinting)
            {
                sprintInfluence = 0.625f;
            }
            else
            {
                sprintInfluence = 1f;
            }

            if (((!W && !S) || (W && S)) && ((!A && !D) || (A && D)))
            {
                influence = 0;
            }
            else
            {
                influence = 1;
            }

            groundError = 0f;

            //anti slope
            aligner.transform.localRotation = Quaternion.Euler(0, 0, 0);
            slopeDirection.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (groundError > 0 && jumped <= 0 && grounded)
        {
            verticalVelocity += (groundError * 90f - rb.velocity.y * 10f) * Time.deltaTime / ((Vector3.Angle(Vector3.up, ground.normal) / (0.3f * 45f)) + 1f) * (v.y/3 + 1);
            //verticalVelocity += (groundError * 90f - rb.velocity.y * 10f) * Time.deltaTime * (v.y/5 + 1);
        }

        //jump
        if (!autoHop)
        {
            if (Input.GetKeyDown("space"))
            {
                jumpTolerance = 0.07f;
            }
        }
        else
        {
            if (Input.GetKey("space"))
            {
                jumpTolerance = 0.07f;
            }
        }

        if (jumpTolerance > 0f)
        {
            jumpTolerance -= Time.deltaTime;
            if (grounded && (jumped <= 0) && (fatigue.x > 0.15f))
            {
                //rb.velocity = new Vector3(rb.velocity.x, 4 * fatigue.x, rb.velocity.z);
                //rb.velocity +=  new Vector3(0, -rb.velocity.y, 0) + transform.up * 4 * fatigue.x;
                //rb.velocity +=  new Vector3(0, -rb.velocity.y, 0);
                //rb.AddForce(new Vector3(0f, 4f * fatigue.x, 0f), ForceMode.VelocityChange);
                verticalVelocity = 4f * fatigue.x;
                fatigue -= new Vector2(0.3f, 0);
                jumpTolerance = 0; 
                jumped = 0.2f;
            }
        }

        if (jumped > 0f)
        {
            jumped -= Time.deltaTime;
        }

        verticalVelocity -= 10f * Time.deltaTime;

        //vectors
        a1 = walkVelocity*Vector3.Normalize(slopeDirection.transform.forward * vertical + slopeDirection.transform.right * horizontal);
        v1 = Vector3.MoveTowards(v1, a1, sprintInfluence * influence * acceleration * Time.deltaTime);
        a2 = walkVelocity*Vector3.Normalize(slopeDirection.transform.forward * vertical + slopeDirection.transform.right * horizontal);
        v2 = Vector3.MoveTowards(v2, a2, 0.2f * sprintInfluence * influence * acceleration * Time.deltaTime);
        aS = sprintVelocity*Vector3.Normalize(slopeDirection.transform.forward * vertical + slopeDirection.transform.right * horizontal);
        vS = Vector3.MoveTowards(vS, aS, sprintInfluence * influence * acceleration * Time.deltaTime * 0.5f);

        //velocity
        v = 0.8f * v1 + 0.2f * v2 + vS;
        velocity = v + new Vector3(0, verticalVelocity, 0);
        rb.velocity = velocity;

        //debug

        M = v.magnitude;
        a = a1 + aS;
        F = fatigue.x;
        V = rb.velocity;
        //text.text = $"velocity<indent=25%>= {V}<br><indent=0%>fatigue<indent=25%>= {F}<br><indent=0%>acceleration<indent=25%>= {aT}<br><indent=0%>autoHop (p)<indent=25%>= {autoHop}<br><align=center><indent=-40%>(click 1/2 to -/+ acceleration)<br>CoD styled movement controller<br>scripted by thor";

        if (Input.GetKey("tab"))
        {
            Cursor.visible = true;
            sens = 0f;
        }
        else
        {
            Cursor.visible = false;
            sens = 1f;
        }

        if (Input.GetKeyDown("p"))
        {
            autoHop = !autoHop;
        }

        if (Input.GetKeyDown("1"))
        {
            aT -= 1f;
        }
        else if (Input.GetKeyDown("2"))
        {
            aT += 1f;
        }
        //rb.AddForce(new Vector3(0f, 10f * Time.deltaTime, 0f), ForceMode.VelocityChange);
    }
}
