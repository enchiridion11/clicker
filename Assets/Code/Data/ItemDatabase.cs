using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabase : ScriptableObject {
    #region Fields

    public List<Item> database = new List<Item>();

    #endregion

    #region Properties

    #endregion

    #region Methods

    #region Unity

   /* void OnEnable () {
        if (database == null)
            database = new List<Item> ();
    }*/

    #endregion

    public void Add (Item item) {
        database.Add (item);
    }

    public void Remove (Item item) {
        database.Remove (item);
    }

    public void RemoveAt (int index) {
        database.RemoveAt (index);
    }

    public int Count {
        get { return database.Count; }
    }

    public Item Item (int index) {
        return database.ElementAt (index);
    }

    public void SortAlphabeticallyAtoZ () {
        database.Sort ((x, y) => string.Compare (x.name, y.name));
    }

    #endregion
}