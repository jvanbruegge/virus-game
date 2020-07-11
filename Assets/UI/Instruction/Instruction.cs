using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

/**
 * The value of these have to match the file name
 */
public enum EInstruction {
    FWD,
    BWD,
    Left,
    Right
}

public class Instruction : MonoBehaviour {
    public EInstruction instruction;
    private static GameObject prefab = null;

    private void Start() {
        this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Instruction/" + instruction);
    }

    public static GameObject Create(EInstruction instruction, Transform parent) {
        if(prefab == null) {
            prefab = Resources.Load("Instruction/Instruction") as GameObject;
        }
        GameObject obj = Instantiate(prefab, parent);
        obj.GetComponent<Instruction>().instruction = instruction;
        return obj;
    }
}
