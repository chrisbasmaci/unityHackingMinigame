using UnityEngine;

namespace CoreScripts
{

    public abstract class AFactory<T>
    {
        protected virtual T Create()
        {
            var obj = Instantiate();
            return Initialize(obj);
        }
        protected abstract GameObject Instantiate();

        protected abstract T Initialize(GameObject obj);
    }

    
}