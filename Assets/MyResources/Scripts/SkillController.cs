using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public RugbyPlayerController rpc;

    public void StepRight()
    {
        rpc.SkillMove(1);
    }

    public void StepLeft()
    {
        rpc.SkillMove(2);
    }

}
