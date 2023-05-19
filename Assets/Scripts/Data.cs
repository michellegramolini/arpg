using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    private bool _canInteract;

    public Data()
    {
        this._canInteract = true;

    }

    public bool CanInteract
    {
        get
        {
            return _canInteract;
        }

        set
        {
            _canInteract = value;
        }
    }
}
