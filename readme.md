# Basic event system

To start using event system just create observable with return type that you need

```cs
var onButtonClicked = new Observable<bool>();
```

Then by calling *Subscribe()* from your *Observable* instance you can pass Action Delegate.
Returned *Subscriber* should be used for unsubscribing the observable when you don't want to listen to the changes any more (for eg. in OnDestroy or OnDisable)

```cs
var subscriber = onButtonClicked.Subscribe((emittedValue) => {
	// do sth
});
```

You can store your subscribers in *Subscription* object:
```cs
_subscription = new Subscription();
_subscription.AddSubscriber(subscriber);
```

Unsubscribing:
```cs
private void OnDestroy() {
    _subscription.Unsubscribe()
}
```

Depending on your needs you can use also *BehaviourSubject* which is other type of *Observable*. *BehaviourSubject* returns the last emitted value in the time of subscribing.

# Pool system

Example implementation:

```cs
public class BowExampleClass : MonoBehaviour { 
    [SerializedField] private ExampleArrow _myArrowPrefab;
    
    private void Start() {
        var amountOfArrowsToPrepare = 10;
        
        // Add your game objects to pool,
        // (instead of passing prefab you can also provide path to prefab in Resource folder)
        ObjectPooler.Instance.PopulatePool(amountOfArrowsToPrepare, _myArrowPrefab);
    }
    
    public void Shoot() {
        var myArrow = ObjectPooler.Instance.GetPooledObject(PoolableKey);
        myArrow.DoOnShoot();
    }
}
```

```cs
// Objects you would like to pool inherit from BasePoolable abstract class
public class ExampleArrow : BasePoolable { 

    public void DoOnShoot() {
        // do sth
    }

    private void Awake() {
        // In your script or from inspector view set PoolableKey value (member of BasePoolable)
        PoolableKey = "fireArrow";
    }
    
    private OnCollisionEnter(Collision collision) {
        ReturnToPool();
    }
}
```

# UnityEngine.dll reference
Project requires reference to *UnityEngine.dll*. To add the reference just right click on project > add > Reference...

![Add ref 1](https://i.imgur.com/NxHPJ3K.png)

Then from Reference Manager view click **Browse** and find UnityEngine.dll file. File should be locate in *C:\YOURPROGRAMSFOLDER\Unity\Editor\Data\Managed*.

![Add ref 2](https://i.imgur.com/HHcHJz2.png)
