// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using System.Linq;
//using MarkerMetro.Unity.WinLegacy.Reflection;

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


		[Tooltip("GameObject containing the component method to be invoked")]
		[SerializeField] protected GameObject targetObject;

		[HideInInspector]
		[Tooltip("Full name of the target component")]
		[SerializeField] protected string targetComponentFullname;

		[HideInInspector]
		[Tooltip("Display name of the target component")]
		[SerializeField] protected string targetComponentText;

		[HideInInspector]
		[Tooltip("Name of target method to invoke on the target component")]
		[SerializeField] protected string targetMethod;

		[HideInInspector]
		[Tooltip("Display name of target method to invoke on the target component")]
		[SerializeField] protected string targetMethodText;


		[HideInInspector]
		[Tooltip("List of parameters to pass to the invoked method")]
		[SerializeField] protected InvokeMethodParameter[] methodParameters;


		[SerializeField] string flagToCheck;

        #region Public members

        public override void OnEnter()
        {

            methods = new List<MethodInfo>();
            methods = targetMonobehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            //Debug.Log("IT IS "+methods.Find(x => x.Name == methodName).Invoke(targetMonobehaviour, new object[0]));
            //bool check = (bool)methods.Find(x => x.Name == methodName).Invoke(targetMonobehaviour, new object[0]);
			object[] newobj = new object[1];
			newobj[0] = flagToCheck;
			bool check = (bool)methods.Find(x => x.Name == methodName).Invoke(targetMonobehaviour,newobj );

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









		protected virtual System.Type[] GetParameterTypes()
		{
			System.Type[] types = new System.Type[methodParameters.Length];

			for (int i = 0; i < methodParameters.Length; i++)
			{
				var item = methodParameters[i];
				var objType = ReflectionHelper.GetType(item.objValue.typeAssemblyname);

				types[i] = objType;
			}

			return types;
		}

		protected virtual object[] GetParameterValues()
		{
			object[] values = new object[methodParameters.Length];
			var flowChart = GetFlowchart();

			for (int i = 0; i < methodParameters.Length; i++)
			{
				var item = methodParameters[i];

				if (string.IsNullOrEmpty(item.variableKey))
				{
					values[i] = item.objValue.GetValue();
				}
				else
				{
					object objValue = null;

					switch (item.objValue.typeFullname)
					{
					case "System.Int32":
						var intvalue = flowChart.GetVariable<IntegerVariable>(item.variableKey);
						if (intvalue != null)
							objValue = intvalue.Value;
						break;
					case "System.Boolean":
						var boolean = flowChart.GetVariable<BooleanVariable>(item.variableKey);
						if (boolean != null)
							objValue = boolean.Value;
						break;
					case "System.Single":
						var floatvalue = flowChart.GetVariable<FloatVariable>(item.variableKey);
						if (floatvalue != null)
							objValue = floatvalue.Value;
						break;
					case "System.String":
						var stringvalue = flowChart.GetVariable<StringVariable>(item.variableKey);
						if (stringvalue != null)
							objValue = stringvalue.Value;
						break;
					case "UnityEngine.Color":
						var color = flowChart.GetVariable<ColorVariable>(item.variableKey);
						if (color != null)
							objValue = color.Value;
						break;
					case "UnityEngine.GameObject":
						var gameObject = flowChart.GetVariable<GameObjectVariable>(item.variableKey);
						if (gameObject != null)
							objValue = gameObject.Value;
						break;
					case "UnityEngine.Material":
						var material = flowChart.GetVariable<MaterialVariable>(item.variableKey);
						if (material != null)
							objValue = material.Value;
						break;
					case "UnityEngine.Sprite":
						var sprite = flowChart.GetVariable<SpriteVariable>(item.variableKey);
						if (sprite != null)
							objValue = sprite.Value;
						break;
					case "UnityEngine.Texture":
						var texture = flowChart.GetVariable<TextureVariable>(item.variableKey);
						if (texture != null)
							objValue = texture.Value;
						break;
					case "UnityEngine.Vector2":
						var vector2 = flowChart.GetVariable<Vector2Variable>(item.variableKey);
						if (vector2 != null)
							objValue = vector2.Value;
						break;
					case "UnityEngine.Vector3":
						var vector3 = flowChart.GetVariable<Vector3Variable>(item.variableKey);
						if (vector3 != null)
							objValue = vector3.Value;
						break;
					default:
						var obj = flowChart.GetVariable<ObjectVariable>(item.variableKey);
						if (obj != null)
							objValue = obj.Value;
						break;
					}

					values[i] = objValue;
				}
			}

			return values;
		}


    }

}



/*
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
			//Debug.Log("IT IS "+methods.Find(x => x.Name == methodName).Invoke(targetMonobehaviour, new object[0]));
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
*/