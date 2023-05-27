using System.Collections.Generic;

namespace AutoRent_Logic.Services.Interfaces
{
    public interface IEnumerator : IEnumerator<Reviews>
    {
        bool MoveNext(); // переміщення на одну позицію вперед в контейнері елементів
        Reviews Current { get; } // поточний елемент у контейнері
        void Reset(); // перемещение в начало контейнера
    }
}
