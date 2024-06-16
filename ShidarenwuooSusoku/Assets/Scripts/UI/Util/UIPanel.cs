using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public abstract class UIPanel : UIBehaviour
{
    public abstract void Open();

    public virtual void Close()
    {
        GameObject.Destroy(gameObject);
    }
}
