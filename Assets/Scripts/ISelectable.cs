using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    bool selected {  get; set; }

    void Select();

    void Unselect();
}
