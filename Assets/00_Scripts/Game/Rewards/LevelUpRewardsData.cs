using System.Collections.Generic;

using _00_Scripts.Game.Items;

using UnityEngine;

namespace _00_Scripts.Game.Rewards
{
  [CreateAssetMenu(fileName = "LevelUpRewardsData", menuName = "Game/Rewards/LevelUpRewardsData")]
  public class LevelUpRewardsData: ScriptableObject
  {
    public List<Item> rewardsPool;
  }
}
