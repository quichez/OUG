using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

namespace Quichez
{
    public class NetworkedResourceSystem
    {
        public NetworkVariableInt Maximum { get; protected set; }
        public NetworkVariableInt Current { get; protected set; }
        public NetworkVariableInt Regeneration { get; protected set; }
        public NetworkVariableFloat RegenSpeed { get; protected set; }

        private NetworkVariableFloat regenTimer;
        private NetworkVariableBool isRegenerating;

        public float ResourcePercentage => ((float)Current.Value / Maximum.Value);

        protected NetworkedResourceSystem(int max, int curr = int.MaxValue, int regen = 0, float regspeed = 1.0f)
        {
            Maximum = new NetworkVariableInt(max);
            Current = new NetworkVariableInt(Mathf.Min(max,curr));
            Regeneration = new NetworkVariableInt(regen);
            RegenSpeed = new NetworkVariableFloat(regspeed);
        }       
        
        public IEnumerator Regenerate(float delay)
        {
            if (isRegenerating.Value)
                yield return null;
            else
            {
                while(Current.Value < Maximum.Value)
                {
                    isRegenerating.Value = true;
                    if (Current.Value == 0)
                        break;
                    else
                    {
                        regenTimer.Value += delay;
                        if(regenTimer.Value  >= RegenSpeed.Value)
                        {
                            regenTimer.Value = 0.0f;
                            Current.Value += Regeneration.Value;
                        }
                        yield return new WaitForSeconds(delay);
                    }
                }
            }
            if (Current.Value >= Maximum.Value)
                isRegenerating.Value = false;
        }
    }

    public class HealthSystem: NetworkedResourceSystem
    {       
        public HealthSystem(int max, int curr = int.MaxValue, int regen = 0, float regspeed = 1.0f) : base(max,curr,regen,regspeed)
        {

        }


    }



}

