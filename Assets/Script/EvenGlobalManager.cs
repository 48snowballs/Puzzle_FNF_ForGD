using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sigtrap.Relays;

public class EvenGlobalManager : Singleton<EvenGlobalManager>
{
    //public Relay OnStartLoadScene = new Relay();
    //public Relay OnFinishLoadScene = new Relay();
    //public Relay OnUpdateSetting = new Relay();

    public Relay OnStartPlay = new Relay();
    public Relay<bool> OnEndPlay = new Relay<bool>();

    public Relay<bool> OnActiveTarget = new Relay<bool>();

    //public Relay OnActiveEffectScore = new Relay();

    //public Relay OnContinue = new Relay();

    //public Relay<int> OnStartEffectBonus = new Relay<int>();
    //public Relay OnEndEffectBonus = new Relay();
}
