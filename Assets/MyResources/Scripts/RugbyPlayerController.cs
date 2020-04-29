using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class RugbyPlayerController : MonoBehaviour
{
    public Animator anim;
    public float movementSpeed = 10f;
    public bool CanMove = true;

    public int state;

    private Vector3 movement = Vector3.zero;

    void Update()
    {
        switch (state)
        {
            case 1:
                Movement();
                break;
            case 2:
                Retreat();
                break;
            case 3:
                Idle();
                break;
        }
    }

    public void Movement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal"); //Input.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical"); //Input.GetAxis("Vertical");

        movement = new Vector3(horizontalInput * movementSpeed * Time.deltaTime, 0, verticalInput * movementSpeed * Time.deltaTime);
        transform.position += movement;

        transform.rotation = movement == Vector3.zero ? transform.rotation : Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);

        float absSpeed = Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput) ? Mathf.Abs(horizontalInput) : Mathf.Abs(verticalInput);
        anim.SetFloat("Speed", absSpeed);
        //Debug.Log(absSpeed);
    }

    public void Retreat()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 0, 0)), 0.2f);
    }

    public void Idle()
    {

    }

    public void SkillMove(int i)
    {
        switch (i)
        {
            case 1:
                anim.SetTrigger("StepRight");
                break;

            case 2:
                anim.SetTrigger("StepLeft");
                break;

        }
    }

    public void Tackled()
    {
        state = 2;
        anim.SetFloat("Speed", 0.0f);
        anim.SetBool("Tackled", true);
    }

    public void AnimReset()
    {
        anim.SetBool("Tackled", false);
    }

}
