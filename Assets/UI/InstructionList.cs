using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class InstructionList : MonoBehaviour
{
    private List<GameObject> instructions = new List<GameObject>();

    public void AddInstruction(EInstruction instruction) {
        this.instructions.Add(Instruction.Create(instruction, transform));
    }

    public void PopInstruction() {
        Destroy(this.instructions[0]);
        this.instructions.RemoveAt(0);
    }
}
