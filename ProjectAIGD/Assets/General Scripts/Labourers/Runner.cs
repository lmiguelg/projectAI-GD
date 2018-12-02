using System.Collections;
using System.Collections.Generic;
using General_Scripts.Enums;
using SteeringBehaviours.Scripts.Basics;
using UnityEngine;

namespace General_Scripts.Labourers
{
    public class Runner : Labourer
    {
        public Teams MyTeam;

        /// <summary>
        /// Our only goal will ever be to mine ore. The MineOreAction will be able to fulfill this goal.
        /// </summary>
        /// <returns></returns>
        public override HashSet<KeyValuePair<string,object>> CreateGoalState ()
        {
            var goal = new HashSet<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("dropFlag", true)
            };

            return goal;
        }

        public IEnumerator SlowDown()
        {
            var steering = GetComponent<SteeringBasics>();
            while (true)
            {
                steering.MaxVelocity--;
                if (steering.MaxVelocity < 0)
                    steering.MaxVelocity = 0;

                yield return new WaitForSeconds(1f);
            }
        }
    }
}

