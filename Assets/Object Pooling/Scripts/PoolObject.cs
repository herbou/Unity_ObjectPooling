using UnityEngine ;

namespace GDTools.ObjectPooling {
   public class PoolObject : MonoBehaviour {
      [HideInInspector] public Pool pool ;

      public virtual void OnObjectInstantiate () {
         gameObject.SetActive (true) ;
      }

      public virtual void OnObjectDestroy () {
         gameObject.SetActive (false) ;
      }

      public void __InvokeDestroy (float delay) {
         Invoke ("__Destroy", delay) ;
      }

      public void __Destroy () {
         pool.DestroyObject (this) ;
      }
   }
}