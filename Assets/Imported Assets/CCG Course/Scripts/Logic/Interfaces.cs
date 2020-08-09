using UnityEngine;
using System.Collections;

public interface ICharacter: IIdentifiable
{	
    int Health { get;    set;}

    void Die();
}

public interface IIdentifiable
{
    int ID { get; }
}
