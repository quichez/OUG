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

            regenTimer = new NetworkVariableFloat(0.0f);
            isRegenerating = new NetworkVariableBool(false);
        }       
        
        // Resource Regeneration coroutine
        public IEnumerator Regenerate(float delay)
        {
            // If already regenerating, kill duplicate coroutine.
            if (isRegenerating.Value)
                yield return null;

            // else, start regeneration corourtine.
            else
            {
                // Regenerates only if below Maximum
                while(Current.Value < Maximum.Value)
                {
                    // Sets regenerating flag to avoid duplicate coroutines
                    isRegenerating.Value = true;

                    // If you are dead while regenerating, you die, so no more regen.
                    if (Current.Value == 0)
                        break;

                    // Otherwise, the regenTimer and regeneration values dictate how much you regenerate.
                    else
                    {
                        regenTimer.Value += delay;
                        if(regenTimer.Value  >= RegenSpeed.Value)
                        {
                            regenTimer.Value = 0.0f;
                            Current.Value += Regeneration.Value;
                        }

                        // Adjust delay input arg to make this run at different frame intervals
                        yield return new WaitForSeconds(delay);
                    }
                }
            }

            // Once you reach full, the regen flag goes false.
            if (Current.Value >= Maximum.Value)
                isRegenerating.Value = false;
        }
    }

    public class HealthSystem: NetworkedResourceSystem
    {       
        public HealthSystem(int max, int curr = int.MaxValue, int regen = 0, float regspeed = 1.0f) : base(max,curr,regen,regspeed)
        {

        }

        public void TakeDamage(int amount)
        {
            Current.Value -= amount;            
        }

    }



}

