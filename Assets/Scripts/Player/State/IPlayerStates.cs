using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerStates
{
    void Enter(PlayerController player);
    void Update(PlayerController player, float xInput, bool jumpPressed);
    void Exit(PlayerController player);
}
