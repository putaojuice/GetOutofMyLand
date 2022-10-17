using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable
{
    public void ApplyStatus(StatusData _data);
    public void UndoStatus();
    public void ApplyNewStatus(string newStatus, float DOT, float DOTInterval, float statusDuration, float damage, float slowDebuff, float towerLevel);
    public void UpdateStatusEffects();
    public void HandleStatus(StatusData _data);

}
