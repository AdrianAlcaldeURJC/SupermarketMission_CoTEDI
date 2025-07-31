using UnityEngine;

public class AnimationCyclesCounter : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private string animationStateName = "DestroyItem";
    [SerializeField] private float animationSpeed = 1f;
    private int cycles = 0;
    private float latestCycle = 0f;
    public int column;
    public int row;

    void Start()
    {
        anim.speed = animationSpeed;
    }

    void Update()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        if (state.IsName(animationStateName))
        {
            float actualCycles = Mathf.Floor(state.normalizedTime);

            if (actualCycles > latestCycle)
            {
                cycles++;
                latestCycle = Mathf.Floor(state.normalizedTime);
            }
        }

        if (cycles == 3)
        {
            anim.speed = 0;

            FindAnyObjectByType<TrolleyDragAndDropManager>().trolley[column, row] = null;
            Destroy(transform.parent.GetChild(0).GetChild(column + row * 8).GetChild(0).gameObject);
            FindAnyObjectByType<TrolleyDragAndDropManager>().EvaluateColumn(column);
            transform.parent.GetChild(0).GetChild(column + row * 8).GetComponent<TrolleyDropField>().RelocateColumnElements(row);
            DestroyImmediate(gameObject);
        }

    }
}
