# PrefabPool
A dynamic prefab pool for unity that fills itself as needed.

Useage: PrefabPool.cs is a monobehaviour, thus needs to be placed on an object in the scene.
If you want to spawn a specific set of objects upon awake(), drag and drop them into the 
public variable in the inspector. These objects will be spawned as inactive. To activate
a gameobject - call ActivateGameObject(gameObject). gameObject can be a reference to a prefab
or a reference to an instance that is already in the pool. If refering to an instance, you
will activate that specific instance. If refering to a prefab, it will activate the first 
match on the Inactive list. If the object is not on the list yet, a new one will be instantiated
and added to the pool.

Deactivation works about the same. If refering to an instance, it will deactivate that instance if
it is in the pool. If refering to a prefab, it will activate the first match on the active list (which is
usually going to be the oldest one unless another script interferes with this order).

