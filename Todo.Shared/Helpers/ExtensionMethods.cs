using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Shared.Helpers;
public static class ExtensionMethods
{
    public static void ReplaceItem<T>(this Collection<T> col, Func<T, bool> match, T newItem)
    {
        T? oldItem = col.FirstOrDefault(i => match(i));
        if (oldItem == null) return;
        int oldIndex = col.IndexOf(oldItem);
        col[oldIndex] = newItem;
    }
}
