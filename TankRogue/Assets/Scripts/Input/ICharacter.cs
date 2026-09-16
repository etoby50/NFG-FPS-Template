using UnityEngine;

public interface ICharacter
{
    public abstract void MoveWASD(Vector2 moveInput);

    public abstract void LookAround(Vector2 lookInput);

    public abstract void Jump();
}
