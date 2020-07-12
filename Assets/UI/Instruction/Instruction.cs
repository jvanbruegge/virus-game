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
    Right,
    Dash,
    Kill
}

public class Instruction : MonoBehaviour {
    private static GameObject[] prefabs = new GameObject[6];

    public static GameObject Create(EInstruction instruction, Transform parent) {
        if(prefabs[0] == null) {
            prefabs[0] = Resources.Load("Instruction/ButtonMove") as GameObject;
            prefabs[1] = Resources.Load("Instruction/ButtonBWD") as GameObject;
            prefabs[2] = Resources.Load("Instruction/ButtonLeft") as GameObject;
            prefabs[3] = Resources.Load("Instruction/ButtonRight") as GameObject;
            prefabs[4] = Resources.Load("Instruction/ButtonDash") as GameObject;
            prefabs[5] = Resources.Load("Instruction/ButtonKill") as GameObject;
        }
        return Instantiate(prefabs[(int)instruction], parent);
    }
}
