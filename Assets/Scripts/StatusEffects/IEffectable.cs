using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable
{
    public void ApplyStatus (StatusData _data);
    public void UndoStatus ();
    public void UpdateStatusEffects ();
}
