using UnityEngine ;
using GDTools.ObjectPooling ;

public class Monster : PoolObject {
   //components references :
   public Rigidbody2D rb2D ;

   /*public override void OnObjectInstantiate () {
      base.OnObjectInstantiate () ;
   }*/

   public override void OnObjectDestroy () {
      base.OnObjectDestroy () ;
      rb2D.velocity = Vector2.zero ;
   }
}
