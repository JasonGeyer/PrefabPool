/* Written By Jason R. Geyer.
 * jason.r.geyer@gmail.com
 * https://github.com/JasonGeyer
 */
using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtendsGameObject
    {
        public static bool Compare(this GameObject thisObj, GameObject compareObj, bool compareTransform = true, bool compareComponents = true)
        {
            Debug.Log("comparing:" + thisObj.name + " to " + compareObj.name);
            if (thisObj == null || compareObj == null) return false;

            bool isNameSame = false;
            bool isTransformSame = false;
            bool isComponentsSame = false;

            if (thisObj.name == compareObj.name && thisObj.tag == compareObj.tag)
            {
                isNameSame = true;
            }
            if (compareTransform)
            {
                if (thisObj.transform == compareObj.transform)
                {
                    isTransformSame = true;
                }
            }
            if (compareComponents)
            {
                Component[] ThisMonos = thisObj.GetComponents(typeof(MonoBehaviour));
                Component[] CompareMonos = thisObj.GetComponents(typeof(MonoBehaviour));

                int matchCount = 0;
                int componentCount = ThisMonos.Length;
                if (componentCount == CompareMonos.Length)
                {
                    for (int i = 0; i < componentCount; i++)
                    {
                        for (int j = 0; j < CompareMonos.Length; j++)
                        {
                            if (ThisMonos[i].GetType().Name == CompareMonos[j].GetType().Name)
                            {
                                matchCount++;
                            }
                        }
                    }
                }
                isComponentsSame = (matchCount == componentCount) ? true : false;
            }

            if (isNameSame || isTransformSame || isComponentsSame) return true;
            else
                return false;
        }
    }
}

