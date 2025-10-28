using UnityEngine;

public interface IExploding 
{
    public Animator Animator { get; }
    public bool HasExploded { get; }
    public void Explode();
}