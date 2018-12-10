using UnityEngine;

namespace Assets.TeamDEL.AI
{
    public interface IUnit
    {
        void SetTarget(Transform transform);
        bool DoFollowPathStep();
    }
}