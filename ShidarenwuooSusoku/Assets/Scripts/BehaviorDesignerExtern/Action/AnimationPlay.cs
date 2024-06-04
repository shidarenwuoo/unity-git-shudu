using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

[TaskCategory("Custom/Animator")]
[TaskDescription("Plays an animator state. Running returns Running. Finished returns Complete.")]
public class AnimationPlay : Action
{
    [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [Tooltip("The name of the state")]
    public SharedString stateName;
    [Tooltip("The layer where the state is")]
    public int layer = -1;
    [Tooltip("The normalized time at which the state will play")]
    public float normalizedTime = float.NegativeInfinity;

    private Animator animator;
    private GameObject prevGameObject;

    public override void OnStart()
    {
        var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        if (currentGameObject != prevGameObject)
        {
            animator = currentGameObject.GetComponent<Animator>();
            prevGameObject = currentGameObject;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator is null");
            return TaskStatus.Failure;
        }

        // ²éÑ¯²¥·Å×´Ì¬
        var stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
        Debug.Log($"Target:{stateName.Value}, Current: {stateInfo.shortNameHash}, Time: {stateInfo.normalizedTime}");
        if (stateInfo.IsName(stateName.Value))
        {
            if (stateInfo.normalizedTime >= 1)
            {
                Debug.Log($"Target:{stateName.Value}, Success");
                return TaskStatus.Success;
            }
            else
            {
                Debug.Log($"Target:{stateName.Value}, Running");
                return TaskStatus.Running;
            }
        }
        else
        {
            animator.Play(stateName.Value, layer, normalizedTime);
            Debug.Log($"Target:{stateName.Value}, Running");
            return TaskStatus.Running;
        }
    }

    public override void OnReset()
    {
        targetGameObject = null;
        stateName = "";
        layer = -1;
        normalizedTime = float.NegativeInfinity;
    }
}
