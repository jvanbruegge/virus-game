using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class InstructionList : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Instruction.Create(EInstruction.FWD, transform);
        Instruction.Create(EInstruction.BWD, transform);
        Instruction.Create(EInstruction.Left, transform);
        Instruction.Create(EInstruction.Right, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
