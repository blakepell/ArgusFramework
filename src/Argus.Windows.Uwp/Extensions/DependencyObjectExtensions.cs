using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Argus.Extensions
{
    public static class DependencyObjectExtensions
    {
        /// <summary>
        ///     Finds the first child of a specifc type in the DependencyObject visual tree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <returns></returns>
        public static T FindFirstChildOfType<T>(this DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;

                    if (typedChild != null)
                    {
                        return typedChild;
                    }

                    queue.Enqueue(child);
                }
            }

            return null;
        }

        /// <summary>
        ///     Finds all of the children of a specifc type in the DependencyObject visual tree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <returns></returns>
        public static List<T> FindChildrenOfType<T>(this DependencyObject root) where T : class
        {
            var children = new List<T>();
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;

                    if (typedChild != null)
                    {
                        children.Add(typedChild);
                    }

                    queue.Enqueue(child);
                }
            }

            return children;
        }
    }
}