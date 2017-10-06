using UnityEngine;

public delegate void ProgressEvent();

public enum AttributeType
{
    Distillable, Boilable, Burnable, Pulverizable, Rottable, Dryable, Edible, Soluble, Vaporable, Subliminable, Crystallizable, Filterable, Calcinable
}

[System.Serializable]
public class ItemAttribute {

	public ItemAttribute(AttributeType attributeType){
		type = attributeType;
		progress = 0f;
	}

    public enum State
    {
        Not, Lightly, Moderately, Very, Fully
    }
    const float stateThreshold = 4f;

    public ProgressEvent OnComplete;

	public AttributeType type;
	private State state = State.Not;
    public float value;
    public float progress;
    public Elements elementsModifier;

	public State GetState(){
		return state;
	}

	public virtual string GetStateAsString(){
		// Future functionality: if last state - return empty string?
		return (string)(System.Enum.GetNames (typeof(State)) [(int)state] + " "+ AttributeCompletionLabels.GetTypeCompletionLabel(type)) ?? "N/A";
	}

	public virtual void Trigger(){
		
	}


	public virtual void Trigger(float rate, Elements elmod){
        if(progress >= 1)
        {
            return;
        }
        progress += rate;
        state = (State)((int)(stateThreshold * progress));
        elementsModifier = elmod * progress;

		if (progress >= 1f) {
			if (OnComplete != null) {
				OnComplete ();
			}
		}
	}
    

    

}

public static class AttributeCompletionLabels{

	public static string GetTypeCompletionLabel(AttributeType type){
		switch (type) {
		case AttributeType.Distillable:
			return "distilled";
			break;
		case AttributeType.Boilable:
			return "boiled";
			break;
		case AttributeType.Burnable:
			return "burnt";
			break;
		case AttributeType.Pulverizable:
			return "pulverized";
			break;
		case AttributeType.Rottable:
			return "rotten";
			break;
		case AttributeType.Dryable:
			return "dried";
			break;
		case AttributeType.Edible:
			return "eaten";
			break;
		case AttributeType.Soluble:
			return "dissolved";
			break;
		case AttributeType.Vaporable:
			return "evaporated";
			break;
		case AttributeType.Subliminable:
			return "gaseous";
			break;
		case AttributeType.Crystallizable:
			return "crystallized";
			break;
		case AttributeType.Filterable:
			return "filtered";
			break;
		case AttributeType.Calcinable:
			return "calcinated";
			break;
		}
		return null;
	}

}
