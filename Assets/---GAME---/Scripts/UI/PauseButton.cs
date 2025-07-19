using DependencyInjection;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [Inject] private PauseManager pauseManager = null;
    [SerializeField] private PauseState toSet;

    public void OnClick()
    {
        pauseManager.SetState(toSet);
    }
}
