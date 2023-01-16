using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string targetBool;
    public bool status;
    public bool onExit;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onExit) { return; }
        else 
        {
            if (targetBool.Contains(","))
            {
                string[] targetBools = targetBool.Split(',');
                
                foreach(string target in targetBools) { Debug.Log(target); animator.SetBool(target, status); }
                targetBool = targetBools[0];
            }
            else
            {
                animator.SetBool(targetBool, status); 
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onExit) 
        {
            if (targetBool.Contains(","))
            {
                string[] targetBools = targetBool.Split(',');
                foreach (string target in targetBools) { animator.SetBool(target, status); }
                targetBool = targetBools[0];
            }
            else
            {
                animator.SetBool(targetBool, status);
            }
        }
        else { return; }
    }
}
