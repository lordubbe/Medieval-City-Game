// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Fungus
{
    /// <summary>
    /// If the test expression is true, execute the following command block.
    /// </summary>
    [CommandInfo("Flow",
                 "IfCheck",
                 "If the test expression is true, execute the following command block.")]
    [AddComponentMenu("")]
    public class IfCheck : VariableCondition
    {

        [Tooltip("Name of the method to call")]
        [SerializeField]
        protected string methodName = "";

        public List<MethodInfo> methods;
        
        public MonoBehaviour targetMonobehaviour;

        #region Public members

        public override void OnEnter()
        {

            methods = new List<MethodInfo>();
            methods = targetMonobehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            Debug.Log("IT IS "+methods.Find(x => x.Name == methodName).Invoke(targetMonobehaviour, new object[0]));
            bool check = (bool)methods.Find(x => x.Name == methodName).Invoke(targetMonobehaviour, new object[0]);

            if (check)
            {
                OnTrue();
            }
            else
            {
                OnFalse();
            }
        }

        public override string GetSummary()
        {
            if (targetMonobehaviour == null)
            {
                return "Error: No target GameObject specified";
            }

            if (methodName.Length == 0)
            {
                return "Error: No named method specified";
            }

            return targetMonobehaviour.name + " : " + methodName;
        }

        #endregion




        #region Public members

        public override Color GetButtonColor()
        {
            return new Color32(253, 253, 150, 255);
        }

        #endregion
    }
}