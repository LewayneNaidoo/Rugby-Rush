using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RugbyPlayerAIController : MonoBehaviour
{
    public Animator anim;
    public GameObject target;
    public GameManager gm;
    public Spawner sp;

    public float movementSpeed = 3f;

    private Vector3 movement = Vector3.zero;
    public bool benched = false;
    private Vector3 benchLocation;

    private void Start()
    {
    //    target = GameObject.FindGameObjectWithTag("Player");
    //    gm = GameObject.FindGameObjectWithTag("GameManager").GetComponentInChildren<GameManager>();
    //    sp = GameObject.FindGameObjectWithTag("Spawner").GetComponentInChildren<Spawner>();

        benchLocation = new Vector3(-32, 2.7f, -58 + UnityEngine.Random.Range(-5, 5));

    }

    void Update()
    {
        if (!benched)
        {
            switch (gm.aiState)
            {
                case 1:
                    ChasePlayer();
                    break;
                case 2:
                    Retreat();
                    break;
                case 3:
                    Idle();
                    break;
            }
        }
        else
        {
            GoToBench();
        }
        
    }

    private void GoToBench()
    {
        if (this.transform.position != benchLocation)
        {
            var heading = benchLocation - this.transform.position;
            var distance = heading.magnitude;
            var direction = heading / distance;

            movement = new Vector3(direction.x * movementSpeed / 2 * Time.deltaTime, 0, direction.z * movementSpeed / 2 * Time.deltaTime);
            transform.position += movement;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);
        }
        else
        {
            Idle();
        }
        
    }

    private void Idle()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = transform.position;
        anim.Play("Idle");
    }

    public void ChasePlayer()
    {
        anim.SetBool("Backtrack", false);
        // Gets a vector that points from the player's position to the target's.
        var heading = target.transform.position - this.transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance; // This is now the normalized direction.

        //if (heading.sqrMagnitude < maxRange * maxRange)
        //{
        // Target is within range.
        //}

        movement = new Vector3(direction.x * movementSpeed * Time.deltaTime, 0, direction.z * movementSpeed * Time.deltaTime);
        transform.position += movement;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);
            anim.SetFloat("Speed", 1.0f);
        }
        else
        {
            anim.SetFloat("Speed", 0.0f);
        }
    }

    public void Retreat()
    {
        if (this.transform.position.z >= gm.newDownsLine + 9)
        {
            anim.SetBool("Backtrack", false);
            Idle();
            return;
        }
        // move backwards
        var heading = new Vector3(this.transform.position.x, this.transform.position.y, gm.newDownsLine + 10) - this.transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;

        movement = new Vector3(direction.x * (movementSpeed/1.5f) * Time.deltaTime, 0, direction.z * (movementSpeed/1.5f) * Time.deltaTime);
        transform.position += movement;

        // face player
        heading = target.transform.position - this.transform.position;
        distance = heading.magnitude;
        direction = heading / distance;

        movement = new Vector3(direction.x * movementSpeed * Time.deltaTime, 0, direction.z * movementSpeed * Time.deltaTime);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);

        // animate
        anim.SetBool("Backtrack", true);

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && gm.AllowedToTackle)
        {
            transform.position = target.transform.position;
            //transform.rotation = target.transform.rotation;
            Tackle();
            target.GetComponentInChildren<RugbyPlayerController>().Tackled();
            Debug.Log("Tackle!!");
            StartCoroutine(AnimReset());
        }
    }

    public void Tackle()
    {
        //this.GetComponentInChildren<SphereCollider>().enabled = false;
        gm.AllowedToTackle = false;
        anim.SetBool("Tackle", true);
    }

    IEnumerator AnimReset()
    {
        yield return new WaitForSeconds(2);
        //target.GetComponentInChildren<RugbyPlayerController>().AnimReset();
        anim.SetBool("Tackle", false);

        FindObjectOfType<GameManager>().NewDown();
        //Destroy(gameObject);
    }

    public void SkillMove(int i)
    {
        switch (i)
        {
            case 1:
                anim.SetTrigger("DiveHigh");
                break;

            case 2:
                anim.SetTrigger("DiveLow");
                break;

        }
    }

}
