using UnityEngine;

public delegate void ProgressEvent();

public enum AttributeType
{
    Distillable, Boilable, Burnable, Pulverizable, Rottable, Dryable, Edible, Soluble, Vaporable, Subliminable, Crystallizable, Filterable, Calcinable
}

[System.Serializable]
public class Attribute {

	public Attribute(AttributeType attributeType){
		type = attributeType;
		progress = 0f;
	}



    public enum State
    {
        Not, Lightly, Moderately, Very, Fully
    }

    public ProgressEvent OnComplete;

	public AttributeType type;
	private State state = State.Not;
	private float progress {
		get { return progress; }
		set { progress = Mathf.Clamp01 (value); }
	}

	public State GetState(){
		return state;
	}

	public virtual string GetStateAsString(){
		// Future functionality: if last state - return empty string?
		return (string)(System.Enum.GetNames (typeof(State)) [(int)state] + AttributeCompletionLabels.GetTypeCompletionLabel(type)) ?? "N/A";
	}

	public virtual void Trigger(){
		
	}


	public virtual void Trigger(float rate){
		progress += rate;

		if (progress >= 1f) {
			if (OnComplete != null) {
				OnComplete ();
			}
		}
	}
}

public static class AttributeCompletionLabels{

	public static string GetTypeCompletionLabel(Attribute.Type type){
		switch (type) {
		case Attribute.Type.Distillable:
			return "distilled";
			break;
		case Attribute.Type.Boilable:
			return "boiled";
			break;
		case Attribute.Type.Burnable:
			return "burnt";
			break;
		case Attribute.Type.Pulverizable:
			return "pulverized";
			break;
		case Attribute.Type.Rottable:
			return "rotten";
			break;
		case Attribute.Type.Dryable:
			return "dried";
			break;
		case Attribute.Type.Edible:
			return "eaten";
			break;
		case Attribute.Type.Soluble:
			return "dissolved";
			break;
		case Attribute.Type.Vaporable:
			return "evaporated";
			break;
		case Attribute.Type.Subliminable:
			return "gaseous";
			break;
		case Attribute.Type.Crystallizable:
			return "crystallized";
			break;
		case Attribute.Type.Filterable:
			return "filtered";
			break;
		case Attribute.Type.Calcinable:
			return "calcinated";
			break;
		}
		return null;
	}

}
