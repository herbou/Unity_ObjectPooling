using UnityEngine ;
using System.Collections ;
using System.Collections.Generic ;
using UnityEngine.Events ;

/// Developed by : Hamza Herbou
/// ------------------------------------
/// Email  : hamza95herbou@gmail.com
/// GITHUB : https://github.com/herbou/

namespace GDTools.ObjectPooling {

   public class Pool : MonoBehaviour {
      [SerializeField] private int capacity = 5 ;
      [SerializeField] private bool autoGrow = false ;
      [SerializeField] private GameObject poolObjectPrefab ;

      private int _activeObjects = 0 ;

      public int activeObjects { get { return _activeObjects ; } }

      [HideInInspector] public UnityAction<PoolObject> onObjectInstantiated ;
      [HideInInspector] public UnityAction<PoolObject> onObjectDestroyed ;

      private Queue<PoolObject> queue = new Queue<PoolObject> () ;


      private void Awake () {
         InitializeQueue () ;
      }

      private void InitializeQueue () {
         for (int i = 0; i < capacity; i++)
            InsertObjectToQueue () ;
      }

      private void InsertObjectToQueue () {
         PoolObject poolObj = Instantiate (poolObjectPrefab, transform).GetComponent <PoolObject> () ;
         poolObj.pool = this ;
         poolObj.OnObjectDestroy () ;

         queue.Enqueue (poolObj) ;
      }



      // Instaciate object : ------------------------------------------------
      public PoolObject InstantiateObject () {
         return InstantiateObject (Vector3.zero, Quaternion.identity) ;
      }

      public PoolObject InstantiateObject (Vector3 position) {
         return InstantiateObject (position, Quaternion.identity) ;
      }

      public PoolObject InstantiateObject (Vector3 position, Quaternion rotation) {
         if (queue.Count == 0) {
            if (autoGrow) {
               capacity++ ;
               InsertObjectToQueue () ;

            } else {
               #if UNITY_EDITOR
               Debug.LogError (@"[ <color=#ff5566><b>Pool out of objects error</b></color> ] : no more gameobjects available in the <i>" + this.name + "</i> pool.\n"
               + "Make sure to increase the <b>Capacity</b> or check the <b>Auto Grow</b> checkbox in the inspector.\n\n", gameObject) ;
               #endif
               return null ;
            }
         }

         PoolObject poolObj = queue.Dequeue () ;

         poolObj.transform.position = position ;
         poolObj.transform.rotation = rotation ;
         poolObj.OnObjectInstantiate () ;

         if (!object.ReferenceEquals (onObjectInstantiated, null))
            onObjectInstantiated.Invoke (poolObj) ;

         _activeObjects++ ;

         return poolObj ;
      }



      // Destroy object : ------------------------------------------------
      public void DestroyObject (PoolObject poolObj) {
         poolObj.OnObjectDestroy () ;

         queue.Enqueue (poolObj) ;

         if (!object.ReferenceEquals (onObjectDestroyed, null))
            onObjectDestroyed.Invoke (poolObj) ;

         _activeObjects-- ;
      }

      public void DestroyObject (PoolObject poolObj, float delay) {
         poolObj.__InvokeDestroy (delay) ;
      }

   }

}