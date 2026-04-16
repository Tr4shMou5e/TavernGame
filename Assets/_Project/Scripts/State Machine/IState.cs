using UnityEngine;

public interface IState
{
    void OnEnter();
    void Update();
    void FixedUpdate();
    void OnExit();
    T Get<T>();
    void Set<T>(T component);
}