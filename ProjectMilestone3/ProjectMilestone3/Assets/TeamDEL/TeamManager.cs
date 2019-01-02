﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.TeamDEL.GoalOrientedBehaviour.Scripts.GameData.Actions;
using General_Scripts;
using General_Scripts.AI.GOAP;
using General_Scripts.Labourers;
using SteeringBehaviours.Scripts.Basics;
using UnityEditor;
using UnityEngine;

namespace Assets.TeamDEL
{
    public class TeamManager : MonoBehaviour
    {
        public bool WeHaveFlag;

        public List<GoapAgent> MyAgents;
        public List<Runner> MyRunners;
        public Runner runnerCarrier;
        private List<Vector3> strategicPositions = new List<Vector3>();
        public Vector3 closestPosition;
        public Vector3 flagPosition;
        public FlagComponent _flag;
        public string myTeamName;
        private GoapAgent goapAgent;

        public void SetTeamNewGoal(string goal)
        {
            foreach (var runner in MyRunners)
            {
                runner.Goals.Clear();
                runner.Goals.Add(goal);
            }
        }

        private void Awake()
        {
            //apenas para testar o modo defensivo;
            goapAgent = GetComponent<GoapAgent>();


            myTeamName = MyRunners[0].MyTeam.ToString();
            print("MyRunners team name" + MyRunners[0].MyTeam.ToString());

            StartCoroutine(RequestNewPlan());
            StartCoroutine(AdaptActionsCosts());
            StartCoroutine(CheckFlag());
            StartCoroutine(CheckRunnerCarrier());
           


            //posições estratégicas----------------------------
            Vector3 position1 = new Vector3(20f, 0f, 0f);
            Vector3 position2 = new Vector3(-20f, 0f, 0f);
            Vector3 position3 = new Vector3(0f, 0f, -20f);
            Vector3 position4 = new Vector3(0f, 0f, 20f);
            Vector3 center = new Vector3( 0f, 0f, 0f);
            strategicPositions.Add(center);
            strategicPositions.Add(position1);
            strategicPositions.Add(position2);
            strategicPositions.Add(position3);
            strategicPositions.Add(position4);


            //criar um novo goal-> defender

        }

     
        public Vector3 GetClosestStrategicPosition(Runner runner)
        {
            closestPosition = strategicPositions[0];

            if (myTeamName.Contains("B"))
            {
                if (flagPosition.x < -0.1f && flagPosition.z < -0.1f)//close to team A base
                {
                    Vector3 defensePosition = new Vector3(-20f, 0, -20f);
                    closestPosition = defensePosition;
                }
                else
                {
                    foreach (var position in strategicPositions)
                    {
                        if (Vector3.Distance(runner.transform.position, position) < Vector3.Distance(runner.transform.position, closestPosition))
                        {
                            closestPosition = position;
                        }
                    }
                }


            }
            else if (myTeamName.Contains("A"))
            {
                if (flagPosition.x > 0.1f && flagPosition.z > 0.1f)//close to team B base
                {
                    Vector3 defensePosition = new Vector3(20f, 0, 20f);
                    closestPosition = defensePosition;
                }
                else
                {
                    foreach (var position in strategicPositions)
                    {
                        if (Vector3.Distance(runner.transform.position, position) < Vector3.Distance(runner.transform.position, closestPosition))
                        {
                            closestPosition = position;
                        }
                    }
                }

            }
            
            return closestPosition;
        }

       


        private IEnumerator RequestNewPlan()
        {

            while (true)
            {
                yield return new WaitForSeconds(1);
                foreach (var agent in MyAgents)
                {
                    if(agent.NeedNewPlan)
                        agent.GetComponent<SteeringBasics>().Stop();
                    

                    agent.AbortPlan();
                }
            }
        }

        private IEnumerator AdaptActionsCosts()
        {
            yield return null;
            while (true)
            {
                foreach (var runner in MyRunners)
                {
                    var steering = runner.GetComponent<SteeringBasics>();
                    runner.GetComponent<DropOffFlag>().Cost = steering.MaxVelocity == 0 ? float.PositiveInfinity : 1 / runner.GetComponent<SteeringBasics>().MaxVelocity;


                    //
                    //+++++++
                    runner.GetComponent<DropOffFlag>().Cost = WeHaveFlag && _flag.Carrier != runner ? float.PositiveInfinity : 1 / runner.GetComponent<SteeringBasics>().MaxVelocity;
                    runner.GetComponent<SecondRunnerAction>().Cost = WeHaveFlag ? 0.1f : 6f;
                    

                    yield return null;
                }
            }
        }
		private IEnumerator CheckFlag()
        {
            yield return null;
            while (true)
            {
                WeHaveFlag = MyRunners.Any(runner => runner.Backpack.HasFlag);
                flagPosition = _flag.transform.position;
                yield return null;
            }
        }
        public IEnumerator CheckRunnerCarrier()
        {
            yield return null;
            while (true)
            {
                runnerCarrier = MyRunners.Find(runner => runner.Backpack.HasFlag);              
                yield return null;
            }
        }


        public void setTeamNewGoal(string goal, string name)
        {
            var run = MyRunners.First(runner => runner.name == name);
            run.Goals.Clear();
            run.Goals.Add(goal);
        }
    }
}