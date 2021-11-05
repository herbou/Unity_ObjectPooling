using UnityEngine ;
using System.Collections ;
using GDTools.ObjectPooling ;

public class MonstersSpawner : MonoBehaviour {
   [SerializeField] private Pool pool ;
   [SerializeField] private float spawnDelay = .5f ;

   WaitForSeconds delay ;

   private void Start () {
      delay = new WaitForSeconds (spawnDelay) ;
      StartCoroutine (RandomSpawning ()) ;
   }

   private IEnumerator RandomSpawning () {
      while (true) {
         yield return delay ;

         // Instanciate monster :
         Monster monster = (Monster)Spawn () ;
         monster.rb2D.velocity = Vector2.up * 3f ;

         // Destroy monster after 2 second :
         pool.DestroyObject (monster, 2f) ;
      }
   }

   private PoolObject Spawn () {
      return pool.InstantiateObject (
         new Vector3 (Random.Range (-3f, 3f), -3f, 0f), 
         Quaternion.identity
      ) ;
   }
}
