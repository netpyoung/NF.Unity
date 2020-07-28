using System.Linq;
using UnityEngine;
using System.Reflection;
using NFRuntime.Attributes;

namespace NFRuntime.Extensions
{
    public static class ExUnity
    {
        private static T FGetComp<T>(this GameObject go) where T : Component
        {
            if (go == null)
            {
                UnityEngine.Debug.LogErrorFormat("[ERROR] go is null");
                return null;
            }

            var t = go.GetComponent<T>();
            if (t != null)
            {
                return t;
            }    
            return go.AddComponent<T>();
        }

        public static T FGetComp<T>(this MonoBehaviour behaviour) where T : Component
        {
            return behaviour.gameObject.FGetComp<T>();
        }

        public static T FindGetComp<T>(this GameObject root_go, string name, bool isContainsDiabled = false) where T : Component
        {
            var go = root_go.GetFindChildGameObjectByGameObjectName(name, isContainsDiabled);
            if (go == null)
                return null;
            return go.FGetComp<T>();
        }

        public static T FindGetComp<T>(this MonoBehaviour behaviour, string name, bool isContainsDiabled = false) where T : Component
        {
            return behaviour.gameObject.FindGetComp<T>(name, isContainsDiabled);
        }

        public static T FindGetComp<T>(this GameObject go) where T : Component
        {
            return go.FGetComp<T>();
        }

        public static T FindGetComp<T>(this MonoBehaviour behaviour) where T : Component
        {
            return behaviour.gameObject.FGetComp<T>();
        }

        public static GameObject GetFindChildGameObjectByGameObjectName(this GameObject root_go, string name, bool isContainsDiabled = false)
        {
            var transforms = root_go.transform.GetComponentsInChildren(typeof(Transform), true);
            var trans = transforms.First(x => x.name == name);
            return trans.gameObject;
        }

        public static GameObject GetFindChildGameObjectByGameObjectName(this MonoBehaviour behaviour, string name, bool isContainsDiabled = false)
        {
            return behaviour.gameObject.GetFindChildGameObjectByGameObjectName(name, isContainsDiabled);
        }

        public static void ResetLocalTransform(this Transform trans)
        {
            trans.position = Vector3.zero;
            trans.rotation = Quaternion.identity;
        }

        public static void ResetLocalTransform(this MonoBehaviour behaviour)
        {
            behaviour.transform.ResetLocalTransform();
        }

        public static void SetActive(this MonoBehaviour behaviour, bool isActive)
        {
            behaviour.gameObject.SetActive(isActive);
        }

        public static void SetParent(this MonoBehaviour behaviour, Transform parentTrans)
        {
            behaviour.gameObject.transform.SetParent(parentTrans);
        }

        public static void SetParent(this MonoBehaviour behaviour, GameObject parentGo)
        {
            behaviour.gameObject.transform.SetParent(parentGo.transform);
        }

        public static void SetParent(this GameObject go, Transform parentTrans)
        {
            go.transform.SetParent(parentTrans);
        }

        public static void SetParent(this GameObject go, GameObject parentGo)
        {
            go.transform.SetParent(parentGo.transform);
        }

        public static Vector3 Rot(this Vector3 p, Vector3 pivot, Vector3 rot)
        {
            return Quaternion.Euler(rot) * (p - pivot) + pivot;
        }

        public static void InitTransform(this GameObject go)
        {
            InitTransform(go.transform);
        }
        public static void InitTransform(this Component component)
        {
            InitTransform(component.transform);
        }

        public static void InitTransform(this Transform transform)
        {
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
        }

        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            Transform result = aParent.Find(aName);
            if (result != null)
            {
                return result;
            }

            foreach (Transform child in aParent)
            {
                result = child.FindDeepChild(aName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }

    public static class ExtAttributeUnity
    {
        public static void InitComponentPathAttributes(this MonoBehaviour behaviour)
		{
			var behaviour_type = behaviour.GetType();
			var comp_type = typeof(Component);
			var path_type = typeof(ComponentPathAttribute);

			var members = behaviour_type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				.Where(m =>
					(m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property)
					&& m.GetMemberType().IsSubclassOf(comp_type)
					&& m.GetCustomAttributes(path_type, true).Length == 1
				);

			foreach(var item in members)
			{
				var attribute = item.GetCustomAttributes(path_type, true)[0] as ComponentPathAttribute;
				var member_type = item.GetMemberType();

				var child = behaviour.transform.Find(attribute.Path);
				if(child == null)
				{
					Debug.LogError(string.Format("can't find child in {0}", attribute.Path));
					continue;
				}

				var member_comp = child.GetComponent(member_type);
				if(member_comp == null)
                {
                    member_comp = child.gameObject.AddComponent(member_type);
                }

				if(member_comp == null)
				{
					Debug.LogError(string.Format("can't find componnet {0} {1}", attribute.Path, member_type));
					continue;
				}

				item.SetValue(behaviour, member_comp);
			}
		}
    }
}