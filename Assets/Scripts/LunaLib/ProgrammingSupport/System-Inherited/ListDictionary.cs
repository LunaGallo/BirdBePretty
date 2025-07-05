using System.Collections.Generic;

public class ListDictionary<TKey, TElement> : Dictionary<TKey, List<TElement>> {

    public void AddElement(TKey key, TElement element) {
        if (ContainsKey(key)) {
            this[key].Add(element);
        }
        else {
            Add(key, new List<TElement>() { element });
        }
    }
    public void RemoveElement(TKey key, TElement element) {
        if (ContainsKey(key)) {
            this[key].Remove(element);
            if (this[key].Count == 0) {
                Remove(key);
            }
        }
    }


}
