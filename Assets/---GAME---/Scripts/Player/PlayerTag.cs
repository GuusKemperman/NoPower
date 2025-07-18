using UnityEngine;

public class PlayerTag : MonoBehaviour, DependencyInjection.IDependencyProvider
{
    [DependencyInjection.Provide]
    public PlayerTag Provide()
    {
        return this;
    }
}
